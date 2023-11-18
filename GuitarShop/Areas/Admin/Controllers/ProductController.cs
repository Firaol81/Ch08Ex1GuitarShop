using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GuitarShop.Models;

namespace GuitarShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private ShopContext context;
        private List<Category> categories;

        public ProductController(ShopContext ctx)
        {
            context = ctx;
            categories = context.Categories
                    .OrderBy(c => c.CategoryID)
                    .ToList();
        }

        public IActionResult Index()
        {
            return RedirectToAction("List");
        }

        [Route("[area]/[controller]s/{id?}")]
        public IActionResult Update(Product product)
        {
            if (ModelState.IsValid)
            {
                if (product.ProductID == 0) // New product
                {
                    context.Products.Add(product);
                    context.SaveChanges();

                    TempData["userMessage"] = $"You just added the product {product.Name}";

                }
                else // Existing product
                {
                    // Update logic here...
                    context.Products.Update(product);
                    context.SaveChanges();

                    TempData["userMessage"] = $"You just updated the product {product.Name}";

                }

                return RedirectToAction("List");
            }
            else // Invalid ModelState
            {
                ViewBag.Action = "Save";
                ViewBag.Categories = categories;
                return View("AddUpdate", product);
            }
        }


        [HttpGet]
        public IActionResult Add()
        {
            // create new Product object
            Product product = new Product();                // create Product object
            product.Category = context.Categories.Find(1);  // add Category object - prevents validation problem

            // use ViewBag to pass action and category data to view
            ViewBag.Action = "Add";
            ViewBag.Categories = categories;

            // bind product to AddUpdate view
            return View("AddUpdate", product);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            // get Product object for specified primary key
            Product product = context.Products
                .Include(p => p.Category)
                .FirstOrDefault(p => p.ProductID == id);

            // use ViewBag to pass action and category data to view
            ViewBag.Action = "Update";
            ViewBag.Categories = categories;

            // bind product to AddUpdate view
            return View("AddUpdate", product);
        }

        [HttpPost]
        public IActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                if (product.ProductID == 0) // New product
                {
                    context.Products.Add(product);
                    context.SaveChanges();

                    TempData["userMessage"] = $"You just added the product {product.Name}";
                }
                else // Existing product
                {
                    // Update logic here...
                    context.Products.Update(product);
                    context.SaveChanges();

                    TempData["userMessage"] = $"You just updated the product {product.Name}";
                }

                return RedirectToAction("List");
            }
            else // Invalid ModelState
            {
                ViewBag.Action = "Save";
                ViewBag.Categories = categories;
                return View("AddUpdate", product);
            }
        }


        [HttpGet]
        public IActionResult Delete(int id)
        {
            Product product = context.Products
                .FirstOrDefault(p => p.ProductID == id);
            return View(product);
        }

        [HttpPost]
        public IActionResult Delete(Product product)
        {
            context.Products.Remove(product);
            context.SaveChanges();
            return RedirectToAction("List");
        }
    }
}
