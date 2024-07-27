using System.Text.RegularExpressions;

namespace CalculatorApp.Logic
{
    public static class CalculatorLogic
    {
        public static string EvaluateExpression(string expression)
        {
            //Remove all whitespace
            expression = Regex.Replace(expression, @"\s+", "");

            if (!CheckParenthesesMatch(expression))
                throw new Exception("Parenthesis are not matching");
            if (!CheckCharacters(expression))
                throw new Exception("Invalid input");

            var answer = RecursiveEval(expression);

            return answer;
        }

        private static string RebuildString(string expression, string evaluatedValue, int startIndex, int endIndex) =>
            expression.Substring(0, startIndex) + evaluatedValue + expression.Substring(endIndex);

        private static string EvaluateParentheses(string expression)
        {
            var opening = expression.IndexOf("(");
            var closing = GetClosingParenthesisIndex(expression);
            var expressionToEvaluate = expression.Substring(opening + 1, closing - opening - 1);
            var evaluatedString = RecursiveEval(expressionToEvaluate);
            return RebuildString(expression, evaluatedString, opening, closing + 1);
        }

        private static string RecursiveEval(string expression)
        {
            while (expression.Contains('('))
            {
                expression = EvaluateParentheses(expression);
            }

            List<char> highOrderOperators = new List<char> { '*', 'x', '/' };
            List<char> lowOrderOperators = new List<char> { '+', '-' };

            while (expression.Any(c => highOrderOperators.Contains(c)))
            {
                for (int i = 0; i < expression.Length; i++)
                {
                    char c = expression[i];
                    if (c == '*' || c == 'x' || c == '/')
                    {
                        expression = Evaluate(expression, i, c);
                    }
                }
            }
            //This Regex is used to ensure the expression is not just a negative number
            var negativeNumbers = "^-?\\d+(\\.\\d+)?$";
            while (expression.Any(c => lowOrderOperators.Contains(c)) && !Regex.IsMatch(expression, negativeNumbers))
            {
                for (int i = 0; i < expression.Length; i++)
                {
                    char c = expression[i];
                    if (c == '+' || c == '-')
                        expression = Evaluate(expression, i, c);
                }
            }
            return expression;
        }

        private static string Evaluate(string expression, int index, char func)
        {
            var firstSubstring = expression.Substring(0, index);
            var secondSubstring = expression.Substring(index + 1);
            var firstNumber = FindNumber(firstSubstring, true);
            var secondNumber = FindNumber(secondSubstring, false);
            double result = 0;
            switch (func)
            {
                case 'x':
                case '*':
                    result = firstNumber * secondNumber;
                    break;
                case '/':
                    if (secondNumber == 0)
                        throw new Exception("Divide by zero error");
                    result = firstNumber / secondNumber;
                    break;
                case '+':
                    result = firstNumber + secondNumber;
                    break;
                case '-':
                    result = firstNumber - secondNumber;
                    break;
            }

            return RebuildString(expression, result.ToString(), index - firstNumber.ToString().Length, index + secondNumber.ToString().Length + 1);

        }

        private static bool CheckParenthesesMatch(string expression)
        {
            try
            {
                Stack<char> parentheses = new Stack<char>();
                foreach (char c in expression)
                {
                    if (c == '(')
                    {
                        parentheses.Push(c);
                    }
                    if (c == ')')
                    {
                        parentheses.Pop();
                    }
                }
                return parentheses.Count == 0;
            }
            catch
            {
                throw new Exception("Parenthesis are not matching");
            }

        }

        private static bool CheckCharacters(string expression)
        {
            Regex r = new Regex("^[()+\\-*/x0123456789]+$");
            return r.IsMatch(expression);
        }

        private static int GetClosingParenthesisIndex(string expression)
        {
            Stack<char> parentheses = new Stack<char>();
            for (int i = 0; i < expression.Length; i++)
            {
                char c = expression[i];
                if (c == '(')
                {
                    parentheses.Push(c);
                }
                if (c == ')')
                {
                    parentheses.Pop();
                    if (parentheses.Count == 0)
                        return i;
                }
            }
            return 0;
        }

        private static double FindNumber(string expression, bool beforeOperator)
        {
            try
            {
                var pattern = @"\d|\.";
                if (beforeOperator)
                {
                    //Go backwards until we reach a character that isn't part of the number.
                    for (int i = expression.Length - 1; i > -1; i--)
                    {
                        char c = expression[i];
                        if (!Regex.IsMatch(c.ToString(), pattern))
                            return double.Parse(expression.Substring(i));
                    }
                }
                else
                {
                    for (int i = 0; i < expression.Length; i++)
                    {
                        char c = expression[i];
                        if (!Regex.IsMatch(c.ToString(), pattern))
                            return double.Parse(expression.Substring(0, i));
                    }
                }
                return double.Parse(expression);
            }
            catch {
                throw new Exception ("Invalid input");
            }

        }
    }
}
