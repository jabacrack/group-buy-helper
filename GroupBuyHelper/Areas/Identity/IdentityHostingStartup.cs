using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(GroupBuyHelper.Areas.Identity.IdentityHostingStartup))]
namespace GroupBuyHelper.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}