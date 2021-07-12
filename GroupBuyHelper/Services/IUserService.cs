using System.Threading.Tasks;
using GroupBuyHelper.Data;

namespace GroupBuyHelper.Services
{
    public interface IUserService
    {
        Task<ApplicationUser> GetCurrentUser();
    }
}