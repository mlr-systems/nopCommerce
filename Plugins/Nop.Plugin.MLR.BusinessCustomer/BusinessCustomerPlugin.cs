using System.Linq;
using System.Web.Routing;
using Nop.Core.Plugins;
using Nop.Plugin.MLR.BusinessCustomer.Data;
using Nop.Services.Common;
using Nop.Web.Framework.Menu;

namespace Nop.Plugin.MLR.BusinessCustomer
{
    public class BusinessCustomerPlugin : BasePlugin, IMiscPlugin, IAdminMenuPlugin
    {
        private readonly BusinessCustomerContext _context;

        public BusinessCustomerPlugin(BusinessCustomerContext context)
        {
            _context = context;
        }

        public override void Install()
        {
            _context.Install();
            base.Install();
        }

        public override void Uninstall()
        {
            _context.Uninstall();
            base.Uninstall();
        }

        public void ManageSiteMap(SiteMapNode rootNode)
        {
            var menuItem = new SiteMapNode()
            {
                SystemName = "MLR.BusinessCustomer",
                Title = "Manage Business Customers",
                ControllerName = "BusinessCustomerHome",
                ActionName = "ListBusinessCustomers",
                Visible = true,
                RouteValues = new RouteValueDictionary() { { "area", null } },
            };
            var pluginNode = rootNode.ChildNodes.FirstOrDefault(x => x.SystemName == "MLR Plugins");
            if (pluginNode != null)
                pluginNode.ChildNodes.Add(menuItem);
            else
                rootNode.ChildNodes.Add(menuItem);
        }

        public void GetConfigurationRoute(out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        {
            actionName = "Configure";
            controllerName = "BusinessCustomerHome";
            routeValues = new RouteValueDictionary { { "Namespaces", "Nop.Plugin.MLR.BusinessCustomer.Controllers" }, { "area", null } };
        }
    }
}
