using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AUPPRB.Models.ViewModels.Schedule;

namespace AUPPRB.Domain.Interfaces
{
    public interface IScheduleService
    {

        #region ScheduleWeb

        /// <summary>
        /// Сгенерировать расписание для пользователя
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="dateFrom">Дата с</param>
        /// <param name="dateTo">Дата по</param>
        /// <returns></returns>
        ScheduleViewModel GenerateSchedule(int userId, DateTime dateFrom, DateTime dateTo);

        /// <summary>
        /// Сгенерировать расписание для пользователя
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <returns></returns>
        ScheduleViewModel GenerateScheduleForGroup(int groupId, DateTime dateFrom, DateTime dateTo);

        #endregion

        #region Schedual Excell
        /// <summary>
        /// Генерирует расписание в Excell
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="dateFrom">Дата с</param>
        /// <param name="dateTo">Дата по</param>
        /// <param name="templatePath">Путь к файлу с шаблоном в excell</param>
        /// <returns>Бинарник файла</returns>
        byte[] GetScheduleInExcellFormat(int userId, DateTime dateFrom, DateTime dateTo, string templatePath);

        /// <summary>
        /// Генерирует расписание в Excell
        /// </summary>
        /// <param name="groupId">Идентификатор пользователя</param>
        /// <param name="dateFrom">Дата с</param>
        /// <param name="dateTo">Дата по</param>
        /// <param name="templatePath">Путь к файлу с шаблоном в excell</param>
        /// <returns>Бинарник файла</returns>
        byte[] GetScheduleInExcellFormatForGroup(int groupId, DateTime dateFrom, DateTime dateTo, string templatePath); 
        #endregion

        #region SchedualConfig

        /// <summary>
        /// Получить список преподавателей
        /// </summary>
        /// <returns></returns>
        Dictionary<int, string> GetPrepodsForSchedualConfiguration();

        Dictionary<int, string> GetSpezialnostiForConfiguration();
        int[] GetGodiPostupForConfiguration(int specialnostId);

        /// <summary>
        /// Выбрать годы поступления
        /// </summary>
        /// <param name="formOfStudyId"></param>
        /// <param name="specialnostId"></param>
        /// <returns></returns>
        int[] GetGodiPostupForConfiguration(int formOfStudyId, int specialnostId);

        Dictionary<int, string> GetGroupsForConfiguration(int specialnostId, int godPostup); 

        /// <summary>
        /// Выбрать формы обучения 
        /// </summary>
        /// <param name="specialostId"></param>
        /// <returns></returns>
       Dictionary<int, string> GetFormOfStudyForConfiguration(int specialostId);

        #endregion


    }
}
