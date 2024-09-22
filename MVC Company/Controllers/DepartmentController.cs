using Company.Data.Models;
using Company.Repository.Interfaces;
using Company.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MVC_Company.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentServices _departmentServices;

        public DepartmentController(IDepartmentServices departmentServices)
        {
            _departmentServices = departmentServices;
        }
        public IActionResult Index()
        {
            var department = _departmentServices.GetAll();
            return View(department);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();

        }
        [HttpPost]
        public IActionResult Create(Department department )
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _departmentServices.Add(department);
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("Error", "Department Error");
                return View(department);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("error", ex.Message);
                return View(department);
            }
        }
    }
    }
