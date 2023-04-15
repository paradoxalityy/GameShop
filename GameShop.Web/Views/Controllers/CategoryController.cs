using GameShop.Web.Data;
using GameShop.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace GameShop.Web.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<Category> gameCategories = _db.Categories.ToList();
            return View(gameCategories);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if(category.Name == category.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Name", "The Name cannot exactly match the Display Order.");
            }

            // Server side validation
            if (ModelState.IsValid)
            {
                _db.Categories.Add(category);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(category);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }

            var categoryToEdit = _db.Categories.FirstOrDefault(c => c.Id == id);

            if(categoryToEdit == null)
            {
                return NotFound();
            }

            return View(categoryToEdit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
            if(category.Name == category.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Name", "The Name cannot exactly match the Display Order.");
            }

            if (ModelState.IsValid)
            {
                _db.Categories.Update(category);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(category);
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }

            var categoryToDelete = _db.Categories.FirstOrDefault(c => c.Id == id);
            
            if(categoryToDelete == null)
            {
                return NotFound();
            }

            return View(categoryToDelete);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }

            var categoryToDelete = _db.Categories.FirstOrDefault(c => c.Id == id);

            if(categoryToDelete == null)
            {
                return NotFound();
            }

            _db.Categories.Remove(categoryToDelete);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
