using System;
using System.ComponentModel.DataAnnotations;
using AUPPRB.Models.DomainModels;

namespace AUPPRB.Models.ViewModels.Notifications
{
    public class NotificationViewModel
    {
        public int NotificationId { get; set; }

        [Display(Name = "Тип")]
        public int NotificationTypeId { get; set; }
        public string NotificationTypeName { get; set; }

        [Display(Name = "Время")]
        public int NotificationTimeId { get; set; }
        public string NotificationTime { get; set; }

        [Display(Name = "Текст заметки")]
        public string NotificationText { get; set; }

        [Display(Name = "Дата")]
        public string NotificationDate { get; set; }

        public static NotificationViewModel ToNotificationViewModel(Notification note)
        {
            return new NotificationViewModel()
            {
                NotificationId = note.NotificationId,
                NotificationText = note.NotificationData,
                NotificationTime = note.VremyaZanyatia.StartTime.ToString("H.mm") + "-" +
                                   note.VremyaZanyatia.EndTime.ToString("H.mm"),
                NotificationTypeId = note.NotificationType,
                NotificationTypeName = note.Dictionary.Name,
                NotificationTimeId = note.IdVremyaZanyatia,
                NotificationDate=note.Date.ToString("dd-MM-yyyy")
            };
        }
    }
}