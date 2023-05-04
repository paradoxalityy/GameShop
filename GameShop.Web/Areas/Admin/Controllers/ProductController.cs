using GameShop.Data.Repository.IRepository;
using GameShop.Models;
using GameShop.Models.ViewModels;
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
            ProductVM productVM = new ProductVM
            {
                Product = new Product(),
                CategoryList = _unitOfWork.Category.GetAll().Select(
                    c => new SelectListItem
                    {
                        Text = c.Name,
                        Value = c.Id.ToString()
                    }),
                PlatformList = _unitOfWork.Platform.GetAll().Select(
                    p => new SelectListItem
                    {
                        Text = p.Name,
                        Value = p.Id.ToString()
                    })
            };     

            if(id == null || id == 0)
            {
                return View(productVM);
            }
            else
            {
                // Updating existing product
            }

            return View(productVM);
        }
    }
}
