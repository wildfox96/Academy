using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations.Utilities;
using System.Linq;
using System.Web;
namespace AUPPRB.Models.ViewModels.ExpertCenterModels
{
    public class PagingInfo
    {
        public int TotalItems { get; set; }
        public int ItemsForPage { get; set; }
        public int CurrentPage { get; set; }

        public int TotalPages
        {
            get {return Convert.ToInt32(Math.Ceiling((double) TotalItems/ItemsForPage)); }
       }
    }
}