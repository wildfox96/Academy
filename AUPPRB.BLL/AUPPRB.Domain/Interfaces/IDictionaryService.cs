using System.Collections.Generic;
using AUPPRB.Common.Enums;
using AUPPRB.Models.OtherModels.ServiceModels;
using AUPPRB.Models.ViewModels.Dictionary;

namespace AUPPRB.Domain.Interfaces
{
    public interface IDictionaryService
    {
        /// <summary>
        /// Получить мета описание словаря
        /// </summary>
        /// <param name="type">Тип словаря</param>
        /// <returns></returns>
        DictionaryMetaViewModel GetDictionaryMeta(DictionaryTypeEnum type);
        /// <summary>
        /// Полуить мета описание всех словарей
        /// </summary>
        /// <returns></returns>
        List<DictionaryMetaViewModel> GetAllDictionariesMeta();

        /// <summary>
        /// Получить словарь
        /// </summary>
        /// <param name="type">Тип словаря</param>
        /// <returns></returns>
        DictionaryViewModel GetDictionary(DictionaryTypeEnum type);
        /// <summary>
        /// Получить словарь
        /// </summary>
        /// <param name="id">Идентификатор словаря</param>
        /// <returns></returns>
        DictionaryViewModel GetDictionary(int id);
        /// <summary>
        /// Получить все словари
        /// </summary>
        /// <returns></returns>
        List<DictionaryViewModel> GetAllDictionaries();
        /// <summary>
        /// Получить термины словаря
        /// </summary>
        /// <param name="type">Тип словаря</param>
        /// <returns></returns>
        List<DictionaryItemViewModel> GetItemsOfDictionary(DictionaryTypeEnum type);
        /// <summary>
        /// Обновить словарь
        /// </summary>
        /// <param name="newDictionary">Обнавленный словарь</param>
        /// <returns></returns>
        RequestResult UpdateDictionary(DictionaryViewModel newDictionary);
    }
}
