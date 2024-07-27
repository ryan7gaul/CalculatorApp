using CalculatorApp.Logic;

namespace CalculatorTest
{
    public class Tests
    {

        [TestCase("5+4", "9")]
        [TestCase("(5*2)/4", "2.5")]
        [TestCase("(5+3)/2", "4")]
        [TestCase("(38-8)x4", "120")]
        [TestCase("((83+34)/(3+3))*5", "97.5")]
        [TestCase("3-5*6", "-27")]
        [TestCase("4-6+5*3", "13")]
        [TestCase("3+2+2", "7")]
        [TestCase("36-5+4+8", "43")]
        [TestCase("5*2*3*2", "60")]
        [TestCase("36-5+4+8/2", "39")]
        [Test]
        public void Test1(string input, string expected)
        {

            
            var result = CalculatorLogic.EvaluateExpression(input);
            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase("2 / 1))", "Parenthesis are not matching")]
        [TestCase("((( 2 + 1 ) / 1)", "Parenthesis are not matching")]
        [TestCase(")2 + 1 ) / 1)", "Parenthesis are not matching")]
        [TestCase("3++2+4", "Invalid input")]
        [TestCase("343+-2+4", "Invalid input")]
        [TestCase("30+6-7*/8", "Invalid input")]
        [TestCase("123+45a", "Invalid input")]
        [TestCase("4/0", "Divide by zero error")]
        [Test]
        public void Test2(string input, string expected)
        {
            Assert.Throws<Exception>(() => CalculatorLogic.EvaluateExpression(input), expected);
        }
    }
}