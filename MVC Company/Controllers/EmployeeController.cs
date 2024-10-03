﻿using Company.Data.Models;
using Company.Services.EmployeeDtos;
using Company.Services.Helper;
using Company.Services.Interfaces;
using Company.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MVC_Company.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeServices _employeeServices;
        private readonly IDepartmentServices _departmentServices;

        public EmployeeController(IEmployeeServices employeeServices , IDepartmentServices departmentServices)
        {
            _employeeServices = employeeServices;
            _departmentServices = departmentServices;
        }
        [HttpGet]
        public IActionResult Index(string SearchInp)
        {
            if (string.IsNullOrEmpty(SearchInp))
            {
                IEnumerable<EmployeeDto> employees;
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
            ViewBag.Departments = _departmentServices.GetAll();
            return View();
        }
        [HttpPost]
        public IActionResult Create(EmployeeDto employee)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    employee.ImgUrl = DocumentSettings.UploadFile(employee.Image, "Images");
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
