using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace GroupBuyHelper.Data
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<ProductList> UserProductsList { get; set; }
        public ICollection<UserOrderItem> Orders { get; set; }
    }
}