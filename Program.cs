using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CalculatorProgram
{
    class Program
    {
        static void Main(string[] args)
        {
            string expression = "2.1+3*4+(4+4)";
            ExpressionEvaluator evaluator = new ExpressionEvaluator();
            Console.WriteLine("Expression: " + expression);
            Console.WriteLine("Result: " + evaluator.EvaluateExpression(expression));
        }
    }

    class ExpressionEvaluator
    {
        public double EvaluateExpression(string expression)
        {
            // Convert the expression to Reverse Polish Notation (RPN)
            RpnConverter converter = new RpnConverter();
            string[] tokens = converter.ConvertToRpn(expression);

            // Evaluate the expression in RPN
            RpnEvaluator evaluator = new RpnEvaluator();
            return evaluator.Evaluate(tokens);
        }
    }

    class RpnConverter
    {
        private readonly Dictionary<string, int> _precedence = new Dictionary<string, int>
        {
            { "+", 1 },
            { "-", 1 },
            { "*", 2 },
            { "/", 2 },
            { "^", 3 }
        };

        private bool IsOperator(string token)
        {
            return _precedence.ContainsKey(token);
        }

        private bool IsRightAssociative(string token)
        {
            return token == "^";
        }

        private int ComparePrecedence(string token1, string token2)
        {
            int precedence1 = _precedence[token1];
            int precedence2 = _precedence[token2];
            if (precedence1 == precedence2)
            {
                if (IsRightAssociative(token1))
                {
                    return -1;
                }
                else
                {
                    return 1;
                }
            }
            else
            {
                return precedence1 > precedence2 ? 1 : -1;
            }
        }

        public string[] ConvertToRpn(string expression)
        {
            var pattern = @"(\d+(\.\d+)?)|(\+|\-|\*|\/|\^|\(|\))";
            var regex = new Regex(pattern);

            var matchs = Regex.Matches(expression, pattern);

            List<string> output = new List<string>();
            Stack<string> stack = new Stack<string>();
            foreach(Match token in matchs)
            {
                if (double.TryParse(token.Value, out double value))
                {
                    output.Add(token.Value);
                }
                else if (token.Value == "(")
                {
                    stack.Push(token.Value);
                }
                else if (token.Value == ")")
                {
                    while (stack.Peek() != "(")
                    {
                        output.Add(stack.Pop());
                    }
                    stack.Pop();
                }
                else if (IsOperator(token.Value))
                {
                    while (stack.Count > 0 && IsOperator(stack.Peek()) && ComparePrecedence(token.Value, stack.Peek()) <= 0)
                    {
                        output.Add(stack.Pop());
                    }
                    stack.Push(token.Value);
                }
            }
            while (stack.Count > 0)
            {
                output.Add(stack.Pop());
            }
            return output.ToArray();
        }
    }


public interface IOperator
{
    double Evaluate(double left, double right);
}

public class AddOperator : IOperator
{
    public double Evaluate(double left, double right)
    {
        return left + right;
    }
}

public class SubtractOperator : IOperator
{
    public double Evaluate(double left, double right)
    {
        return left - right;
    }
}

public class MultiplyOperator : IOperator
{
    public double Evaluate(double left, double right)
    {
        return left * right;
    }
}

public class DivideOperator : IOperator
{
    public double Evaluate(double left, double right)
    {
        return left / right;
    }
}

public class PowerOperator : IOperator
{
    public double Evaluate(double left, double right)
    {
        return Math.Pow(left, right);
    }
}

public class RpnEvaluator
{
    private readonly Dictionary<string, IOperator> _operators;

    public RpnEvaluator()
    {
        _operators = new Dictionary<string, IOperator>
        {
            { "+", new AddOperator() },
            { "-", new SubtractOperator() },
            { "*", new MultiplyOperator() },
            { "/", new DivideOperator() },
            { "^", new PowerOperator() }
        };
    }

    public double Evaluate(string[] tokens)
    {
        Stack<double> stack = new Stack<double>();
        foreach (string token in tokens)
        {
            if (double.TryParse(token, out double value))
            {
                stack.Push(value);
            }
            else if (_operators.ContainsKey(token))
            {
                double right = stack.Pop();
                double left = stack.Pop();
                stack.Push(_operators[token].Evaluate(left, right));
            }
        }
        return stack.Pop();
    }
}

//
    // class RpnEvaluator
    // {
    //     public double EvaluateRpn(string[] tokens)
    //     {
    //         Stack<double> stack = new Stack<double>();
    //         foreach (string token in tokens)
    //         {
    //             if (double.TryParse(token, out double value))
    //             {
    //                 stack.Push(value);
    //             }
    //             else if (token == "+")
    //             {
    //                 double right = stack.Pop();
    //                 double left = stack.Pop();
    //                 stack.Push(left + right);
    //             }
    //             else if (token == "-")
    //             {
    //                 double right = stack.Pop();
    //                 double left = stack.Pop();
    //                 stack.Push(left - right);
    //             }
    //             else if (token == "*")
    //             {
    //                 double right = stack.Pop();
    //                 double left = stack.Pop();
    //                 stack.Push(left * right);
    //             }
    //             else if (token == "/")
    //             {
    //                 double right = stack.Pop();
    //                 double left = stack.Pop();
    //                 stack.Push(left / right);
    //             }
    //             else if (token == "^")
    //             {
    //                 double right = stack.Pop();
    //                 double left = stack.Pop();
    //                 stack.Push(Math.Pow(left, right));
    //             }
    //         }
    //         return stack.Pop();
    //     }
    // }
}
