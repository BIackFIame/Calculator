using System;
using System.Text.RegularExpressions;
using System.Data;
using System.Linq;

namespace CalculatorApp
{
    public class ExpressionManager
    {
        private string expressionString;
        private string originalExpressionString;

        public ExpressionManager()
        {
            expressionString = string.Empty;
            originalExpressionString = string.Empty;
        }

        public void AppendToExpression(string value)
        {
            expressionString += value;
            originalExpressionString += value;
        }

        public void AppendFunction(string functionName)
        {
            expressionString += functionName;
            originalExpressionString += functionName;
        }

        public string? Evaluate()
        {
            try
            {
                string processedExpression = originalExpressionString;

                processedExpression = Regex.Replace(processedExpression, @"sqrt\((-?\d+(\.\d+)?)\)", match =>
                {
                    string argument = match.Groups[1].Value;
                    return $"({EvaluateSquareRoot(argument) ?? string.Empty})";
                });

                processedExpression = Regex.Replace(processedExpression, @"(\d+(\.\d+)?)\^(\d+(\.\d+)?)", match =>
                {
                    string baseValue = match.Groups[1].Value;
                    string exponent = match.Groups[3].Value;
                    return $"({EvaluatePower(baseValue, exponent) ?? string.Empty})";
                });

                processedExpression = Regex.Replace(processedExpression, @"(\d+)!", match =>
                {
                    string argument = match.Groups[1].Value;
                    return $"({EvaluateFactorial(argument) ?? string.Empty})";
                });

                processedExpression = Regex.Replace(processedExpression, @"(\d+(\.\d+)?)\^\((\d+(\.\d+)?)\)", match =>
                {
                    string baseValue = match.Groups[1].Value;
                    string exponent = match.Groups[3].Value;
                    return $"({EvaluatePower(baseValue, exponent) ?? string.Empty})";
                });

                var result = new DataTable().Compute(processedExpression, null);
                expressionString = result?.ToString() ?? string.Empty;
                return expressionString;
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
            return originalExpressionString;
        }

        public void ClearExpression()
        {
            expressionString = string.Empty;
            originalExpressionString = string.Empty;
        }

        public void RemoveLastCharacter()
        {
            if (originalExpressionString.Length > 0)
            {
                originalExpressionString = originalExpressionString.Substring(0, originalExpressionString.Length - 1);
                expressionString = originalExpressionString;
                Evaluate();
            }
        }
    }
}