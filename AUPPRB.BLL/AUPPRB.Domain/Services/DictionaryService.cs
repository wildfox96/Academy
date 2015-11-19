using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AUPPRB.Common.Enums;
using AUPPRB.Common.Extensions;
using AUPPRB.Domain.Interfaces;
using AUPPRB.Models.DomainModels;
using AUPPRB.Models.OtherModels.ServiceModels;
using AUPPRB.Models.ViewModels.Dictionary;

namespace AUPPRB.Domain.Services
{
    public class DictionaryService:BaseServices,IDictionaryService
    {
        /// <summary>
        /// Ключ в кэше
        /// </summary>
        private const string DictionaryCacheKey = "Dictionaries";
        /// <summary>
        /// Достает словари из базы данных или кэша
        /// </summary>
        /// <returns></returns>
        private List<DictionaryViewModel> GetDictionariesFromDatabase()
        {
            return ServiceCache.GetOrAdd(DictionaryCacheKey, () => DictionaryViewModel.ToDictionaryViewModel(DataProvider.DictionaryTypes.GetAll().ToList()));
           
        }

        public DictionaryMetaViewModel GetDictionaryMeta(DictionaryTypeEnum type)
        {
            var dictionary = GetDictionariesFromDatabase().FirstOrDefault(p => p.Id == type.GetEnumValue());
           return dictionary==null ? new DictionaryMetaViewModel() : DictionaryMetaViewModel.ToDictionaryViewModel(dictionary);
        }

        public List<DictionaryMetaViewModel> GetAllDictionariesMeta()
        {
            var dictionaryViewModels = GetDictionariesFromDatabase().Where(p => !p.IsSystem);
            return DictionaryMetaViewModel.ToDictionaryViewModel(dictionaryViewModels);
        }

        public DictionaryViewModel GetDictionary(DictionaryTypeEnum type)
        {
            var dictionary = GetDictionariesFromDatabase().FirstOrDefault(p => p.Id == type.GetEnumValue());
            return dictionary ?? new DictionaryViewModel();
        }

        public DictionaryViewModel GetDictionary(int id)
        {
            var dictionary = GetDictionariesFromDatabase().FirstOrDefault(p => p.Id == id);
            return dictionary ?? new DictionaryViewModel();
        }

        public List<DictionaryViewModel> GetAllDictionaries()
        {
            return GetDictionariesFromDatabase().Where(p => !p.IsSystem).ToList();
        }

        public List<DictionaryItemViewModel> GetItemsOfDictionary(int id)
        {
            var dictionary = GetDictionariesFromDatabase().FirstOrDefault(p => p.Id == id)?? new DictionaryViewModel();;
            return dictionary.DictionaryItems;
        }

        public List<DictionaryItemViewModel> GetItemsOfDictionary(DictionaryTypeEnum type)
        {
            var dictionary = GetDictionariesFromDatabase().FirstOrDefault(p => p.Id == type.GetEnumValue()) ?? new DictionaryViewModel(); ;
            return dictionary.DictionaryItems;
        }

        public RequestResult UpdateDictionary(DictionaryViewModel newDictionary)
        {
            if (newDictionary.DictionaryItems.Any(p => string.IsNullOrEmpty(p.Name)))
                return new RequestResult(1, "Невозможно сохранить пустой термин");

            //проверяем уникальность
            if (newDictionary.DictionaryItems.Select(p => p.Name.ToLower().Replace(" ", "")).Distinct().Count() != newDictionary.DictionaryItems.Count)
                return new RequestResult(1, "Значения в справочниках должны быть уникальны");

            //проверка на то что справочник используется в системе (придумать крутую)

            var databaseDictionaries = DataProvider.Dictionaries.Filter(p => p.DictionaryTypeId == newDictionary.Id).ToArray();

            var oldItems = databaseDictionaries.Where(p => !newDictionary.DictionaryItems.Select(c => c.Id).Contains(p.Id));

            #region    удаляем старые термины
            foreach (var olditem in oldItems)
            {
                DataProvider.Dictionaries.Delete(olditem);
            }
            try
            {
                DataProvider.Save();
            }
            catch (Exception)
            {
                return new RequestResult(1, "Ошибка удаления стрых терминов в справочнике");
            }
            #endregion

            #region обновляем и добавляем новые
            foreach (var itemForUpdate in newDictionary.DictionaryItems)
            {
                Dictionary dbDictionaryItem = databaseDictionaries.FirstOrDefault(p => p.Id == itemForUpdate.Id) ?? new Dictionary();
                dbDictionaryItem.Name = itemForUpdate.Name;
                dbDictionaryItem.DictionaryTypeId = newDictionary.Id;
                if (itemForUpdate.Id == 0)
                    DataProvider.Dictionaries.Add(dbDictionaryItem);
            }
            try
            {
                DataProvider.Save();
            }
            catch (Exception)
            {
                return new RequestResult(1, "Ошибка обновления терминов в справочнике");
            }
            finally
            {
                ServiceCache.Remove(DictionaryCacheKey);
            }
            #endregion

            return RequestResult.Ok;
        }
    }
}
