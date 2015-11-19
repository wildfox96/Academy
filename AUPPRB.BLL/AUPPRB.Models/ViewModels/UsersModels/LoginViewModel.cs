using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AUPPRB.Models.ViewModels
{
    public class LoginViewModel
    {
       
        [Display(Name = "Логин")]
        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Длина логина должна быть от 3 символов")]
        public string Login { get; set; }


        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Длина пароля должна быть от 3 символов")]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

         [Display(Name = "Запомнить меня")]
        public bool RememberMe { get; set; }


   
   
       
    }
}
