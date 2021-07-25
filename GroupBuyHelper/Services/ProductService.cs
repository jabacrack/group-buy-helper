using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GroupBuyHelper.Data;
using GroupBuyHelper.Dtos;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Math.EC.Rfc7748;

namespace GroupBuyHelper.Services
{
    public class ProductService
    {
        private readonly ApplicationContext applicationContext;
        private readonly IUserService userService;

        public ProductService(ApplicationContext applicationContext, IUserService userService)
        {
            this.applicationContext = applicationContext;
            this.userService = userService;
        }

        public async Task<ProductListInfo[]> GetAllProductLists()
        {
            return await applicationContext.ProductLists
                .Include(x => x.Owner)
                .Select(p => new ProductListInfo(p.Id, p.Name, p.Owner.Id))
                .ToArrayAsync();
        }

        public async Task<ProductInfo[]> GetProducts(int productsListId)
        {
            var list = await applicationContext.ProductLists.Include(l => l.Products).FirstAsync(x => x.Id == productsListId);
            return list.Products.Select(x => new ProductInfo(x.Id, x.Name, x.Description, x.Price, x.Amount)).ToArray();
        }

        public async Task<Dictionary<int, int>> GetProductTotal(int productsListId)
        {
            UserOrderItem[] items = await applicationContext.UserOrderItems.Where(o => o.ProductListId == productsListId).ToArrayAsync();
            var summed = items.GroupBy(x => x.ProductId)
                .Select(group => new {Id = group.Key, Sum = group.Sum(x => x.Amount)})
                .ToDictionary(x => x.Id, x => x.Sum);

            return summed;
        }

        public async Task<string[]> AddList(ImportRequest importRequest)
        {
            var parser = new ListParser(columnSeparator: importRequest.ColumnSeparator, numberSeparator: importRequest.NumberSeparator, currencySymbol: importRequest.CurrencySymbol);
            Product[] products = parser.Parse(importRequest.Items, out var validations);

            if (validations.Any())
                return validations.ToArray();

            var productList = new ProductList
            {
                Name = importRequest.Name,
                Products = products,
                Closed = false,
                Owner = await userService.GetCurrentUser(),
            };

            await applicationContext.ProductLists.AddAsync(productList);
            await applicationContext.SaveChangesAsync();

            return Array.Empty<string>();
        }

        public async Task DeleteList(int productsListId)
        {
            var list = await applicationContext.ProductLists
                .Include(x => x.Owner)
                .Include(x => x.Products)
                .Include(x => x.OrderItems)
                .FirstOrDefaultAsync(x => x.Id == productsListId);
            
            if (list == null)
                //TODO add error
                return;

            var currentUser = await userService.GetCurrentUser();
            if (list.Owner != currentUser)
                //TODO add error
                return;

            applicationContext.ProductLists.Remove(list);
            await applicationContext.SaveChangesAsync();
        }

        public async Task SetUserOrder(ApplicationUser user, int productListId, Dictionary<int, int> orderData)
        {
            int[] productIds = orderData.Keys.ToArray();
            var existItems = await applicationContext.UserOrderItems.Where(i => productIds.Contains(i.ProductId) && i.Owner == user).ToArrayAsync();
            var missedItems = productIds.Except(existItems.Select(x => x.ProductId)).Select(id => new UserOrderItem{Owner = user, ProductId = id, ProductListId = productListId}).ToArray();
            await applicationContext.UserOrderItems.AddRangeAsync(missedItems);
            var allItems = existItems.Concat(missedItems);

            foreach (UserOrderItem item in allItems)
            {
                item.Amount = orderData[item.ProductId];
            }

            await applicationContext.SaveChangesAsync();
        }

        //TODO replace dictionary by class with better property names
        public async Task<Dictionary<int, int>> GetUserOrder(ApplicationUser user, int productListId)
        {
            Dictionary<int, int> orderData = await applicationContext.UserOrderItems.Where(i => i.ProductListId == productListId && i.Owner == user).ToDictionaryAsync(x => x.ProductId, x => x.Amount);
            return orderData;
        }
    }
}