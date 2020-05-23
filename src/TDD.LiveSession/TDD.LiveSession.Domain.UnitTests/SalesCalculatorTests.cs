using System;
using System.Reflection.Emit;
using NUnit.Framework;

namespace TDD.LiveSession.Domain.UnitTests
{
    [TestFixture]
    public class SalesCalculatorTests
    {
        [Test]
        public void SalesCalculator_Calculate_Test()
        {
            MoneyValue salesValue = MoneyValue.Of(1000);
            Points points = SalesCalculator.Calculate(salesValue);

            Assert.That(points, Is.EqualTo(Points.Of(10)));
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

        public static decimal operator /(MoneyValue moneyValue, int number)
        {
            return moneyValue._value / number;
        }
    }

    public class SalesCalculator
    {
        public static Points Calculate(MoneyValue salesValue)
        {
            return Points.Of(salesValue / 100);
        }
    }
}