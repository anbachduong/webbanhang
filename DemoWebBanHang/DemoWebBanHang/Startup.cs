using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DemoWebBanHang.Startup))]
namespace DemoWebBanHang
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
