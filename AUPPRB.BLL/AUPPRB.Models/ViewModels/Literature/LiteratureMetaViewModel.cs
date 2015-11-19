using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AUPPRB.Models.DomainModels;

namespace AUPPRB.Models.ViewModels.Literature
{
    public class LiteratureMetaViewModel
    {
        public int Id { get; set; }
        public string ShortName { get; set; }
        public string Author { get; set; }
        public int Year { get; set; }
        public int TotalCount { get; set; }
        public int FreeCount { get; set; }

        public static LiteratureMetaViewModel ToLiteratureMetaViewModel(Library_Literature literature)
        {
            var totalCount = literature.Library_Book.Count(p => !p.IsDeleted);
            var freeCount = totalCount -
                            literature.Library_Book.Count(p => !p.IsDeleted && p.Library_ReservateBook.Any());

            return new LiteratureMetaViewModel()
            {
                Id = literature.Id,
                Author = literature.MainAuthor,
                ShortName = literature.ShortName,
                Year = literature.Year,
                TotalCount = totalCount,
                FreeCount = freeCount
            };
        }
    }
}
