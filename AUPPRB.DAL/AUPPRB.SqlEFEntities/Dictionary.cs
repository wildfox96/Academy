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
    
    public partial class Dictionary
    {
        public Dictionary()
        {
            this.Nagruzka = new HashSet<Nagruzka>();
            this.Prepod_PrepodMeta = new HashSet<Prepod_PrepodMeta>();
            this.Prepod_PrepodiCafedri = new HashSet<Prepod_PrepodiCafedri>();
            this.Prepod_PrepodiCafedri1 = new HashSet<Prepod_PrepodiCafedri>();
            this.Raspisanie = new HashSet<Raspisanie>();
            this.Raspisanie1 = new HashSet<Raspisanie>();
            this.Spezialnost_SpezialnostMeta = new HashSet<Spezialnost_SpezialnostMeta>();
            this.Spezialnost_SpezialnostMeta1 = new HashSet<Spezialnost_SpezialnostMeta>();
            this.Spezialnost_SpezialnostMeta2 = new HashSet<Spezialnost_SpezialnostMeta>();
            this.Spezialnost_SpezialnostMeta3 = new HashSet<Spezialnost_SpezialnostMeta>();
            this.SpezialRazdeliDisziplini = new HashSet<SpezialRazdeliDisziplini>();
            this.SpisokGrupp = new HashSet<SpisokGrupp>();
            this.SpisokGrupp1 = new HashSet<SpisokGrupp>();
            this.Student_StudentMeta = new HashSet<Student_StudentMeta>();
            this.Student_StudentMeta1 = new HashSet<Student_StudentMeta>();
            this.Student_StudentMeta2 = new HashSet<Student_StudentMeta>();
            this.VidiRabot = new HashSet<VidiRabot>();
            this.Notifications = new HashSet<Notification>();
            this.Student_StudentMeta3 = new HashSet<Student_StudentMeta>();
            this.Student_StudentMeta4 = new HashSet<Student_StudentMeta>();
            this.Student_StudentMeta5 = new HashSet<Student_StudentMeta>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public int DictionaryTypeId { get; set; }
    
        public virtual DictionaryType DictionaryType { get; set; }
        public virtual ICollection<Nagruzka> Nagruzka { get; set; }
        public virtual ICollection<Prepod_PrepodMeta> Prepod_PrepodMeta { get; set; }
        public virtual ICollection<Prepod_PrepodiCafedri> Prepod_PrepodiCafedri { get; set; }
        public virtual ICollection<Prepod_PrepodiCafedri> Prepod_PrepodiCafedri1 { get; set; }
        public virtual ICollection<Raspisanie> Raspisanie { get; set; }
        public virtual ICollection<Raspisanie> Raspisanie1 { get; set; }
        public virtual ICollection<Spezialnost_SpezialnostMeta> Spezialnost_SpezialnostMeta { get; set; }
        public virtual ICollection<Spezialnost_SpezialnostMeta> Spezialnost_SpezialnostMeta1 { get; set; }
        public virtual ICollection<Spezialnost_SpezialnostMeta> Spezialnost_SpezialnostMeta2 { get; set; }
        public virtual ICollection<Spezialnost_SpezialnostMeta> Spezialnost_SpezialnostMeta3 { get; set; }
        public virtual ICollection<SpezialRazdeliDisziplini> SpezialRazdeliDisziplini { get; set; }
        public virtual ICollection<SpisokGrupp> SpisokGrupp { get; set; }
        public virtual ICollection<SpisokGrupp> SpisokGrupp1 { get; set; }
        public virtual ICollection<Student_StudentMeta> Student_StudentMeta { get; set; }
        public virtual ICollection<Student_StudentMeta> Student_StudentMeta1 { get; set; }
        public virtual ICollection<Student_StudentMeta> Student_StudentMeta2 { get; set; }
        public virtual ICollection<VidiRabot> VidiRabot { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<Student_StudentMeta> Student_StudentMeta3 { get; set; }
        public virtual ICollection<Student_StudentMeta> Student_StudentMeta4 { get; set; }
        public virtual ICollection<Student_StudentMeta> Student_StudentMeta5 { get; set; }
    }
}
