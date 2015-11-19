using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using AUPPRB.Models.DomainModels;

namespace AUPPRB.Models.ViewModels.Literature
{
    public class BookViewModel
    {

        [Display(Name = "Полное название")]
        public string FullName { get; set; }

        [Display(Name = "Краткое название")]
        public string ShortName { get; set; }

        [Display(Name = "Год издания")]

        public int Year { get; set; }

        [Display(Name = "Издательство")]

        public string PublishingHouse { get; set; }

        [Display(Name = "Автор")]

        public string MainAuthor { get; set; }

        [Display(Name = "Соавторы")]

        public string AdditionalAuthors { get; set; }

        [Display(Name = "Описание")]
        public string Desqription { get; set; }

        [Display(Name = "Номер книги")]
        public string BookNumber { get; set; }

        public static BookViewModel ToBookViewModel(Library_Book book)
        {
            return new BookViewModel()
            {
                FullName = book.Library_Literature.FullName,
                ShortName = book.Library_Literature.ShortName,
                Year = book.Library_Literature.Year,
                PublishingHouse = book.Library_Literature.PublishingHouse,
                MainAuthor = book.Library_Literature.MainAuthor,
                AdditionalAuthors = book.Library_Literature.AdditionalAuthors,
                Desqription = book.Library_Literature.Desqription,
                BookNumber = book.BookNumber
            };
        }
    }
}
