using System;
using System.Collections.Generic;
using System.Linq;
using AUPPRB.Domain.Interfaces;
using AUPPRB.Models.DomainModels;
using AUPPRB.Models.OtherModels.ServiceModels;
using AUPPRB.Models.ViewModels.Literature;
using AUPPRB.Models.ViewModels.UsersModels;
using DocumentFormat.OpenXml.Drawing.Charts;

namespace AUPPRB.Domain.Services
{
    public class LiteratureService : BaseServices, ILiteratureService
    {
        public List<LiteratureMetaViewModel> GetLiteratureMetas()
        {
            return
                DataProvider.LibraryLiteratures.Filter(p => !p.IsDeleted)
                    .ToList()
                    .Select(LiteratureMetaViewModel.ToLiteratureMetaViewModel)
                    .ToList();
        }

        public List<ReservateBookMetaViewModel> GetUserBooksMetas(int userId)
        {
           return DataProvider.LibraryReservateBook.Filter(p => p.UserId == userId)
                .ToList()
                .Select(ReservateBookMetaViewModel.ToReservateBookMetaViewModel)
                .ToList();
        }

        public LiteratureViewModel GetLiterature(int id)
        {
            var literature = DataProvider.LibraryLiteratures.FirstOrDefault(p => p.Id == id);
            literature = (literature==null || literature.IsDeleted)? new Library_Literature():literature;
            
            return LiteratureViewModel.ToLiteratureViewModel(literature);
        }

        public BookViewModel GetBook(int id)
        {
            var book = DataProvider.LibraryBooks.FirstOrDefault(p => p.Id == id);

            return book == null ? new BookViewModel() : BookViewModel.ToBookViewModel(book);
        }

        public RequestResult DeleteBook(int id)
        {
            var book = DataProvider.LibraryBooks.FirstOrDefault(p => p.Id == id);
            if (book == null || book.IsDeleted)
                return RequestResult.Ok;
           
            if(book.Library_ReservateBook.Any())
                return new RequestResult(1,"Нельзя удалить книгу, так как она на руках");

            book.IsDeleted = true;
            book.BookNumber = "_____deleted_____";
            try
            {
                DataProvider.Save();
                return RequestResult.Ok;
            }
            catch (Exception)
            {
                return new RequestResult(1, "Неизвестная ошибка");
            }
        }

        public RequestResult EditLiterature(LiteratureViewModel lit)
        {
            foreach (var book in lit.Books.Where(book => DataProvider.LibraryBooks.Filter(p=>p.BookNumber==book.BookNumber && p.Id!=book.RowId).Any()))
            {
                return new RequestResult(1, "Книга с  номером - " + book.BookNumber + "- уже существует");
            }
            var literature = DataProvider.LibraryLiteratures.FirstOrDefault(p => p.Id == lit.Id);
            
            literature = literature ?? new Library_Literature();

            literature.FullName = lit.FullName;
            literature.ShortName = lit.ShortName;
            literature.Year = lit.Year;
            literature.PublishingHouse = lit.PublishingHouse;
            literature.MainAuthor = lit.MainAuthor;
            literature.AdditionalAuthors = lit.AdditionalAuthors;
            literature.Desqription = lit.Desqription;

           try
            {
                if (literature.Id==0)
                    DataProvider.LibraryLiteratures.Add(literature);

                DataProvider.Save();


                foreach (var book in lit.Books)
                {
                    var editBook = literature.Library_Book.FirstOrDefault(p => p.Id == book.RowId);

                    if (editBook == null)
                    {
                        DataProvider.LibraryBooks.Add(new Library_Book()
                        {
                            BookNumber = book.BookNumber,
                            Library_Literature = literature
                        });

                    }
                    else
                        editBook.BookNumber = book.BookNumber;
                    DataProvider.Save();
                }


                return RequestResult.Ok;
            }
            catch (Exception)
            {
              return new RequestResult(1,"ЗАполните все данные формы");
            }
        }

        public RequestResult DeleteLiterature(int id)
        {
            var literature=DataProvider.LibraryLiteratures.FirstOrDefault(p => p.Id == id);

            if(literature==null || literature.IsDeleted)
                return  RequestResult.Ok;

            if(literature.Library_Book.Any(p=>p.Library_ReservateBook.Any()))
                return new RequestResult(1,"Ошибка удаления литературы. Часть кинг находится в пользовании");

            literature.IsDeleted = true;
            foreach (var book in literature.Library_Book)
            {
                book.IsDeleted = true;
                book.BookNumber = "_____deleted_____";
            }

            try
            {
                DataProvider.Save();
                 return  RequestResult.Ok;
            }
            catch (Exception)
            {
                return new RequestResult(1, "Ошибка удаления литературы. Ошибка работы приложения");

            }
        }

        public RequestResult ReturnBook(string number)
        {
            var book = DataProvider.LibraryReservateBook.FirstOrDefault(p => p.Library_Book.BookNumber == number);

            if (book == null || book.Library_Book.IsDeleted)
            {
                return new RequestResult(1, "Данной книги не существует");
            }

            DataProvider.LibraryReservateBook.Delete(book);
      
            DataProvider.LibraryHistorys.Add(new Library_History()
            {
                BookId = book.BookId,
                UserId = book.UserId,
                EventTypeId = 2,
                HistoryDate = DateTime.Now
            });

            try
            {
                DataProvider.Save();
                return RequestResult.Ok;
            }
            catch (Exception)
            {
                return new RequestResult(1, "Ошибка работы приложения");
            }
        }


        public Dictionary<int, string> GetFreeBookNumbers(int? literatureId)
        {
            return DataProvider.LibraryBooks.Filter(p => p.LitaratureId == literatureId && !p.IsDeleted && !p.Library_ReservateBook.Any())
                .ToDictionary(p => p.Id, p => p.BookNumber);
        }

        public Dictionary<int, string> GetLiteratureList()
        {
            return DataProvider.LibraryLiteratures.Filter(p=>!p.IsDeleted)
                .ToDictionary(p => p.Id, p => p.ShortName);
        }

        public List<UserCommonViewModel> GetUsersForReservate()
        {
            return DataProvider.UserMetadata.GetAll().Select(p => new UserCommonViewModel
            {
                Id = p.UserId,
                FirstName = p.FirstName,
                LastName = p.LastName,
                MiddleName = p.MiddleName,
                Login = p.User.Login
            })
            .ToList();
        }

        public RequestResult ReservateBook(int bookId, int userId,int managerId)
        {
            if(DataProvider.LibraryReservateBook.Filter(p=>bookId==p.BookId).Any())
                return new RequestResult(1,"Данная книга уже взята");

            var book = DataProvider.LibraryBooks.FirstOrDefault(p => p.Id == bookId);

            if (book == null || book.IsDeleted)
            {
                return new RequestResult(1, "Данной книги не существует");
            }

            DataProvider.LibraryReservateBook.Add(new Library_ReservateBook()
            {
                ManagerId = managerId,
                BookId = bookId,
                UserId = userId,
                ReservateDate = DateTime.Now
            });
            DataProvider.LibraryHistorys.Add(new Library_History()
            {
                BookId = bookId,
                UserId = userId,
                EventTypeId = 1,
                HistoryDate = DateTime.Now
            });

            try
            {
                DataProvider.Save();
                return RequestResult.Ok;
            }
            catch (Exception)
            {
                return new RequestResult(1, "Ошибка работы приложения");
            }

        }

        public ReservatorInfoViewModel GetReservatorInfo(string bookNumber)
        {
            var data=DataProvider.LibraryReservateBook.FirstOrDefault(p => p.Library_Book.BookNumber == bookNumber);

            return data == null ? null : ReservatorInfoViewModel.ToReservatirInfoViewModel(data);

        }
    }
}
