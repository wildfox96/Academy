using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AUPPRB.Models.ViewModels.UsersModels
{
    public class PrepodViewModel
    {
        [Display(Name = "Кафедра")]
        //[Required(ErrorMessage = "Необходимо указать кафедру")]
        public List<int> DepartmentsIds { get; set; }

        [Display(Name = "Ученая степень")]
        [Required(ErrorMessage = "Необходимо указать ученую степень")]
        public int DegreeId { get; set; }

        [Display(Name = "Внешний преподаватель")]
        public bool IsExtern { get; set; }

        public string DepartmentsIdsString { get; set; }


    }
}
