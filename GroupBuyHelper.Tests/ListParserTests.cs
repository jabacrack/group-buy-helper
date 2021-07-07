using System;
using System.Collections.Generic;
using GroupBuyHelper.Data;
using GroupBuyHelper.Services;
using Xunit;

namespace GroupBuyHelper.Tests
{
    public class ListParserTests
    {
        private readonly Product[] defaultProducts = 
        {
            new() {Name = "Name 1", Amount = 20, Price = 2.5},
            new() {Name = "Name 2", Amount = 35, Price = 1.02}
        };

        private readonly IEqualityComparer<Product> comparer = new ProductComparer();

        class ProductComparer : IEqualityComparer<Product>
        {
            public bool Equals(Product x, Product y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return x.Name == y.Name && Nullable.Equals(x.Price, y.Price) && x.Amount == y.Amount;
            }

            public int GetHashCode(Product obj)
            {
                return HashCode.Combine(obj.Name, obj.Price, obj.Amount);
            }
        }
        

        [Fact]
        public void OneColumn()
        {
            var text = GetString(
                "Name 1",
                "Name 2"
            );

            var parser = new ListParser("\t", ",", "");
            Product[] products = parser.Parse(text, out var validations);

            Assert.Equal(2, products.Length);
            Assert.Empty(validations);
            Assert.Equal("Name 1", products[0].Name);
            Assert.Null(products[0].Amount);
            Assert.Null(products[0].Price);
            Assert.Equal("Name 2", products[1].Name);
            Assert.Null(products[1].Amount);
            Assert.Null(products[1].Price);
        }

        [Fact]
        public void TwoColumns()
        {
            var text = GetString(
                "Name 1;2,01",
                "Name 2;3,5"
            );

            var parser = new ListParser(";", ",", "");
            Product[] products = parser.Parse(text, out var validations);

            Assert.Equal(2, products.Length);
            Assert.Empty(validations);
            Assert.Equal("Name 1", products[0].Name);
            Assert.Equal(2.01, products[0].Price);
            Assert.Null(products[0].Amount);
            Assert.Equal("Name 2", products[1].Name);
            Assert.Equal(3.5, products[1].Price);
            Assert.Null(products[1].Amount);
        }

        [Fact]
        public void ThreeColumns()
        {
            var text = GetString(
                "Name 1;20;2,5",
                "Name 2;35;1,02"
            );

            var parser = new ListParser(";", ",", "");
            Product[] products = parser.Parse(text, out var validations);

            Assert.Equal(2, products.Length);
            Assert.Empty(validations);
            Assert.Equal(defaultProducts, products, comparer);
        }

        [Fact]
        public void TooManyColumns()
        {
            var text = GetString(
                "Name 1;20;2,5;3,2",
                "Name 2;35;1,02;1,2"
            );

            var parser = new ListParser(";", ",", "");
            parser.Parse(text, out var validations);

            Assert.Equal(2, validations.Count);
        }

        [Theory]
        [InlineData("\t", "\t")]
        [InlineData("\\t", "\t")]
        [InlineData(";", ";")]
        [InlineData("|", "|")]
        public void DifferentColumnSeparators(string separator, string realSeparator)
        {
            var text = GetString(
                $"Name 1{realSeparator}20{realSeparator}2,5",
                $"Name 2{realSeparator}35{realSeparator}1,02"
            );

            var parser = new ListParser(separator, ",", "");
            Product[] products = parser.Parse(text, out var validations);

            Assert.Equal(2, products.Length);
            Assert.Empty(validations);
            Assert.Equal(defaultProducts, products, comparer);
        }

        [Theory]
        [InlineData(",")]
        [InlineData(".")]
        public void DifferentNumberSeparators(string separator)
        {
            var text = GetString(
                $"Name 1;20;2{separator}5",
                $"Name 2;35;1{separator}02"
            );

            var parser = new ListParser(";", separator, "");
            Product[] products = parser.Parse(text, out var validations);

            Assert.Equal(2, products.Length);
            Assert.Empty(validations);
            Assert.Equal(defaultProducts, products, comparer);
        }

        [Fact]
        public void WrongNumberSeparators()
        {
            var text = GetString(
                "Name 1;20;2,5",
                "Name 2;35;1,02"
            );

            var parser = new ListParser(";", ".", "");
            parser.Parse(text, out var validations);

            Assert.Equal(2, validations.Count);
        }

        [Theory]
        [InlineData("2.5$", "1.02$")]
        [InlineData("$2.5", "$1.02")]
        [InlineData("$2.5", "1.02$")]
        [InlineData("$2.5", "1.02")]
        public void CurrencySymbol(string price1, string price2)
        {
            var text = GetString(
                $"Name 1;20;{price1}",
                $"Name 2;35;{price2}"
            );

            var parser = new ListParser(";", ".", "$    ");
            Product[] products = parser.Parse(text, out var validations);

            Assert.Equal(2, products.Length);
            Assert.Empty(validations);
            Assert.Equal(defaultProducts, products, comparer);
        }

        [Fact]
        public void WrongCurrencySymbol()
        {
            var text = GetString(
                "Name 1;20;2.5$",
                "Name 2;35;1.02$"
            );

            var parser = new ListParser(";", ".", "%");
            parser.Parse(text, out var validations);

            Assert.Equal(2, validations.Count);
        }

        [Fact]
        public void NoCurrencySymbol()
        {
            var text = GetString(
                "Name 1;20;2.5$",
                "Name 2;35;1.02$"
            );

            var parser = new ListParser(";", ".", "");
            parser.Parse(text, out var validations);

            Assert.NotEmpty(validations);
            Assert.Equal(2, validations.Count);
        }

        [Fact]
        public void ValuesWithSpaces()
        {
            var text = GetString(
                "   Name 1   ;  20  ;  2,5  ",
                "  Name 2  ;  35  ;   1,02  "
            );

            var parser = new ListParser(";", ",", "");
            Product[] products = parser.Parse(text, out var validations);

            Assert.Equal(2, products.Length);
            Assert.Empty(validations);
            Assert.Equal(defaultProducts, products, comparer);
        }



        private string GetString(params string[] strings)
        {
            return string.Join("\n", strings);
        }
    }
}
