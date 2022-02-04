using ECommerceLiteBLL.Repository;
using ECommerceLiteBLL.Settings;
using ECommerceLiteEntity.Models;
using ECommerceLiteUI.Models;
using Mapster;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using ECommerceLiteBLL.Account;

namespace ECommerceLiteUI.Controllers
{
    public class ProductController : Controller
    {
        //Global alan
        ProductRepo myProductRepo = new ProductRepo();
        CategoryRepo myCategoryRepo = new CategoryRepo();
        ProductPictureRepo myProductPictureRepo = new ProductPictureRepo();

        public ActionResult ProductList(int page = 1, string search = "", bool isNew = false)
        {
            List<SelectListItem> subCategories = new List<SelectListItem>();
            myCategoryRepo.Queryable()
                .Where(x => x.BaseCategoryId != null)
                .ToList().ForEach(x => subCategories.Add(new SelectListItem()
                {
                    Text = x.CategoryName,
                    Value = x.Id.ToString()
                }));
            ViewBag.CategoryList = subCategories;


            List<Product> allProductList = new List<Product>();
            if (string.IsNullOrEmpty(search))
            {
                allProductList = myProductRepo.GetAll();
            }
            else
            {
                allProductList =
                    myProductRepo.Queryable()
                    .Where(x => x.ProductName.Contains(search)).ToList();
            }

            if (isNew)
            {
                allProductList = myProductRepo.GetAll();
                allProductList = allProductList.Where(x =>
                x.RegisterDate >= DateTime.Now.AddDays(-1)).ToList();

            }
            var user = MembershipTools.GetNameSurname();
            LogManager.LogMessage("geldi", userInfo: user, pageInfo: "Product/ProductList");
            return View(allProductList.ToPagedList(page, 3));
        }


        [HttpGet]
        public ActionResult Create()
        {
            List<SelectListItem> subCategories = new List<SelectListItem>();
            myCategoryRepo.Queryable().Where(x => x.BaseCategoryId != null).ToList().ForEach(x => subCategories.Add(new SelectListItem()
            {
                Text = x.CategoryName,
                Value = x.Id.ToString()
            }));
            ViewBag.CategoryList = subCategories;

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductViewModel model)
        {
            try
            {
                List<SelectListItem> subCategories = new List<SelectListItem>();
                myCategoryRepo.Queryable().Where(x => x.BaseCategoryId != null).ToList().ForEach(x => subCategories.Add(new SelectListItem()
                {
                    Text = x.CategoryName,
                    Value = x.Id.ToString()
                }));
                ViewBag.CategoryList = subCategories;

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
                    if (model.Files.Count(x => x != null) > 0)
                    {
                        ProductPicture productPicture = new ProductPicture();
                        productPicture.ProductId = newProduct.Id;
                        productPicture.RegisterDate = DateTime.Now;

                        //db de birden fazla resim ekleme işlemi olacağı için counter kullandık
                        int counter = 1;
                        foreach (var item in model.Files)
                        {
                            if (item != null && item.ContentType.Contains("image") && item.ContentLength > 0)
                            {

                                string filename = SiteSettings.UrlFormatConverter(model.ProductName).ToLower().Replace("-", "");
                                string extName = Path
                                    .GetExtension(item.FileName);

                                string guid = Guid.NewGuid()
                                    .ToString().Replace("-", "");
                                var directoryPath = Server.MapPath($"~/ProductPictures/{filename}/{model.ProductCode}");
                                var filePath = Server.MapPath($"~/ProductPictures/" +
                                    $"{filename}/{model.ProductCode}/") + filename + counter + "-" + guid + extName;
                                if (!Directory.Exists(directoryPath))
                                {
                                    Directory.CreateDirectory(directoryPath);
                                }
                                item.SaveAs(filePath);
                                //item kayıt olduğu andan itibaren Dbde oluşmalıdır
                                if (counter == 1)
                                {
                                    productPicture.ProductPicture1 = $"/ProductPictures/" +
                                        $"{filename}/{model.ProductCode}/" + filename + counter + "-" + guid + extName;
                                }
                                if (counter == 2)
                                {
                                    productPicture.ProductPicture2 = $"/ProductPictures/" +
                                       $"{filename}/{model.ProductCode}/" + filename + counter + "-" + guid + extName;
                                }
                                if (counter == 3)
                                {
                                    productPicture.ProductPicture3 = $"/ProductPictures/" +
                                       $"{filename}/{model.ProductCode}/" + filename + counter + "-" + guid + extName;
                                }
                                if (counter == 4)
                                {
                                    productPicture.ProductPicture4 = $"/ProductPictures/" +
                                       $"{filename}/{model.ProductCode}/" + filename + counter + "-" + guid + extName;
                                }
                                if (counter == 5)
                                {
                                    productPicture.ProductPicture5 = $"/ProductPictures/" +
                                       $"{filename}/{model.ProductCode}/" + filename + counter + "-" + guid + extName;
                                }


                            }

                            counter++;
                        }

                        int pictureInsertResult = myProductPictureRepo.Insert(productPicture);
                        if (pictureInsertResult > 0)
                        {
                            return RedirectToAction("ProductList", "Product");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Ürün eklendi ancak ürüne ait görseller eklenirken bir hata oluştu!" +
                                "Görsel eklemek için tekrar deneyiniz!");
                            return View(model);

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
                var user = MembershipTools.GetNameSurname();
                LogManager.LogMessage(ex.ToString(), userInfo: user, pageInfo: "Product/Create");
                return View(model);
            }
        }

        public ActionResult CategoryProducts()
        {
            try
            {
                var list = myCategoryRepo.GetBaseCategoriesProductCount();
                return View(list);
            }
            catch (Exception ex)
            {
                //loglanacak
                var user = MembershipTools.GetNameSurname();
                LogManager.LogMessage(ex.ToString(), userInfo: user, pageInfo: "Product/CategoryProducts");
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public ActionResult Edit(ProductViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["ProductModalError"] = "Veri girişleri düzgün olmalıdır!";
                    return RedirectToAction("ProductList", "Product");
                }
                var product = myProductRepo.GetById(model.Id);
                product.ProductName = model.ProductName;
                product.Description = model.Description;
                product.Quantity = model.Quantity;
                product.Price = model.Price;
                product.CategoryId = model.CategoryId;
                if (model.Files.Count(x=>x!=null)>0)
                {
                    //Önce sildik
                    var pictureList =
                          myProductPictureRepo.Queryable()
                          .Where(x => x.ProductId == model.Id).ToList();
                    foreach (var item in pictureList)
                    {
                        myProductPictureRepo.Delete(item);
                    }
                    //sonra yeni eklediklerini oluşturacağız.
                    ProductPicture productPicture = new ProductPicture();
                    productPicture.ProductId = model.Id;
                    productPicture.RegisterDate = DateTime.Now;
                    int counter = 1;
                    foreach (var item in model.Files)
                    {

                        if (item != null && item.ContentType.Contains("image") && item.ContentLength > 0)
                        {

                            string filename = SiteSettings.UrlFormatConverter(model.ProductName).ToLower().Replace("-", "");
                            string extName = Path
                                .GetExtension(item.FileName);

                            string guid = Guid.NewGuid()
                                .ToString().Replace("-", "");
                            var directoryPath = Server.MapPath($"~/ProductPictures/{filename}/{model.ProductCode}");
                            var filePath = Server.MapPath($"~/ProductPictures/{filename}/{model.ProductCode}/") + filename + counter + "-" + guid + extName;
                            if (!Directory.Exists(directoryPath))
                            {
                                Directory.CreateDirectory(directoryPath);
                            }
                            item.SaveAs(filePath);
                            if (counter == 1)
                            {
                                productPicture.ProductPicture1 = $"/ProductPictures/{filename}/{model.ProductCode}/" + filename + counter + "-" + guid + extName;
                            }
                            if (counter == 2)
                            {
                                productPicture.ProductPicture2 = $"/ProductPictures/{filename}/{model.ProductCode}/" + filename + counter + "-" + guid + extName;
                            }
                            if (counter == 3)
                            {
                                productPicture.ProductPicture3 = $"/ProductPictures/{filename}/{model.ProductCode}/" + filename + counter + "-" + guid + extName;
                            }
                            if (counter == 4)
                            {
                                productPicture.ProductPicture4 = $"/ProductPictures/{filename}/{model.ProductCode}/" + filename + counter + "-" + guid + extName;
                            }
                            if (counter == 5)
                            {
                                productPicture.ProductPicture5 = $"/ProductPictures/{filename}/{model.ProductCode}/" + filename + counter + "-" + guid + extName;
                            }


                        }
                        counter++;
                    }

                    int pictureInsertResult =
                        myProductPictureRepo.Insert(productPicture);
                    if (pictureInsertResult == 0)
                    {
                        TempData["ProductModalError"] = "Ürün eklendi ama ürüne ait fotoğraflar eklenirken " +
                            "bir hata oluştu. Fotoğraf eklemek için tekrar deneyiniz!";
                    }
                    else
                    {
                        TempData["ProductModalError"] = string.Empty;
                    }
                }

                int updateResult = myProductRepo.Update();
                if (updateResult > 0)
                {
                    return RedirectToAction("ProductList", "Product");
                }
                else
                {
                    //geçici
                    return RedirectToAction("ProductList", "Product");

                }

            }
            catch (Exception ex)
            {
                var user = MembershipTools.GetNameSurname();
                LogManager.LogMessage(ex.ToString(),
                    userInfo: user, pageInfo: "Product/Edit");
                //geçici
                return RedirectToAction("ProductList", "Product");
            }
        }

        public JsonResult GetProductDetails(int id)
        {
            var product = myProductRepo.GetById(id);
            if (product != null)
            {
                var data = product.Adapt<ProductViewModel>();
                return Json(new { isSuccess = true, data }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { isSuccess = false }, JsonRequestBehavior.AllowGet);
        }

    }
}
