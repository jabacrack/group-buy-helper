using GroupBuyHelper.Data;
using Microsoft.EntityFrameworkCore;

namespace GroupBuyHelper.Tests.Fixtures
{
    public class DatabaseFixture
    {
        public readonly DbContextOptions<ApplicationContext> Options;

        public DatabaseFixture()
        {

            Options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase("TestDatabase")
                .Options;

        }

        public void ResetDb()
        {
            using var context = new ApplicationContext(Options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }
    }
}