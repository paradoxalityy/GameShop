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
    }
}
