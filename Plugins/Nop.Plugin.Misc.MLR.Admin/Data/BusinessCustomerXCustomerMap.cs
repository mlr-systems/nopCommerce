using System.Data.Entity.ModelConfiguration;

namespace Nop.Plugin.Misc.MLR.Admin.Data
{
    public class BusinessCustomerXCustomerMap : EntityTypeConfiguration<BusinessCustomerXCustomerMap>
    {
        public BusinessCustomerXCustomerMap()
        {
            ToTable("BusinessCustomerXCustomer");

        }
    }
}
