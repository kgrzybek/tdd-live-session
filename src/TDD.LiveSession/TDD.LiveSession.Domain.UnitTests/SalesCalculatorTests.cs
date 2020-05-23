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
            var points = SalesCalculator.Calculate(salesValue);

            Assert.That(points, Is.EqualTo(10));
        }
    }

    public struct MoneyValue
    {
        private readonly decimal _value;

        public MoneyValue(decimal value)
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
        public static decimal Calculate(MoneyValue salesValue)
        {
            return salesValue / 100;
        }
    }
}