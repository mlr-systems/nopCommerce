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
        public ActionResult Create()
        {
            
            var model = new Domain.BusinessCustomer();
           
            return View("~/Plugins/MLR.BusinessCustomer/Views/BusinessCustomerHome/Create.cshtml", model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        [ValidateInput(false)]
        public ActionResult Create(Domain.BusinessCustomer model, bool continueEditing, FormCollection form)
        {
            Domain.BusinessCustomer businessCustomer = new Domain.BusinessCustomer();

            if (ModelState.IsValid)
            {
                businessCustomer.BusinessName = model.BusinessName;
                businessCustomer.BusinessCustomerContact = model.BusinessCustomerContact;
                businessCustomer.ResellerNumber = model.ResellerNumber;
                businessCustomer.W9 = model.W9;
                businessCustomer.Id = model.Id;

                _businessCustomerRepo.Insert(businessCustomer);
            }

            if (continueEditing)
            {
                return RedirectToAction("Edit", "BusinessCustomerHome", new { id = businessCustomer.Id });
            }
            return RedirectToAction("List", "BusinessCustomerHome");
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

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        public ActionResult Edit(Domain.BusinessCustomer model, bool continueEditing, FormCollection form)
        {
            var businessCustomer = _businessCustomerRepo.Table.Single(x => x.Id == model.Id);

            //if (businessCustomer == null || businessCustomer.Deleted)
            if (businessCustomer == null)
            {
                //No customer found with the specified id
                return RedirectToAction("List");
            }
                
            if (ModelState.IsValid)
            {
                businessCustomer.BusinessName = model.BusinessName;
                businessCustomer.BusinessCustomerContact = model.BusinessCustomerContact;
                businessCustomer.ResellerNumber = model.ResellerNumber;
                businessCustomer.W9 = model.W9;
                businessCustomer.Id = model.Id;

                _businessCustomerRepo.Update(businessCustomer);
            }

            if (continueEditing)
            {
                return RedirectToAction("Edit", "BusinessCustomerHome", new { id = businessCustomer.Id });
            }
            return RedirectToAction("List", "BusinessCustomerHome");
        }
    }
}
