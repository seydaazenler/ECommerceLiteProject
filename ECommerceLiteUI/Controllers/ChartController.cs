using ECommerceLiteBLL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECommerceLiteUI.Controllers
{
    public class ChartController : Controller
    {
        //Global Alan
        CategoryRepo myCategoryRepo = new CategoryRepo();
        
        
        public ActionResult VisualizePieChartResult()
        {
            //ajax : asenkronik javascript ve xml
            //ihtiyacımız olan Piechartta göstermek istediğimiz datayı alırız
            //bu dataya dashboarddaki ajax işlemine gönderebilmek için return ile Json olarak işlem yapılır
            var data = myCategoryRepo.GetBaseCategoriesProductCount();
            return Json(data,JsonRequestBehavior.AllowGet);
        }

        
    }
}