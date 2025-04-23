using ecom.DAO;
using ecom.Models;
using ecom.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ecom.Controllers
{
    public class CategoryController : BaseController
    {
        public ApplicationDbContext _db;
        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult CategoryDetail(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = _db.Category.Where(x => x.CategoryId == id).ToList();
            if (category == null)
            {
                return NotFound();
            }

            var productitem = _db.ProductItem.Where(x => x.CategoryId == id).ToList();

            var viewmodel = new DashboardVM {
                CategoryInfo = category,
                ProductItems = productitem,
            };

            return View(viewmodel);
        }
        
        public IActionResult Index()
        {

            // Only show category page if the user is an admin
            if (!IsAdmin)
            {
                return RedirectToAction("AdminAccess", "Admin");
            }

            var datas = _db.Category.ToList();
            return View(datas);
        }
        

        public JsonResult Save(int hiddenId, string CategoryName, string CategoryCode)
        {
            if (hiddenId == 0) { 
                Category category = new Category();
                category.CategoryName = CategoryName;
                category.CategoryCode = CategoryCode;
                category.CreatedAt = System.DateTime.Now;

                _db.Add(category);
                _db.SaveChanges();

                return Json(new
                {
                    success = true,
                    message = "Category has been saved successfully"
                });
            }
            else
            {
                var dbData = _db.Category.Where(x => x.CategoryId == hiddenId).FirstOrDefault();
                if (dbData == null)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Category not found"
                    });
                }
                else
                {
                    dbData.CategoryName = CategoryName;
                    dbData.CategoryCode = CategoryCode;

                    _db.Update(dbData);
                    _db.SaveChanges();

                    return Json(new
                    {
                        success = true,
                        message = "Category has been updated successfully"
                    });
                }
            
        }
        }
        
        public JsonResult Edit(int id)
        {
            var dbData = _db.Category.Where(x => x.CategoryId == id).FirstOrDefault();

            if (dbData == null)
            {
                return Json(new
                {
                    Success = false,
                    Message = "Category not found"
                });
            }
            else
            {
                return Json(new
                {
                    Success = true,
                    Data = dbData
                });
            }
        }

        //Delete JsonResult

        public JsonResult Delete(int? id)
        {
            Category category = new Category();

            var dbData = _db.Category.Where(x => x.CategoryId == id).FirstOrDefault();

            if(dbData == null)
            {
                return Json(new
                {
                    Success = false,
                    Message = "Category not found"
                });
            }
            else
            {
                _db.Category.Remove(dbData);
                _db.SaveChanges();

                return Json(new
                {
                    Success = true,
                    Message = "Category deleted"
                });
            }

        }
    }
}

