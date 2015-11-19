using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AUPPRB.Common.Utils;

namespace AUPPRB.Models.ViewModels.Discipline
{
    public class DisciplineMetaViewModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string ShortName { get; set; }

        public string TranslitName  { get; set; }

        public DisciplineMetaViewModel()
        {
            //TranslitName = TranslitHelper.RusToEng(ShortName);
        }


    }
}
