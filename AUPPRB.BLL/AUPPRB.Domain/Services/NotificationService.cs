using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using AUPPRB.Common.Enums;
using AUPPRB.Common.Extensions;
using AUPPRB.Domain.Interfaces;
using AUPPRB.Models.DomainModels;
using AUPPRB.Models.OtherModels.ServiceModels;
using AUPPRB.Models.ViewModels.Notifications;
using DocumentFormat.OpenXml.Drawing.Charts;

namespace AUPPRB.Domain.Services
{
    public class NotificationService : BaseServices, INotificationService
    {
        public Dictionary<int, string> GetNotificationTypes()
        {
            return ServiceCache.GetOrAdd("GetNotificationTypes", () =>
            {
                var dictionaryType = DictionaryTypeEnum.ТипыЗаметок.GetEnumValue();
                return DataProvider.Dictionaries.Filter(p => p.DictionaryTypeId == dictionaryType)
                    .ToDictionary(p => p.Id, p => p.Name);
            });
        }

        public Dictionary<int, string> GetNotificationTimes()
        {
            return ServiceCache.GetOrAdd("GetNotificationTimes", () => DataProvider.VremyaZanyatia.GetAll().OrderBy(p => p.Id).ToDictionary(p => p.Id, p => p.StartTime.ToString("HH.mm") + "-" + p.EndTime.ToString("HH.mm")));
        }

        public NotificationViewModel GetNotification(int id)
        {
            var notification = DataProvider.Notifications.FirstOrDefault(p => p.NotificationId == id);
            return notification == null ? null : NotificationViewModel.ToNotificationViewModel(notification);
        }

        public RequestResult AddNotice(int userId, NotificationViewModel notice)
        {
            var date = DateTime.Parse(notice.NotificationDate);

            if(DataProvider.Notifications.Filter(p=>p.UserId==userId && p.Date==date && p.IdVremyaZanyatia==notice.NotificationTimeId).Count()>=5)
                throw new Exception("Количество заметок  не может быть больше 5");

            try
            {
                DataProvider.Notifications.Add(new Notification()
                {
                    Date = date,
                    IdVremyaZanyatia = notice.NotificationTimeId,
                    NotificationData = notice.NotificationText,
                    UserId = userId,
                    NotificationType = notice.NotificationTypeId
                });
                DataProvider.Save();
                return RequestResult.Ok;
            }
            catch (Exception e)
            {
                throw new Exception("Ошибка сохранения. Проверьте корректность данных");
            }
        }

        public RequestResult EditNotice(NotificationViewModel editNotice)
        {
            var date = DateTime.Parse(editNotice.NotificationDate);

            var notice = DataProvider.Notifications.FirstOrDefault(p => p.NotificationId == editNotice.NotificationId);
            if(notice==null)
                throw new Exception("Ошибка идентификации заметки");

            if (DataProvider.Notifications.Filter(p => p.UserId == notice.UserId && p.Date == date && p.IdVremyaZanyatia == editNotice.NotificationTimeId).Count() >=5)
                throw new Exception("Количество заметок  не может быть больше 5");

            notice.NotificationType = editNotice.NotificationTypeId;
            notice.Date = date;
            notice.IdVremyaZanyatia = editNotice.NotificationTimeId;
            notice.NotificationData = editNotice.NotificationText;

            try
            {
                DataProvider.Save();
                return RequestResult.Ok;
            }
            catch (Exception e)
            {
                throw new Exception("Ошибка сохранения. Проверьте корректность данных");
            }
        }

        public void DeleteNotification(int userId, int id)
        {
            var notice = DataProvider.Notifications.FirstOrDefault(p => p.NotificationId == id);
            if (notice == null)
                throw new Exception("Ошибка идентификации заметки");

            if(notice.UserId!=userId)
                throw new Exception("Вы не можете удалить чужую заметку");

            try
            {
                DataProvider.Notifications.Delete(notice);
                DataProvider.Save();
            }
            catch (Exception)
            {
                throw new Exception("Ошибка работы приложения");
            }
        }
    }
}
