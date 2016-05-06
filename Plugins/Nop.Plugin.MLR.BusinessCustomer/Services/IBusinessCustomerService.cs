using Nop.Plugin.MLR.BusinessCustomer.Domain;

namespace Nop.Plugin.MLR.BusinessCustomer.Services
{
    public interface IBusinessCustomerService
    {
        MLR_BusinessCustomer GetById(int businessCustomerId);

        void UpdateBusinessCustomer(MLR_BusinessCustomer businessCustomer);
    }
}
