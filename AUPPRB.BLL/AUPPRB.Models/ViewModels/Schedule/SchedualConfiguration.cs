using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AUPPRB.Models.ViewModels.Schedule
{
    public class SchedualConfiguration
    {
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public int? UserId { get; set; }
        public int? GroupId { get; set; }
    }
}
