using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace AUPPRB.Models.ViewModels.Schedule
{
    public class ScheduleViewModel
    {
        public ScheduleViewModel()
        {
            Weeks = new List<ScheduleWeekViewModel>();
            LessonsTime=new List<string>();
        }

        public List<ScheduleWeekViewModel> Weeks { get; set; }

        public int DaysCount { get; set; }

        public string DateFrom { get; set; }

        public List<string> LessonsTime { get; set; }

        public int MaxLessonsInSimilarTime { get; set; }
    }
}
