using Nop.Core;
using Nop.Core.Domain.Customers;

namespace Nop.Plugin.MLR.BusinessCustomer.Domain
{
    public class MLR_BusinessCustomerEmployee : BaseEntity
    {
        public int BusinessCustomerId { get; set; } 
        public int BusinessCustomerEmployeeId { get; set; } 
        public bool Active { get; set; }

        public virtual Customer Customer { get; set; } 
        public virtual MLR_BusinessCustomer BusinessCustomer { get; set; }
    }
}
