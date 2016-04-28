using System;
using System.Linq;
using System.Web.Mvc;
using Nop.Core;
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

            var businessCustomers = _businessCustomerRepo.Table.OrderBy(x => x.BusinessCustomerId);

            IPagedList<Domain.BusinessCustomer> pagedBusinessCustomers = 
                    new PagedList<Domain.BusinessCustomer>(businessCustomers, command.Page - 1, command.PageSize);

            var gridModel = new DataSourceResult
            {
                Data = pagedBusinessCustomers,
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
            return View("~/Plugins/MLR.BusinessCustomer/Views/BusinessCustomerHome/ListBusinessCustomers.cshtml");
        }

        public ActionResult Configure()
        {
            return View("~/Plugins/MLR.BusinessCustomer/Views/BusinessCustomerHome/Configure.cshtml");
        }

        public ActionResult Edit(int id)
        {
            var businessCustomer = _businessCustomerRepo.Table.Single(x => x.Id == id);

            if (businessCustomer == null)
            {
                RedirectToAction("List", "BusinessCustomerHome");
            }

            return View("~/Plugins/MLR.BusinessCustomer/Views/BusinessCustomerHome/Edit.cshtml", businessCustomer);
        }
    }
}
