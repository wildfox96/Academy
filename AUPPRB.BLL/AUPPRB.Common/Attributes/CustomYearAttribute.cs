using System;
using System.ComponentModel.DataAnnotations;

namespace AUPPRB.Common.Attributes
{
    public class CustomYearAttribute : RequiredAttribute
    {
        private int _beginYear { get; set; }


        public CustomYearAttribute(int beginYear)
        {
            _beginYear = beginYear;
            ErrorMessage = "Год должен быть больше " + beginYear + " и меньше " + DateTime.Now.Year;
        }
        /// <summary>
        /// Валидный если прошел проверку по дате
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool IsValid(object value)
        {
            if (value != null)
                return ((int)value >= _beginYear) && ((int)value <= DateTime.Now.Year);
            return true;
        }
    }
}
