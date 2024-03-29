﻿using GameShop.Data.Repository.IRepository;
using GameShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace GameShop.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> avaiableProducts = _unitOfWork.Product.GetAll(includeProperties: "Category,Platform");
            return View(avaiableProducts);
        }

        [HttpGet]
        public IActionResult Details(int productId)
        {
            ShoppingCart shoppingCart = new ShoppingCart
            {
                Product = _unitOfWork.Product.GetFirstOrDefault(p => p.Id == productId, includeProperties: "Category,Platform"),
                ProductId = productId,
                Count = 1
            };

            return View(shoppingCart);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            shoppingCart.ApplicationUserId = claim.Value;

            var shoppingCartFromDb = _unitOfWork.ShoppingCart.GetFirstOrDefault(
                c => c.ApplicationUserId == claim.Value && c.ProductId == shoppingCart.ProductId);

            if (shoppingCartFromDb != null)
            {
                _unitOfWork.ShoppingCart.IncrementCount(shoppingCart, shoppingCart.Count);
            }
            else
            {
                _unitOfWork.ShoppingCart.Add(shoppingCart);
            }

            _unitOfWork.Save();
            return RedirectToAction("Index");
        } 

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}