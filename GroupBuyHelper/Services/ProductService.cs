using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using GroupBuyHelper.Data;
using GroupBuyHelper.Dtos;
using Microsoft.EntityFrameworkCore;

namespace GroupBuyHelper.Services
{
    public class ProductService
    {
        private readonly ApplicationContext applicationContext;
        private const string WindowsStringEnd = "\n";

        public ProductService(ApplicationContext applicationContext)
        {
            this.applicationContext = applicationContext;
        }

        public async Task<ProductListInfo[]> GetAllProductLists()
        {
            return await applicationContext.ProductLists.Select(p => new ProductListInfo(p.Id, p.Name)).ToArrayAsync();
        }

        public async Task<ProductInfo[]> GetProducts(int productsListId)
        {
            var list = await applicationContext.ProductLists.Include(l => l.Products).FirstAsync(x => x.Id == productsListId);
            return list.Products.Select(x => new ProductInfo(x.Id, x.Name, x.Price, x.Amount)).ToArray();
        }

        public async Task<Dictionary<int, int>> GetProductTotal(int productsListId)
        {
            UserOrderItem[] items = await applicationContext.UserOrderItems.Where(o => o.ProductListId == productsListId).ToArrayAsync();
            var summed = items.GroupBy(x => x.ProductId)
                .Select(group => new {Id = group.Key, Sum = group.Sum(x => x.Amount)})
                .ToDictionary(x => x.Id, x => x.Sum);

            return summed;
        }

        public async Task<string[]> AddList(ApplicationUser user, ImportRequest importRequest)
        {
            Product[] products = Parse(importRequest.Items, importRequest.ColumnSeparator, importRequest.NumberSeparator, importRequest.CurrencySymbol, out var validations);

            if (validations.Any())
                return validations.ToArray();

            var productList = new ProductList
            {
                Name = importRequest.Name,
                Products = products,
                Closed = false,
                Owner = user,
            };

            await applicationContext.ProductLists.AddAsync(productList);
            await applicationContext.SaveChangesAsync();

            return Array.Empty<string>();
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

        private Product[] Parse(string data, string columnsSeparator, string numberSeparator, string currencySymbol, out IList<string> validations)
        {
            validations = new List<string>();

            var numberFormat = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
            numberFormat.NumberDecimalSeparator = numberSeparator;
            numberFormat.CurrencyDecimalSeparator = numberSeparator;
            numberFormat.CurrencySymbol = currencySymbol;
            // numberFormat.

            string[][] values = data
                .Trim().Split(WindowsStringEnd).Select(line => line.Trim().Split(columnsSeparator).Select(value => value.Trim()).ToArray()).ToArray();

            ValidateStructure(values, validations);

            if (validations.Count > 0)
                return Array.Empty<Product>();

            var products = new List<Product>();

            for (int rowIndex = 0; rowIndex < values.Length; rowIndex++)
            {
                products.Add(CreateProduct(rowIndex + 1, values[rowIndex], numberFormat, validations));
            }

            return products.ToArray();
        }

        private Product CreateProduct(int row, string[] values, NumberFormatInfo numberFormat, IList<string> validations)
        {
            if (values is null)
                return null;

            return values.Length switch
            {
                0 => null,
                1 => new Product {Name = values[0]},
                2 => new Product {Name = values[0], Price = ParsePriceValue(row, values[1], numberFormat, validations) },
                _ => new Product {Name = values[0], Amount = ParseAmountValue(row, values[1], validations), Price = ParsePriceValue(row, values[1], numberFormat, validations) },
            };
        }

        private void ValidateStructure(string[][] values, IList<string> validations)
        {
            var groups = values.Select((line, i) => new {Row = i + 1, Values = line}).GroupBy(line => line.Values.Length).ToArray();

            if (groups.Length > 1)
            {
                int biggestGroup = groups.Max(group => group.Key);
                var invalidGroups = groups.Where(group => group.Key != biggestGroup);

                foreach (var group in invalidGroups)
                {
                    foreach (var groupValues in group)
                    {
                        validations.Add(group.Key == 0
                            ? $"Empty row {groupValues.Row}."
                            : $"Most of lines have {biggestGroup} columns. But row {groupValues.Row} contains {group.Key}.");
                    }
                }
            }

            var unsupportedColumnNumber = groups.Where(group => group.Key > 3).ToArray();

            if (unsupportedColumnNumber.Any())
            {
                foreach (var group in unsupportedColumnNumber)
                {
                    foreach (var groupValues in group)
                    {
                        validations.Add($"Line {groupValues.Row} contains more than 3 columns.");
                    }
                }
            }
        }

        private double ParsePriceValue(int row, string str, NumberFormatInfo numberFormat, IList<string> validations)
        {
            if (double.TryParse(str, NumberStyles.Any, numberFormat, out double result))
                return result;

            validations.Add($"Value {str} cannot be parsed as price in row {row}.");
            return default;
        }

        private int ParseAmountValue(int row, string str, IList<string> validations)
        {
            if (int.TryParse(str, out int result))
                return result;

            validations.Add($"Value {str} cannot be parsed as amount in row {row}.");
            return default;
        }
    }
}