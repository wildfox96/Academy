using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AUPPRB.Models.ViewModels.ExpertCenterModels
{
    public class OutputModel
    {
        public List<string> List { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public List<ParsePrepodList> PrepCabinetList { get; set; }
        public List<ParseDisciplineList> PrepDispList { get; set; }
        public string[] PrepodList { get; set; }
        public string[] DispList { get; set; }
        public string Name { get; set; }
        public string Check { get; set; }
    }
}