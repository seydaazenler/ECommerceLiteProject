using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ECommerceLiteEntity.Models
{
    public class TheBase<T> : ITheBase
    {
        [Key]
        [Column(Order=1)]
        public T Id { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name ="Kayıt Tarihi")]
        public DateTime RegisterDate { get; set; } = DateTime.Now;
    }
}