using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BPL_Project.Startup))]
namespace BPL_Project
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
