using System;
using System.Threading.Tasks;
using System.Web.Hosting;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(DotvvmMvcIntegration.Startup))]

namespace DotvvmMvcIntegration
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var applicationPath = HostingEnvironment.ApplicationPhysicalPath;

            app.UseDotVVM<DotvvmStartup>(applicationPath);
        }
    }
}
