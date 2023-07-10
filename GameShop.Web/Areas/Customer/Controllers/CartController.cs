﻿using GameShop.Data.Repository.IRepository;
using GameShop.Models;
using GameShop.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GameShop.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
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

            foreach (var shoppingCart in shoppingCartVM.ShoppingCarts)
            {
                shoppingCart.Price = GetPriceBasedOnQuantity(shoppingCart.Count,
                                                             shoppingCart.Product.Price,
                                                             shoppingCart.Product.Price50,
                                                             shoppingCart.Product.Price100);

                shoppingCartVM.OrderHeader.OrderTotal += (shoppingCart.Price * shoppingCart.Count);
            }

            return View(shoppingCartVM);
        }

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

            foreach (var shoppingCart in shoppingCartVM.ShoppingCarts)
            {
                shoppingCart.Price = GetPriceBasedOnQuantity(shoppingCart.Count,
                                                             shoppingCart.Price,
                                                             shoppingCart.Product.Price50,
                                                             shoppingCart.Product.Price100);

                shoppingCartVM.OrderHeader.OrderTotal += shoppingCart.Price;
			}

            return View(shoppingCartVM);
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
