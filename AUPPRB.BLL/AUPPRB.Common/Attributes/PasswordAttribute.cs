using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace AUPPRB.Common.Attributes
{
    public class PasswordAttribute : ValidationAttribute
    {
        private String PasswordPattern { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public PasswordAttribute()
            : base()
        {
            PasswordPattern = "[A-Za-z0-9_]{6,}";
            ErrorMessage = "Пароль должен содержать не менее 6 символов и состоять из латинских букв, арабских цифр и знака '_'";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="passwordPattern"></param>
        public PasswordAttribute(String passwordPattern)
            : base()
        {
            PasswordPattern = passwordPattern;
            ErrorMessage = "Пароль должен содержать не менее 6 символов и состоять из латинских букв, арабских цифр и знака '_'";
        }
        /// <summary>
        /// Валидный если прошел проверку по паттерну
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool IsValid(object value)
        {
            //return (String.IsNullOrEmpty(value as String) || Regex.IsMatch((string)value, PasswordPattern));
            if (String.IsNullOrEmpty(value as String))
                return false;
            return Regex.IsMatch((string)value, PasswordPattern);
        }
    }
}
