using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EaglesNestMMA.Startup))]
namespace EaglesNestMMA
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
