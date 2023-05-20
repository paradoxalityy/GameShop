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

        [HttpPost]
        public IActionResult Upsert(Company obj)
        {
            if (ModelState.IsValid)
            {
                if (obj.Id == 0)
                {
                    // Creating new Company
                    _unitOfWork.Company.Add(obj);
                    TempData["Success"] = "Company created successfully";
                }
                else
                {
                    // Updating existing company
                    _unitOfWork.Company.Update(obj);
                    TempData["Success"] = "Company updated successfully";
                }
                
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }

            return View(obj);
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
