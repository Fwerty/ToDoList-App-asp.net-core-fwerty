using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection.Metadata.Ecma335;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.Extensions;
using myAppToDoList.Areas.Identity.Data;





namespace myAppToDoList.Controllers
{
    public class ProductsController : Controller
    {

        private DBContextSample _context;


        public ProductsController(DBContextSample context)
        {
            _context = context;

        }

        public IActionResult Index()
        {

            var products = _context.Products.ToList();
            return View(products);
        }

        public IActionResult Remove(string id)
        {
            var product = _context.Products.Find(id);
            _context.Products.Remove(product);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }



        [HttpGet]
        public IActionResult Add()
        {

            ViewBag.Expire = new List<string>() { "1 ay", "3 ay", "6 ay", "12 ay" };


            ViewBag.Expire2 = new Dictionary<string, int>()
            {
                { "1 ay",1},
                { "3 ay",3},
                { "6 ay",6},
                { "12 ay",12}

            };



            ViewBag.ColorSelect = new SelectList(new List<ColorSelectList>()
            {
                new(){Data="Mavi",Value="Mavi"},
                new(){Data="Kırmızı",Value="Kırmızı"},
                new(){Data="Sarı",Value="Sarı"},
                new(){Data="Yeşil",Value="Yeşil"}
            }, "Value", "Data");


            return View();
        }



        [HttpPost]
        public IActionResult SaveProduct(Product newProduct, UrunEkle p)
        {
            if (ModelState.IsValid)
            {
                if (p.url != null)
                {
                    var extension = Path.GetExtension(p.url.FileName);
                    var newimagename = Guid.NewGuid() + extension;
                    var location = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/resimler/", newimagename);

                    using (var stream = new FileStream(location, FileMode.Create))
                    {
                        p.url.CopyTo(stream);
                    }

                    newProduct.url = newimagename;
                }

                
                _context.Products.Add(newProduct);
                _context.SaveChanges();
                TempData["status"] = "\nContent updated successfully.";
            }
            else
            {
                TempData["status"] = "Invalid model. The transaction could not be performed.";
            }

            return RedirectToAction("Index");
        }




        [HttpGet]
        public IActionResult Update(string id)
        {
            var product = _context.Products.Find(id);




            return View(product);
        }


        [HttpPost]
        public IActionResult Update(Product updatedProduct, IFormFile url)
        {
            if (updatedProduct != null)
            {
                var existingProduct = _context.Products.Find(updatedProduct.Id);

                if (existingProduct != null)
                {
                    if (url != null)
                    {
                        var extension = Path.GetExtension(url.FileName);
                        var newImageName = Guid.NewGuid() + extension;
                        var location = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/resimler/", newImageName);

                        using (var stream = new FileStream(location, FileMode.Create))
                        {
                            url.CopyTo(stream);
                        }

                        existingProduct.url = newImageName;
                    }

                    existingProduct.Name = updatedProduct.Name;
                    existingProduct.Description = updatedProduct.Description;

                    _context.Products.Update(existingProduct);
                    _context.SaveChanges();

                    TempData["status"] = "Content updated successfully.";
                }
                else
                {
                    TempData["status"] = "Content not found";
                }
            }
            else
            {
                TempData["status"] = "Geçersiz model. Güncelleme işlemi yapılamadı.";
            }

            return RedirectToAction("Index");
        }






        public IActionResult WishListPage()
        {

            

            var products = _context.Products.ToList();
            return View(products);
        }

        public IActionResult MyAccount()
        {


            return View();
        }

    }
}
