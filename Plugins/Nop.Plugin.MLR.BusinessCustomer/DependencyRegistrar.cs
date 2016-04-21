using Autofac;
using Autofac.Core;
using Nop.Core.Configuration;
using Nop.Core.Data;
using Nop.Core.Infrastructure;
using Nop.Core.Infrastructure.DependencyManagement;
using Nop.Data;
using Nop.Plugin.MLR.BusinessCustomer.Controllers;
using Nop.Plugin.MLR.BusinessCustomer.Data;
using Nop.Web.Framework.Mvc;

namespace Nop.Plugin.MLR.BusinessCustomer
{
    public class MlrAdminDependencyRegistrar : IDependencyRegistrar
    {
        private const string ContextName = "nop_object_context_mlr_businesscustomer";

        public void Register(ContainerBuilder builder, ITypeFinder typeFinder, NopConfig config)
        {
            this.RegisterPluginDataContext<BusinessCustomerContext>(builder, ContextName);

            builder.RegisterType<EfRepository<Domain.BusinessCustomer>>()
                .As<IRepository<Domain.BusinessCustomer>>()
                .WithParameter(ResolvedParameter.ForNamed<IDbContext>(ContextName))
                .InstancePerLifetimeScope();

            builder.RegisterType<BusinessCustomerHomeController>();
        }

        public int Order { get { return 1; } }
    }
}
