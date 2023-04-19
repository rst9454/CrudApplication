using CrudApplication.Data;
using CrudApplication.Models.Stored_Proc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrudApplication.Controllers.Stored_Procedure
{
    public class StoredController : Controller
    {
        private readonly ApplicationContext context;

        public StoredController(ApplicationContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            var data = context.Wendors.FromSqlRaw("exec spGetAllWendorList").ToList();
            return View(data);
        }

        //Insert Wendor
        public IActionResult Insert_Wendor()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Insert_Wendor(Wendor model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //context.Database.ExecuteSqlRaw($"spInsert_Wendo '{model.Name}','{model.Gender}','{model.City}',{model.PinCode}");

                    //context.Database.ExecuteSqlRaw($"spInsert_Wendo @name='{model.Name}',@city='{model.City}',@pincode='{model.PinCode}',@gender='{model.Gender}'");

                    string parameter = $"spInsert_Wendo @name='{model.Name}',@city='{model.City}',@gender='{model.Gender}',@pincode='{model.PinCode}'";
                    context.Database.ExecuteSqlRaw(parameter);
                    TempData["Success"] = "Record is successfully inserted";
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", "ModelState is not Valid!");
            }
            catch(Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View();
        }

        public IActionResult Delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var data = context.Database.ExecuteSqlRaw($"spDelete_Wendor '{id}'");
            TempData["Success"] = "Record Successfully deleted!";
            return RedirectToAction("Index");
        }
        public IActionResult Edit(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            else
            {
                var data = context.Wendors.FromSqlRaw($"spGetWendorById '{id}'");
                Wendor w = new Wendor();
                foreach(var d in data)
                {
                    w.Id = d.Id;
                    w.Name = d.Name;
                    w.City = d.City;
                    w.Gender = d.Gender;
                    w.PinCode = d.PinCode;
                }
                return View("SaveOrUpdate", w);
            }
        }
        [HttpPost]
        public IActionResult Edit(Wendor model)
        {
            if (ModelState.IsValid)
            {
                string parameter = $"spUpdate_Wendor @id='{model.Id}', @name='{model.Name}',@city='{model.City}',@gender='{model.Gender}',@pincode='{model.PinCode}'";
                context.Database.ExecuteSqlRaw(parameter);
                TempData["Success"] = "Record is successfully Updated!";
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public IActionResult SaveOrUpdate()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SaveOrUpdate(Wendor model)
        {
            ModelState.Remove("Id");
            if (ModelState.IsValid)
            {
                string parameter = $"spSaveOrUpdate_Wendor @name='{model.Name}',@city='{model.City}',@gender='{model.Gender}',@pincode='{model.PinCode}',@id='{model.Id}'";
                context.Database.ExecuteSqlRaw(parameter);
                TempData["Success"] = "Record has been Saved!";
                return RedirectToAction("Index");
            }
            ModelState.AddModelError(string.Empty, "ModelState is not valid");
            return View(model);
        }
    }
}
