using System.ComponentModel.DataAnnotations;

namespace AUPPRB.Common.Attributes
{
    public class EmailAttribute : RegularExpressionAttribute
    {
        private const string pattern = "^[a-z0-9_\\+-]+(\\.[a-z0-9_\\+-]+)*@[a-z0-9-]+(\\.[a-z0-9-]+)*\\.([a-z]{2,4})$";

        public EmailAttribute()
            : base(pattern)
        {
            ErrorMessage = "Неверный формат E-mail";
        }
    }
}
