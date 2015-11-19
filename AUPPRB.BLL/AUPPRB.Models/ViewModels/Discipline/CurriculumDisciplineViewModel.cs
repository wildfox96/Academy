using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AUPPRB.Models.ViewModels.Discipline
{
    public class CurriculumDisciplineViewModel
    {
        public int Id { get; set; }

        public DisciplineMetaViewModel Discipline { get; set; }
        public int LecturesCount { get; set; }
        public int SeminarsCount { get; set; }


    }
}
