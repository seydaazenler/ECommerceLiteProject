using ECommerceLiteBLL.Repository;
using ECommerceLiteEntity.Models;
using ECommerceLiteUI.Models;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECommerceLiteUI.Controllers
{
    public class ProductController : Controller
    {
        //Global alan
        ProductRepo myProductRepo = new ProductRepo();
        CategoryRepo myCategoryRepo = new CategoryRepo();
        public ActionResult ProductList()
        {
            var allProductList =
                myProductRepo.GetAll();
            return View(allProductList);
        }

        [HttpGet]
        public ActionResult Create()
        {
            List<SelectListItem> allCategories = new List<SelectListItem>();
            myCategoryRepo.GetAll().ToList().ForEach(x => allCategories.Add(new SelectListItem()
            {
                Text = x.CategoryName,
                Value = x.Id.ToString()
            }));
            ViewBag.CategoryList = allCategories;

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductViewModel model)
        {
            try
            {
                List<SelectListItem> allCategories = new List<SelectListItem>();
                myCategoryRepo.GetAll().ToList().ForEach(x => allCategories.Add(new SelectListItem()
                {
                    Text = x.CategoryName,
                    Value = x.Id.ToString()
                }));
                ViewBag.CategoryList = allCategories;

                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError("", "Veri girişleriniz düzgün olmalıdır!");
                    return View(model);
                }

                //Adapt komutu bir clas ile diğer classı birbirine eşleştirir. Map işlemi yapar
                //Yapabilmesi için isim ve tip aynı olmalıdır!!!

                Product newProduct = model.Adapt<Product>();
                int insertResult = myProductRepo.Insert(newProduct);

                if (insertResult > 0)
                {
                    //ürünün fotoğraflarıda eklensin.
                    if (model.Files.Any())
                    {
                        ProductPicture productPicture = new ProductPicture();
                        productPicture.ProductId = newProduct.Id;
                        productPicture.RegisterDate = DateTime.Now;

                        foreach (var item in model.Files)
                        {

                        }
                    }
                    else
                    {
                        return RedirectToAction("ProductList", "Product");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Ürün ekleme işleminde hata oluştu, tekrar deneyiniz!");
                    return View(model);

                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Beklenmedik hata oluştu!");
                //TODO: ex loglanacak
                return View(model);
            }
        }
    }
}
