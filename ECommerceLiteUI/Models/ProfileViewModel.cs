using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ECommerceLiteUI.Models
{
	public class ProfileViewModel
	{
        [Required]
        [Display(Name = "Ad")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Soyad")]
        public string Surname { get; set; }
        
        [Display(Name = "Kullanıcı Adı")]
        public string Username { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Mevcut Şifre")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }
        [StringLength(100)]
        [Display(Name = "Yeni Şifre")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^[a-zA-Z]\w{4,14}$", ErrorMessage = @"	
            Şifrenin ilk karakteri bir harf olmalı,
            en az 5 karakter ve en fazla 15 karakter içerir ve hayır
            harfler, sayılar ve alt çizgi dışındaki karakterler kullanılabilir")]
        public string NewPassword { get; set; }
        [StringLength(100)]
        [Display(Name = "Yeni Şifre Tekrar")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Şifreler uyuşmuyor")]
        public string ConfirmNewPassword { get; set; }

    }
}