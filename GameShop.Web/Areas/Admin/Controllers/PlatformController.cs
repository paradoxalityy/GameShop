using GameShop.Data.Repository.IRepository;
using GameShop.Models;
using Microsoft.AspNetCore.Mvc;

namespace GameShop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PlatformController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public PlatformController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Platform> platforms = _unitOfWork.Platform.GetAll();
            return View(platforms);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();  
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Platform platform)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Platform.Add(platform);
                _unitOfWork.Save();
                TempData["Success"] = "Platform created successfully.";
                return RedirectToAction("Index");
            }

            return View(platform);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var category = _unitOfWork.Platform.GetFirstOrDefault(p => p.Id == id);
            if(category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Platform platform)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Platform.Update(platform);
                _unitOfWork.Save();
                TempData["Success"] = "Platform Edited Successfully.";
                return RedirectToAction("Index");
            }

            return View(platform);
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }

            var platformToDelete = _unitOfWork.Platform.GetFirstOrDefault(p => p.Id == id);
            if(platformToDelete == null)
            {
                return NotFound();
            }

            return View(platformToDelete);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }

            var platformToDelete = _unitOfWork.Platform.GetFirstOrDefault(p => p.Id == id);
            if(platformToDelete == null)
            {
                return NotFound();
            }

            _unitOfWork.Platform.Remove(platformToDelete);
            _unitOfWork.Save();

            TempData["Success"] = "Platform Deleted Successfully.";
            return RedirectToAction("Index");
        }
    }
}
