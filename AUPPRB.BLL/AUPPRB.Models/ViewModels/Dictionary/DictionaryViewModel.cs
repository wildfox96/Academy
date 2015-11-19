using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using AUPPRB.Models.DomainModels;

namespace AUPPRB.Models.ViewModels.Dictionary
{
    public class DictionaryViewModel
    {
        [HiddenInput(DisplayValue=false)]
        public int Id { get; set; }
        
        [Display(Name = "Название справочника")]
        public string Name { get; set; }

        public bool IsSystem { get; set; }
        
        public List<DictionaryItemViewModel> DictionaryItems { get; set; }

        public DictionaryViewModel()
        {
            DictionaryItems=new List<DictionaryItemViewModel>();
        }

        #region Convertors
        public static DictionaryViewModel ToDictionaryViewModel(DictionaryType item)
        {
            var dictionary = new DictionaryViewModel
            {
                Id = item.Id,
                Name = item.Name,
                IsSystem = item.IsSystem,
                DictionaryItems = item.Dictionary.Select(p => new DictionaryItemViewModel
                {
                    Id = p.Id,
                    DictionaryType = p.DictionaryTypeId,
                    Name = p.Name
                }).ToList()
            };
            return dictionary;
        }

        public static List<DictionaryViewModel> ToDictionaryViewModel(IEnumerable<DictionaryType> items)
        {
            var dictionaryViewModels = items.Select(item => new DictionaryViewModel()
            {
                Id = item.Id,
                Name = item.Name,
                IsSystem = item.IsSystem,
                DictionaryItems = item.Dictionary
                                      .Select(p => new DictionaryItemViewModel
                                      {
                                          Id = p.Id,
                                          DictionaryType = p.DictionaryTypeId,
                                          Name = p.Name
                                      })
                                      .ToList()
            }).ToList();

            return dictionaryViewModels;
        }
        #endregion

       
    }
}
