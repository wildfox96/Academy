using System;
using AUPPRB.Models.DomainModels;

namespace AUPPRB.Repository.DB
{
    public class DataProvider : IDataProvider
    {
        private SqlEFEntities.AcademyDBEntities _dbContext;

        public DataProvider()
        {
            CreateDbContext();
        }

        protected void CreateDbContext()
        {
            _dbContext = new SqlEFEntities.AcademyDBEntities();



            // Do NOT enable proxied entities, else serialization fails
            //if false it will not get the associated certification and skills when we get the applicants
            _dbContext.Configuration.ProxyCreationEnabled = true;

            // Load navigation properties explicitly (avoid serialization trouble)
            _dbContext.Configuration.LazyLoadingEnabled = true;

            // Because Web API will perform validation, we don't need/want EF to do so
            _dbContext.Configuration.ValidateOnSaveEnabled = true;

        }


        public IGenericRepository<ZaniatiaySemestr> ZaniatiaySemestr { get; private set; }

        public void Save()
        {
            _dbContext.SaveChanges();
        }


        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_dbContext != null)
                {
                    _dbContext.Dispose();
                }
            }
        }


        #endregion


        #region Fields

        private IGenericRepository<User> _users;

        private IGenericRepository<UserRole> _userRoles;

        private IGenericRepository<UserTask> _userTasks;

        private IGenericRepository<UserMeta> _userMetadata;

        private IGenericRepository<Task> _tasks;

        private IGenericRepository<Role> _roles;

        private IGenericRepository<RoleTask> _roleTasks;

        private IGenericRepository<Student_StudentMeta> _studentMetadata;

        private IGenericRepository<Prepod_PrepodMeta> _prepodMetadata;

        private IGenericRepository<Dictionary> _dictionaries;

        private IGenericRepository<DictionaryType> _dictionaryTypes;

        private IGenericRepository<Nagruzka> _nagruzka;
        private IGenericRepository<Prepod_PrepodiCafedri> _prepod_PrepodiCafedri;
        private IGenericRepository<PrepodDiscipline> _prepodDiscipline;
        private IGenericRepository<Raspisanie> _raspisanie;
        private IGenericRepository<SostavGrupp> _sostavGrupp;
        private IGenericRepository<Spezialnost> _spezialnost;
        private IGenericRepository<Spezialnost_SpezialnostMeta> _spezialnostMeta;
        private IGenericRepository<SpezialRazdeliDisziplini> _spezialRazdeliDisziplini;
        private IGenericRepository<SpisokGrupp> _spisokGrupp;
        private IGenericRepository<Contact> _contacts;
        private IGenericRepository<VidiRabot> _vidiRabot;
        private IGenericRepository<VremyaZanyatia> _vremyaZanyatia;
        private IGenericRepository<ZaniatiaySemestr> _zaniatiaySemestr;
        private IGenericRepository<Discipline> _disciplines;
        private IGenericRepository<Notification> _notifications;

        private IGenericRepository<Library_Book> _libraryBooks;
        private IGenericRepository<Library_EventType> _libraryEventTypes;
        private IGenericRepository<Library_History> _libraryHistorys;
        private IGenericRepository<Library_Literature> _libraryLiteratures;
        private IGenericRepository<Library_ReservateBook> _libraryReservateBook;
        private IGenericRepository<Prepod_PrepodLikes> _prepodLikes;



        #endregion


        public IGenericRepository<User> Users
        {
            get { return _users ?? (_users = new EfGenericRepository<User>(_dbContext)); }
        }

        public IGenericRepository<UserRole> UserRoles
        {
            get { return _userRoles ?? (_userRoles = new EfGenericRepository<UserRole>(_dbContext)); }
        }

        public IGenericRepository<UserTask> UserTasks
        {
            get { return _userTasks ?? (_userTasks = new EfGenericRepository<UserTask>(_dbContext)); }
        }

        public IGenericRepository<UserMeta> UserMetadata
        {
            get { return _userMetadata ?? (_userMetadata = new EfGenericRepository<UserMeta>(_dbContext)); }
        }

        public IGenericRepository<Task> Tasks
        {
            get { return _tasks ?? (_tasks = new EfGenericRepository<Task>(_dbContext)); }
        }

        public IGenericRepository<Role> Roles
        {
            get { return _roles ?? (_roles = new EfGenericRepository<Role>(_dbContext)); }
        }


        public IGenericRepository<RoleTask> RoleTasks
        {
            get { return _roleTasks ?? (_roleTasks = new EfGenericRepository<RoleTask>(_dbContext)); }
        }


        public IGenericRepository<Student_StudentMeta> StudentMetadata
        {
            get
            {
                return _studentMetadata ?? (_studentMetadata = new EfGenericRepository<Student_StudentMeta>(_dbContext));
            }
        }


        public IGenericRepository<Prepod_PrepodMeta> PrepodMetadata
        {
            get { return _prepodMetadata ?? (_prepodMetadata = new EfGenericRepository<Prepod_PrepodMeta>(_dbContext)); }
        }

        public IGenericRepository<Dictionary> Dictionaries
        {
            get { return _dictionaries ?? (_dictionaries = new EfGenericRepository<Dictionary>(_dbContext)); }
        }

        public IGenericRepository<DictionaryType> DictionaryTypes
        {
            get { return _dictionaryTypes ?? (_dictionaryTypes = new EfGenericRepository<DictionaryType>(_dbContext)); }
        }

        public IGenericRepository<Contact> Contacts
        {
            get { return _contacts ?? (_contacts = new EfGenericRepository<Contact>(_dbContext)); }
        }

        public IGenericRepository<Discipline> Disciplines
        {
            get { return _disciplines ?? (_disciplines = new EfGenericRepository<Discipline>(_dbContext)); }
        }
        public IGenericRepository<Nagruzka> Nagruzka
        {
            get { return _nagruzka ?? (_nagruzka = new EfGenericRepository<Nagruzka>(_dbContext)); }
        }
        public IGenericRepository<Prepod_PrepodiCafedri> PrepodiCafedri
        {
            get { return _prepod_PrepodiCafedri ?? (_prepod_PrepodiCafedri = new EfGenericRepository<Prepod_PrepodiCafedri>(_dbContext)); }
        }
        public IGenericRepository<PrepodDiscipline> PrepodDiscipline
        {
            get { return _prepodDiscipline ?? (_prepodDiscipline = new EfGenericRepository<PrepodDiscipline>(_dbContext)); }
        }
        public IGenericRepository<Raspisanie> Raspisanie
        {
            get { return _raspisanie ?? (_raspisanie = new EfGenericRepository<Raspisanie>(_dbContext)); }
        }

        public IGenericRepository<SostavGrupp> SostavGrupp
        {
            get { return _sostavGrupp ?? (_sostavGrupp = new EfGenericRepository<SostavGrupp>(_dbContext)); }
        }
        public IGenericRepository<Spezialnost> Spezialnost
        {
            get { return _spezialnost ?? (_spezialnost = new EfGenericRepository<Spezialnost>(_dbContext)); }
        }
        public IGenericRepository<Spezialnost_SpezialnostMeta> SpezialnostMeta
        {
            get { return _spezialnostMeta ?? (_spezialnostMeta = new EfGenericRepository<Spezialnost_SpezialnostMeta>(_dbContext)); }
        }
        public IGenericRepository<SpezialRazdeliDisziplini> SpezialRazdeliDisziplini
        {
            get { return _spezialRazdeliDisziplini ?? (_spezialRazdeliDisziplini = new EfGenericRepository<SpezialRazdeliDisziplini>(_dbContext)); }
        }
        public IGenericRepository<SpisokGrupp> SpisokGrupp
        {
            get { return _spisokGrupp ?? (_spisokGrupp = new EfGenericRepository<SpisokGrupp>(_dbContext)); }
        }
        public IGenericRepository<VidiRabot> VidiRabot
        {
            get { return _vidiRabot ?? (_vidiRabot = new EfGenericRepository<VidiRabot>(_dbContext)); }
        }
        public IGenericRepository<VremyaZanyatia> VremyaZanyatia
        {
            get { return _vremyaZanyatia ?? (_vremyaZanyatia = new EfGenericRepository<VremyaZanyatia>(_dbContext)); }
        }

        public IGenericRepository<Notification> Notifications
        {
            get { return _notifications ?? (_notifications = new EfGenericRepository<Notification>(_dbContext)); }
        }



        public IGenericRepository<Library_Book> LibraryBooks
        {
            get { return _libraryBooks ?? (_libraryBooks = new EfGenericRepository<Library_Book>(_dbContext)); }
        }
        public IGenericRepository<Library_EventType> LibraryEventTypes
        {
            get { return _libraryEventTypes ?? (_libraryEventTypes = new EfGenericRepository<Library_EventType>(_dbContext)); }
        }
        public IGenericRepository<Library_History> LibraryHistorys
        {
            get { return _libraryHistorys ?? (_libraryHistorys = new EfGenericRepository<Library_History>(_dbContext)); }
        }
        public IGenericRepository<Library_Literature> LibraryLiteratures
        {
            get { return _libraryLiteratures ?? (_libraryLiteratures = new EfGenericRepository<Library_Literature>(_dbContext)); }
        }
        public IGenericRepository<Library_ReservateBook> LibraryReservateBook
        {
            get { return _libraryReservateBook ?? (_libraryReservateBook = new EfGenericRepository<Library_ReservateBook>(_dbContext)); }
        }

        public IGenericRepository<Prepod_PrepodLikes> PrepodLikes
        {
            get { return _prepodLikes ?? (_prepodLikes = new EfGenericRepository<Prepod_PrepodLikes>(_dbContext)); }
        }

    }
}
