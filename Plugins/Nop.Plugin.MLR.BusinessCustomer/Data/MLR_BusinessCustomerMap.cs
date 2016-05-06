using System.Data.Entity.ModelConfiguration;

namespace Nop.Plugin.MLR.BusinessCustomer.Data
{
    public class MLR_BusinessCustomerMap : EntityTypeConfiguration<Domain.MLR_BusinessCustomer>
    {
        public MLR_BusinessCustomerMap()
        {
            ToTable("MLR_BusinessCustomers");

            HasKey(x => x.BusinessCustomerId);
            Ignore(x => x.Id);
            

            Property(x => x.MlrBusinessCode)
                .HasMaxLength(10);

            Property(x => x.BusinessName);
            Property(x => x.BusinessCustomerContact);
            Property(x => x.ResellerNumber);
            Property(x => x.W9);
            Property(x => x.Active);
            Property(x => x.Deleted);

           

            //HasMany(x => x.BusinessCustomerEmployees)
            //    .WithMany()
            //    .Map(x => x.ToTable("MLR_BusinessCustomersEmployees"));

            //HasMany(x => x.MlrEmployees)
            //    .WithMany()
            //    .Map(x => x.ToTable("MLR_BusinessCustomersXMLR_Employees"));

            HasMany(x => x.Addresses)
                .WithMany()
                .Map(x => x.ToTable("MLR_BusinessCustomersXAddress").MapLeftKey("BusinessCustomerId"));
            HasOptional(x => x.BillingAddress);
            HasOptional(x => x.ShippingAddress);

        }
    }
}
