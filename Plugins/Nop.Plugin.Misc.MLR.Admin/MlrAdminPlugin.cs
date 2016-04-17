using System;
using System.Web.Routing;
using Nop.Core.Plugins;
using Nop.Plugin.Misc.MLR.Admin.Data;
using Nop.Services.Common;

namespace Nop.Plugin.Misc.MLR.Admin
{
    public class MlrAdminPlugin : BasePlugin, IMiscPlugin
    {
        private MlrAdminContext _context;

        public MlrAdminPlugin(MlrAdminContext context)
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

        public void GetConfigurationRoute(out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        {
            actionName = "Configure";
            controllerName = "MlrAdmin";
            routeValues = new RouteValueDictionary { { "Namespaces", "Nop.Plugin.Misc.MLR.Admin.Controllers" }, { "area", null } };
        }
    }
}
