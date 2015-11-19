namespace AUPPRB.Models.OtherModels.ServiceModels
{
    public class UserValidationResult
    {
        public bool IsDeleted { get; set; }
        public bool IsBlocked { get; set; }
        public bool IsRightPassword { get; set; }
        public string BlockReason { get; set; }
        public bool IsExist { get; set; }

        public bool IsValid
        {
            get { return (IsExist && IsRightPassword && !IsBlocked && !IsDeleted); }
        }

    }
}
