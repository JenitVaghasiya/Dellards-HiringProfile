using Microsoft.Owin;
using Owin;
using System.Net.Http;

[assembly: OwinStartupAttribute(typeof(svihire.Startup))]
namespace svihire
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
