using Company.Data.Models;
using Company.Services.Interfaces;
using Company.Services.Services;
using Microsoft.AspNetCore.Mvc;

namespace MVC_Company.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeServices _employeeServices;

        public EmployeeController(IEmployeeServices employeeServices)
        {
            _employeeServices = employeeServices;
        }
        [HttpGet]
        public IActionResult Index(string SearchInp)
        {
            if (string.IsNullOrEmpty(SearchInp))
            {
                var emp = _employeeServices.GetAll();
                return View(emp);
            }
            else 
            {
                var emp = _employeeServices.GetByName(SearchInp);
                return View(emp);
            }

        }
        public IActionResult Create()
        {
            return View();

        }
        [HttpPost]
        public IActionResult Create(Employee employee)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _employeeServices.Add(employee);
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("Error", "employee Error");
                return View(employee);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("error", ex.Message);
                return View(employee);
            }
        }
    }
}
