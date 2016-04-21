using System.Data.Entity.ModelConfiguration;

namespace Nop.Plugin.MLR.BusinessCustomer.Data
{
    public class BusinessCustomerMap : EntityTypeConfiguration<Domain.BusinessCustomer>
    {
        public BusinessCustomerMap()
        {
            ToTable("BusinessCustomer");

            HasKey(x => x.BusinessCustomerId);
            Property(x => x.BusinessName);
            Property(x => x.BusinessCustomerContact);
            Property(x => x.ResellerNumber);
            Property(x => x.W9);
        }
    }
}
