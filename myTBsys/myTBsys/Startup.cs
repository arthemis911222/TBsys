using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(myTBsys.Startup))]
namespace myTBsys
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
