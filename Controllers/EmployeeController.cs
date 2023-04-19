using CrudApplication.Data;
using CrudApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrudApplication.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly ApplicationContext context;

        public EmployeeController(ApplicationContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            var result = context.Employees.ToList();
            return View(result);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        public IActionResult Create(Employee model)
        {
            if (ModelState.IsValid)
            {
                var emp = new Employee()
                {
                    Name = model.Name,
                    City = model.City,
                    State = model.State,
                    Salary = model.Salary
                };
                context.Employees.Add(emp);
                context.SaveChanges();
                TempData["error"] = "Record Saved!";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["message"] = "Empty field Can't Submit";
                return View(model);
            }

        }
        public IActionResult Delete(int id)
        {
            var emp = context.Employees.SingleOrDefault(e => e.Id == id);
            context.Employees.Remove(emp);
            context.SaveChanges();
            TempData["error"] = "Record Deleted";
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var emp = context.Employees.SingleOrDefault(e => e.Id == id);
            var result = new Employee()
            {
                Name = emp.Name,
                City = emp.City,
                State = emp.State,
                Salary = emp.Salary
            };
            return View(result);
        }
        [HttpPost]
        public IActionResult Edit(Employee model)
        {
            var emp = new Employee()
            {
                Id = model.Id,
                Name = model.Name,
                City = model.City,
                State = model.State,
                Salary = model.Salary
            };
            context.Employees.Update(emp);
            context.SaveChanges();
            TempData["error"] = "Record Updated";
            return RedirectToAction("Index");
        }

    }
}
