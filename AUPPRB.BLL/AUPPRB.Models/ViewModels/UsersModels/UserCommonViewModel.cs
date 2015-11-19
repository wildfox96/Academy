using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using AUPPRB.Common.Attributes;

namespace AUPPRB.Models.ViewModels.UsersModels
{
    public class UserCommonViewModel
    {

        public UserCommonViewModel()
        {

            //Tasks=new List<int>();
            //Roles=new List<int>();

        }
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Display(Name = "Имя")]
        [Required(ErrorMessage = "Необходимо ввести имя")]
        public string FirstName { get; set; }

        [Display(Name = "Фамилия")]
        [Required(ErrorMessage = "Необходимо ввести фамилию")]
        public string LastName { get; set; }

        [Display(Name = "Отчество")]
        [Required(ErrorMessage = "Необходимо ввести отчество")]
        public string MiddleName { get; set; }

        [Display(Name = "Логин")]
        [Required(ErrorMessage = "Необходимо ввести логин")]
        [RegularExpression("^[a-zA-Z0-9_]+$", ErrorMessage = "Допускается использование латинских букв различного регистра, арабских цифр и знака _")]
        [StringLength(50, ErrorMessage = "Логин должен содержать не более 50 символов")]
        public string Login { get; set; }

        [Display(Name = "Пароль")]
        [Password]
        public string Password { get; set; }

        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Некорректный адрес электронной почты")]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [RegularExpression(@"[0-9-]+", ErrorMessage = "Некорректный контактный телефон ")]
        [Display(Name = "Контактный телефон")]
        public string Phone { get; set; }
        [RegularExpression(@"[0-9-]+", ErrorMessage = "Некорректный мобильный телефон ")]
        [Display(Name = "Мобильный телефон")]
        public string AdditionalPhone { get; set; }
        [RegularExpression(@"[A-Za-z0-9._%+-]+", ErrorMessage = "Некорректные данные поля Skype")]
        [Display(Name = "Skype")]
        public string Skype { get; set; }

        [Display(Name = "Заблокировать пользователя")]
        public bool IsBlocked { get; set; }

        [Display(Name = "Дата регистрации пользователя")]
        public DateTime RegDate { get; set; }

        public string Tasks { get; set; }
        public string Roles { get; set; }

        public StudentViewModel StudentMeta { get; set; }
        public PrepodViewModel PrepodMeta { get; set; }

        public bool IsNewUser { get; set; }

        public bool IsPartialView { get; set; }
    }



}
