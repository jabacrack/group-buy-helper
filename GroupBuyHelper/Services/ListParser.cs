using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using GroupBuyHelper.Data;
using Array = System.Array;

namespace GroupBuyHelper.Services
{
    public class ListParser
    {
        private const string WindowsStringEnd = "\n";

        private readonly string columnSeparator;
        private NumberFormatInfo numberFormat;
        private NumberStyles numberStyles = NumberStyles.AllowCurrencySymbol |
                                            NumberStyles.AllowDecimalPoint |
                                            NumberStyles.AllowLeadingWhite |
                                            NumberStyles.AllowTrailingWhite;

        public ListParser([NotNull]string columnSeparator, [NotNull]string numberSeparator, string currencySymbol)
        {
            if (columnSeparator == "\\t")
                columnSeparator = "\t";

            this.columnSeparator = columnSeparator;

            numberFormat = (NumberFormatInfo) CultureInfo.InvariantCulture.NumberFormat.Clone();
            numberFormat.NumberDecimalSeparator = numberSeparator;
            numberFormat.CurrencyDecimalSeparator = numberSeparator;
            numberFormat.CurrencySymbol = currencySymbol ?? string.Empty;
        }

        public Product[] Parse(string data, out IList<string> validations)
        {
            validations = new List<string>();

            string[][] values = data
                .Trim().Split(WindowsStringEnd).Select(line =>
                    line.Trim().Split(columnSeparator).Select(value => value.Trim()).ToArray()).ToArray();

            ValidateStructure(values, validations);

            if (validations.Count > 0)
                return Array.Empty<Product>();

            var products = new List<Product>();

            for (int rowIndex = 0; rowIndex < values.Length; rowIndex++)
            {
                products.Add(CreateProduct(rowIndex + 1, values[rowIndex], validations));
            }

            return products.ToArray();
        }

        private Product CreateProduct(int row, string[] values, IList<string> validations)
        {
            if (values is null)
                return null;

            return values.Length switch
            {
                0 => null,
                1 => new Product {Name = values[0]},
                2 => new Product {Name = values[0], Price = ParsePriceValue(row, values[1], validations)},
                _ => new Product
                {
                    Name = values[0], Amount = ParseAmountValue(row, values[1], validations),
                    Price = ParsePriceValue(row, values[2], validations)
                },
            };
        }

        private void ValidateStructure(string[][] values, IList<string> validations)
        {
            var groups = values.Select((line, i) => new {Row = i + 1, Values = line})
                .GroupBy(line => line.Values.Length).ToArray();

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

        private double ParsePriceValue(int row, string str, IList<string> validations)
        {
            if (double.TryParse(str, numberStyles, numberFormat, out double result))
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