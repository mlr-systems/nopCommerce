using System.Data.Entity.ModelConfiguration;

namespace Nop.Plugin.MLR.BusinessCustomer.Data
{
    public class BusinessCustomerXCustomerMap : EntityTypeConfiguration<BusinessCustomerXCustomerMap>
    {
        public BusinessCustomerXCustomerMap()
        {
            ToTable("BusinessCustomerXCustomer");

        }
    }
}
