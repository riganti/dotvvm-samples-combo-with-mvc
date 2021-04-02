using DotVVM.Framework.Configuration;
using DotVVM.Framework.Routing;
using DotvvmMvcIntegration.DotVVM.Controls;
using Microsoft.Extensions.DependencyInjection;

namespace DotvvmMvcIntegration
{
    public class DotvvmStartup : IDotvvmStartup, IDotvvmServiceConfigurator
    {
        public void Configure(DotvvmConfiguration config, string applicationPath)
        {
            ConfigureRoutes(config, applicationPath);
            ConfigureControls(config, applicationPath);
            ConfigureResources(config, applicationPath);
        }

        private void ConfigureRoutes(DotvvmConfiguration config, string applicationPath)
        {
            config.RouteTable.Add("DotvvmSample", "DotvvmSample", "DotVVM/Views/DotvvmSample.dothtml");
            
            // URLs that are not registered in DotVVM are passed to the next middleware (MVC)
        }

        private void ConfigureControls(DotvvmConfiguration config, string applicationPath)
        {
            // register code-only controls and markup controls
            config.Markup.AddCodeControls("cc", typeof(RenderMvcAction));
        }

        private void ConfigureResources(DotvvmConfiguration config, string applicationPath)
        {
            // register custom resources and adjust paths to the built-in resources
        }

        public void ConfigureServices(IDotvvmServiceCollection options)
        {
        }
    }
}