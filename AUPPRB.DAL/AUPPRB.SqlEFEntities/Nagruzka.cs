//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AUPPRB.SqlEFEntities
{
    using System;
    using System.Collections.Generic;
    
    public partial class Nagruzka
    {
        public int IdNagrz { get; set; }
        public int IdZanSem { get; set; }
        public int Pot { get; set; }
        public Nullable<int> IdSpiskaGrupp { get; set; }
        public Nullable<int> IdPodGrupp { get; set; }
        public int Semestr { get; set; }
        public double Chasov { get; set; }
        public int Chelovek { get; set; }
        public int IdPrepodaCafedri { get; set; }
    
        public virtual Dictionary PotDictionary { get; set; }
        public virtual Prepod_PrepodiCafedri Prepod_PrepodiCafedri { get; set; }
        public virtual SpisokGrupp SpisokGrupp { get; set; }
        public virtual ZaniatiaySemestr ZaniatiaySemestr { get; set; }
    }
}
