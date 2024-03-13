using System;
using System.Text.RegularExpressions;
using System.Data;
using System.Linq;

namespace CalculatorApp
{
    public class ExpressionManager
    {
        private string expressionString;

        public ExpressionManager()
        {
            expressionString = string.Empty;
        }

        public void AppendToExpression(string value)
        {
            expressionString += value;
        }

        public void AppendFunction(string functionName)
        {
            expressionString += functionName;
        }

        public string? Evaluate()
        {
            try
            {
                expressionString = Regex.Replace(expressionString, @"sqrt\((-?\d+(\.\d+)?)\)", match =>
                {
                    string argument = match.Groups[1].Value;
                    return $"({EvaluateSquareRoot(argument) ?? string.Empty})";
                });

                expressionString = Regex.Replace(expressionString, @"(\d+(\.\d+)?)\^(\d+(\.\d+)?)", match =>
                {
                    string baseValue = match.Groups[1].Value;
                    string exponent = match.Groups[3].Value;
                    return $"({EvaluatePower(baseValue, exponent) ?? string.Empty})";
                });

                expressionString = Regex.Replace(expressionString, @"(\d+)!", match =>
                {
                    string argument = match.Groups[1].Value;
                    return $"({EvaluateFactorial(argument) ?? string.Empty})";
                });

                expressionString = Regex.Replace(expressionString, @"(\d+(\.\d+)?)\^\((\d+(\.\d+)?)\)", match =>
                {
                    string baseValue = match.Groups[1].Value;
                    string exponent = match.Groups[3].Value;
                    return $"({EvaluatePower(baseValue, exponent) ?? string.Empty})";
                });

                var result = new DataTable().Compute(expressionString, null);
                return result?.ToString();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public string? EvaluateSquareRoot(string argument)
        {
            double value = double.Parse(argument);
            double result = Math.Sqrt(value);
            return result.ToString();
        }

        public string? EvaluatePower(string baseValue, string exponent)
        {
            double value = double.Parse(baseValue);
            double power = double.Parse(exponent);
            double result = Math.Pow(value, power);
            return result.ToString();
        }

        public string? EvaluateFactorial(string argument)
        {
            int value = int.Parse(argument);
            int result = Enumerable.Range(1, value).Aggregate(1, (p, item) => p * item);
            return result.ToString();
        }

        public string GetCurrentExpression()
        {
            return expressionString;
        }

        public void ClearExpression()
        {
            expressionString = string.Empty;
        }

        public void RemoveLastCharacter()
        {
            if (expressionString.Length > 0)
            {
                expressionString = expressionString.Substring(0, expressionString.Length - 1);
            }
        }
    }
}