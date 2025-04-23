using ecom.DAO;
using ecom.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace ecom.Controllers
{
    public class SingleCategoryController : Controller
    {
        private readonly ApplicationDbContext _db;

        public SingleCategoryController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult CategoryDetails(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var category = _db.Category.FirstOrDefault(c => c.CategoryId == id);
            if (category == null)
            {
                return NotFound();
            }

            var products = _db.ProductItem.Where(p => p.CategoryId == id).ToList();

            var viewModel = new SingleCategoryVM
            {
                CategoryInfo = category,
                ProductItems = products
            };

            return View(viewModel);
        }
    }
}
