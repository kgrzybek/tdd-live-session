using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection.Emit;
using NUnit.Framework;

namespace TDD.LiveSession.Domain.UnitTests
{
    [TestFixture]
    public class SalesCalculatorTests
    {
        [Test]
        public void SalesCalculator_CalculateStandardProduct_Test()
        {
            MoneyValue salesValue = MoneyValue.Of(1000);
            SalesData salesData = new SalesData(salesValue, "Standard");
            var priceList = CreatePriceList();

            Points points = SalesCalculator.Calculate(salesData, priceList);

            Assert.That(points, Is.EqualTo(Points.Of(10)));
        }

        [Test]
        public void SalesCalculator_CalculatePremiumProduct_Test()
        {
            MoneyValue salesValue = MoneyValue.Of(1000);
            SalesData salesData = new SalesData(salesValue, "Premium");
            var priceList = CreatePriceList();

            Points points = SalesCalculator.Calculate(salesData, priceList);

            Assert.That(points, Is.EqualTo(Points.Of(20)));
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

    public struct PriceList
    {
        public List<PriceListItem> PriceListItems { get; }

        public PriceList(List<PriceListItem> priceListItems)
        {
            PriceListItems = priceListItems;
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
    }

    public class SalesCalculator
    {
        public static Points Calculate(SalesData salesData, PriceList priceList)
        {
            MoneyValue moneyForOnePoint = priceList.PriceListItems
                .SingleOrDefault(x => x.ProductCategory == salesData.ProductCategory).ValueForPoint;

            return Points.Of(salesData.Value / moneyForOnePoint);
        }
    }
}