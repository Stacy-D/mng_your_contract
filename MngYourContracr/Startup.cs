using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using MngYourContracr.Models;
using Owin;
using System.Security.Claims;

[assembly: OwinStartupAttribute(typeof(MngYourContracr.Startup))]
namespace MngYourContracr
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }

    }
}
