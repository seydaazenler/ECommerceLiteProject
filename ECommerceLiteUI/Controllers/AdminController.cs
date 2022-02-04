using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ECommerceLiteBLL.Repository;

namespace ECommerceLiteUI.Controllers
{
    //[AllowAnonymous] bilinmeyen kullanıcıya izin vermeyi sağlar
    [Authorize(Roles ="Admin")]
    public class AdminController : BaseController
    {
        //Global Alan
        OrderRepo myOrderRepo = new OrderRepo();
        CategoryRepo myCategoryRepo = new CategoryRepo();

        // GET: Admin
        public ActionResult Dashboard()
        {
            var orderList = myOrderRepo.GetAll();
            var newOrderCount = orderList.Where(x => x.RegisterDate >= DateTime.Now.AddMonths(-1)).ToList().Count();

            ViewBag.NewOrderCount = newOrderCount;

            return View();
        }


    }
}