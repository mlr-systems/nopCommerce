using System.Collections.Generic;
using Nop.Core;
using Nop.Core.Domain.Customers;

namespace Nop.Plugin.MLR.BusinessCustomer.Domain
{
    public class MLR_Employee : BaseEntity
    {
        public int MlrEmployeeId { get; set; } 

        public virtual Customer Customer { get; set; } 

        
    }
}
