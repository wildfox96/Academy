using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AUPPRB.Models.DomainModels;

namespace AUPPRB.Models.ViewModels.Literature
{
    public class ReservatorInfoViewModel
    {
        public string BookName { get; set; }
        public string ReservateDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }

        public static ReservatorInfoViewModel ToReservatirInfoViewModel(Library_ReservateBook reservation)
        {
            var user = reservation.User.UserMeta.First();
            return new ReservatorInfoViewModel()
            {
                MiddleName = user.MiddleName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                ReservateDate = reservation.ReservateDate.ToShortDateString(),
                BookName = reservation.Library_Book.Library_Literature.ShortName
            };
        }
    }
}
