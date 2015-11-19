using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AUPPRB.Models.ViewModels.ExpertCenterModels
{
    public class ParseDisciplineList
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Photo { get; set; }
        public string MiddleName { get; set; }
        public string FirstName { get; set; }
        public int Likes { get; set; }
        public int IdSotr { get; set; }
    }
}