using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AUPPRB.Models.ViewModels
{
    public class StudentViewModel
    {

        [Display(Name = "Факультет")]
        [Required(ErrorMessage = "Необходимо указать факультет")]
        public int FacultyId { get; set; }

        [Display(Name = "Специальность")]
       [Required(ErrorMessage = "Необходимо указать специальность")]
        public int SpecialityId { get; set; }

        [Display(Name = "Поток")]
     [Required(ErrorMessage = "Необходимо указать номер потока")]
        public int FlowId { get; set; }

        [Display(Name = "Группа")]
     [Required(ErrorMessage = "Необходимо указать номер группы")]
        public int GroupId { get; set; }

        //для добавления пользователей теперь не используются
        [Display(Name = "Год начала обучения")]
        // [Required(ErrorMessage = " Необходимо указать год начала обучения")]
        public int AdmissionDate { get; set; }

        //для добавления пользователей теперь не используются
        [Display(Name = "Год окончания обучения ")]
        //[Required(ErrorMessage = "Необходимо указать год окончания обучения")]

        public int GraduationDate { get; set; }

        [Display(Name = "Заблокировать студента")]
        public bool IsDismissed { get; set; }


        [Display(Name = "Номер зачетной книжки")]
        [Required(ErrorMessage = "Необходимо указать номер зачетной книжки")]
        public int MarkBookNumber { get; set; }

        [Display(Name = "Номер студенческого билета")]
        [Required(ErrorMessage = "Необходимо указать номер студенческого билета")]
        public int StudentCardNumber { get; set; }

        [Display(Name = "Срок обучения")]
        [Required(ErrorMessage = "Необходимо указать срок обучения")]
        public int IdSpecialtyMeta { get; set; }

    }
}
