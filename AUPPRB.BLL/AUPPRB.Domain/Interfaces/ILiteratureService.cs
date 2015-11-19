using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AUPPRB.Models.OtherModels.ServiceModels;
using AUPPRB.Models.ViewModels.Literature;
using AUPPRB.Models.ViewModels.UsersModels;
using Ninject.Activation;

namespace AUPPRB.Domain.Interfaces
{
    public interface ILiteratureService
    {
        List<LiteratureMetaViewModel> GetLiteratureMetas();
        List<ReservateBookMetaViewModel> GetUserBooksMetas(int userId);

        LiteratureViewModel GetLiterature(int id);
        BookViewModel GetBook(int id);

        RequestResult DeleteBook(int id);

        RequestResult EditLiterature(LiteratureViewModel lit);

        RequestResult DeleteLiterature(int id);
        RequestResult ReturnBook(string number);

        Dictionary<int, string> GetFreeBookNumbers(int? literatureId);
        Dictionary<int, string> GetLiteratureList();

        List<UserCommonViewModel> GetUsersForReservate();

        RequestResult ReservateBook(int bookId, int userId,int managerId);

        ReservatorInfoViewModel GetReservatorInfo(string bookNumber);
    }


  
}
