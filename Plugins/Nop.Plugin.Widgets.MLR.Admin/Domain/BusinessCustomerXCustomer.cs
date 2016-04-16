using Nop.Core;
using Nop.Core.Domain.Customers;

namespace Nop.Plugin.Misc.MLR.Admin.Domain
{
    public class BusinessCustomerXCustomer : BaseEntity
    {
        public int BusinessCustomerId { get; set; }
        public int CustomerId { get; set; }

        public BusinessCustomer BusinessCustomer { get; set; }
        public Customer Customer { get; set; }
    }
}
