using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TestAPP.Startup))]
namespace TestAPP
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
