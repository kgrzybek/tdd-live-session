using NUnit.Framework;

namespace TDD.LiveSession.Domain.UnitTests
{
    [TestFixture]
    public class SalesCalculatorTests
    {
        [Test]
        public void SalesCalculator_Calculate_Test()
        {
            decimal salesValue = 1000;
            var points = SalesCalculator.Calculate(salesValue);

            Assert.That(points, Is.EqualTo(10));
        }
    }

    public class SalesCalculator
    {
        public static decimal Calculate(decimal salesValue)
        {
            return salesValue / 100;
        }
    }
}