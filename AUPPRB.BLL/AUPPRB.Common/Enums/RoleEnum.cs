using AUPPRB.Common.Attributes;

namespace AUPPRB.Common.Enums
{
    public enum RoleEnum
    {
        [EnumValue(1)]
        ГлобальныйАдминистратор,

        [EnumValue(3)]
        Преподаватель,

        [EnumValue(2)]
        Студент
    }
}
