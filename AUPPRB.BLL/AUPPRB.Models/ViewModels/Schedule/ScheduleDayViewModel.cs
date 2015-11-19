using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AUPPRB.Models.ViewModels.Notifications;

namespace AUPPRB.Models.ViewModels.Schedule
{
    public class ScheduleDayViewModel
    {
        public ScheduleDayViewModel()
        {
            Lessons=new List<LessonViewModel>();
            Notifications=new List<NotificationViewModel>();
        }
        public string DateOfDay { get; set; }
       
        public List<LessonViewModel> Lessons { get; set; }

        public List<NotificationViewModel> Notifications { get; set; }


        public bool IsPast { get; set; }
        public bool IsCurrentDay { get; set; }

        public bool IsEmpty
        {
            get { return Lessons.Any(); } 
        }

        public bool NeedSkip { get; set; }

        /// <summary>
        /// TODO:Добавил для отображения расписания на 1 день для всех групп, отрефакторить в будущем этот говнокод
        /// </summary>
        public int MaxLessonsInSimilarTime { get; set; }

        /// <summary>
        ///  TODO:Добавил для отображения расписания на 1 день для всех групп, отрефакторить в будущем этот говнокод
        /// </summary>
        public List<string> LessonsTime { get; set; }


    }
}
