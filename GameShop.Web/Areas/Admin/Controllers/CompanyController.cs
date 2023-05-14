using GameShop.Data.Repository.IRepository;
using GameShop.Models;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Imaging;
using System.Reflection.Metadata.Ecma335;

namespace GameShop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Upsert(int? id)
        {
            if(id == null || id == 0)
            {
                // Creating new Company
                var company = new Company();
                return View(company);
            }
            else
            {
                // Updating existing company
                var companyToUpdate = _unitOfWork.Company.GetFirstOrDefault(c => c.Id == id);
                return View(companyToUpdate);
            }
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var companies = _unitOfWork.Company.GetAll();
            return Json(new { data = companies });
        }
        #endregion API CALLS
    }
}
