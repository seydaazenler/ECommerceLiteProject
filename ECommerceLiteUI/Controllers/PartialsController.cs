using ECommerceLiteBLL.Account;
using ECommerceLiteBLL.Repository;
using ECommerceLiteEntity.IdentityModels;
using ECommerceLiteUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECommerceLiteUI.Controllers
{
    public class PartialsController : BaseController
    {
        //Global Alan

        CategoryRepo myCategoryRepo = new CategoryRepo();
        private object MemberShipTools;

        public PartialViewResult AdminSideBarResult()
        {
            //TODO: name surname alınacak
            TempData["NameSurname"] = "";
            return PartialView("_PartialAdminSideBar");
        }
       public PartialViewResult AdminSideBarMenuResult()
        {

            return PartialView("_PartialAdminSideBarMenu");
        }

        public PartialViewResult UserNameSurnameOnHomePage()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var loggedInUser = MembershipTools.GetUser();
                return PartialView("_PartialUserNameSurnameOnHomePage", loggedInUser);
                
            }
            return PartialView("_PartialUserNameSurnameOnHomePage",null);
        }

        public PartialViewResult ShoppingCart()
        {
            var shoppingCart = Session["ShoppingCart"] as List<CartViewModel>;
            if (shoppingCart==null)
            {
                return PartialView("_PartialShoppingCart", new List<CartViewModel>());

            }
            else
            {
                return PartialView("_PartialShoppingCart", shoppingCart);

            }
        }

        public PartialViewResult AdminSideBarCategories()
        {
            //controller arasında veri taşımaya yarar Actiondan sayfaya veri taşıma
            TempData["AllCategoriesCount"] = myCategoryRepo.Queryable().Where(x => x.BaseCategory == null).ToList().Count;
            return PartialView("_PartialAdminSideBarCategories");
        }

    }
}