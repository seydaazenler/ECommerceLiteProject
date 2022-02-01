using ECommerceLiteBLL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mapster;
using ECommerceLiteUI.Models;
using ECommerceLiteEntity.Models;
using ECommerceLiteBLL.Account;

namespace ECommerceLiteUI.Controllers
{
    public class HomeController : Controller
    {
        //Global Alan
        CategoryRepo myCategoryRepo = new CategoryRepo();
        ProductRepo myProductRepo = new ProductRepo();
        OrderRepo myOrderRepo = new OrderRepo();
        OrderDetailRepo myOrderDetailRepo = new OrderDetailRepo();
        CustomerRepo myCustomerRepo = new CustomerRepo();
        public ActionResult Index()
        {
            var categoryList = myCategoryRepo.Queryable().Where(x => x.BaseCategoryId == null).ToList();
            //Take(4) vererek sadece 4 tanesinin görünmesini sağlarız
            // var categoryList = myCategoryRepo.Queryable().Where(x => x.BaseCategoryId == null).Take(4).ToList(); 
            ViewBag.CategoryList = categoryList;
            var productList = myProductRepo.GetAll();

            List<ProductViewModel> model = new List<ProductViewModel>();
            foreach (var item in productList)
            {
                model.Add(item.Adapt<ProductViewModel>());
            }
            foreach (var item in model)
            {
                item.SetCategory();
                item.SetProductPictures();
            }


            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult AddToCart(int id)
        {
            try
            {
                var shoppingCart =
                    Session["ShoppingCart"] as List<CartViewModel>;
                if (shoppingCart == null)
                {
                    shoppingCart = new List<CartViewModel>();
                }
                if (id > 0)
                {
                    var product =
                        myProductRepo.GetById(id);
                    if (product == null)
                    {
                        TempData["AddToCart"] = "Ürün ekleme işlemi başarısız! Lütfen Tekrar Deneyiniz";
                        return RedirectToAction("Index", "Home");
                    }
                    var productAddtoCart = product.Adapt<CartViewModel>();
                    if (shoppingCart.Count(x => x.Id == productAddtoCart.Id) > 0)
                    {
                        shoppingCart.FirstOrDefault(x => x.Id == productAddtoCart.Id).Quantity++;
                    }
                    else
                    {
                        productAddtoCart.Quantity = 1;
                        shoppingCart.Add(productAddtoCart);
                    }
                    Session["ShoppingCart"] = shoppingCart;
                    TempData["AddToCart"] = "Ürün Eklendi";
                }

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                TempData["AddToCart"] = "Ürün ekleme işlemi başarısız! Lütfen Tekrar Deneyiniz";
                return RedirectToAction("Index", "Home");

            }
        }
        [Authorize]
        public ActionResult Buy()
        {
            try
            {
                var shoppingCart = Session["ShoppingCart"] as List<CartViewModel>;

                if (shoppingCart != null)
                {
                    if (shoppingCart.Count > 0)
                    {

                        var user = MembershipTools.GetUser();
                        var customer = myCustomerRepo.Queryable().FirstOrDefault(x => x.UserId == user.Id);
                        Order newOrder = new Order()
                        {
                            CustomerTCNumber = customer.TCNumber,
                            RegisterDate = DateTime.Now,
                            OrderNumber = "1234567"
                        };
                        int orderInsertResult = myOrderRepo.Insert(newOrder);
                        if (orderInsertResult > 0)
                        {
                            //eklendi ise
                            foreach (var item in shoppingCart)
                            {
                                OrderDetail newOrderDetail = new OrderDetail()
                                {
                                    OrderId = newOrder.Id,
                                    ProductId = item.Id,
                                    Discount = 0,
                                    ProductPrice = item.Price,
                                    Quantity = item.Quantity,
                                    RegisterDate = DateTime.Now
                                };
                                if (newOrderDetail.Discount > 0)
                                {
                                    newOrderDetail.TotalPrice = newOrderDetail.Quantity *
                                        (newOrderDetail.ProductPrice - (newOrderDetail.ProductPrice *
                                        Convert.ToDecimal(newOrderDetail.Discount / 100)));
                                }
                                else
                                {
                                    newOrderDetail.TotalPrice = newOrderDetail.Quantity * newOrderDetail.ProductPrice;
                                }

                                int detailInsertResult = myOrderDetailRepo.Insert(newOrderDetail);
                                if (detailInsertResult > 0)
                                {
                                    return RedirectToAction("Order", "Home", new { id = newOrder.Id });
                                }
                            }

                        }

                    }
                }
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                //ex loglanacak
                //TempData ile sonuç anasayfaya gönderilebilir
                return RedirectToAction("Index", "Home");

            }
        }
        [Authorize]
        public ActionResult Order(int id)
        {
            try
            {
                if (id > 0)
                {
                    //order alma işlemi
                    Order customerOrder = myOrderRepo.GetById(id);
                    List<OrderDetail> orderDetails = new List<OrderDetail>();
                    if (customerOrder!=null)
                    {
                        orderDetails = myOrderDetailRepo.Queryable().Where(x => x.OrderId == customerOrder.Id).ToList();
                        foreach (var item in orderDetails)
                        {
                            item.Product = myProductRepo.GetById(item.ProductId);
                        }
                        ViewBag.OrderSuccess = "Siparişiniz başarıyla oluşturuldu";
                        return View(orderDetails);
                    }
                    else
                    {
                        ModelState.AddModelError("", "Ürün bulunamadı! Tekrar deneyiniz");
                        return View();
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Ürün bulunamadı! Tekrar deneyiniz");
                    return View(new List<OrderDetail>());
                }
            }
            catch (Exception ex)
            {

                ModelState.AddModelError("", "Beklenmedik bir hata oluştu!");
                //ex loglanacak
                return View(new List<OrderDetail>());
            }
        }

    }
}
