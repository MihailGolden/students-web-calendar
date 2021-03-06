[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(WebCalendar.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(WebCalendar.App_Start.NinjectWebCommon), "Stop")]

namespace WebCalendar.App_Start
{
    using DAL.Infrastructure;
    using Infrastructure;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using Ninject;
    using Ninject.Activation;
    using Ninject.Modules;
    using Ninject.Web.Common;
    using Services.Infrastructure;
    using System;
    using System.Collections.Generic;
    using System.Web;

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
            //    System.Web.Mvc.DependencyResolver.SetResolver(new
            //WebCalendar.Infrastructure.NinjectDependencyResolver(kernel));
            var modules = new List<INinjectModule> {
                new NinjectDataAccessModule(),
                new NinjectSericeModule(),
                new NinjectUIModule()
            };
            kernel.Load(modules);
        }
        private static Func<IContext, object> GetRequestScopeCallback()
        {
            var kernel = new StandardKernel();
            var result = kernel.Bind<string>().ToSelf().InRequestScope().BindingConfiguration.ScopeCallback;
            return result;
        }
    }
}
