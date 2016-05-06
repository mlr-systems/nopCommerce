using System.Data.Entity.ModelConfiguration;

namespace Nop.Plugin.MLR.BusinessCustomer.Data
{
    public class MLR_EmployeeMap : EntityTypeConfiguration<Domain.MLR_Employee>
    {
        public MLR_EmployeeMap()
        {
            this.ToTable("MLR_Employees");
            this.HasKey(x => x.MlrEmployeeId);
            Ignore(x => x.Id);

            Property(x => x.MlrEmployeeId);

            this.HasRequired(x => x.Customer);

        }
    }
}
