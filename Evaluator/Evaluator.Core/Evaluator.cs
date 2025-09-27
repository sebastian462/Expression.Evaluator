using System.Data;
using System.Runtime.CompilerServices;

namespace Evaluator.Core
{
    public class ExpressionEvaluator
    {
        public static double Evaluate(string infix)
        {
            var postfix = InfixToPostfix(Tokenize(infix));
            return Calculate(postfix);
        }

        private static List<string> InfixToPostfix(List<string> infix)

        {
            var stack = new Stack<string>();
            var postfix = new List<string>();

            foreach (var item in infix)
            {
                if (IsOperator(item))
                {
                    if (item == ")")
                    {
                        while (stack.Peek() != "(")
                        {
                            postfix.Add(stack.Pop());
                        }
                        stack.Pop();
                    }
                    else
                    {
                        while (stack.Count > 0 && PriorityInfix(item) <= PriorityStack(stack.Peek()))
                        {
                            postfix.Add(stack.Pop());
                        }
                        stack.Push(item);
                    }
                }
                else
                {
                    postfix.Add(item);
                }
            }

            while (stack.Count > 0)
            {
                postfix.Add (stack.Pop());
            }

            return postfix;
        }

        private static bool IsOperator(string item) => item == "^" || item == "*" || item == "%" || item == "/" || item == "+" || item == "-" || item == "(" || item == ")";
        
        private static int PriorityInfix(string op)
        {
            return op switch
            {
                "^" => 4,
                "*" or "/" or "%" => 2,
                "+" or "-" => 1,
                "(" => 5,
                _ => throw new Exception("Invalid expression"),
            };
        }
        private static int PriorityStack(string op)
        {
            return op switch
            {
                "^" => 3,
                "*" or "/" or "%" => 2,
                "+" or "-" => 1,
                "(" => 0,
                _ => throw new Exception("Invalid expression."),
            };
        }

        private static double Calculate(List<string> postfix)
        {
            var stack = new Stack<double>();

            foreach (var item in postfix)
            {
                if (IsOperator(item))
                {
                    double op2 = stack.Pop();
                    double op1 = stack.Pop();
                    stack.Push(Calculate(op1, item, op2)); 
                }
                else
                {
                    stack.Push(Convert.ToDouble(item));
                }
            }
            return stack.Peek();
        }
        private static double Calculate(double op1, string item, double op2)
        {
            return item switch
            {
                "*" => op1 * op2,
                "/" => op1 / op2,
                "^" => Math.Pow(op1, op2),
                "+" => op1 + op2,
                "-" => op1 - op2,
                _ => throw new Exception("Invalid expression."),
            };
        }

        private static List<string> Tokenize(string infix)
        {
            var numop = new List<string>();
            var number = "";

            foreach (char ch in infix)
            {
                if (char.IsDigit(ch) || ch == '.')
                {
                    number += ch;
                }
                else
                {
                    if (!string.IsNullOrEmpty(number))
                    {
                        numop.Add(number);
                        number = "";
                    }

                    if (!char.IsWhiteSpace(ch))
                        numop.Add(ch.ToString());
                  
                }
            }
            if (!string.IsNullOrEmpty(number))
                numop.Add(number);
            return numop;
        }
    }
}

