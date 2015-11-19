using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using AUPPRB.Models.DomainModels;

namespace AUPPRB.Models.ViewModels.Dictionary
{
    public class DictionaryMetaViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Display(Name = "Название справочника")]
        public string Name { get; set; }

        public DictionaryMetaViewModel()
        {
            
        }

        public DictionaryMetaViewModel(int id,string name)
        {
            Id = id;
            Name = name;
        }

        public static DictionaryMetaViewModel ToDictionaryViewModel(DictionaryType item)
        {
            var dictionary = new DictionaryMetaViewModel(item.Id, item.Name);
            return dictionary;
        }

        public static DictionaryMetaViewModel ToDictionaryViewModel(DictionaryViewModel item)
        {
            var dictionary = new DictionaryMetaViewModel(item.Id, item.Name);
            return dictionary;
        }

        public static List<DictionaryMetaViewModel> ToDictionaryViewModel(IEnumerable<DictionaryType> items)
        {
            var dictionaryViewModels = items.Select(item => new DictionaryMetaViewModel()
            {
                Id = item.Id,
                Name = item.Name,
            }).ToList();

            return dictionaryViewModels;
        }

        public static List<DictionaryMetaViewModel> ToDictionaryViewModel(IEnumerable<DictionaryViewModel> items)
        {
            var dictionaryViewModels = items.Select(item => new DictionaryMetaViewModel()
            {
                Id = item.Id,
                Name = item.Name,
            }).ToList();

            return dictionaryViewModels;
        }

      
    }
}