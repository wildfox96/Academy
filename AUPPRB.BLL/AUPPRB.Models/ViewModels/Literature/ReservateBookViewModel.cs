using System.Collections.Generic;
using AUPPRB.Models.ViewModels.jqGridModels;

namespace AUPPRB.Models.ViewModels.Literature
{
    public class ReservateBookViewModel
    {
        public int? SelectedLiterature { get; set; }
        public Dictionary<int, string> LiteratureNames { get; set; }
        public Dictionary<int, string> BookNumbers { get; set; }
        public UserForReservateLiteratureJqGridModel UsersGrid { get; set; }
    }
}
