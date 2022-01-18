using ECommerceLiteEntity.IdentityModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ECommerceLiteEntity.Models
{
    [Table("Customers")]
    public class Customer : PersonBase
    {
        public string UserId { get; set; }
        //ıdentity modelin ID değeri burada foreign key olacaktır
        [ForeignKey("UserId")]

        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}