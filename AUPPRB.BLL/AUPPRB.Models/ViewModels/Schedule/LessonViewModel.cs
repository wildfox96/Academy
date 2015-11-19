using System;
using System.Linq;
using AUPPRB.Common.Utils;
using AUPPRB.Models.DomainModels;
using AUPPRB.Models.ViewModels.Discipline;
using AUPPRB.Models.ViewModels.UsersModels;

namespace AUPPRB.Models.ViewModels.Schedule
{
    public class LessonViewModel
    {
        public int Id { get; set; }

        public DisciplineMetaViewModel Discipline { get; set; }

        public int LessonTypeId { get; set; }
        public string LessonTypeName { get; set; }
        public string LessonTypeShortName { get; set; }

        public UserMetaViewModel PropodMeta { get; set; }

        public int FlowId { get; set; }

        public string  FlowName { get; set; }

        public int? GroupId { get; set; }

        public string  GroupName { get; set; }

        public int? SubGroupId { get; set; }

        public string SubGroupName { get; set; }

        public int SpecialityId { get; set; }
        public string SpecialityName { get; set; }

        public int FormOfStudyId { get; set; }
        public string FormOfStudyName { get; set; }

        public string LessonTime { get; set; }
        public DateTime LessonStartTime { get; set; }

        public string Classroom { get; set; }



        public static LessonViewModel ToLessonViewModel(Raspisanie p)
        {
            return new LessonViewModel()
            {
                Id = p.Id,
                //предмет
                Discipline = new DisciplineMetaViewModel()
                {
                    Id = p.SpezialRazdeliDisziplini.Discipline.Id,
                    FullName = p.SpezialRazdeliDisziplini.Discipline.Name,
                    ShortName = p.SpezialRazdeliDisziplini.Discipline.ShortName,
                    TranslitName = TranslitHelper.RusToEng(p.SpezialRazdeliDisziplini.Discipline.ShortName)
                },
                //учебная информация о занятии
                FlowId = p.Pot,
                FlowName = p.PotDictionary.Name,
                GroupId = p.IdSpiskaGrupp,
                GroupName = p.IdSpiskaGrupp.HasValue ? p.SpisokGrupp.Gruppa : "",
                SubGroupId = p.NomerPodGrupp,
                SubGroupName = p.NomerPodGrupp.HasValue ? p.NomerPodGrupp.Value.ToString() : "",
                SpecialityId = p.SpezialRazdeliDisziplini.IdSpezMeta,
                SpecialityName = p.SpezialRazdeliDisziplini.Spezialnost_SpezialnostMeta.Spezialnost.Spez +
                                 p.SpezialRazdeliDisziplini.Spezialnost_SpezialnostMeta.GodPostup,
                FormOfStudyId = p.SpezialRazdeliDisziplini.Spezialnost_SpezialnostMeta.IdFrmObuch,
                FormOfStudyName = p.SpezialRazdeliDisziplini.Spezialnost_SpezialnostMeta.FormObuchDictionary.Name,
                ////время занятия
                LessonTime = p.VremyaZanyatia.StartTime.ToString("H.mm") + "-" +
                             p.VremyaZanyatia.EndTime.ToString("H.mm"),
                LessonStartTime = p.VremyaZanyatia.StartTime,

                LessonTypeId = p.VidiRabot.Id,
                LessonTypeName = p.VidiRabot.VidRabSokr,
                LessonTypeShortName = p.VidiRabot.VidRabAbbreviatura,
                Classroom = p.Auditoriya,

                PropodMeta = new UserMetaViewModel()
                {
                    UserId = p.Prepod_PrepodiCafedri.Prepod_PrepodMeta.UserId,
                    UserFullName = p.Prepod_PrepodiCafedri.Prepod_PrepodMeta.User.UserMeta.First().LastName + " " +
                                   p.Prepod_PrepodiCafedri.Prepod_PrepodMeta.User.UserMeta.First().FirstName + " " +
                                   p.Prepod_PrepodiCafedri.Prepod_PrepodMeta.User.UserMeta.First().MiddleName
                }
            };
        }
    }
}
