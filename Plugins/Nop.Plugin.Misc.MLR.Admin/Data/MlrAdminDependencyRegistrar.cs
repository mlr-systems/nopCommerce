using Autofac;
using Autofac.Core;
using Nop.Core.Configuration;
using Nop.Core.Data;
using Nop.Core.Infrastructure;
using Nop.Core.Infrastructure.DependencyManagement;
using Nop.Data;
using Nop.Plugin.Misc.MLR.Admin.Domain;
using Nop.Web.Framework.Mvc;

namespace Nop.Plugin.Misc.MLR.Admin.Data
{
    public class MlrAdminDependencyRegistrar : IDependencyRegistrar
    {
        private const string ContextName = "nop_object_context_mlr_admin";

        public void Register(ContainerBuilder builder, ITypeFinder typeFinder, NopConfig config)
        {
            this.RegisterPluginDataContext<MlrAdminContext>(builder, ContextName);

            builder.RegisterType<EfRepository<BusinessCustomer>>()
                .As<IRepository<BusinessCustomer>>()
                .WithParameter(ResolvedParameter.ForNamed<IDbContext>(ContextName))
                .InstancePerLifetimeScope();
        }

        public int Order { get { return 1; } }
    }
}
