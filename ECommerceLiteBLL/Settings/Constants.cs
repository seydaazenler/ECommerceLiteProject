using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace ECommerceLiteBLL.Settings
{
    public static class Constants
    {
        public static string EMailAddress
        {
            get 
            {
                try
                {
                    return ConfigurationManager.AppSettings["MAILADDRESS"].ToString();
                }
                catch (Exception ex)
                {

                    throw new Exception(ex.Message + "WebConfig dosyasında 'MAILADDRESS' adlı bir anahtar olmalıdır!");
                }
            }
            
        }

        public static string EMailPassword
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["MAILPASSWORD"].ToString();
                }
                catch (Exception ex)
                {

                    throw new Exception(ex.Message + "WebConfig dosyasında 'MAILPASSWORD' adlı bir anahtar olmalıdır!");
                }
            }

        }
    }
}
