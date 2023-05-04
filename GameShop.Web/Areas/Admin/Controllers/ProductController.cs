using GameShop.Data.Repository.IRepository;
using GameShop.Models;
using GameShop.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Runtime.Remoting;

namespace GameShop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM obj, IFormFile? formFile)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (formFile != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"images\products");
                    var fileExtension = Path.GetExtension(formFile.FileName);

                    using(var fileStream = new FileStream(Path.Combine(uploads, fileName + fileExtension), FileMode.Create))
                    {
                        formFile.CopyTo(fileStream);
                    }

                    obj.Product.ImageUrl = @"images\products" + fileName + fileExtension;
                }

                _unitOfWork.Product.Add(obj.Product);
                _unitOfWork.Save();
                TempData["Success"] = "Product created successfully.";
                return RedirectToAction("Index");
            }

            return View(obj);
        }
    }
}
