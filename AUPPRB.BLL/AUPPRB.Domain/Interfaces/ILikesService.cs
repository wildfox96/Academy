using AUPPRB.Models.DomainModels;

namespace AUPPRB.Domain.Interfaces
{
    public interface ILikesService
    {
        int GetPrepodLikesCount(int idSotr);
        void AddLike(Prepod_PrepodLikes prepodLike);
        void RemoveLike(Prepod_PrepodLikes prepodLike);
        bool IsUserAddLike(int userId, int idSotr);
    }
}
