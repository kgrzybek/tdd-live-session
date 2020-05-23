﻿using System;
using System.Globalization;
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
            
            Points points = SalesCalculator.Calculate(salesData);

            Assert.That(points, Is.EqualTo(Points.Of(10)));
        }

        [Test]
        public void SalesCalculator_CalculatePremiumProduct_Test()
        {
            MoneyValue salesValue = MoneyValue.Of(1000);
            SalesData salesData = new SalesData(salesValue, "Premium");

            Points points = SalesCalculator.Calculate(salesData);

            Assert.That(points, Is.EqualTo(Points.Of(20)));
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
        public static Points Calculate(SalesData salesData)
        {
            MoneyValue moneyForOnePoint;
            if (salesData.ProductCategory == "Standard")
            {
                moneyForOnePoint = MoneyValue.Of(100);
            }
            else
            {
                moneyForOnePoint = MoneyValue.Of(50);
            }

            return Points.Of(salesData.Value / moneyForOnePoint);
        }
    }
}