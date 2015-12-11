using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using AUPPRB.Models.DomainModels;
using AUPPRB.Models.ViewModels.Schedule;

namespace AUPPRB.Domain.Interfaces
{
    public interface IAdminScheduleService
    {
        /// <summary>
        /// Get all disciplines
        /// </summary>
        /// <returns></returns>
        IEnumerable<SelectListItem> GetSpezialnosti();

        /// <summary>
        /// Выбрать все потоки
        /// </summary>
        /// <returns></returns>
        IEnumerable<SelectListItem> GetPotoki();

        /// <summary>
        /// Выбрать время занятий
        /// </summary>
        /// <returns></returns>
        List<VremyaZanyatia> GetTimeOfPairs();

        /// <summary>
        /// Выбрать все годы поступления
        /// </summary>
        /// <param name="spezialnostId"></param>
        /// <returns></returns>
        IEnumerable<SelectListItem> GetGodPostuplinia(int spezialnostId);

        /// <summary>
        /// Get spezialnost by name
        /// </summary>
        /// <param name="spezialnost"></param>
        /// <returns></returns>
        Spezialnost GetSpezialnostByName(string spezialnost);

        /// <summary>
        /// Get Spezialnost_SpezialnostMeta
        /// </summary>
        /// <param name="spezialnostId"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        Spezialnost_SpezialnostMeta GetSpezialnostMeta(int spezialnostId, int year);

        /// <summary>
        /// Get Spezialnost_SpezialnostMeta
        /// </summary>
        /// <param name="spezialnostId"></param>
        /// <returns></returns>
        IEnumerable<Spezialnost_SpezialnostMeta> GetSpezialnostMeta(int spezialnostId);

        /// <summary>
        /// Get SpezialRazdeliDisziplini 
        /// </summary>
        /// <param name="spezMeta"></param>
        /// <returns></returns>
        SpezialRazdeliDisziplini GetSpezialRazdeliDisziplini(int spezMeta);

        /// <summary>
        /// Get расписание на конкретный день
        /// </summary>
        /// <param name="date"></param>
        /// <param name="idSpiskaGrupp"></param>
        /// <param name="idUchPlanaDisziplini"></param>
        /// <returns></returns>
        ScheduleDayViewModel GetRaspisanieForADay(DateTime date, int idSpiskaGrupp, int idUchPlanaDisziplini);


        /// <summary>
        /// Get расписание для препода на конкретный день
        /// </summary>
        /// <param name="date"></param>
        /// <param name="prepodId"></param>
        /// <returns></returns>
        ScheduleDayViewModel GetRaspisanieForADay(DateTime date, int prepodId);


        /// <summary>
        /// Выбрать все потоки для специальность данного года поступления
        /// </summary>
        /// <param name="idSpez"></param>
        /// <param name="yearOfEntering"></param>
        /// <returns></returns>
        Dictionary<int, string> GetPotoki(int idSpez, int yearOfEntering);

        /// <summary>
        /// Выбрать все типы занятий 
        /// </summary>
        /// <returns></returns>
        Dictionary<int, string> GetTypesOfPara();

        /// <summary>
        /// Выбрать все время занятий
        /// </summary>
        /// <returns></returns>
        Dictionary<int, string> GetVremiaZaniatiu();

        /// <summary>
        /// Выбрать все дисциплины
        /// </summary>
        /// <returns></returns>
        Dictionary<int, string> GetDisziplini();

        /// <summary>
        /// Выбрать преподов для дисциплины
        /// </summary>
        /// <param name="disciplineId"></param>
        /// <returns></returns>
        Dictionary<int, string> GetPrepodiDisciplines(int disciplineId);

        /// <summary>
        /// Выбрать все дисциплины которые ведет препод
        /// </summary>
        /// <param name="prepodId"></param>
        /// <returns></returns>
        Dictionary<int, string> GetDisciplinesOfPrepod(int prepodId);

        /// <summary>
        /// Выбрать группы на потоке для специальности
        /// </summary>
        /// <param name="idSpezMeta"></param>
        /// <returns></returns>
        Dictionary<int, string> GetGroups(int idSpezMeta);


        /// <summary>
        /// Выбрать все подгруппы для конкретной специальности
        /// </summary>
        /// <param name="idSpezMeta"></param>
        /// <returns></returns>
        Dictionary<int, string> GetSubGroups(int idSpezMeta);

        /// <summary>
        /// Выбрать все институты
        /// </summary>
        /// <returns></returns>
        Dictionary<int, string> GetInstitutes();


        /// <summary>
        /// LДобавить пару
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Raspisanie AddPara(ParaViewModel model);

        /// <summary>
        /// Добавить пару для препода
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Raspisanie AddParaForPrepod(ParaViewModel model);

        /// <summary>
        /// Удаление пары в таблица Раписание
        /// </summary>
        /// <param name="paraId"></param>
        void DeletePara(int paraId);

        /// <summary>
        /// Get raspisanie entity
        /// </summary>
        /// <param name="paraId"></param>
        /// <returns></returns>
        Raspisanie GetRaspisanie(int paraId);


        /// <summary>
        /// Get Disziplini From SpezialRazdeliDisziplini
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IEnumerable<SelectListItem> GetDiszipliniFromSpezialRazdeliDisziplini(int id);

        /// <summary>
        /// Выбрать все преподов
        /// </summary>
        /// <returns></returns>
        Dictionary<int, string> GetAllPrepods();

        /// <summary>
        /// Выбрать потоки для определенной формы обучения
        /// </summary>
        /// <param name="idSpezialnostMeta"></param>
        /// <returns></returns>
        List<SpisokGrupp> GetPotokiForSpezMeta(int idSpezialnostMeta);

        /// <summary>
        /// Выбрать группы с потока для определенной формы обучения
        /// </summary>
        /// <param name="idPotok"></param>
        /// <param name="idSpezMeta"></param>
        /// <returns></returns>
        List<SpisokGrupp> GetSpisokGrupp(int idPotok, int idSpezMeta);

        /// <summary>
        /// Выбрать курсы по факультеты
        /// </summary>
        /// <param name="facultyId"></param>
        /// <returns></returns>
        List<Spezialnost> GetListOfSpecialities(int facultyId);

        /// <summary>
        /// Выбрать срок обучения для года
        /// </summary>
        /// <param name="spezId"></param>
        /// <returns></returns>
        Spezialnost_SpezialnostMeta GetSpezMetaInfo(int spezId);
    }
}
