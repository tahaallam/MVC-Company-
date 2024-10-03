using Company.Data.Models;
using Company.Repository.Interfaces;
using Company.Services.DeptDto;
using Company.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MVC_Company.Controllers
{
    [Authorize]
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
        public IActionResult Create(DepartmentDto department )
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
        public IActionResult Details(int? id , string ViewName = "Details")
        {
            
            if (id is null)
            {
                return BadRequest();
            }
            var department = _departmentServices.GetById(id.Value);

            if (department == null )
            {
                return NotFound();
            }
            return View(ViewName,department);
            
        }
        [HttpGet]
        public IActionResult Update(int? id)
        {
            return Details(id, "Update");
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Update(DepartmentDto department ,[FromRoute] int? id)
        {
            if (id != department.Id)
            {
                return BadRequest();
            }
            if (department is null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _departmentServices.Update(department);
                return RedirectToAction(nameof(Index));
            }
            return View(department);
            
        }
        public IActionResult Delete(int? id) 
        {
            var dept = _departmentServices.GetById(id);
            if (dept is null)
            {
                return NotFound();
            }
            _departmentServices.Delete(dept);
            return RedirectToAction(nameof(Index));
        }
        //[HttpGet]
        //public IActionResult Delete(int? id)
        //{
            
        //    return Details(id, "Delete"); 
        //}
        //[HttpPost]
        //public IActionResult Delete(Department department , int?id)
        //{
        //    if (id != department.Id)
        //    {
        //        return BadRequest();
        //    }
        //    if (department is null) { return NotFound(); }
        //    _departmentServices.Delete(department);
        //    return RedirectToAction(nameof(Index)); 
        //}
    }
    }
