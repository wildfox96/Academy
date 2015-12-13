using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AUPPRB.Common.Extensions;
using AUPPRB.Domain.Interfaces;
using AUPPRB.Models.DomainModels;
using AUPPRB.Models.ViewModels.Notifications;
using AUPPRB.Models.ViewModels.Schedule;
using ClosedXML.Excel;

namespace AUPPRB.Domain.Services
{
    class ScheduleService : BaseServices, IScheduleService
    {
        #region ScheduleWeb
        public ScheduleViewModel GenerateSchedule(int userId, DateTime dateFrom, DateTime dateTo)
        {
            var user = DataProvider.Users.FirstOrDefault(p => p.Id == userId);
            if (user == null || (user.Prepod_PrepodMeta == null && user.Student_StudentMeta == null))
                return new ScheduleViewModel();

            List<IGrouping<DateTime, Raspisanie>> schedulesDateGroups;


            dateFrom = new DateTime(dateFrom.Year, dateFrom.Month, dateFrom.Day);
            dateTo = new DateTime(dateTo.Year, dateTo.Month, dateTo.Day);

            #region Правка даты

            if (dateTo.WeeksInYear() != dateFrom.WeeksInYear())
            {
                dateFrom = dateFrom.StartOfWeek(DayOfWeek.Monday);
                dateTo = dateTo.EndOfWeek(DayOfWeek.Saturday);
            }

            #endregion

            #region Условия для отбора данных из базы

            if (user.Prepod_PrepodMeta != null && user.Prepod_PrepodMeta.Any())
            {

                var prepodIds = DataProvider.PrepodiCafedri.Filter(p => p.Prepod_PrepodMeta.UserId == userId)
                                           .Select(p => p.Id);

                schedulesDateGroups = DataProvider.Raspisanie.Filter(p => prepodIds.Contains(p.IdPrepodaCafedri) && p.Data >= dateFrom && p.Data <= dateTo)

                                                 .GroupBy(p => p.Data)
                                                 .OrderBy(p => p.Key)
                                                 .ToList();


            }

            else
            {
                var userGroup = user.Student_StudentMeta.FirstOrDefault()
                                    .SostavGrupp.OrderByDescending(p => p.DateEdit)
                                               .FirstOrDefault();

                var flow = userGroup.SpisokGrupp.Pot;
                var groupId = userGroup.SpisokGrupp.IdGroup;
                var subGroup = userGroup.Podgroup;
                var specialityId = userGroup.SpisokGrupp.IdSpezMeta;
                //var formOfStudy = user.Student_StudentMeta.FirstOrDefault().FormOfStuduId;
                schedulesDateGroups = DataProvider.Raspisanie.Filter(p => p.Data >= dateFrom &&
                                                                           p.Data <= dateTo &&
                                                                           p.SpezialRazdeliDisziplini.IdSpezMeta == specialityId &&
                                                                           p.Pot == flow &&
                                                                           (!p.IdSpiskaGrupp.HasValue || p.IdSpiskaGrupp == groupId) &&
                                                                           (!p.NomerPodGrupp.HasValue || p.NomerPodGrupp == subGroup)
                                                                          )
                                                .GroupBy(p => p.Data)
                                                .OrderBy(p => p.Key)
                                                .ToList();
            }

            #endregion

            #region Отбор заметок

            var notifications = DataProvider.Notifications.Filter(p => userId == p.UserId && p.Date >= dateFrom && p.Date <= dateTo).ToList();

            #endregion

            var daysCount = (dateTo - dateFrom).Days + 1;


            var scheduleViewModel = new ScheduleViewModel()
            {
                DaysCount = daysCount,
                DateFrom = dateFrom.ToString("yyyy-MM-dd")
            };

            ScheduleWeekViewModel scheduleWeek = new ScheduleWeekViewModel();
            var scheduleDay = new ScheduleDayViewModel();
            var totalCount = schedulesDateGroups.Count;

            int maxLessonsInSimilarTime = 0;

            DateTime dateOfDay = new DateTime();
            List<string> lessonsTimes = new List<string>();

            #region Формируем расписание

            foreach (var schedulesDateGroup in schedulesDateGroups)
            {
                dateOfDay = schedulesDateGroup.Key;
                scheduleDay.DateOfDay = dateOfDay.ToString("yyyy-MM-dd");
                scheduleDay.IsCurrentDay = dateOfDay == DateTime.Now.Date;
                scheduleDay.IsPast = dateOfDay == DateTime.Now.Date;

                #region Добавляем уроки

                scheduleDay.Lessons = schedulesDateGroup.OrderBy(p => p.VremyaZanyatia.StartTime)
                                                   .Select(LessonViewModel.ToLessonViewModel)
                                                   .ToList();
                #endregion

                #region Добавляем заметки

                scheduleDay.Notifications = notifications.Where(p => p.Date == schedulesDateGroup.Key).OrderBy(p => p.IdVremyaZanyatia).Select(NotificationViewModel.ToNotificationViewModel).ToList();

                #endregion

                var tempCount = scheduleDay.Lessons.GroupBy(p => p.LessonTime).Select(p => p.Count()).Max(p => p);
                maxLessonsInSimilarTime = maxLessonsInSimilarTime < tempCount ? tempCount : maxLessonsInSimilarTime;


                lessonsTimes.AddRange(scheduleDay.Lessons.Select(p => p.LessonTime));
                lessonsTimes.AddRange(scheduleDay.Notifications.Select(p => p.NotificationTime));

                switch (dateOfDay.DayOfWeek)
                {
                    case DayOfWeek.Monday:
                        scheduleWeek.Monday = scheduleDay;
                        break;
                    case DayOfWeek.Tuesday:
                        scheduleWeek.Tuesday = scheduleDay;
                        break;
                    case DayOfWeek.Wednesday:
                        scheduleWeek.Wednesday = scheduleDay;
                        break;
                    case DayOfWeek.Thursday:
                        scheduleWeek.Thursday = scheduleDay;
                        break;
                    case DayOfWeek.Friday:
                        scheduleWeek.Friday = scheduleDay;
                        break;
                    case DayOfWeek.Saturday:
                        scheduleWeek.Saturday = scheduleDay;
                        break;
                }

                //если один день то сразу в новую неделю
                if (daysCount == 1)
                {
                    scheduleViewModel.Weeks.Add(scheduleWeek);
                }
                //если суббота или последняя запись то  сохраняем неделю и созаем новую 
                if (dateOfDay.DayOfWeek == DayOfWeek.Saturday || schedulesDateGroups.IndexOf(schedulesDateGroup) == totalCount - 1 || schedulesDateGroup.Key.WeeksInYear() != schedulesDateGroups[schedulesDateGroups.IndexOf(schedulesDateGroup) + 1].Key.WeeksInYear())
                {
                    scheduleViewModel.Weeks.Add(scheduleWeek);
                    scheduleWeek = new ScheduleWeekViewModel();
                }
                scheduleDay = new ScheduleDayViewModel();
            }


            #endregion

            List<string> lessons = lessonsTimes.Distinct().OrderBy(p => p).ToList();
            List<string> finalLessons = new List<string>();

            Dictionary<int, string> list = DataProvider.VremyaZanyatia.GetAllNoTracking().AsEnumerable().Select(x => new
            {
                Id = x.Id,
                Name = x.StartTime.ToString("H.mm") + '-' + x.EndTime.ToString("H.mm")
            }).AsEnumerable().ToDictionary(x => x.Id, x => x.Name);

            bool isExist = false;
            for (int i = 1; i <= list.Count; i++)
            {
                for (int j = 0; j < lessons.Count; j++)
                {
                    if (list[i] == lessons[j])
                    {
                        isExist = true;
                        break;
                    }
                }

                if (isExist)
                    finalLessons.Add(list[i]);

                isExist = false;
            }
            scheduleViewModel.MaxLessonsInSimilarTime = maxLessonsInSimilarTime;
            scheduleViewModel.LessonsTime = finalLessons;

            return scheduleViewModel;
        }

        public ScheduleViewModel GenerateScheduleForGroup(int groupId, DateTime dateFrom, DateTime dateTo)
        {
            var memberOfGroup = DataProvider.SostavGrupp.FirstOrDefault(p => p.IdSpisGroup == groupId);
            return memberOfGroup == null ? null : GenerateSchedule(memberOfGroup.Student_StudentMeta.UserId, dateFrom, dateTo);
        }

        #endregion

        #region Schedual Excell
        public byte[] GetScheduleInExcellFormat(int userId, DateTime dateFrom, DateTime dateTo, string templatePath)
        {
            var activeFilePath = Path.GetTempPath() + Guid.NewGuid() + ".xlsx";

            File.Copy(templatePath, activeFilePath);

            #region InitializeData data
            List<IGrouping<DateTime, Raspisanie>> schedulesDateGroups;

            var user = DataProvider.Users.FirstOrDefault(p => p.Id == userId);
            if (user.Prepod_PrepodMeta != null && user.Prepod_PrepodMeta.Any())
            {
                var prepodIds = DataProvider.PrepodiCafedri.Filter(p => p.Prepod_PrepodMeta.UserId == userId)
                                           .Select(p => p.Id);


                schedulesDateGroups = DataProvider.Raspisanie.Filter(p => prepodIds.Contains(p.IdPrepodaCafedri) && p.Data >= dateFrom && p.Data <= dateTo)
                                                 .GroupBy(p => p.Data)
                                                 .OrderBy(p => p.Key)
                                                 .ToList();
            }

            else
            {
                var userGroup = user.Student_StudentMeta.FirstOrDefault()
                                    .SostavGrupp.OrderByDescending(p => p.DateEdit)
                                               .FirstOrDefault();

                var flow = userGroup.SpisokGrupp.Pot;
                var groupId = userGroup.SpisokGrupp.IdGroup;
                var subGroup = userGroup.Podgroup;
                var specialityId = userGroup.SpisokGrupp.IdSpezMeta;
                schedulesDateGroups = DataProvider.Raspisanie.Filter(p => p.Data >= dateFrom &&
                                                                           p.Data <= dateTo &&
                                                                           p.SpezialRazdeliDisziplini.IdSpezMeta == specialityId &&
                                                                           p.Pot == flow &&
                                                                           (!p.IdSpiskaGrupp.HasValue || p.IdSpiskaGrupp == groupId) &&
                                                                           (!p.NomerPodGrupp.HasValue || p.NomerPodGrupp == subGroup)
                                                                          )
                                                .GroupBy(p => p.Data)
                                                .OrderBy(p => p.Key)
                                                .ToList();
            }

            #endregion


            #region RenderData

            #region TempData

            var currentUserMetaData = user.UserMeta.FirstOrDefault();

            DateTime currentDate;
            int numberCell = 5;
            int maxSizeColumn = 0;
            int weeksInRow = 7;
            int currentNumberCell = 0;
            char cellLetterAddress = 'A';

            #endregion

            using (var workbook = new XLWorkbook())
            {
                var ws = workbook.Worksheets.Add("Расписание");
                ws.Cell("A1").Value = "Расписание для:";

                #region CheckUser
                if (DataProvider.PrepodiCafedri.Filter(p => p.Prepod_PrepodMeta.UserId == userId)
                                          .Select(p => p.Id).Count() > 0)
                {
                    ws.Cell("B1").Value = currentUserMetaData.LastName
                                            + " " + currentUserMetaData.FirstName[0]
                                            + "." + currentUserMetaData.MiddleName[0]
                                            + ".";
                }
                else
                {
                    var stud = user.Student_StudentMeta.FirstOrDefault();
                    if (stud != null)
                    {
                        var NumGroup = stud.SostavGrupp.FirstOrDefault().SpisokGrupp.Gruppa;
                        var GodPostup = stud.Spezialnost_SpezialnostMeta.GodPostup;
                        var Special = stud.Spezialnost_SpezialnostMeta.Spezialnost.Spez;
                        ws.Cell("B1").Value = Special + GodPostup + "-" + NumGroup + " группа";
                    }
                }
                #endregion

                ws.Cell("A2").Value = "Сгенирировано на сроки:";
                ws.Range("A2:B2").Merge();
                ws.Cell("A3").Value = "от:" + dateFrom.Day + "." + dateFrom.Month + "." + dateFrom.Year;
                ws.Cell("B3").Value = "до:" + dateTo.Day + "." + dateTo.Month + "." + dateTo.Year;
                var scheduleDay = new ScheduleDayViewModel();

                int count = 0; bool IsDateNoReply, firstColumn = true, firstday = true;

                foreach (var schedulesDateGroup in schedulesDateGroups)
                {
                    IsDateNoReply = true;
                    currentDate = schedulesDateGroup.FirstOrDefault().Data;
                    scheduleDay.Lessons = schedulesDateGroup.OrderBy(p => p.VremyaZanyatia.StartTime)
                                                     .Select(LessonViewModel.ToLessonViewModel)
                                                     .ToList();


                    if (currentDate.DayOfWeek.ToString() == "Monday" || firstColumn)
                    {
                        if (!firstColumn)
                        {
                            count++;
                            if (numberCell > maxSizeColumn)
                                maxSizeColumn = numberCell;
                            if (count == weeksInRow)
                            {
                                cellLetterAddress = 'A'; numberCell = ++maxSizeColumn; count = 0; maxSizeColumn = 0;
                            }
                            else
                            {
                                cellLetterAddress = (Char)(Convert.ToUInt16(cellLetterAddress) + 5); numberCell = currentNumberCell;
                            }
                        }
                        string[] shapka = new string[] { "Дата занятия", "Время", "Занятие", "Тип занятия", "Аудитория" };
                        for (int i = 0; i < shapka.Length; i++)
                        {
                            ws.Cell((cellLetterAddress) + numberCell.ToString()).Style.Font.FontColor = XLColor.White;
                            ws.Cell((cellLetterAddress) + numberCell.ToString()).Style.Fill.BackgroundColor = XLColor.Black;
                            ws.Cell((cellLetterAddress++) + numberCell.ToString()).Value = shapka[i];
                        }
                        cellLetterAddress = (Char)(Convert.ToUInt16(cellLetterAddress) - 5); numberCell++;
                        firstColumn = false; firstday = true;
                    }


                    foreach (var item in scheduleDay.Lessons)
                    {
                        if (IsDateNoReply)
                        {
                            ws.Cell((cellLetterAddress) + numberCell.ToString()).Style.Fill.BackgroundColor = XLColor.Aqua;
                            ws.Cell((cellLetterAddress++) + numberCell.ToString()).Value = currentDate.Day.ToString() + "." + currentDate.Month.ToString() + " \n" + _rusDay[(int)currentDate.DayOfWeek - 1];
                            if (firstday)
                                currentNumberCell = numberCell - 1;
                        }
                        else
                            cellLetterAddress++;
                        firstday = false;
                        IsDateNoReply = false;
                        ws.Cell((cellLetterAddress++) + numberCell.ToString()).Value = item.LessonTime;
                        ws.Cell((cellLetterAddress++) + numberCell.ToString()).Value = item.Discipline.ShortName;
                        ws.Cell((cellLetterAddress++) + numberCell.ToString()).Value = item.LessonTypeShortName;
                        ws.Cell((cellLetterAddress++) + numberCell.ToString()).Value = item.Classroom;
                        cellLetterAddress = (Char)(Convert.ToUInt16(cellLetterAddress) - 5);
                        numberCell++;
                    }
                }
                workbook.SaveAs(activeFilePath);
            }
            #endregion


            var fileBinaryData = File.ReadAllBytes(activeFilePath);
            File.Delete(activeFilePath);

            return fileBinaryData;
        }

        public byte[] GetScheduleInExcellFormatForGroup(int groupId, DateTime dateFrom, DateTime dateTo, string templatePath)
        {
            var memberOfGroup = DataProvider.SostavGrupp.FirstOrDefault(p => p.IdSpisGroup == groupId);
            return memberOfGroup == null ? null : GetScheduleInExcellFormat(memberOfGroup.Student_StudentMeta.UserId, dateFrom, dateTo, templatePath);
        }
        #endregion

        #region SchedualConfig
        public Dictionary<int, string> GetPrepodsForSchedualConfiguration()
        {
            return ServiceCache.GetOrAdd("GetPrepodsForSchedualConfiguration",
                () => DataProvider.Users.Filter(p => p.Prepod_PrepodMeta.Any() && p.UserMeta.Any())
                    .ToList()
                    .Select(p => p.UserMeta.First())
                    .OrderBy(p => p.LastName + " " + p.FirstName + " " + p.MiddleName)
                    .ToDictionary(p => p.UserId, p => p.LastName + " " + p.FirstName + " " + p.MiddleName));
        }

        public Dictionary<int, string> GetSpezialnostiForConfiguration()
        {
            return ServiceCache.GetOrAdd("GetSpezialnostiForConfiguration",
               () => DataProvider.Spezialnost.GetAll()
                                 .OrderBy(p => p.Spezialnost1)
                                 .ToDictionary(p => p.IdSpez, p => p.Spezialnost1));

        }

        public int[] GetGodiPostupForConfiguration(int specialnostId)
        {
            return ServiceCache.GetOrAdd("GetGodiPostupForConfiguration" + specialnostId + "|",
                () =>
                {
                    //TODO: возможно нужно ограничить годы поступления
                    return DataProvider.SpisokGrupp.Filter(
                        p => p.Spezialnost_SpezialnostMeta.Spezialnost.IdSpez == specialnostId)
                        .Select(p => p.Spezialnost_SpezialnostMeta.GodPostup)
                        .Distinct()
                        .OrderBy(p => p)
                        .ToArray();
                });
        }

        public int[] GetGodiPostupForConfiguration(int formOfStudyId, int specialnostId)
        {
            return ServiceCache.GetOrAdd("GetGodiPostupForConfiguration" + specialnostId + "|",
                () =>
                {
                    //TODO: возможно нужно ограничить годы поступления
                    return DataProvider.SpisokGrupp.Filter(
                        p => p.Spezialnost_SpezialnostMeta.Spezialnost.IdSpez == specialnostId && p.Spezialnost_SpezialnostMeta.IdFrmObuch == formOfStudyId)
                        .Select(p => p.Spezialnost_SpezialnostMeta.GodPostup)
                        .Distinct()
                        .OrderBy(p => p)
                        .ToArray();
                });
        }

        public Dictionary<int, string> GetGroupsForConfiguration(int specialnostId, int godPostup)
        {
            return ServiceCache.GetOrAdd("GetGroupsForConfiguration" + specialnostId + "|" + godPostup + "|",
               () => DataProvider.SpisokGrupp.Filter(
                   p =>
                       p.Spezialnost_SpezialnostMeta.Spezialnost.IdSpez == specialnostId &&
                       p.Spezialnost_SpezialnostMeta.GodPostup == godPostup)
                   .OrderBy(p => p.Gruppa)
                   .ToList()
                   .ToDictionary(p => p.IdGroup, p => (p.Gruppa + " (" + p.PotokDictionary.Name + ")")));
        }


        public Dictionary<int, string> GetFormOfStudyForConfiguration(int specialostId)
        {

            Dictionary<int, string> formsOfStudy = DataProvider.SpezialnostMeta
                .FilterNoTracking(x => x.IdSpez == specialostId)
                .ToList()
                .Select(x => new
            {
                Id = x.IdFrmObuch,
                Name = x.FormObuchDictionary.Name
            }).Distinct().ToDictionary(x => x.Id, x => x.Name);

            return formsOfStudy;
        }

        #endregion


        private readonly string[] _rusDay = { "Понедельник", "Вторник", "Среда", "Четверг", "Пятница", "Суббота", "Воскресенье" };
    }
}
