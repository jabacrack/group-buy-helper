using System;
using System.Threading.Tasks;
using GroupBuyHelper.Data;
using GroupBuyHelper.Dtos;
using GroupBuyHelper.Services;
using GroupBuyHelper.Tests.Fixtures;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using Xunit;

namespace GroupBuyHelper.Tests
{
    public class ProductsListTests : IClassFixture<DatabaseFixture>, IDisposable
    {
        ProductService productService;
        ApplicationContext applicationContext;
        DbContextOptions<ApplicationContext> options;

        public ProductsListTests(DatabaseFixture fixture)
        {
            fixture.ResetDb();

            options = fixture.Options;

            applicationContext = new ApplicationContext(options);
            productService = new ProductService(applicationContext);
        }

        [Fact]
        public async Task AddProductsList()
        {
            //arrange
            var importRequest = new ImportRequest
            {
                Name = "List",
                ColumnSeparator = "\t",
                CurrencySymbol = "",
                NumberSeparator = ",",
                Items = "Name 1\t20\t2,5\n" +
                        "Name 2\t35\t1,02"
            };

            ApplicationUser user = new ApplicationUser()
            {
                UserName = "Test User"
            };
            await applicationContext.Users.AddAsync(user);
            await applicationContext.SaveChangesAsync();

            //act
            var validations = await productService.AddList(user, importRequest);

            //assert
            Assert.Empty(validations);
            using (var readContext = new ApplicationContext(options))
            {
                var productLists = await readContext.ProductLists.Include(x => x.Products).Include(x => x.Owner).ToArrayAsync();

                ProductList list = Assert.Single(productLists);
                Assert.Equal(importRequest.Name, list.Name);
                Assert.Equal(user.UserName, list.Owner.UserName);
            }
        }

        [Fact]
        public async Task OneUserSetAmount()
        {
            //arrange
            var importRequest = new ImportRequest
            {
                Name = "List",
                ColumnSeparator = "\t",
                CurrencySymbol = "",
                NumberSeparator = ",",
                Items = "Name 1\t20\t2,5\n" +
                        "Name 2\t35\t1,02"
            };
            
            ApplicationUser user = new ApplicationUser()
            {
                UserName = "Test User"
            };
            await applicationContext.Users.AddAsync(user);
            await applicationContext.SaveChangesAsync();
            
            //act
            var validations = await productService.AddList(user, importRequest);
            
            //assert
            Assert.Empty(validations);
            using (var readContext = new ApplicationContext(options))
            {
                var productLists = await readContext.ProductLists.Include(x => x.Products).Include(x => x.Owner).ToArrayAsync();

                ProductList list = Assert.Single(productLists);
                // Assert.Equal(importRequest.Name, list.Name);
                // Assert.Equal(user.UserName, list.Owner.UserName);
            }
        }

        public void Dispose()
        {
            applicationContext?.Dispose();
        }
    }
}