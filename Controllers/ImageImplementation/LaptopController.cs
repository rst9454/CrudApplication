using CrudApplication.Data;
using CrudApplication.Models.ImageImplementation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CrudApplication.Controllers.ImageImplementation
{
    public class LaptopController : Controller
    {
        private readonly ApplicationContext context;
        private readonly IWebHostEnvironment environment;

        public LaptopController(ApplicationContext context, IWebHostEnvironment environment)
        {
            this.context = context;
            this.environment = environment;
        }
        public IActionResult Index()
        {
            var data = context.Laptops.ToList();
            return View(data);
        }

        public IActionResult AddLaptop()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddLaptop(Laptop model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string uniqueFileName = UploadImage(model);
                    var data = new Laptop()
                    {
                        Brand = model.Brand,
                        Description = model.Description,
                        Path = uniqueFileName
                    };
                    context.Laptops.Add(data);
                    context.SaveChanges();
                    TempData["Success"] = "Record Successfully saved!";
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError(string.Empty, "Model property is not valid, please check");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View(model);
        }

        private string UploadImage(Laptop model)
        {
            string uniqueFileName = string.Empty;
            if (model.ImagePath != null)
            {
                string uploadFolder = Path.Combine(environment.WebRootPath, "Content/Laptop/");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ImagePath.FileName;
                string filePath = Path.Combine(uploadFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.ImagePath.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

        public IActionResult Delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            else
            {
                var data = context.Laptops.Where(e => e.Id == id).SingleOrDefault();
                if (data != null)
                {
                    string deleteFromFolder = Path.Combine(environment.WebRootPath, "Content/Laptop/");
                    string currentImage = Path.Combine(Directory.GetCurrentDirectory(), deleteFromFolder, data.Path);
                    if (currentImage != null)
                    {
                        if (System.IO.File.Exists(currentImage))
                        {
                            System.IO.File.Delete(currentImage);
                        }
                    }
                    context.Laptops.Remove(data);
                    context.SaveChanges();
                    TempData["Success"] = "Record Deleted!";
                }
            }
            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var data = context.Laptops.Where(e => e.Id == id).SingleOrDefault();
            return View(data);
        }

        public IActionResult Edit(int id)
        {
            var data = context.Laptops.Where(e => e.Id == id).SingleOrDefault();
            if (data != null)
            {
                return View(data);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public IActionResult Edit(Laptop model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var data = context.Laptops.Where(e => e.Id == model.Id).SingleOrDefault();
                    string uniqueFileName = string.Empty;
                    if (model.ImagePath != null)
                    {
                        if (data.Path != null)
                        {
                            string filepath = Path.Combine(environment.WebRootPath, "Content/Laptop", data.Path);
                            if (System.IO.File.Exists(filepath))
                            {
                                System.IO.File.Delete(filepath);
                            }
                        }
                        uniqueFileName = UploadImage(model);
                    }
                    data.Brand = model.Brand;
                    data.Description = model.Description;
                    if (model.ImagePath != null)
                    {
                        data.Path = uniqueFileName;
                    }
                    context.Laptops.Update(data);
                    context.SaveChanges();
                    TempData["Success"] = "Record Updated Successfully!";
                }
                else
                {
                    return View(model);
                }

            }
            catch(Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return RedirectToAction("index");
        }
    }
}