using System.Web.Mvc;
using Nop.Core.Data;
using Nop.Web.Framework.Controllers;

namespace Nop.Plugin.MLR.BusinessCustomer.Controllers
{
    public class BusinessCustomerHomeController : BasePluginController
    {
        private IRepository<Domain.BusinessCustomer> _businessCustomerRepo;

        //public BusinessCustomerHomeController(IRepository<Domain.BusinessCustomer> businessCustomerRepo)
        //{
        //    _businessCustomerRepo = businessCustomerRepo;
        //}

        public ActionResult ListBusinessCustomers()
        {
            return View("~/Plugins/MLR.BusinessCustomer/Views/BusinessCustomerHome/ListBusinessCustomers.cshtml");
        }

        //[ChildActionOnly]
        //[AdminAuthorize]
        public ActionResult Configure()
        {
            return View("~/Plugins/MLR.BusinessCustomer/Views/BusinessCustomerHome/Configure.cshtml");
        }
    }
}
