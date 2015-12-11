using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AUPPRB.Domain.Interfaces;
using AUPPRB.Models.DomainModels;
using AUPPRB.Models.ViewModels.Schedule;
using AUPPRB.Repository.DB;

namespace AUPPRB.Domain.Services
{
    public class AdminScheduleService : BaseServices, IAdminScheduleService
    {
        private IDataProvider _dataProvider;
        public AdminScheduleService(IDataProvider dataProvider)
        {
            _dataProvider = dataProvider;

        }

        public IEnumerable<SelectListItem> GetSpezialnosti()
        {

            IEnumerable<SelectListItem> disciplines = _dataProvider.Spezialnost.GetAllNoTracking()
                .Select(x => new { x.IdSpez, x.Spezialnost1 })
                .AsEnumerable()
                .Select(x => new SelectListItem() { Text = x.Spezialnost1, Value = x.IdSpez.ToString() });

            return disciplines;
        }


        public IEnumerable<SelectListItem> GetPotoki()
        {
            IEnumerable<SelectListItem> potoki = _dataProvider.Dictionaries.GetAllNoTracking()
                .Where(x => x.DictionaryType.Name == "Потоки")
                .Select(x => new { x.Id, x.Name })
                .AsEnumerable()
                .Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString() });

            return potoki;
        }


        public List<VremyaZanyatia> GetTimeOfPairs()
        {
            List<VremyaZanyatia> timeOfPairs = _dataProvider.VremyaZanyatia.GetAllNoTracking().ToList();

            return timeOfPairs;
        }

        public IEnumerable<SelectListItem> GetGodPostuplinia(int spezialnostId)
        {
            IEnumerable<SelectListItem> godiPostuplinia = _dataProvider.SpezialnostMeta.FilterNoTracking(x => x.IdSpez == spezialnostId)
                .Select(x => new { x.GodPostup })
                .AsEnumerable()
                .Select(x => new SelectListItem() { Value = x.GodPostup.ToString(), Text = x.GodPostup.ToString() });

            return godiPostuplinia;
        }



        public Spezialnost GetSpezialnostByName(string spezialnost)
        {
            Spezialnost spez = _dataProvider.Spezialnost.FirstOrDefault(x => x.Spezialnost1 == spezialnost);

            return spez;

        }


        public Spezialnost_SpezialnostMeta GetSpezialnostMeta(int spezialnostId, int year)
        {
            Spezialnost_SpezialnostMeta spezialnostSpezialnostMeta = _dataProvider.SpezialnostMeta.FirstOrDefault(x => x.IdSpez == spezialnostId && x.GodPostup == year);

            return spezialnostSpezialnostMeta;
        }


        public SpezialRazdeliDisziplini GetSpezialRazdeliDisziplini(int spezMeta)
        {
            SpezialRazdeliDisziplini spezialRazdeliDisziplini = _dataProvider.SpezialRazdeliDisziplini.FirstOrDefault(x => x.IdSpezMeta == spezMeta);

            return spezialRazdeliDisziplini;
        }


        /// <summary>
        /// Выбрать расписание на один день для всего потока
        /// </summary>
        /// <param name="date"></param>
        /// <param name="potokId"></param>
        /// <param name="idUchPlanaDisziplini"></param>
        /// <returns></returns>
        public ScheduleDayViewModel GetRaspisanieForADay(DateTime date, int potokId, int idUchPlanaDisziplini)
        {
            SpezialRazdeliDisziplini spezialRazdeliDisziplini = _dataProvider.SpezialRazdeliDisziplini.FirstOrDefault(x => x.IdUpPlanDisciplini == idUchPlanaDisziplini);


            List<IGrouping<DateTime, Raspisanie>> raspisanie = DataProvider.Raspisanie.Filter(p => p.Data == date &&
                                                            p.SpezialRazdeliDisziplini.IdSpezMeta == spezialRazdeliDisziplini.IdSpezMeta &&
                                                            p.Pot == potokId)
                                                            .GroupBy(p => p.Data).OrderBy(x => x.Key).ToList();


            var dateOfDay = new DateTime();

            var scheduleDay = new ScheduleDayViewModel();

            List<string> lessonsTimes = new List<string>();
            #region Формирование расписания

            foreach (IGrouping<DateTime, Raspisanie> item in raspisanie)
            {
                dateOfDay = item.Key;
                scheduleDay.DateOfDay = dateOfDay.ToString("yyyy-MM-dd");
                //scheduleDay.IsCurrentDay = dateOfDay == DateTime.Now.Date;
                //scheduleDay.IsPast = dateOfDay == DateTime.Now.Date;

                #region Добавляем уроки

                scheduleDay.Lessons = item.OrderBy(p => p.VremyaZanyatia.StartTime)
                                                   .Select(LessonViewModel.ToLessonViewModel)
                                                   .ToList();
                #endregion

                scheduleDay.MaxLessonsInSimilarTime = scheduleDay.Lessons.GroupBy(p => p.LessonTime).Select(p => p.Count()).Max(p => p);


                List<string> vremyaZanyatiu =
                    _dataProvider.VremyaZanyatia.GetAllNoTracking()
                        .ToList()
                        .Select(x => x.StartTime.ToString("H.mm") + "-" + x.EndTime.ToString("H.mm"))
                        .ToList();
                lessonsTimes.AddRange(vremyaZanyatiu);
                scheduleDay.LessonsTime = lessonsTimes;
            }

            #endregion

            return scheduleDay;
        }

        /// <summary>
        /// Расписание для препода
        /// </summary>
        /// <param name="date"></param>
        /// <param name="prepodId"></param>
        /// <returns></returns>
        public ScheduleDayViewModel GetRaspisanieForADay(DateTime date, int prepodId)
        {
            int[] prepodIds = DataProvider.PrepodiCafedri.Filter(p => p.Prepod_PrepodMeta.UserId == prepodId)
                .Select(p => p.Id).ToArray();


            List<IGrouping<DateTime, Raspisanie>> raspisanie = DataProvider.Raspisanie
.Filter(p => prepodIds.Contains(p.IdPrepodaCafedri) && p.Data == date)
                .GroupBy(p => p.Data)
                .OrderBy(p => p.Key)
                .ToList();

            var dateOfDay = new DateTime();

            var scheduleDay = new ScheduleDayViewModel();

            List<string> lessonsTimes = new List<string>();
            List<string> vremyaZanyatiu =
                   _dataProvider.VremyaZanyatia.GetAllNoTracking()
                       .ToList()
                       .Select(x => x.StartTime.ToString("H.mm") + "-" + x.EndTime.ToString("H.mm"))
                       .ToList();
            #region Формирование расписания

            foreach (IGrouping<DateTime, Raspisanie> item in raspisanie)
            {
                dateOfDay = item.Key;
                scheduleDay.DateOfDay = dateOfDay.ToString("yyyy-MM-dd");


                #region Добавляем уроки

                scheduleDay.Lessons = item.OrderBy(p => p.VremyaZanyatia.StartTime)
                                                   .Select(LessonViewModel.ToLessonViewModel)
                                                   .ToList();
                #endregion

                scheduleDay.MaxLessonsInSimilarTime = scheduleDay.Lessons.GroupBy(p => p.LessonTime).Select(p => p.Count()).Max(p => p);



                lessonsTimes.AddRange(vremyaZanyatiu);
                scheduleDay.LessonsTime = lessonsTimes;
            }

            #endregion
            return scheduleDay;
        }

        public Dictionary<int, string> GetPotoki(int idSpez, int yearOfEntering)
        {
            Spezialnost_SpezialnostMeta spezialnostSpezialnostMeta =
                _dataProvider.SpezialnostMeta.FirstOrDefault(x => x.IdSpez == idSpez && x.GodPostup == yearOfEntering);

            var potokiTest = _dataProvider.SpisokGrupp.FilterNoTracking(x => x.IdSpezMeta == spezialnostSpezialnostMeta.IdSpez).Select(x => new { Id = x.Pot, Name = x.PotokDictionary.Name })
                   .Distinct().ToList();

            Dictionary<int, string> potoki = _dataProvider.SpisokGrupp.FilterNoTracking(x => x.IdSpezMeta == spezialnostSpezialnostMeta.IdSpez)
                .Select(x => new { Id = x.Pot, Name = x.PotokDictionary.Name }).Distinct()
                .ToDictionary(x => x.Id, x => x.Name);


            return potoki;
        }

        public Dictionary<int, string> GetTypesOfPara()
        {
            Dictionary<int, string> typesOfPara = _dataProvider.VidiRabot.GetAllNoTracking()
                .Select(x => new { Id = x.Id, Name = x.VidRabSokr })
                .ToDictionary(x => x.Id, x => x.Name);
            return typesOfPara;
        }


        public Dictionary<int, string> GetVremiaZaniatiu()
        {
            Dictionary<int, string> vremiaZanitiu = _dataProvider.VremyaZanyatia.GetAllNoTracking()
                .ToList()
                .Select(
                    x => new { Id = x.Id, Name = x.StartTime.ToShortTimeString() + "-" + x.EndTime.ToShortTimeString() }).ToDictionary(x => x.Id, x => x.Name);

            return vremiaZanitiu;
        }


        public Dictionary<int, string> GetDisziplini()
        {
            //TODO доставать дисциплины нуно будет из тб спец раздел дисц
            Dictionary<int, string> disciplines = _dataProvider.Disciplines.FilterNoTracking(x => x.IsActive)
                .ToList()
                .Select(x => new { Id = x.Id, Name = x.Name })
                .ToDictionary(x => x.Id, x => x.Name);
            return disciplines;
        }

        //TODO: нужно брать id из prepodKafedri
        public Dictionary<int, string> GetPrepodiDisciplines(int disciplineId)
        {
            List<int> prepodDisciplinesIds
                = _dataProvider.PrepodDiscipline.FilterNoTracking(x => x.IdDisciplini == disciplineId).Select(x => x.IdSotr).ToList();
            List<int> prepodPrepodMetasIds = _dataProvider.PrepodMetadata.FilterNoTracking(x => prepodDisciplinesIds.Any(z => z == x.Id)).ToList().Select(x => x.UserId).ToList();

            Dictionary<int, string> prepods = _dataProvider.UserMetadata.FilterNoTracking(x => prepodPrepodMetasIds.Any(z => z == x.UserId))
               .ToList()
               .Select(x => new
               {
                   Id = x.User.Prepod_PrepodMeta.Select(z => z.Prepod_PrepodiCafedri.FirstOrDefault(k => k.IdSotr == z.Id).Id).FirstOrDefault(),
                   Name = x.FirstName + " " + x.LastName + " " + x.MiddleName
               }).ToDictionary(x => x.Id, x => x.Name);

            return prepods;
        }


        public Dictionary<int, string> GetDisciplinesOfPrepod(int prepodId)
        {
            Prepod_PrepodMeta prepodPrepodMeta = _dataProvider.PrepodMetadata.FirstOrDefault(x => x.UserId == prepodId);
            Dictionary<int, string> disziplines = _dataProvider.PrepodDiscipline.FilterNoTracking(x => x.IdSotr == prepodPrepodMeta.Id).ToList().Select(x => new
            {
                Id = x.IdDisciplini,
                Name = x.Discipline.Name
            }).ToDictionary(x => x.Id, x => x.Name);

            return disziplines;
        }


        public Dictionary<int, string> GetGroups(int idSpezMeta)
        {
            Dictionary<int, string> groups = _dataProvider.SpisokGrupp.FilterNoTracking(x => x.IdSpezMeta == idSpezMeta)
                .Select(x => new { Id = x.IdGroup, Name = x.Gruppa })
                .ToDictionary(x => x.Id, x => x.Name);

            return groups;
        }



        public Dictionary<int, string> GetSubGroups(int idSpezMeta)
        {
            int kolPodGrupp = _dataProvider.SpisokGrupp.FilterNoTracking(x => x.IdSpezMeta == idSpezMeta).ToList().Select(x => x.KolPodGroup).Max();

            Dictionary<int, string> subGrups = new Dictionary<int, string>();

            for (int i = 1; i < kolPodGrupp + 1; i++)
            {
                subGrups.Add(i, string.Format("{0} подгруппа", i));
            }

            return subGrups;

        }

        public Dictionary<int, string> GetInstitutes()
        {
            Dictionary<int, string> instituti = _dataProvider.Dictionaries.FilterNoTracking(x => x.DictionaryTypeId == 17).Select(x => new
            {
                Id = x.Id,
                Name = x.Name
            }).ToDictionary(x => x.Id, x => x.Name);

            return instituti;
        }


        public Raspisanie AddPara(ParaViewModel model)
        {
            SpezialRazdeliDisziplini spezialRazdeliDisziplini = _dataProvider.SpezialRazdeliDisziplini.FirstOrDefault(
                x => x.IdDisciplini == model.DisciplineId && x.IdSpezMeta == model.IdSpezMeta);

            DateTime dateOfPara = DateTime.Parse(model.DateOfPara);

            Raspisanie raspisanie = new Raspisanie()
            {
                Data = dateOfPara,
                IdUchPlanaDisciplini = spezialRazdeliDisziplini.IdUpPlanDisciplini,
                IdVidaRaboti = model.TypeOfParaId,
                IdVremyaZanyatia = model.TimeOfParaId,
                Pot = model.PotokId,
                IdSpiskaGrupp = model.GroupId != 0 ? model.GroupId : (int?)null,
                NomerPodGrupp = model.SubgroupId != 0 ? model.SubgroupId : (int?)null,
                IdPrepodaCafedri = model.PrepodiDisziplineId,
                NomerZanyatiaVSemestre = 0,
                Auditoriya = model.Auditory,
                TipNagruzki = 80
            };

            bool isParaExist = CheckForParaIdentity(raspisanie);
            bool isGroupHasNakladki = PrivateCheckForNakladkiForGroup(raspisanie);
            bool isPrepodHasNakladki = PrivateCheckForNakladkiForPrepod(raspisanie);

            if (!isParaExist && !isGroupHasNakladki && !isPrepodHasNakladki)
            {
                _dataProvider.Raspisanie.Add(raspisanie);
                _dataProvider.Save();
            }
            else
            {
                if (model.GroupId != 0)
                {
                    throw new ArgumentException("У группы накладки, измените время/или дату пары");
                }
                else
                {
                    throw new ArgumentException("У потока накладки, измените время/дату пары");
                }
            }


            return raspisanie;
        }

        public Raspisanie AddParaForPrepod(ParaViewModel model)
        {
            Spezialnost_SpezialnostMeta spezialnostSpezialnostMeta =
                _dataProvider.SpezialnostMeta.FirstOrDefault(
                    x => x.IdSpez == model.SpezialnostId && x.GodPostup == model.Year);

            SpezialRazdeliDisziplini spezialRazdeliDisziplini = _dataProvider.SpezialRazdeliDisziplini.FirstOrDefault(
               x => x.IdDisciplini == model.DisciplineId && x.IdSpezMeta == spezialnostSpezialnostMeta.Id);

            DateTime dateOfPara = DateTime.Parse(model.DateOfPara);

            Prepod_PrepodMeta prepodPrepodMeta = _dataProvider.PrepodMetadata.FirstOrDefault(x => x.UserId == model.PrepodId);
            Prepod_PrepodiCafedri prepodPrepodiCafedri = _dataProvider.PrepodiCafedri.FirstOrDefault(x => x.IdSotr == prepodPrepodMeta.Id);

            Raspisanie raspisanie = new Raspisanie()
            {
                Data = dateOfPara,
                IdUchPlanaDisciplini = spezialRazdeliDisziplini.IdUpPlanDisciplini,
                IdVidaRaboti = model.TypeOfParaId,
                IdVremyaZanyatia = model.TimeOfParaId,
                Pot = model.ПотокИд,//для преподов на русском
                IdSpiskaGrupp = model.GroupId != 0 ? model.GroupId : (int?)null,
                NomerPodGrupp = model.SubgroupId != 0 ? model.SubgroupId : (int?)null,
                IdPrepodaCafedri = prepodPrepodiCafedri.Id,
                NomerZanyatiaVSemestre = 0,
                Auditoriya = model.Auditory,
                TipNagruzki = 80
            };

            bool isParaExist = CheckForParaIdentity(raspisanie);
            bool isPrepodHasNakladki = PrivateCheckForNakladkiForPrepod(raspisanie);
            bool isGroupHasNakladki = PrivateCheckForNakladkiForGroup(raspisanie);

            if (!isParaExist && !isPrepodHasNakladki && !isGroupHasNakladki)
            {
                _dataProvider.Raspisanie.Add(raspisanie);
                _dataProvider.Save();
            }
            else
            {
                throw new ArgumentException("У преподавателя накладки, измените время/дату пары");
            }


            return raspisanie;
        }

        public void DeletePara(int paraId)
        {
            Raspisanie raspisanie = _dataProvider.Raspisanie.FirstOrDefault(x => x.Id == paraId);

            _dataProvider.Raspisanie.Delete(raspisanie);

            _dataProvider.Save();
        }

        public Raspisanie GetRaspisanie(int paraId)
        {
            Raspisanie raspisanie = _dataProvider.Raspisanie.FirstOrDefault(x => x.Id == paraId);

            return raspisanie;
        }


        public IEnumerable<SelectListItem> GetDiszipliniFromSpezialRazdeliDisziplini(int id)
        {
            Raspisanie raspisanie = _dataProvider.Raspisanie.FirstOrDefault(x => x.Id == id);

            IEnumerable<SelectListItem> disziplini = _dataProvider.SpezialRazdeliDisziplini.FilterNoTracking(
                x => x.IdSpezMeta == raspisanie.SpezialRazdeliDisziplini.IdSpezMeta).Select(x => new
                {
                    Id = x.Discipline.Id,
                    Name = x.Discipline.Name
                }).AsEnumerable().Select(x => new SelectListItem()
                {
                    Selected = raspisanie.SpezialRazdeliDisziplini.Discipline.Id == x.Id,
                    Text = x.Name,
                    Value = x.Id.ToString()
                });

            return disziplini;
        }

        public Dictionary<int, string> GetAllPrepods()
        {
            IEnumerable<Prepod_PrepodiCafedri> prepodPrepodiCafedris = _dataProvider.PrepodiCafedri.GetAllNoTracking().AsEnumerable();

            Dictionary<int, string> prepodi = prepodPrepodiCafedris.Select(x => new
            {
                Id = x.Id,
                Name = x.Prepod_PrepodMeta.User.UserMeta.FirstOrDefault(z => z.UserId == x.Prepod_PrepodMeta.UserId).LastName
            }).AsEnumerable().ToDictionary(x => x.Id, x => x.Name);


            return prepodi;
        }


        /// <summary>
        /// Проверка на идентичность пары(дублирование)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private bool CheckForParaIdentity(Raspisanie model)
        {
            bool isParaExist = _dataProvider.Raspisanie.FilterNoTracking(x => x.Data == model.Data)
                .Any(x => x.Auditoriya == model.Auditoriya &&
                        x.IdPrepodaCafedri == model.IdPrepodaCafedri &&
                        x.IdVremyaZanyatia == model.IdVremyaZanyatia &&
                        x.IdUchPlanaDisciplini == model.IdUchPlanaDisciplini &&
                        x.IdVidaRaboti == model.IdVidaRaboti &&
                        x.Pot == model.Pot &&
                        x.IdSpiskaGrupp == model.IdSpiskaGrupp &&
                        x.NomerPodGrupp == model.NomerPodGrupp);
            return isParaExist;
        }


        /// <summary>
        /// проверка на то чтобы в одно и то же время у препода не было другой пары
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private bool PrivateCheckForNakladkiForPrepod(Raspisanie model)
        {

            bool isPrepodHasNakladki
                = _dataProvider.Raspisanie.FilterNoTracking(
                x => x.Data == model.Data && x.IdPrepodaCafedri == model.IdPrepodaCafedri)
                .Any(x => x.IdVremyaZanyatia == model.IdVremyaZanyatia);

            return isPrepodHasNakladki;
        }

        /// <summary>
        /// проверка на то чтобы в одно и то же время у группы не было другой пары
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool PrivateCheckForNakladkiForGroup(Raspisanie model)
        {
            bool isGroupHasAnotherParaInTheSameTime;

            List<Raspisanie> raspisanies = _dataProvider.Raspisanie.FilterNoTracking(
                x => x.Data == model.Data && x.Pot == model.Pot && x.IdVremyaZanyatia == model.IdVremyaZanyatia && x.IdVidaRaboti == 5).ToList();
            if (raspisanies.Count > 0)
            {
                isGroupHasAnotherParaInTheSameTime = true;

            }
            else
            {
                isGroupHasAnotherParaInTheSameTime = _dataProvider.Raspisanie.FilterNoTracking(
               x => x.Data == model.Data && x.Pot == model.Pot && x.IdSpiskaGrupp == model.IdSpiskaGrupp && x.NomerPodGrupp == model.NomerPodGrupp)
               .Any(x => x.IdVremyaZanyatia == model.IdVremyaZanyatia);
            }

            return isGroupHasAnotherParaInTheSameTime;

        }
                    //Изменение функциональности на странице Добавление пользователя (инфа о поступлении)
        public IEnumerable<Spezialnost_SpezialnostMeta> GetSpezialnostMeta(int spezialnostId)
        {


            IEnumerable<Spezialnost_SpezialnostMeta> spezialnostSpezialnostMeta = _dataProvider.SpezialnostMeta.GetAll().Where(x => x.IdSpez == spezialnostId).AsEnumerable<Spezialnost_SpezialnostMeta>();

            return spezialnostSpezialnostMeta;
        }

        public List<SpisokGrupp> GetPotokiForSpezMeta(int idSpezialnostMeta)
        {

            List<SpisokGrupp> potoki = _dataProvider.SpisokGrupp.FilterNoTracking(x => x.IdSpezMeta == idSpezialnostMeta).ToList<SpisokGrupp>();

            return potoki;
        }

        public List<SpisokGrupp> GetSpisokGrupp(int idPotok, int idSpezMeta)
        {
            List<SpisokGrupp> groups = _dataProvider.SpisokGrupp
                .FilterNoTracking(x => x.IdSpezMeta == idSpezMeta & x.Pot == idPotok)
                .ToList<SpisokGrupp>();

            return groups;
        }

        public List<Spezialnost> GetListOfSpecialities(int facultyId)
        {
            if (facultyId == 7)
                return DataProvider.Spezialnost.GetAll().Where(p => p.IdSpez == 1).ToList();
            else if (facultyId == 6)
                return DataProvider.Spezialnost.GetAll().Where(p => p.IdSpez != 1).ToList();
            else return DataProvider.Spezialnost.GetAll().ToList();
        }

        public Spezialnost_SpezialnostMeta GetSpezMetaInfo(int spezId)
        {
           return _dataProvider.SpezialnostMeta.GetAll().FirstOrDefault(p => p.Id == spezId);
        
        }

    }
}
