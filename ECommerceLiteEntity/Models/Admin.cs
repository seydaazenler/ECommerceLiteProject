using ECommerceLiteEntity.IdentityModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ECommerceLiteEntity.Models
{
    [Table("Admins")]
    public class Admin : PersonBase
    {
        public string UserId { get; set; }

        [ForeignKey("UserId")]

        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}