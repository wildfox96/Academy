using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using AUPPRB.Models.DomainModels;

namespace AUPPRB.Models.ViewModels.Literature
{
    public class LiteratureViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Display(Name = "Полное название")]
        [Required(ErrorMessage = "Введите полное название")]
        [StringLength(30, MinimumLength = 8, ErrorMessage = "Слишком коротко")]
        public string FullName { get; set; }

        [Display(Name = "Краткое название")]
        [Required(ErrorMessage = "Введите краткое название")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Слишком коротко")]
        public string ShortName { get; set; }

        [Display(Name = "Год издания")]
        [Required(ErrorMessage = "Введите год издания")]
        public int Year { get; set; }

        [Display(Name = "Издательство")]
        [Required(ErrorMessage = "Введите издательство")]
        [StringLength(30, MinimumLength = 8, ErrorMessage = "Слишком коротко")]
        public string PublishingHouse { get; set; }

        [Display(Name = "Автор")]
        [Required(ErrorMessage = "Введите автора")]
        [StringLength(30, MinimumLength = 8, ErrorMessage = "Слишком коротко")]
        public string MainAuthor { get; set; }

        [Display(Name = "Соавторы")]

        public string AdditionalAuthors { get; set; }

        [Display(Name = "Описание")]
        public string Desqription { get; set; }

        public List<BookOfLiteratureViewModel> Books { get; set; }

        [Display(Name = "Доступно в бтблиотеке")]
        public int FreeBooksCount { get; set; }

        public LiteratureViewModel()
        {
            Books = new List<BookOfLiteratureViewModel>();
        }

        public static LiteratureViewModel ToLiteratureViewModel(Library_Literature literature)
        {
            var books = literature.Library_Book.Where(p => !p.IsDeleted).Select(p => new BookOfLiteratureViewModel()
            {
                BookNumber = p.BookNumber,
                RowId = p.Id
            })
                .OrderBy(p => p.BookNumber)
                .ToList();
           
            return new LiteratureViewModel()
            {
                Id = literature.Id,
                FullName = literature.FullName,
                ShortName = literature.ShortName,
                Year = literature.Year,
                PublishingHouse = literature.PublishingHouse,
                MainAuthor = literature.MainAuthor,
                AdditionalAuthors = literature.AdditionalAuthors,
                Desqription = literature.Desqription,
                Books = books,
                FreeBooksCount = books.Count-literature.Library_Book.Count(p => p.Library_ReservateBook.Any())
              
            };
        }
    }
}
