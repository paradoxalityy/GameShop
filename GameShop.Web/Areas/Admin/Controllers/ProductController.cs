using GameShop.Data.Repository.IRepository;
using GameShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GameShop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> products = _unitOfWork.Product.GetAll();
            return View(products);
        }

        [HttpGet]
        public IActionResult Upsert(int? id)
        {
            Product product = new Product();

            IEnumerable<SelectListItem> productCategories = _unitOfWork.Category.GetAll().Select(
                c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                });
            IEnumerable<SelectListItem> productPlatforms = _unitOfWork.Platform.GetAll().Select(
                p => new SelectListItem
                {
                    Text = p.Name,
                    Value = p.Id.ToString()
                });

            if(id == null || id == 0)
            {
                // Using two ways of passing data to the view
                ViewBag.productCategories = productCategories;
                ViewData["productPlatforms"] = productPlatforms;
                return View(product);
            }
            else
            {
                // Updating existing product
            }

            return View(product);
        }
    }
}
