using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace AUPPRB.Common.Attributes
{
    public class CustomDateAttribute : RequiredAttribute
    {
        private DateTime _beginDate { get; set; }
        private DateTime? _finishDate { get; set; }


        public CustomDateAttribute(string beginDate)
        {
            _beginDate = Convert.ToDateTime(beginDate, new CultureInfo("ru"));
            _finishDate = null;
            ErrorMessage = "Дата должна находиться в промежутке от " + beginDate + " до " + DateTime.Now.ToShortDateString();
        }

        public CustomDateAttribute(string beginDate, string finishDate)
        {
            _beginDate = Convert.ToDateTime(beginDate, new CultureInfo("ru"));
            _finishDate = Convert.ToDateTime(finishDate, new CultureInfo("ru"));
            ErrorMessage = "Дата должна находиться в промежутке от " + beginDate + " до " + finishDate;
        }
        /// <summary>
        /// Валидный если прошел проверку по дате
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool IsValid(object value)
        {
            if (value != null)
            {
                if (_finishDate == null)
                    return (value is DateTime) && ((DateTime)value >= _beginDate) && ((DateTime)value <= DateTime.Now);
                return (value is DateTime) && ((DateTime)value >= _beginDate) && ((DateTime)value <= _finishDate);
            }

            return true;
        }
    }
}
