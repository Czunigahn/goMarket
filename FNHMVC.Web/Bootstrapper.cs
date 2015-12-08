using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using System.Reflection;
using FNHMVC.Model;
using FNHMVC.CommandProcessor.Command;
using FNHMVC.CommandProcessor.Dispatcher;
using FNHMVC.Data.Infrastructure;
using FNHMVC.Data.Repositories;
using FNHMVC.Web.Core.Authentication;
using System.Web.Http;
using FNHMVC.Web.Core.Autofac;
using NHibernate;
//using log4net;
using System;


namespace FNHMVC.Web
{
    public static class Bootstrapper
    {
        // private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(Bootstrapper).Name);

       // private static readonly ILog logger = LogManager.GetLogger(typeof(Bootstrapper));

        public static void Run()
        {
            SetAutofacContainer();
        }

        private static void SetAutofacContainer()
        {
            var builder = new ContainerBuilder();

            //logger.Debug(" logger.Debug - > starting SetAutofacContainer Bootstrapper Repository");
           Helpers.LoggerManager.WriteDebug("Helpers.LoggerManager.WriteDebug -> starting SetAutofacContainer Bootstrapper Repository");


            //logger.Debug("Debugging Message");
            //logger.Info("Info message");
            //logger.Warn("Warning Message");
            //logger.Fatal("Fatal error");
            //logger.Error("Error normal");


            //try
            //{
            //    log4net.Appender.LogentriesAppender f = new log4net.Appender.LogentriesAppender();
            //    f.AccountKey = "46995b2a-40ab-4e31-a980-1fed29d8a837";
            //    f.Token = "c9b8de7c-afff-47cd-b400-846804146da2";
            //    f.Location = "gomarket.apphb.com";
            //    f.UseSsl = true;

                
                
            //}
            //catch (Exception ex)
            //{
            //    var e = ex.Message;
            //}

            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterType<DefaultCommandBus>().As<ICommandBus>().InstancePerHttpRequest();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerHttpRequest();

            builder.RegisterAssemblyTypes(typeof(IRepository<Expense>).Assembly).Where(t => t.Name.EndsWith("ExpenseRepository")).AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(typeof(IRepository<Category>).Assembly).Where(t => t.Name.EndsWith("CategoryRepository")).AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(typeof(IRepository<User>).Assembly).Where(t => t.Name.EndsWith("UserRepository")).AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(typeof(IRepository<Token>).Assembly).Where(t => t.Name.EndsWith("TokenRepository")).AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(typeof(IRepository<Sale>).Assembly).Where(t => t.Name.EndsWith("SaleRepository")).AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(typeof(IRepository<Cupon>).Assembly).Where(t => t.Name.EndsWith("CuponRepository")).AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(typeof(IRepository<Follow>).Assembly).Where(t => t.Name.EndsWith("FollowRepository")).AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(typeof(IRepository<Denounce>).Assembly).Where(t => t.Name.EndsWith("DenounceRepository")).AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(typeof(IRepository<Transaction>).Assembly).Where(t => t.Name.EndsWith("TransactionRepository")).AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(typeof(IRepository<GoodDeal>).Assembly).Where(t => t.Name.EndsWith("GoodDealRepository")).AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(typeof(IRepository<UserReviews>).Assembly).Where(t => t.Name.EndsWith("UserReviewsRepository")).AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(typeof(IRepository<SalePendingChange>).Assembly).Where(t => t.Name.EndsWith("SalePendingChangeRepository")).AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(typeof(IRepository<SaleImagesRepository>).Assembly).Where(t => t.Name.EndsWith("SaleImagesRepository")).AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(typeof(IRepository<Cart>).Assembly).Where(t => t.Name.EndsWith("CartRepository")).AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(typeof(IRepository<UserInbox>).Assembly).Where(t => t.Name.EndsWith("UserInboxRepository")).AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(typeof(IRepository<UserAddress>).Assembly).Where(t => t.Name.EndsWith("UserAddressRepository")).AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(typeof(IRepository<UserWishList>).Assembly).Where(t => t.Name.EndsWith("UserWishListRepository")).AsImplementedInterfaces().InstancePerLifetimeScope();


            var services = Assembly.Load("FNHMVC.Domain");
            builder.RegisterAssemblyTypes(services).AsClosedTypesOf(typeof(ICommandHandler<>)).InstancePerHttpRequest();
            builder.RegisterAssemblyTypes(services).AsClosedTypesOf(typeof(IValidationHandler<>)).InstancePerHttpRequest();
            builder.RegisterType<DefaultFormsAuthentication>().As<IFormsAuthentication>().InstancePerHttpRequest();
            builder.RegisterFilterProvider();

            builder.Register(c => FNHMVC.Data.Infrastructure.ConnectionHelper.BuildSessionFactory(FNHMVC.Data.Infrastructure.Utility.ConnectionStringName)).As<ISessionFactory>().SingleInstance();
            builder.Register(c => c.Resolve<ISessionFactory>().OpenSession()).InstancePerLifetimeScope();

            IContainer container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));


            //logger.Info("logger.Info -> finish SetAutofacContainer Bootstrapper Repository");
            //logger.Warn("logger.Warn ->This is a warning message");


            //Helpers.LoggerManager.WriteInfo("Helpers.LoggerManager.WriteInfo -> finish SetAutofacContainer Bootstrapper Repository");
            //Helpers.LoggerManager.WriteAlert("Helpers.LoggerManager.WriteAlert -? This is a warning message");
            Helpers.LoggerManager.WriteDebug("Helpers.LoggerManager.WriteDebug -> finish SetAutofacContainer Bootstrapper Repository");
        }
    }
}
