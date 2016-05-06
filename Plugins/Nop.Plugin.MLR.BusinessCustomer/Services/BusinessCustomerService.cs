using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Data;
using Nop.Data;
using Nop.Plugin.MLR.BusinessCustomer.Domain;

namespace Nop.Plugin.MLR.BusinessCustomer.Services
{
    public class BusinessCustomerService : IBusinessCustomerService
    {
        private readonly IRepository<MLR_BusinessCustomer> _businessCustomerRepository;

        public BusinessCustomerService(IRepository<MLR_BusinessCustomer> businessCustomerRepository)
        {
            _businessCustomerRepository = businessCustomerRepository;
        }

        public virtual MLR_BusinessCustomer GetById(int businessCustomerId)
        {
            if (businessCustomerId == 0)
            {
                return null;
            }

            return _businessCustomerRepository.GetById(businessCustomerId);
        }




        public virtual void UpdateBusinessCustomer(MLR_BusinessCustomer businessCustomer)
        {
            if (businessCustomer == null)
                throw new ArgumentNullException("businessCustomer");

            _businessCustomerRepository.Update(businessCustomer);
        }
    }
}
