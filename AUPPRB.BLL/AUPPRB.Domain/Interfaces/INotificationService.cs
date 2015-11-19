using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AUPPRB.Models.OtherModels.ServiceModels;
using AUPPRB.Models.ViewModels.Notifications;

namespace AUPPRB.Domain.Interfaces
{
    public interface INotificationService
    {
        Dictionary<int, string> GetNotificationTypes();
        Dictionary<int, string> GetNotificationTimes();
        NotificationViewModel GetNotification(int id);
        RequestResult AddNotice(int userId, NotificationViewModel notice);
        RequestResult EditNotice(NotificationViewModel notice);
        void DeleteNotification(int userId,int id);


    }
}
