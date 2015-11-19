using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AUPPRB.Models.ViewModels.Schedule
{
    public class ScheduleWeekViewModel
    {
        //TODO 
        public ScheduleDayViewModel Monday { get; set; }
        public ScheduleDayViewModel Tuesday { get; set; }
        public ScheduleDayViewModel Wednesday { get; set; }
        public ScheduleDayViewModel Thursday { get; set; }
        public ScheduleDayViewModel Friday { get; set; }
        public ScheduleDayViewModel Saturday { get; set; }

        public ScheduleDayViewModel GetFirstNotNullDay()
        {
            if (Monday != null)
                return Monday;
            if (Tuesday != null)
                return Tuesday;
            if (Wednesday != null)
                return Wednesday;
            if (Thursday != null)
                return Thursday;
            if (Friday != null)
                return Friday;
            if (Saturday != null)
                return Saturday;

            return null;
        }

    
    }
}
