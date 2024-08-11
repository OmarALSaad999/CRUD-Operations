using Market.Models;
using MARKET.DataAccess.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;
using System;
namespace Market.Controllers
{
    public class CategoryController : Controller
    {

        private readonly ApplicationDbContext _db;

        private readonly IWebHostEnvironment _webHostEnvironment;




        public CategoryController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;

        }
        public IActionResult Index()
        {
            List<Category> categorysList = _db.Categories.ToList();
            return View(categorysList);
        }

        /*================ Create ==============*/
        public IActionResult Create()
        {

            return View();
        }
        [HttpPost]
        public IActionResult Create(Category obj, IFormFile? file)
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            if (file != null)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string myPath = Path.Combine(wwwRootPath, "images");

                using (var fileStream = new FileStream(Path.Combine(myPath, fileName), FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
                obj.ImageUrl = "/images/" + fileName;
            }

            if (ModelState.IsValid)
            {
                _db.Categories.Add(obj);
                _db.SaveChanges();
                TempData["success"] = "Category added successfully";
                return RedirectToAction("Index", "Category");
            }
            return View();
        }


        /*================= Edit ==============*/

        public IActionResult Edit(int? id)
        {
            if (id == null || id <= 0)
            {
                return NotFound();
            }
            Category CategoryToUpdate = _db.Categories.FirstOrDefault(c => c.Id == id);

            if (CategoryToUpdate == null)
            {
                return NotFound();
            }
            return View(CategoryToUpdate);
        }

        [HttpPost]
public IActionResult Edit(Category obj, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                var category = _db.Categories.Find(obj.Id);
                if (category == null)
                {
                    return NotFound();
                }

                // Update category properties
                category.Name = obj.Name;
                category.DisplayOrder = obj.DisplayOrder;

                if (file != null)
                {
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string myPath = Path.Combine(wwwRootPath, "images");

                    // Delete the old image if it exists
                    if (!string.IsNullOrEmpty(category.ImageUrl))
                    {
                        string oldImagePath = Path.Combine(wwwRootPath, category.ImageUrl.TrimStart('/'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStream = new FileStream(Path.Combine(myPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    category.ImageUrl = "/images/" + fileName;
                }

                _db.Categories.Update(category);
                _db.SaveChanges();
                TempData["success"] = "Category updated successfully";
                return RedirectToAction("Index", "Category");
            }
            return View(obj);
        }


        /*===========Delete==========*/

        public IActionResult Delete(int? id)
        {
            if (id == null || id <= 0)
            {
                return NotFound();
            }
            Category CategoryToDelete = _db.Categories.FirstOrDefault(c => c.Id == id);

            if (CategoryToDelete == null)
            {
                return NotFound();
            }
            return View(CategoryToDelete);
        }



        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            Category CategoryToDelete = _db.Categories.FirstOrDefault(u => u.Id == id);
            // Server Side validation
            if (CategoryToDelete == null)
            {

                return NotFound();

            }

            _db.Categories.Remove(CategoryToDelete);
            _db.SaveChanges();
            TempData["success"] = "Category Deletey Successfuly";
            return RedirectToAction("Index", "Category");


        }

    }
}