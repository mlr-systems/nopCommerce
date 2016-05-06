using System.Collections.Generic;
using Nop.Core;
using Nop.Core.Domain.Common;

namespace Nop.Plugin.MLR.BusinessCustomer.Domain
{
    public class MLR_BusinessCustomer : BaseEntity
    {
        public virtual int BusinessCustomerId { get; set; }
        public virtual string MlrBusinessCode { get; set; }
        public virtual string BusinessName { get; set; }
        public virtual string BusinessCustomerContact { get; set; }
        public virtual int ResellerNumber { get; set; }
        public virtual string W9 { get; set; }
        public virtual Address BillingAddress { get; set; }
        public virtual Address ShippingAddress { get; set; }
        public virtual bool Active { get; set; }
        public virtual bool Deleted { get; set; }

        private ICollection<Address> _addresses;
        public virtual ICollection<Address> Addresses
        {
            get { return _addresses ?? (_addresses = new List<Address>()); }
            protected set { _addresses = value; }
        }

        //private ICollection<MLR_BusinessCustomerEmployee> _businessCustomerEmployees;

        //public virtual ICollection<MLR_BusinessCustomerEmployee> BusinessCustomerEmployees
        //{
        //    get {return _businessCustomerEmployees ?? (_businessCustomerEmployees = new List<MLR_BusinessCustomerEmployee>());}
        //    protected set { _businessCustomerEmployees = value; }
        //}

        //private ICollection<MLR_Employee> _mlrEmployees;

        //public virtual ICollection<MLR_Employee> MlrEmployees
        //{
        //    get { return _mlrEmployees ?? (_mlrEmployees = new List<MLR_Employee>()); }
        //    protected set { _mlrEmployees = value; }
        //}
    }
}
