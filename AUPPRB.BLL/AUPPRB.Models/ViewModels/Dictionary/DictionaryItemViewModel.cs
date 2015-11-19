using System.Web.Mvc;

namespace AUPPRB.Models.ViewModels.Dictionary
{
    public class DictionaryItemViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }
        public int DictionaryType { get; set; }
        public string Name { get; set; }

        public bool Selected { get; set; }
    }
}