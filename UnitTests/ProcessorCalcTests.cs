using NUnit.Framework;
using Processor;

namespace UnitTests
{
    [TestFixture]
    public class ProcessorCalcTests
    {
        [SetUp]
        public void SetUp()
        {

        }

        [Test]
        public void CaseCalculationDefaultConstructorTests()
        {
            CaseCalculation c = new();
            Assert.That(c.KeyPrice, Is.EqualTo(2.35d));
        }

        [Test]
        [TestCase(1.23d)]
        [TestCase(3.45d)]
        [TestCase(5.67d)]
        public void CaseCalculationParameterizedConstructorTests(double keyprice)
        {
            CaseCalculation c = new(keyprice);
            Assert.That(c.KeyPrice, Is.EqualTo(keyprice));
        }

        [Test]
        [TestCase([0.45d, 10.23d, 3])]
        [TestCase([0.89d, 20.83d, 6])]
        [TestCase([1.24d, 10.83d, 3])]
        public void CaseCalculationFromMoneyTests(double caseprice, double availableMoney, int result)
        {
            CaseCalculation c = new();
            Assert.That(c.CalculateFromAvailableMoney(caseprice, availableMoney), Is.EqualTo(result));
        }

        [Test]
        [TestCase([1.89, 0.45d, 10.23d, 4])]
        [TestCase([1.89, 0.89d, 20.83d, 7])]
        [TestCase([1.89, 1.24d, 10.83d, 3])]
        public void CaseCalculationFromMoneyDifferentKeyPriceTests(double keyprice, double caseprice, double availableMoney, int result)
        {
            CaseCalculation c = new(keyprice);
            Assert.That(c.CalculateFromAvailableMoney(caseprice, availableMoney), Is.EqualTo(result));

            c = new()
            {
                KeyPrice = keyprice
            };
            Assert.That(c.CalculateFromAvailableMoney(caseprice, availableMoney), Is.EqualTo(result));
        }

        [Test]
        [TestCase([0.45d, 3, 8.4d])]
        [TestCase([0.89d, 6, 19.44d])]
        [TestCase([1.24d, 3, 10.77d])]
        public void CaseCalculationFromCasecountTests(double caseprice, int casecount, double result)
        {
            CaseCalculation c = new();
            Assert.That(c.CalculateFromCaseCount(caseprice, casecount), Is.EqualTo(result));
        }

        [Test]
        [TestCase([1.89, 0.45d, 3, 7.02d])]
        [TestCase([1.89, 0.89d, 6, 16.68d])]
        [TestCase([1.89, 1.24d, 3, 9.39d])]
        public void CaseCalculationFromCasecountKeyPriceTests(double keyprice, double caseprice, int casecount, double result)
        {
            CaseCalculation c = new(keyprice);
            Assert.That(c.CalculateFromCaseCount(caseprice, casecount), Is.EqualTo(result));

            c = new()
            {
                KeyPrice = keyprice
            };
            Assert.That(c.CalculateFromCaseCount(caseprice, casecount), Is.EqualTo(result));
        }

        [Test]
        public void FailTests()
        {
            CaseCalculation c = new();
            Assert.That(c.CalculateFromAvailableMoney(default, default), Is.EqualTo(0));
            Assert.That(c.CalculateFromCaseCount(default, default), Is.EqualTo(0));

            c = new(default);
            Assert.That(c.CalculateFromAvailableMoney(default, default), Is.EqualTo(0));
            Assert.That(c.CalculateFromCaseCount(default, default), Is.EqualTo(0));

            c = new(-0.1d);
            Assert.That(c.CalculateFromAvailableMoney(default, default), Is.EqualTo(0));
            Assert.That(c.CalculateFromCaseCount(default, default), Is.EqualTo(0));

            c = new(-154.1d);
            Assert.That(c.CalculateFromAvailableMoney(default, default), Is.EqualTo(0));
            Assert.That(c.CalculateFromCaseCount(default, default), Is.EqualTo(0));

            c = new(default);
            Assert.That(c.CalculateFromAvailableMoney(-1.54d, default), Is.EqualTo(0));
            Assert.That(c.CalculateFromCaseCount(-1.54d, default), Is.EqualTo(0));

            c = new(default);
            Assert.That(c.CalculateFromAvailableMoney(25d, -1.54d), Is.EqualTo(0));
            Assert.That(c.CalculateFromCaseCount(25d, -5), Is.EqualTo(0));
        }

        [TearDown]
        public void TearDown()
        {

        }
    }
}
