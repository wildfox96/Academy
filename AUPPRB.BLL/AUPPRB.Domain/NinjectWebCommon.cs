using System;
using System.Web;
using AUPPRB.Domain;
using AUPPRB.Domain.Interfaces;
using AUPPRB.Domain.Services;
using AUPPRB.Repository.DB;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Ninject;
using Ninject.Web.Common;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(NinjectWebCommon), "Stop")]

namespace AUPPRB.Domain
{
    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }

        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<IDataProvider>().To<DataProvider>();
            kernel.Bind<IAccountService>().To<AccountService>();
            kernel.Bind<IScheduleService>().To<ScheduleService>();
            kernel.Bind<IDictionaryService>().To<DictionaryService>();
            kernel.Bind<IDisciplineService>().To<DisciplineService>();
            kernel.Bind<IAdminScheduleService>().To<AdminScheduleService>();
            kernel.Bind<IExpertCenterService>().To<ExpertCenterService>();
            kernel.Bind<INotificationService>().To<NotificationService>();
            kernel.Bind<ILiteratureService>().To<LiteratureService>();
            kernel.Bind<ISyncronizationService>().To<SyncronizationService>();
            kernel.Bind<ILikesService>().To<LikesService>();
        }
    }
}
