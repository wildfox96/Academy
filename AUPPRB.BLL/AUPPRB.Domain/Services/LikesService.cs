using System.Linq;
using AUPPRB.Domain.Interfaces;
using AUPPRB.Models.DomainModels;
using AUPPRB.Repository.DB;

namespace AUPPRB.Domain.Services
{
    public class LikesService : ILikesService
    {
        private IDataProvider _dataProvider;
        public LikesService(IDataProvider dataProvider)
        {
            _dataProvider = dataProvider;

        }
        public int GetPrepodLikesCount(int idSotr)
        {
            return _dataProvider.PrepodLikes.Filter(m => m.IdSotr == idSotr).Count();
        }

        public bool IsUserAddLike(int userId, int idSotr)
        {
            var count = _dataProvider.PrepodLikes.Filter(m => m.IdSotr == idSotr && m.IdUser == userId).Count();
            if (count == 0)
                return true;
            return false;
        }

        public void RemoveLike(Prepod_PrepodLikes prepodLike)
        {
            _dataProvider.PrepodLikes.Delete(prepodLike);
            _dataProvider.Save();
        }
        public void AddLike(Prepod_PrepodLikes prepodLike)
        {
            _dataProvider.PrepodLikes.Add(prepodLike);
            _dataProvider.Save();
        }
    }
}
