using System.Data.Entity.ModelConfiguration;
using Nop.Plugin.Misc.MLR.Admin.Domain;

namespace Nop.Plugin.Misc.MLR.Admin.Data
{
    public class BusinessCustomerMap : EntityTypeConfiguration<BusinessCustomer>
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
