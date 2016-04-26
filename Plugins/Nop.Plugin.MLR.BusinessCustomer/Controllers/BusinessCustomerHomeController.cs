using System;
using System.Linq;
using System.Web.Mvc;
using Nop.Core.Data;
using Nop.Services.Security;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Kendoui;
using Nop.Web.Framework.Mvc;

namespace Nop.Plugin.MLR.BusinessCustomer.Controllers
{
    public class BusinessCustomerHomeController : BasePluginController
    {
        private IRepository<Domain.BusinessCustomer> _businessCustomerRepo;

        public BusinessCustomerHomeController(IRepository<Domain.BusinessCustomer> businessCustomerRepo)
        {
            _businessCustomerRepo = businessCustomerRepo;
        }

        [HttpPost]
        public ActionResult BusinessCustomerList(DataSourceRequest command, Domain.BusinessCustomer model)
        {

            var businessCustomers = _businessCustomerRepo.Table;

            var gridModel = new DataSourceResult
            {
                Data = businessCustomers,
                Total = businessCustomers.Count()
            };

            return Json(gridModel);
        }

        public ActionResult Index()
        {
            return RedirectToAction("List", "BusinessCustomerHome");
        }

        public ActionResult List()
        {
            //var businessCustomers = _businessCustomerRepo.Table.ToList();

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
