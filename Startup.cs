using System.Net;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GreatEastForex.Startup))]
namespace GreatEastForex
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //ConfigureAuth(app);
            ServicePointManager.SecurityProtocol =
            SecurityProtocolType.Tls12 |
            SecurityProtocolType.Tls11 |
            SecurityProtocolType.Tls;
        }
    }
}
