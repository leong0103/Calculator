using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SelfEvaluatingExpressionEvaluator
{
    interface IOperator
    {
        double Evaluate(double left, double right);
    }

    class AdditionOperator : IOperator
    {
        public double Evaluate(double left, double right)
        {
            return left + right;
        }
    }

    class SubtractionOperator : IOperator
    {
        public double Evaluate(double left, double right)
        {
            return left - right;
        }
    }

    class MultiplicationOperator : IOperator
    {
        public double Evaluate(double left, double right)
        {
            return left * right;
        }
    }

    class DivisionOperator : IOperator
    {
        public double Evaluate(double left, double right)
        {
            return left / right;
        }
    }

    class PowerOperator : IOperator
    {
        public double Evaluate(double left, double right)
        {
            return Math.Pow(left, right);
        }
    }

    interface IExpressionEvaluator
    {
        double Evaluate(string expression);
    }

    class SelfEvaluatingExpressionEvaluator : IExpressionEvaluator
    {
        private Dictionary<string, IOperator> _operators;

        public SelfEvaluatingExpressionEvaluator()
        {
            _operators = new Dictionary<string, IOperator>
            {
                { "+", new AdditionOperator() },
                { "-", new SubtractionOperator() },
                { "*", new MultiplicationOperator() },
                { "/", new DivisionOperator() },
                { "^", new PowerOperator() }
            };
        }

        private static readonly Regex _expressionRegex = new Regex(@"(\d+\.\d+|\d+|[\+\-\*\/\(\)\^])");


        private List<string> TokenizeExpression(string expression)
        {
            var tokens = new List<string>();
            foreach (Match match in _expressionRegex.Matches(expression))
            {
                tokens.Add(match.Value);
            }
            return tokens;
        }

        public double Evaluate(string expression)
        {
            Stack<double> stack = new Stack<double>();
            List<string> tokens = TokenizeExpression(expression);

            foreach (string token in tokens)
            {
                if (double.TryParse(token, out double number))
                {
                    stack.Push(number);
                }
                else
                {
                    double right = stack.Pop();
                    double left = stack.Pop();
                    IOperator op = _operators[token];
                    stack.Push(op.Evaluate(left, right));
                }
            }

            return stack.Pop();
        }
    }
}