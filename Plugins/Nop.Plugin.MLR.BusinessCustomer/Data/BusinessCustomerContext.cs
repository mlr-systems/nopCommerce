using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using Nop.Core;
using Nop.Data;
using Nop.Data.Mapping.Common;
using Nop.Data.Mapping.Directory;

namespace Nop.Plugin.MLR.BusinessCustomer.Data
{
    public class BusinessCustomerContext : DbContext, IDbContext
    {
        public BusinessCustomerContext(string connection) : base(connection)
        {
            this.Database.Log = x => Debug.WriteLine(x);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new MLR_BusinessCustomerMap());
            //modelBuilder.Configurations.Add(new MLR_EmployeeMap());
            modelBuilder.Configurations.Add(new AddressMap());
            modelBuilder.Configurations.Add(new CountryMap());
            modelBuilder.Configurations.Add(new StateProvinceMap());

            base.OnModelCreating(modelBuilder);
        }

        public string CreateDatabaseInstallationScript()
        {
            return ((IObjectContextAdapter)this).ObjectContext.CreateDatabaseScript();
        }

        public void Install()
        {
            Database.SetInitializer<BusinessCustomerContext>(null);
            //Database.ExecuteSqlCommand(CreateDatabaseInstallationScript());
            //SaveChanges();
        }

        public void Uninstall()
        {
            //this.DropPluginTable("MLR_BusinessCustomer");
            //this.DropPluginTable("MLR_Agent");
        }

        public new IDbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity
        {
            return base.Set<TEntity>();
        }

        public IList<TEntity> ExecuteStoredProcedureList<TEntity>(string commandText, params object[] parameters) where TEntity : BaseEntity, new()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<TElement> SqlQuery<TElement>(string sql, params object[] parameters)
        {
            throw new System.NotImplementedException();
        }

        public int ExecuteSqlCommand(string sql, bool doNotEnsureTransaction = false, int? timeout = null, params object[] parameters)
        {
            throw new System.NotImplementedException();
        }

        public void Detach(object entity)
        {
            throw new System.NotImplementedException();
        }

        public bool ProxyCreationEnabled { get; set; }
        public bool AutoDetectChangesEnabled { get; set; }
    }
}
