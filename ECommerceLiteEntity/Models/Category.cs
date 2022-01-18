using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ECommerceLiteEntity.Models
{
    [Table("Categories")]
    public class Category : TheBase<int> //generic classta tip istiyor içerisine INT verdik
    {
        [Required]
        [StringLength(50,MinimumLength =2,ErrorMessage="Kategori adının uzunluğu " +
            "2 ile 50 karakter arasında olmalıdır!")]

        public string CategoryName { get; set; }
        [StringLength(500,ErrorMessage ="Kategori açıklamasının uzunluğu en fazla 500 karakter olmalıdır!")]
        public string CategoryDescription { get; set; }

        public int TopCategoryId { get; set; }
        [ForeignKey("TopCategoryId")]
        public virtual Category TopCategory { get; set; }
        public virtual List<Product> ProductList{ get; set; }
        public virtual List<Category> CategoryList { get; set; }
    }
}