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
    
    public partial class Discipline
    {
        public Discipline()
        {
            this.PrepodDiscipline = new HashSet<PrepodDiscipline>();
            this.SpezialRazdeliDisziplini = new HashSet<SpezialRazdeliDisziplini>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public bool IsActive { get; set; }
        public Nullable<double> Rate { get; set; }
    
        public virtual ICollection<PrepodDiscipline> PrepodDiscipline { get; set; }
        public virtual ICollection<SpezialRazdeliDisziplini> SpezialRazdeliDisziplini { get; set; }
    }
}