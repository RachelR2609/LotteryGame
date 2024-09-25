using LotteryGame;

namespace Tests
{
    [TestFixture]
    public class LotteryGameTests
    {
        private Program _program;

        [SetUp]
        public void Setup()
        {
            _program = new Program();
        }

        [Test]
        public void CanSumTwoIntegers()
        {
            if (_program.Sum(2, 2) == 4) Assert.Pass();
            else Assert.Fail();
        }
    }
}