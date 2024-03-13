using System;
using System.Text.RegularExpressions;
using Jace;

namespace CalculatorApp
{
    public class Calculator
    {
        private string expression;
        private double lastResult;

        public Calculator()
        {
            expression = "";
            lastResult = 0;
        }

        public void AppendToExpression(string value)
        {
            expression += value;
        }

        public void ClearExpression()
        {
            expression = "";
            lastResult = 0;
        }

        public void DeleteCharacter()
        {
            if (!string.IsNullOrEmpty(expression))
            {
                expression = expression.Substring(0, expression.Length - 1);
            }
        }

        public void EvaluateExpression()
        {
            try
            {
                var engine = new CalculationEngine();
                engine.AddConstant("pi", Math.PI);
                engine.AddConstant("e", Math.E);
                lastResult = (double)engine.Calculate(expression);
                expression = lastResult.ToString();
            }
            catch (Exception ex)
            {
                expression = "Error: " + ex.Message;
                lastResult = 0;
            }
        }

        public string GetExpression()
        {
            return expression;
        }

        public string GetLastResult()
        {
            return lastResult.ToString();
        }
    }
}