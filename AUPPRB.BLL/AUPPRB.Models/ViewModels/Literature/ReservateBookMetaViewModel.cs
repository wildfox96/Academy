using AUPPRB.Models.DomainModels;

namespace AUPPRB.Models.ViewModels.Literature
{
    public class ReservateBookMetaViewModel
    {
        public int Id { get; set; }
        public string ShortName { get; set; }
        public string Author { get; set; }
        public string BookNumber { get; set; }
        public string ReservateDate { get; set; }

        public static ReservateBookMetaViewModel ToReservateBookMetaViewModel(Library_ReservateBook book)
        {
            return new ReservateBookMetaViewModel()
            {
                Id = book.BookId,
                ShortName = book.Library_Book.Library_Literature.ShortName,
                BookNumber = book.Library_Book.BookNumber,
                ReservateDate = book.ReservateDate.ToShortDateString(),
               Author = book.Library_Book.Library_Literature.MainAuthor
            };
        }
   
    }
}
