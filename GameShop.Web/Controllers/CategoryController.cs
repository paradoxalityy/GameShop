using GameShop.Web.Data;
using GameShop.Web.Models;
using Microsoft.AspNetCore.Mvc;

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

        
    }
}
