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
    
    public partial class Raspisanie
    {
        public int Id { get; set; }
        public System.DateTime Data { get; set; }
        public int IdUchPlanaDisciplini { get; set; }
        public int IdVidaRaboti { get; set; }
        public int IdVremyaZanyatia { get; set; }
        public int Pot { get; set; }
        public Nullable<int> IdSpiskaGrupp { get; set; }
        public Nullable<int> NomerPodGrupp { get; set; }
        public int IdPrepodaCafedri { get; set; }
        public int NomerZanyatiaVSemestre { get; set; }
        public string Auditoriya { get; set; }
        public int TipNagruzki { get; set; }
    
        public virtual Dictionary PotDictionary { get; set; }
        public virtual Dictionary TipNagruzkiDictionary { get; set; }
        public virtual Prepod_PrepodiCafedri Prepod_PrepodiCafedri { get; set; }
        public virtual SpezialRazdeliDisziplini SpezialRazdeliDisziplini { get; set; }
        public virtual SpisokGrupp SpisokGrupp { get; set; }
        public virtual VidiRabot VidiRabot { get; set; }
        public virtual VremyaZanyatia VremyaZanyatia { get; set; }
    }
}
