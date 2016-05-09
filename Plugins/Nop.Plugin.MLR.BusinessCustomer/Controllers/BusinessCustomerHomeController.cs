using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Nop.Admin.Extensions;
using Nop.Admin.Models.Common;
using Nop.Admin.Models.Customers;
using Nop.Core;
using Nop.Core.Data;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Directory;
using Nop.Plugin.MLR.BusinessCustomer.Domain;
using Nop.Plugin.MLR.BusinessCustomer.Extensions;
using Nop.Plugin.MLR.BusinessCustomer.Models;
using Nop.Plugin.MLR.BusinessCustomer.Services;
using Nop.Services.Common;
using Nop.Services.Directory;
using Nop.Services.Security;
//using Nop.Web.Extensions;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Kendoui;
using Nop.Web.Framework.Mvc;

namespace Nop.Plugin.MLR.BusinessCustomer.Controllers
{
    public class BusinessCustomerHomeController : BasePluginController
    {
        private readonly IRepository<Domain.MLR_BusinessCustomer> _businessCustomerRepo;
        private readonly IAddressService _addressService;
        private readonly ICountryService _countryService;
        private readonly IStateProvinceService _stateProvinceService;
        private readonly AddressSettings _addressSettings;
        private readonly IAddressAttributeParser _addressAttributeParser;
        private readonly IAddressAttributeService _addressAttributeService;
        private readonly IAddressAttributeFormatter _addressAttributeFormatter;
        private readonly IBusinessCustomerService _businessCustomerService;

        public BusinessCustomerHomeController(IRepository<Domain.MLR_BusinessCustomer> businessCustomerRepo,
                                              AddressSettings addressSettings, IAddressService addressService,
                                              ICountryService countryService, IStateProvinceService stateProvinceService,
                                              IAddressAttributeParser addressAttributeParser, IAddressAttributeService addressAttributeService,
                                              IAddressAttributeFormatter addressAttributeFormatter, IBusinessCustomerService businessCustomerService)
        {
            _businessCustomerRepo = businessCustomerRepo;
            _addressSettings = addressSettings;
            _addressService = addressService;
            _countryService = countryService;
            _stateProvinceService = stateProvinceService;
            _addressAttributeParser = addressAttributeParser;
            _addressAttributeService = addressAttributeService;
            _addressAttributeFormatter = addressAttributeFormatter;
            _businessCustomerService = businessCustomerService;
        }

        [HttpPost]
        public ActionResult BusinessCustomerList(DataSourceRequest command, Domain.MLR_BusinessCustomer model)
        {

            var businessCustomers = _businessCustomerRepo.Table.OrderBy(x => x.BusinessCustomerId).Select(x => new BusinessCustomerModel()
            {
                BusinessCustomerId = x.BusinessCustomerId,
                BusinessName = x.BusinessName,
                MlrCustomerCode = x.MlrBusinessCode
            });

            IPagedList<BusinessCustomerModel> pagedBusinessCustomers =
                    new PagedList<BusinessCustomerModel>(businessCustomers, command.Page - 1, command.PageSize);

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

            var model = new Domain.MLR_BusinessCustomer();

            return View("~/Plugins/MLR.BusinessCustomer/Views/BusinessCustomerHome/Create.cshtml", model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        [ValidateInput(false)]
        public ActionResult Create(Domain.MLR_BusinessCustomer model, bool continueEditing, FormCollection form)
        {
            Domain.MLR_BusinessCustomer businessCustomer = new Domain.MLR_BusinessCustomer();

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
            var businessCustomer = _businessCustomerRepo.Table.Single(x => x.BusinessCustomerId == id);

            if (businessCustomer == null)
            {
                RedirectToAction("List", "BusinessCustomerHome");
            }

            return View("~/Plugins/MLR.BusinessCustomer/Views/BusinessCustomerHome/Edit.cshtml", businessCustomer);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        public ActionResult Edit(Domain.MLR_BusinessCustomer model, bool continueEditing, FormCollection form)
        {
            var businessCustomer = _businessCustomerRepo.Table.Single(x => x.BusinessCustomerId == model.BusinessCustomerId);

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

        #region Addresses

        [HttpPost]
        public ActionResult AddressesSelect(int businessCustomerId, DataSourceRequest command)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageCustomers))
            //    return AccessDeniedView();

            var businessCustomer = _businessCustomerRepo.Table.Single(x => x.BusinessCustomerId == businessCustomerId);

            if (businessCustomer == null)
                throw new ArgumentException("Unable to find selected Business Customer", "businessCustomerId");

            var addresses = businessCustomer.Addresses.OrderByDescending(a => a.CreatedOnUtc).ThenByDescending(a => a.Id).ToList();
            var gridModel = new DataSourceResult
            {
                Data = addresses.Select(x =>
                {
                    var model = x.ToModel();
                    var addressHtmlSb = new StringBuilder("<div>");
                    if (_addressSettings.CompanyEnabled && !String.IsNullOrEmpty(model.Company))
                        addressHtmlSb.AppendFormat("{0}<br />", Server.HtmlEncode(model.Company));
                    if (_addressSettings.StreetAddressEnabled && !String.IsNullOrEmpty(model.Address1))
                        addressHtmlSb.AppendFormat("{0}<br />", Server.HtmlEncode(model.Address1));
                    if (_addressSettings.StreetAddress2Enabled && !String.IsNullOrEmpty(model.Address2))
                        addressHtmlSb.AppendFormat("{0}<br />", Server.HtmlEncode(model.Address2));
                    if (_addressSettings.CityEnabled && !String.IsNullOrEmpty(model.City))
                        addressHtmlSb.AppendFormat("{0},", Server.HtmlEncode(model.City));
                    if (_addressSettings.StateProvinceEnabled && !String.IsNullOrEmpty(model.StateProvinceName))
                        addressHtmlSb.AppendFormat("{0},", Server.HtmlEncode(model.StateProvinceName));
                    if (_addressSettings.ZipPostalCodeEnabled && !String.IsNullOrEmpty(model.ZipPostalCode))
                        addressHtmlSb.AppendFormat("{0}<br />", Server.HtmlEncode(model.ZipPostalCode));
                    if (_addressSettings.CountryEnabled && !String.IsNullOrEmpty(model.CountryName))
                        addressHtmlSb.AppendFormat("{0}", Server.HtmlEncode(model.CountryName));
                    var customAttributesFormatted = _addressAttributeFormatter.FormatAttributes(x.CustomAttributes);
                    if (!String.IsNullOrEmpty(customAttributesFormatted))
                    {
                        //already encoded
                        addressHtmlSb.AppendFormat("<br />{0}", customAttributesFormatted);
                    }
                    addressHtmlSb.Append("</div>");
                    model.AddressHtml = addressHtmlSb.ToString();
                    return model;
                }),
                Total = addresses.Count
            };

            return Json(gridModel);
        }

        [HttpPost]
        public ActionResult AddressDelete(int id, int businessCustomerId)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageCustomers))
            //    return AccessDeniedView();

            var businessCustomer = _businessCustomerService.GetById(businessCustomerId);

            if (businessCustomer == null)
            {
                throw new ArgumentException("No Business Customer found with the specified id", "businessCustomerId");
            }

            var address = businessCustomer.Addresses.FirstOrDefault(a => a.Id == id);
            if (address == null)
            {
                //No customer found with the specified id
                return Content("No Business Customer found with the specified id");
            }

            businessCustomer.RemoveAddress(address);
            _businessCustomerService.UpdateBusinessCustomer(businessCustomer);
            
            // now delete the address record
            // the AddressService has a different context so we need to force
            // the attachment so we can delete it
            address = _addressService.GetAddressById(address.Id);
            _addressService.DeleteAddress(address);

            return new NullJsonResult();
        }

        public ActionResult AddressCreate(int businessCustomerId)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageCustomers))
            //    return AccessDeniedView();

            var businessCustomer = _businessCustomerService.GetById(businessCustomerId);
            if (businessCustomer == null)
                //No customer found with the specified id
                return RedirectToAction("List");

            var model = new BusinessCustomerAddressModel();
            PrepareAddressModel(model, null, businessCustomer, false);

            return View("~/Plugins/MLR.BusinessCustomer/Views/BusinessCustomerHome/AddressCreate.cshtml", model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddressCreate(BusinessCustomerAddressModel model, FormCollection form)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageCustomers))
            //    return AccessDeniedView();

            var businessCustomer = _businessCustomerService.GetById(model.BusinessCustomerId);
            if (businessCustomer == null)
                //No customer found with the specified id
                return RedirectToAction("List");

            //custom address attributes
            var customAttributes = form.ParseCustomAddressAttributes(_addressAttributeParser, _addressAttributeService);
            var customAttributeWarnings = _addressAttributeParser.GetAttributeWarnings(customAttributes);
            foreach (var error in customAttributeWarnings)
            {
                ModelState.AddModelError("", error);
            }

            if (ModelState.IsValid)
            {
                var address = model.Address.ToEntity();
                address.CustomAttributes = customAttributes;
                address.CreatedOnUtc = DateTime.UtcNow;
                //some validation
                if (address.CountryId == 0)
                    address.CountryId = null;
                if (address.StateProvinceId == 0)
                    address.StateProvinceId = null;
                businessCustomer.Addresses.Add(address);
                _businessCustomerService.UpdateBusinessCustomer(businessCustomer);

                SuccessNotification("Address Added");
                return RedirectToAction("AddressEdit", new { addressId = address.Id, businessCustomerId = model.BusinessCustomerId });
            }

            //If we got this far, something failed, redisplay form
            PrepareAddressModel(model, null, businessCustomer, true);
            return View("~/Plugins/MLR.BusinessCustomer/Views/BusinessCustomerHome/AddressCreate.cshtml", model);
        }

        public ActionResult AddressEdit(int addressId, int businessCustomerId)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageCustomers))
            //    return AccessDeniedView();

            var customer = _businessCustomerService.GetById(businessCustomerId);
            if (customer == null)
                //No customer found with the specified id
                return RedirectToAction("List");

            var address = _addressService.GetAddressById(addressId);
            if (address == null)
                //No address found with the specified id
                return RedirectToAction("Edit", new { id = customer.Id });

            var model = new BusinessCustomerAddressModel();
            PrepareAddressModel(model, address, customer, false);
            return View("~/Plugins/MLR.BusinessCustomer/Views/BusinessCustomerHome/AddressEdit.cshtml", model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddressEdit(BusinessCustomerAddressModel model, FormCollection form)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageCustomers))
            //    return AccessDeniedView();

            var customer = _businessCustomerService.GetById(model.BusinessCustomerId);
            if (customer == null)
                //No customer found with the specified id
                return RedirectToAction("List");

            var address = _addressService.GetAddressById(model.Address.Id);
            if (address == null)
                //No address found with the specified id
                return RedirectToAction("Edit", new { id = customer.Id });

            //custom address attributes
            var customAttributes = form.ParseCustomAddressAttributes(_addressAttributeParser, _addressAttributeService);
            var customAttributeWarnings = _addressAttributeParser.GetAttributeWarnings(customAttributes);
            foreach (var error in customAttributeWarnings)
            {
                ModelState.AddModelError("", error);
            }

            if (ModelState.IsValid)
            {
                address = model.Address.ToEntity(address);
                address.CustomAttributes = customAttributes;
                _addressService.UpdateAddress(address);

                SuccessNotification("Address Updated");
                return RedirectToAction("AddressEdit", new { addressId = model.Address.Id, businessCustomerId = model.BusinessCustomerId });
            }

            //If we got this far, something failed, redisplay form
            PrepareAddressModel(model, address, customer, true);

            return View("~/Plugins/MLR.BusinessCustomer/Views/BusinessCustomerHome/AddressEdit.cshtml", model);
        }

        #endregion

        [NonAction]
        protected virtual void PrepareAddressModel(BusinessCustomerAddressModel model, Address address, MLR_BusinessCustomer businessCustomer, bool excludeProperties)
        {
            if (businessCustomer == null)
                throw new ArgumentNullException("customer");

            model.BusinessCustomerId = businessCustomer.BusinessCustomerId;
            if (address != null)
            {
                if (!excludeProperties)
                {
                    model.Address = address.ToModel();
                }
            }

            if (model.Address == null)
                model.Address = new AddressModel();

            model.Address.FirstNameEnabled = true;
            model.Address.FirstNameRequired = true;
            model.Address.LastNameEnabled = true;
            model.Address.LastNameRequired = true;
            model.Address.EmailEnabled = true;
            model.Address.EmailRequired = true;
            model.Address.CompanyEnabled = _addressSettings.CompanyEnabled;
            model.Address.CompanyRequired = _addressSettings.CompanyRequired;
            model.Address.CountryEnabled = _addressSettings.CountryEnabled;
            model.Address.StateProvinceEnabled = _addressSettings.StateProvinceEnabled;
            model.Address.CityEnabled = _addressSettings.CityEnabled;
            model.Address.CityRequired = _addressSettings.CityRequired;
            model.Address.StreetAddressEnabled = _addressSettings.StreetAddressEnabled;
            model.Address.StreetAddressRequired = _addressSettings.StreetAddressRequired;
            model.Address.StreetAddress2Enabled = _addressSettings.StreetAddress2Enabled;
            model.Address.StreetAddress2Required = _addressSettings.StreetAddress2Required;
            model.Address.ZipPostalCodeEnabled = _addressSettings.ZipPostalCodeEnabled;
            model.Address.ZipPostalCodeRequired = _addressSettings.ZipPostalCodeRequired;
            model.Address.PhoneEnabled = _addressSettings.PhoneEnabled;
            model.Address.PhoneRequired = _addressSettings.PhoneRequired;
            model.Address.FaxEnabled = _addressSettings.FaxEnabled;
            model.Address.FaxRequired = _addressSettings.FaxRequired;
            //countries
            model.Address.AvailableCountries.Add(new SelectListItem { Text = "Select Country", Value = "0" });
            foreach (var c in _countryService.GetAllCountries(showHidden: true))
                model.Address.AvailableCountries.Add(new SelectListItem { Text = c.Name, Value = c.Id.ToString(), Selected = (c.Id == model.Address.CountryId) });
            //states
            var states = model.Address.CountryId.HasValue ? _stateProvinceService.GetStateProvincesByCountryId(model.Address.CountryId.Value, showHidden: true).ToList() : new List<StateProvince>();
            if (states.Count > 0)
            {
                foreach (var s in states)
                    model.Address.AvailableStates.Add(new SelectListItem { Text = s.Name, Value = s.Id.ToString(), Selected = (s.Id == model.Address.StateProvinceId) });
            }
            else
                model.Address.AvailableStates.Add(new SelectListItem { Text = "Other Non US", Value = "0" });
            //customer attribute services
            model.Address.PrepareCustomAddressAttributes(address, _addressAttributeService, _addressAttributeParser);
        }
    }
}
