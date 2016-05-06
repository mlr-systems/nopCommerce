using Nop.Admin.Models.Common;
using Nop.Web.Framework.Mvc;

namespace Nop.Plugin.MLR.BusinessCustomer.Models
{
    public class BusinessCustomerAddressModel : BaseNopModel
    {
        public int BusinessCustomerId { get; set; }

        public AddressModel Address { get; set; }
    }
}
