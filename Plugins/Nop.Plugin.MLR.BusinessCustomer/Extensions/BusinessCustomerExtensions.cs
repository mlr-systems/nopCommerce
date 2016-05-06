using Nop.Core.Domain.Common;
using Nop.Plugin.MLR.BusinessCustomer.Domain;

namespace Nop.Plugin.MLR.BusinessCustomer.Extensions
{
    public static class BusinessCustomerExtensions
    {
        public static void RemoveAddress(this MLR_BusinessCustomer businessCustomer, Address address)
        {
            if (businessCustomer.Addresses.Contains(address))
            {
                if (businessCustomer.BillingAddress == address)
                {
                    businessCustomer.BillingAddress = null;
                }

                if (businessCustomer.ShippingAddress == address)
                {
                    businessCustomer.ShippingAddress = null;
                }

                businessCustomer.Addresses.Remove(address);
            }
        }
    }
}
