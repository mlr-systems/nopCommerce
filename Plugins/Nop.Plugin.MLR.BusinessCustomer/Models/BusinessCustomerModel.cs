using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.MLR.BusinessCustomer.Models
{
    public class BusinessCustomerModel
    {
        public int BusinessCustomerId { get; set; }
        public string MlrCustomerCode { get; set; }
        public string BusinessName { get; set; }
    }
}
