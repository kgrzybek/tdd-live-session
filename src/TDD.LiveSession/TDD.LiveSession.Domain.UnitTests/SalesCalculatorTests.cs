using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection.Emit;
using Microsoft.VisualBasic.CompilerServices;
using NUnit.Framework;

namespace TDD.LiveSession.Domain.UnitTests
{
    [TestFixture]
    public class SalesCalculatorTests
    {
        [TestCaseSource(typeof(TestCasesSet), nameof(TestCasesSet.TestCases))]
        public void SalesCalculator_Calculate_Test(
            SalesData salesData, 
            Points expectedPoints)
        {
            var priceList = CreatePriceList();

            Points points = SalesCalculator.Calculate(salesData, priceList);

            Assert.That(points, Is.EqualTo(expectedPoints));
        }

        private static PriceList CreatePriceList()
        {
            List<PriceListItem> priceListItems = new List<PriceListItem>();
            priceListItems.Add(new PriceListItem("Standard", MoneyValue.Of(100)));
            priceListItems.Add(new PriceListItem("Premium", MoneyValue.Of(50)));
            PriceList priceList = new PriceList(priceListItems);

            return priceList;
        }
    }

    public class TestCasesSet
    {
        public static IEnumerable<TestCaseData> TestCases { get; }

        static TestCasesSet()
        {
            List<TestData> testDataList = new List<TestData>();

            MoneyValue salesValue = MoneyValue.Of(1000);
            SalesData salesData = new SalesData(salesValue, "Standard");
            testDataList.Add(new TestData(salesData, Points.Of(10)));

            MoneyValue salesValuePremium = MoneyValue.Of(1000);
            SalesData salesDataPremium = new SalesData(salesValuePremium, "Premium");
            testDataList.Add(new TestData(salesDataPremium, Points.Of(20)));

            MoneyValue salesValueProductWithoutPrice = MoneyValue.Of(1000);
            SalesData salesDataProductWithoutPrice = 
                new SalesData(salesValueProductWithoutPrice, "Undefined");
            testDataList.Add(new TestData(salesDataProductWithoutPrice, Points.Of(0)));

            TestCases = testDataList.Select(x => new TestCaseData(x.SalesData, x.ExpectedPoints));
        }

        private class TestData
        {
            public TestData(SalesData salesData, Points expectedPoints)
            {
                SalesData = salesData;
                ExpectedPoints = expectedPoints;
            }

            public SalesData SalesData { get; }

            public Points ExpectedPoints { get; }
        }
    }

    public struct PriceList
    {
        private readonly List<PriceListItem> _priceListItems;

        public PriceList(List<PriceListItem> priceListItems)
        {
            _priceListItems = priceListItems;
        }

        public MoneyValue GetValueForPointForProductCategory(string productCategory)
        {
            return _priceListItems
                .SingleOrDefault(x => x.ProductCategory == productCategory).ValueForPoint;
        }
    }

    public struct PriceListItem
    {
        public string ProductCategory { get; }
        public MoneyValue ValueForPoint { get; }

        public PriceListItem(string productCategory, MoneyValue valueForPoint)
        {
            ProductCategory = productCategory;
            ValueForPoint = valueForPoint;
        }
    }

    public struct SalesData
    {
        public MoneyValue Value { get; }

        public string ProductCategory { get; }

        public SalesData(MoneyValue value, string productCategory)
        {
            this.Value = value;
            this.ProductCategory = productCategory;
        }

        public override string ToString()
        {
            return $"Value:{this.Value}, ProductCategory:{this.ProductCategory}";
        }
    }

    public struct Points
    {
        private decimal _value;

        private Points(decimal value)
        {
            _value = value;
        }

        public static Points Of(decimal value)
        {
            return new Points(value);
        }

        public override string ToString()
        {
            return _value.ToString(CultureInfo.InvariantCulture);
        }
    }

    public struct MoneyValue
    {
        private readonly decimal _value;

        private MoneyValue(decimal value)
        {
            _value = value;
        }

        public static MoneyValue Of(decimal value)
        {
            return new MoneyValue(value);
        }

        public static decimal operator /(MoneyValue moneyValueLeft, MoneyValue moneyValueRight)
        {
            return moneyValueLeft._value / moneyValueRight._value;
        }

        public static bool operator ==(MoneyValue left, MoneyValue right)
        {
            return left._value == right._value;
        }

        public static bool operator !=(MoneyValue left, MoneyValue right)
        {
            return !(left == right);
        }

        public bool Equals(MoneyValue other)
        {
            return _value == other._value;
        }

        public override bool Equals(object obj)
        {
            return obj is MoneyValue other && Equals(other);
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        public override string ToString()
        {
            return _value.ToString(CultureInfo.InvariantCulture);
        }
    }

    public class SalesCalculator
    {
        public static Points Calculate(SalesData salesData, PriceList priceList)
        {
            MoneyValue moneyForOnePoint = priceList.GetValueForPointForProductCategory(salesData.ProductCategory);

            if (moneyForOnePoint != MoneyValue.Of(0))
            {
                return Points.Of(salesData.Value / moneyForOnePoint);
            }

            return Points.Of(0);
        }
    }
}