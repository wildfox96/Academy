using System;
using AUPPRB.Models.DomainModels;
using Contact = AUPPRB.Models.DomainModels.Contact;
using Dictionary = AUPPRB.Models.DomainModels.Dictionary;
using DictionaryType = AUPPRB.Models.DomainModels.DictionaryType;
using Discipline = AUPPRB.Models.DomainModels.Discipline;
using Nagruzka = AUPPRB.Models.DomainModels.Nagruzka;
using PrepodDiscipline = AUPPRB.Models.DomainModels.PrepodDiscipline;
using Prepod_PrepodiCafedri = AUPPRB.Models.DomainModels.Prepod_PrepodiCafedri;
using Prepod_PrepodMeta = AUPPRB.Models.DomainModels.Prepod_PrepodMeta;
using Raspisanie = AUPPRB.Models.DomainModels.Raspisanie;
using Role = AUPPRB.Models.DomainModels.Role;
using RoleTask = AUPPRB.Models.DomainModels.RoleTask;
using SostavGrupp = AUPPRB.Models.DomainModels.SostavGrupp;
using Spezialnost = AUPPRB.Models.DomainModels.Spezialnost;
using Spezialnost_SpezialnostMeta = AUPPRB.Models.DomainModels.Spezialnost_SpezialnostMeta;
using SpezialRazdeliDisziplini = AUPPRB.Models.DomainModels.SpezialRazdeliDisziplini;
using SpisokGrupp = AUPPRB.Models.DomainModels.SpisokGrupp;
using Student_StudentMeta = AUPPRB.Models.DomainModels.Student_StudentMeta;
using User = AUPPRB.Models.DomainModels.User;
using UserMeta = AUPPRB.Models.DomainModels.UserMeta;
using UserRole = AUPPRB.Models.DomainModels.UserRole;
using UserTask = AUPPRB.Models.DomainModels.UserTask;
using VidiRabot = AUPPRB.Models.DomainModels.VidiRabot;
using VremyaZanyatia = AUPPRB.Models.DomainModels.VremyaZanyatia;

namespace AUPPRB.Repository.DB
{
    /// <summary>
    /// Maintains a list of repositories affected by a business transaction and coordinates 
    /// the writing out of changes and the resolution of concurrency problems.
    /// </summary>
    public interface IDataProvider : IDisposable
    {
        /// <summary>
        /// Provides access to user repository.
        /// </summary>
        IGenericRepository<User> Users { get; }

        /// <summary>
        /// Provides access to userRoles repository.
        /// </summary>
        IGenericRepository<UserRole> UserRoles { get; }

        /// <summary>
        /// Provides access to userTask repository.
        /// </summary>
        IGenericRepository<UserTask> UserTasks { get; }

        /// <summary>
        /// Provides access to userMetadata repository.
        /// </summary>
        IGenericRepository<UserMeta> UserMetadata { get; }

        /// <summary>
        /// Provides access to tasks repository.
        /// </summary>
        IGenericRepository<Models.DomainModels.Task> Tasks { get; }

        /// <summary>
        /// Provides access to role repository.
        /// </summary>
        IGenericRepository<Role> Roles { get; }

        /// <summary>
        /// Provides access to roleTask repository.
        /// </summary>
        IGenericRepository<RoleTask> RoleTasks { get; }

        /// <summary>
        /// Provides access to studentMetadata repository.
        /// </summary>
        IGenericRepository<Student_StudentMeta> StudentMetadata { get; }

        /// <summary>
        /// Provides access to prepodMetadata repository.
        /// </summary>
        IGenericRepository<Prepod_PrepodMeta> PrepodMetadata { get; }

        /// <summary>
        /// Provides access to Dictionaries repository.
        /// </summary>
        IGenericRepository<Dictionary> Dictionaries { get; }

        /// <summary>
        /// Provides access to dictionaryTypes repository.
        /// </summary>
        IGenericRepository<DictionaryType> DictionaryTypes { get; }

        /// <summary>
        /// Provides access to contacts repository.
        /// </summary>
        IGenericRepository<Contact> Contacts { get; }

        /// <summary>
        /// Provides access to disciplines repository.
        /// </summary>
        IGenericRepository<Discipline> Disciplines { get; }

        IGenericRepository<Prepod_PrepodLikes> PrepodLikes { get; }
        IGenericRepository<Prepod_PrepodiCafedri> PrepodiCafedri { get; }
        IGenericRepository<PrepodDiscipline> PrepodDiscipline { get; }
        IGenericRepository<Raspisanie> Raspisanie { get; }
        IGenericRepository<SostavGrupp> SostavGrupp { get; }
        IGenericRepository<Spezialnost> Spezialnost { get; }
        IGenericRepository<Spezialnost_SpezialnostMeta> SpezialnostMeta { get; }
        IGenericRepository<SpezialRazdeliDisziplini> SpezialRazdeliDisziplini { get; }
        IGenericRepository<SpisokGrupp> SpisokGrupp { get; }
        IGenericRepository<VidiRabot> VidiRabot { get; }
        IGenericRepository<VremyaZanyatia> VremyaZanyatia { get; }
        IGenericRepository<ZaniatiaySemestr> ZaniatiaySemestr { get; }

        IGenericRepository<Notification> Notifications { get; }

        IGenericRepository<Library_Book> LibraryBooks { get; }
        IGenericRepository<Library_EventType> LibraryEventTypes { get; }
        IGenericRepository<Library_History> LibraryHistorys { get; }
        IGenericRepository<Library_Literature> LibraryLiteratures { get; }
        IGenericRepository<Library_ReservateBook> LibraryReservateBook { get; }



        /// <summary>
        /// Saves changes to shared data source.
        /// </summary>
        void Save();



    }
}
