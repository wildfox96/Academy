//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AUPPRB.Models.DomainModels
{
    using System;
    using System.Collections.Generic;
    
    public partial class SpezialRazdeliDisziplini
    {
        public SpezialRazdeliDisziplini()
        {
            this.Raspisanie = new HashSet<Raspisanie>();
            this.ZaniatiaySemestr = new HashSet<ZaniatiaySemestr>();
        }
    
        public int IdUpPlanDisciplini { get; set; }
        public int IdSpezMeta { get; set; }
        public int IdRzdRab { get; set; }
        public int IdDisciplini { get; set; }
        public int Nm1 { get; set; }
        public int Nm2 { get; set; }
        public int Nm3 { get; set; }
        public int Lekcii { get; set; }
        public int PracticheskieZanyatia { get; set; }
        public int LaboratornieRaboti { get; set; }
        public int SeminarskieZanyatia { get; set; }
        public int SamostoyatelnieRaboti { get; set; }
        public int Sem1 { get; set; }
        public int Sem2 { get; set; }
        public int Sem3 { get; set; }
        public int Sem4 { get; set; }
        public int Sem5 { get; set; }
        public int Sem6 { get; set; }
        public int Sem7 { get; set; }
        public int Sem8 { get; set; }
        public int Sem9 { get; set; }
        public int Sem10 { get; set; }
        public int Sem11 { get; set; }
        public int Sem12 { get; set; }
        public string ExamSem { get; set; }
        public string DifZachetSem { get; set; }
        public string ZachetSem { get; set; }
        public string KursovayaSem { get; set; }
        public string KontrolnayanSem { get; set; }
        public string KfP { get; set; }
        public string KfT { get; set; }
    
        public virtual Dictionary RazdRabotDictionary { get; set; }
        public virtual Discipline Discipline { get; set; }
        public virtual ICollection<Raspisanie> Raspisanie { get; set; }
        public virtual Spezialnost_SpezialnostMeta Spezialnost_SpezialnostMeta { get; set; }
        public virtual ICollection<ZaniatiaySemestr> ZaniatiaySemestr { get; set; }
    }
}
