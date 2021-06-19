using System.Security.Claims;
using System.Threading.Tasks;
using GroupBuyHelper.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace GroupBuyHelper.Services
{
    public class UserService
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ApplicationContext applicationContext;
        private readonly UserManager<ApplicationUser> userManager;

        public UserService(IHttpContextAccessor httpContextAccessor, ApplicationContext applicationContext, UserManager<ApplicationUser> userManager)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.applicationContext = applicationContext;
            this.userManager = userManager;
        }

        public async Task<ApplicationUser> GetCurrentUser()
        {
            return await userManager.FindByEmailAsync(httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email).Value);
        }

    }
}