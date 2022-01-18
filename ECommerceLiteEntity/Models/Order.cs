using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ECommerceLiteEntity.Models
{
    [Table("Orders")]
    public class Order: TheBase<int>
    {
        [Required]
        [StringLength(7,ErrorMessage="Satış numarası gereklidir!")]
        public string OrderNumber { get; set; }
        public string CustomerTCNumber { get; set; }
        [ForeignKey("CustomerTCNumber")]
        public virtual Customer Customer { get; set; }
        public virtual List<OrderDetail> OrderDetailList { get; set; }
    }
}