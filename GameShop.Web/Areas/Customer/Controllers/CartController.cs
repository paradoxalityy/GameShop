using GameShop.Data.Repository.IRepository;
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
                    c => c.ApplicationUserId == claim.Value, includeProperties: "Product")
            };

            foreach (var shoppingCart in shoppingCartVM.ShoppingCarts)
            {
                shoppingCart.Price = GetPriceBasedOnQuantity(shoppingCart.Count,
                                                             shoppingCart.Product.Price,
                                                             shoppingCart.Product.Price50,
                                                             shoppingCart.Product.Price100);
            }

            return View(shoppingCartVM);
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
