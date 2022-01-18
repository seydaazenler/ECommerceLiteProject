using ECommerceLiteDAL;
using ECommerceLiteEntity.IdentityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Web;

namespace ECommerceLiteBLL.Account
{
    public static class MembershipTools
    {
        public static UserStore<ApplicationUser> NewUserStore()
        {
            return new UserStore<ApplicationUser>(new MyContext());
        }
        public static UserManager<ApplicationUser> NewUserManager()
        {
            return new UserManager<ApplicationUser>(NewUserStore());
        }

        public static RoleStore<ApplicationRole> NewRoleStore()
        {
            return new RoleStore<ApplicationRole>(new MyContext());
        }

        public static RoleManager<ApplicationRole> NewRoleManager()
        {
            return new RoleManager<ApplicationRole>(NewRoleStore());
        }
        //username getiren
        public static string GetUserName(string id)
        {
            var myUserManager = NewUserManager();
            var user = myUserManager.FindById(id);
            if (user !=null)
            {
                return user.UserName;
            }
            return null;
        }

        public static string GetNameSurname()
        {
            var id = HttpContext.Current.User.Identity.GetUserId();
            if (string.IsNullOrEmpty(id))
            {
                return "Guest";
            }
            else
            {
                var myUserManager = NewUserManager();
                var user = myUserManager.FindById(id);
                string nameSurname = null;
                nameSurname =
                    user != null ?
                    string.Format("{0} {1}",user.Name, user.Surname)
                    : null;

                return nameSurname;
            }
        }
    }
}
