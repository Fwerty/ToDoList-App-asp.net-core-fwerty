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
        //private readonly ProductRepository _productRepository;




        public ProductsController(DBContextSample context)
        {
            //_productRepository = new ProductRepository();

            _context = context;


            // formlara geçmeden önce kullanıcıdan veri alamadığımız için biz default veriler olusturmustuk fakat 
            // artık gerek kalmadı çünkü biz kednimiz formlar ile veri girebiliyoruz
            //if (!_context.Products.Any())
            //{
            //    // decimal veri kullanıyosan sayıdan sonra m harfi de yazmalısın
            //    _context.Products.Add(new Product { Name = "product1", Price = 12.96m, Stock = 3, Color = "red" });
            //    _context.Products.Add(new Product { Name = "product2", Price = 32, Stock = 7, Color = "blue" });
            //    _context.Products.Add(new Product { Name = "product3", Price = 13, Stock = 4 });


            //    _context.SaveChanges();
            //}


        }

        public IActionResult Index()
        // FromServices özelliği dependency injection patterni olan DI kullanılacağını haber vermektedir
        // yani dependency injection ifadesini parametre olarak kullancak olursak bunu kullanmak zorundayız
        {

            // burada singleton ozelliği ile dependency injection yapıyoruz , singleton ozelliği 
            // yalnızca bir nesne uretılmesını saglar


            var products = _context.Products.ToList();
            return View(products);
        }

        public IActionResult Remove(int id)
        {
            var product = _context.Products.Find(id);
            _context.Products.Remove(product);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Add()
        {
            // expire(sitede durma süresi) 2 farklı yoldan gosterildi asagıda
            ViewBag.Expire = new List<string>() { "1 ay", "3 ay", "6 ay", "12 ay" };


            ViewBag.Expire2 = new Dictionary<string, int>()
            {
                { "1 ay",1},
                { "3 ay",3},
                { "6 ay",6},
                { "12 ay",12}

            };



            // SelectList özel bir sınıftır , ben olusturmadım .
            ViewBag.ColorSelect = new SelectList(new List<ColorSelectList>()
            {
                new(){Data="Mavi",Value="Mavi"},
                new(){Data="Kırmızı",Value="Kırmızı"},
                new(){Data="Sarı",Value="Sarı"},
                new(){Data="Yeşil",Value="Yeşil"}
            }, "Value", "Data");


            return View();
        }

        //kullanıcıdan veri alma yolları

        // read me : 1.ve 2. yöntem için add kısmında farklı bir view kullanıyoruz ve o view i siliyorum 
        // çünkü 3. yöntemden (sınıf ile veri alma) devam ediyorum


        //1.yöntem(var değişkeni tanımlayarak)
        //[HttpPost]
        //public IActionResult SaveProduct()
        //{

        //    var name = HttpContext.Request.Form["Name"].ToString();
        //    var price = decimal.Parse(HttpContext.Request.Form["Price"].ToString());
        //    var stock = int.Parse(HttpContext.Request.Form["Stock"].ToString());
        //    var color = HttpContext.Request.Form["Color"].ToString();

        //    Product newProduct = new Product() { Name = name, Price = price, Stock = stock, Color = color };
        //    _context.Products.Add(newProduct);
        //    _context.SaveChanges();

        //    return RedirectToAction("Index");
        //}


        // 2.yöntem (parametre kullanarak kullanıcıdan veri alma)  
        //[HttpPost]
        //public IActionResult SaveProduct(string name , decimal price , int stock , string color)
        //{
        //    Product newProduct = new Product() { Name = name, Price = price, Stock = stock, Color = color };
        //    _context.Products.Add(newProduct);
        //    _context.SaveChanges();

        //    return RedirectToAction("Index");
        //}



        // 3.yöntem (sınıf aracılığı ile kullanıcıdan veri alma)  











        ////2
        // RESİM KIRPMA İŞLEMİ OLMADAN ÇALIAN KOD : 
        //[HttpPost]
        //public IActionResult SaveProduct(Product newProduct, UrunEkle p)
        //{
        //    if (p.url != null)
        //    {
        //        var extension = Path.GetExtension(p.url.FileName);
        //        var newimagename = Guid.NewGuid() + extension;
        //        var location = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/resimler/", newimagename);
        //        var stream = new FileStream(location, FileMode.Create);
        //        p.url.CopyTo(stream);
        //        newProduct.url = newimagename;
        //    }
        //    else
        //    {
        //        // Eğer p.url null ise, newProduct.url'a bir değer atamak gerekir.
        //        // Örneğin:
        //        // newProduct.url = "default_image.jpg"; // Veya null'a izin veriliyorsa newProduct.url = null;
        //    }

        //    _context.Products.Add(newProduct);
        //    _context.SaveChanges();

        //    TempData["status"] = "\nÜrün başarıyla eklendi";

        //    return RedirectToAction("Index");
        //}


        ////1
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

                if (newProduct.Id == 0)
                {
                    _context.Products.Add(newProduct);
                }
                else
                {
                    var existingProduct = _context.Products.Find(newProduct.Id);
                    if (existingProduct != null)
                    {
                        existingProduct.Name = newProduct.Name;
                        // Diğer özellikleri de güncellemek gerekiyorsa burada güncellenmeli.
                        // existingProduct.Price = newProduct.Price;
                        // existingProduct.Stock = newProduct.Stock;
                        // existingProduct.Color = newProduct.Color;
                        // ...

                        // Dosya yükleme işlemi başarısız olsa bile URL'i güncellemek gerekiyor.
                        existingProduct.url = newProduct.url;

                        _context.Products.Update(existingProduct);
                    }
                    else
                    {
                        TempData["status"] = "Ürün bulunamadı";
                        return RedirectToAction("Index");
                    }
                }

                _context.SaveChanges();
                TempData["status"] = "\nÜrün başarıyla eklendi";
            }
            else
            {
                TempData["status"] = "Geçersiz model. İşlem yapılamadı.";
            }

            return RedirectToAction("Index");
        }



        //[HttpPost]
        //public IActionResult SaveProduct(UrunEkle p)
        //{
        //    Product k = new Product();
        //    if(p.url!=null)
        //    {
        //        var extension = Path.GetExtension(p.url.FileName);
        //        var newimagename = Guid.NewGuid() + extension;
        //        var location = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot/resimler/",newimagename);
        //        var stream = new FileStream(location, FileMode.Create);
        //        p.url.CopyTo(stream);
        //        k.url = newimagename;


        //    }
        //    return RedirectToAction("Index");
        //}














        [HttpGet]
        public IActionResult Update(int id)
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
                    // Check if a new image file is provided
                    if (url != null)
                    {
                        // Handle the new image file upload
                        var extension = Path.GetExtension(url.FileName);
                        var newImageName = Guid.NewGuid() + extension;
                        var location = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/resimler/", newImageName);

                        using (var stream = new FileStream(location, FileMode.Create))
                        {
                            url.CopyTo(stream);
                        }

                        // Update the URL to point to the new image
                        existingProduct.url = newImageName;
                    }

                    // Update other product properties
                    existingProduct.Name = updatedProduct.Name;
                    existingProduct.Description = updatedProduct.Description;
                    // Add other fields to update as necessary

                    _context.Products.Update(existingProduct);
                    _context.SaveChanges();

                    TempData["status"] = "Ürün başarıyla güncellendi";
                }
                else
                {
                    TempData["status"] = "Ürün bulunamadı";
                }
            }
            else
            {
                TempData["status"] = "Geçersiz model. Güncelleme işlemi yapılamadı.";
            }

            return RedirectToAction("Index");
        }






        public IActionResult WishListPage()
        // FromServices özelliği dependency injection patterni olan DI kullanılacağını haber vermektedir
        // yani dependency injection ifadesini parametre olarak kullancak olursak bunu kullanmak zorundayız
        {

            // burada singleton ozelliği ile dependency injection yapıyoruz , singleton ozelliği 
            // yalnızca bir nesne uretılmesını saglar


            var products = _context.Products.ToList();
            return View(products);
        }

        public IActionResult MyAccount()
        {


            return View();
        }

    }
}
