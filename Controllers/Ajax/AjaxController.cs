using CrudApplication.Data;
using CrudApplication.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrudApplication.Controllers.Ajax
{
    public class AjaxController : Controller
    {
        private readonly ApplicationContext context;

        public AjaxController(ApplicationContext context)
        {
            this.context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public JsonResult EmployeeList()
        {
            var data = context.Employees.ToList();
            return new JsonResult(data);
        }
        [HttpPost]
        public JsonResult AddEmployee(Employee employee)
        {
            var emp = new Employee()
            {
                Name=employee.Name,
                State=employee.State,
                City=employee.City,
                Salary=employee.Salary
            };
            context.Employees.Add(emp);
            context.SaveChanges();
            return new JsonResult("Data is Saved");
        }
        public JsonResult Delete(int id)
        {
            var data = context.Employees.Where(e => e.Id == id).SingleOrDefault();
            context.Employees.Remove(data);
            context.SaveChanges();
            return new JsonResult("Data Deleted");
        }

        public JsonResult Edit(int id)
        {
            var data = context.Employees.Where(e => e.Id == id).SingleOrDefault();
            return new JsonResult(data);
        }
        [HttpPost]
        public JsonResult Update(Employee employee)
        {
            context.Employees.Update(employee);
            context.SaveChanges();
            return new JsonResult("Record Updated!");
        }
    }
}
