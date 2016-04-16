﻿using System.Collections.Generic;
using Nop.Core;

namespace Nop.Plugin.Misc.MLR.Admin.Domain
{
    public class BusinessCustomer : BaseEntity
    {
        //public BusinessCustomer()
        //{
        //    BusinessCustomerXCustomers = new List<BusinessCustomerXCustomer>();
        //}

        public virtual int BusinessCustomerId { get; set; }
        public virtual string BusinessName { get; set; }
        public virtual string BusinessCustomerContact { get; set; }
        public virtual int ResellerNumber { get; set; }
        public virtual string W9 { get; set; }

        //public virtual List<BusinessCustomerXCustomer> BusinessCustomerXCustomers { get; set; }
    }
}
