﻿using GameShop.Data.Repository.IRepository;
using GameShop.Models;
using GameShop.Models.ViewModels;
using GameShop.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stripe.Checkout;
using System.Security.Claims;
using static System.Net.WebRequestMethods;

namespace GameShop.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        [BindProperty]
        public ShoppingCartVM shoppingCartVM { get; set; }
        public int OrderTotal { get; set; }
        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork= unitOfWork;
        }

        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            shoppingCartVM = new ShoppingCartVM
            {
                ShoppingCarts = _unitOfWork.ShoppingCart.GetAll(
                    c => c.ApplicationUserId == claim.Value, includeProperties: "Product"),
                OrderHeader = new OrderHeader()
            };

            CalculateOrderTotalPrice(shoppingCartVM);

            return View(shoppingCartVM);
        }

        [HttpGet]
        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            shoppingCartVM = new ShoppingCartVM()
            {
                ShoppingCarts = _unitOfWork.ShoppingCart.GetAll(
                    s => s.ApplicationUserId == claim.Value, includeProperties: "Product"),
                OrderHeader = new OrderHeader()
            };
             
            shoppingCartVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.GetFirstOrDefault(
                u => u.Id == claim.Value);

            shoppingCartVM.OrderHeader.Name = shoppingCartVM.OrderHeader.ApplicationUser.Name;
            shoppingCartVM.OrderHeader.PhoneNumber = shoppingCartVM.OrderHeader.ApplicationUser.PhoneNumber;
            shoppingCartVM.OrderHeader.StreetAddress = shoppingCartVM.OrderHeader.ApplicationUser.StreetAddress;
            shoppingCartVM.OrderHeader.City = shoppingCartVM.OrderHeader.ApplicationUser.City;
            shoppingCartVM.OrderHeader.State = shoppingCartVM.OrderHeader.ApplicationUser.State;
            shoppingCartVM.OrderHeader.PostalCode = shoppingCartVM.OrderHeader.ApplicationUser.PostalCode;

            CalculateOrderTotalPrice(shoppingCartVM);

            return View(shoppingCartVM);
        }

        [HttpPost]
        [ActionName("Summary")]
        [ValidateAntiForgeryToken]
        public IActionResult SummaryPOST()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            shoppingCartVM.ShoppingCarts = _unitOfWork.ShoppingCart.GetAll(
                c => c.ApplicationUserId == claim.Value, includeProperties: "Product");

            shoppingCartVM.OrderHeader.OrderDate = DateTime.Now;
            shoppingCartVM.OrderHeader.ApplicationUserId = claim.Value;

            CalculateOrderTotalPrice(shoppingCartVM);

            var applicationUser = _unitOfWork.ApplicationUser.GetFirstOrDefault(
                u => u.Id == claim.Value);

            if(applicationUser.CompanyId == null)
            {
				shoppingCartVM.OrderHeader.OrderStatus = SD.OrderStatusPending;
				shoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
			}
            else
            {
                // Companies are allowed to pay at a later date
                shoppingCartVM.OrderHeader.OrderStatus = SD.OrderStatusApproved;
                shoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusDelayedPayment;
            }

            _unitOfWork.OrderHeader.Add(shoppingCartVM.OrderHeader);
            _unitOfWork.Save();

            foreach (var shoppingCart in shoppingCartVM.ShoppingCarts)
            {
                var orderDetail = new OrderDetail()
                {
                    ProductId = shoppingCart.ProductId,
                    OrderId = shoppingCartVM.OrderHeader.Id,
                    Price = shoppingCart.Price,
                    Count = shoppingCart.Count,
                };

                _unitOfWork.OrderDetail.Add(orderDetail);
                _unitOfWork.Save();
            }

            // Stripe Settings
            if(applicationUser.CompanyId.GetValueOrDefault() == 0)
            {
                var domain = "https://localhost:7075/";
                var options = new SessionCreateOptions
                {
                    PaymentMethodTypes = new List<string>
                    {
                        "card"
                    },
                    LineItems = new List<SessionLineItemOptions>(),
                    Mode = "payment",
                    SuccessUrl = domain+$"customer/cart/OrderConfirmation?id={shoppingCartVM.OrderHeader.Id}",
                    CancelUrl = domain+$"customer/cart/Index"
                };

                foreach (var shoppingCart in shoppingCartVM.ShoppingCarts)
                {
                    var sessionLineItem = new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(shoppingCart.Price*100), // 100 -> 1.00 (cents)
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = shoppingCart.Product.Name
                            }
                        },
                        Quantity = shoppingCart.Count
                    };

                    options.LineItems.Add(sessionLineItem);
                }

                var service = new SessionService();
                var session = service.Create(options);

                _unitOfWork.OrderHeader.UpdateStripePaymentID(shoppingCartVM.OrderHeader.Id,
                                                              session.Id,
                                                              session.PaymentIntentId);
                _unitOfWork.Save();

                Response.Headers.Add("Location", session.Url);
                return new StatusCodeResult(303);
            }
            else
            {
                return RedirectToAction(nameof(OrderConfirmation), "Cart", new { id = shoppingCartVM.OrderHeader.Id });
            }
		}

        [HttpGet]
        public IActionResult OrderConfirmation(int id)
        {
            var orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(o => o.Id == id);

            if(orderHeader.PaymentStatus != SD.PaymentStatusDelayedPayment)
            {
                // Checking Stripe Status
                var service = new SessionService();
                var session = service.Get(orderHeader.SessionId);

                if (session.PaymentStatus.ToLower() == "paid")
                {
                    _unitOfWork.OrderHeader.UpdateStatus(id,
                                                         SD.OrderStatusApproved,
                                                         paymentStatus: SD.PaymentStatusApproved);
                    _unitOfWork.Save();
                }
            }

            var shoppingCartsToDelete = _unitOfWork.ShoppingCart.GetAll(
                c => c.ApplicationUserId == orderHeader.ApplicationUserId).ToList();

            _unitOfWork.ShoppingCart.RemoveRange(shoppingCartsToDelete);
            _unitOfWork.Save();

            return View(id);
        }

        public IActionResult Add(int cartId)
        {
            var shoppingCart = _unitOfWork.ShoppingCart.GetFirstOrDefault(s => s.Id == cartId);
            _unitOfWork.ShoppingCart.IncrementCount(shoppingCart, 1);
            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Minus(int cartId)
        {
            var shoppingCart = _unitOfWork.ShoppingCart.GetFirstOrDefault(s => s.Id == cartId);
            
            if(shoppingCart.Count <= 1)
            {
                _unitOfWork.ShoppingCart.Remove(shoppingCart);
                return RedirectToAction(nameof(Index));
            }

            _unitOfWork.ShoppingCart.DecrementCount(shoppingCart, 1);
            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Remove(int cartId)
        {
            var shoppingCart = _unitOfWork.ShoppingCart.GetFirstOrDefault(s => s.Id == cartId);
            _unitOfWork.ShoppingCart.Remove(shoppingCart);
            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        private void CalculateOrderTotalPrice(ShoppingCartVM shoppingCartVM)
        {
            foreach (var shoppingCart in shoppingCartVM.ShoppingCarts)
            {
				shoppingCart.Price = GetPriceBasedOnQuantity(shoppingCart.Count,
															 shoppingCart.Product.Price,
															 shoppingCart.Product.Price50,
															 shoppingCart.Product.Price100);

				shoppingCartVM.OrderHeader.OrderTotal += (shoppingCart.Count * shoppingCart.Price);
			}
        }

        private double GetPriceBasedOnQuantity(double quantity, double price, double price50, double price100)
        {
            if (quantity <= 50) return price;
            else
            {
                if (quantity <= 100) return price50;
                else return price100;
            }
        }
    }
}
