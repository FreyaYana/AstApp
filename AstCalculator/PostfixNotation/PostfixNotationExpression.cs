using System;
using System.Collections.Generic;
using ConsoleAppAst.Models.RDParser;

namespace ConsoleAppAst.PostfixNotation
{
    //последовательно читаем выражение, для каждой лексемы по её типу:
    //число -> создать ноду (ЧИСЛО, значение), положить её на стек;
    //унарный оператор -> снять со стека ноду (потомок), создать ноду (УНАРНАЯОПЕРАЦИЯ, тип, потомок),
        //положить новую ноду на стек;
    //бинарный оператор -> снять со стека две ноды (потомок1 и потомок2),
        //создать ноду (БИНАРНАЯОПЕРАЦИЯ, тип, потомок1, потомок2), положить новую ноду на стек;
    //функция -> снять со стека N нод по числу аргументов функции (потомок1,... потомокN),
    //создать ноду (ФУНКЦИЯ, имя, потомок1,... потомокN), положить новую ноду на стек;

    //В конце работы по корректному выражению на стеке должна лежать одна нода, она и будет корнем дерева.
    public class PostfixNotationExpression
    {
        public PostfixNotationExpression()
        {
            _operators = new List<string>(_standartOperators);

        }

        private readonly List<string> _operators;

        private readonly List<string> _standartOperators =
            new (new [] {"(", ")", "+", "-", "*", "/", "^"});

        public IEnumerable<string> Separate(string input)
        {
            var pos = 0;
            while (pos < input.Length)
            {
                var s = string.Empty + input[pos];
                if (!_standartOperators.Contains(input[pos].ToString()))
                {
                    if (char.IsDigit(input[pos]))
                        for (var i = pos + 1;
                            i < input.Length &&
                            (char.IsDigit(input[i]) || input[i] == ',' || input[i] == '.');
                            i++)
                            s += input[i];
                    else if (char.IsLetter(input[pos]))
                        for (var i = pos + 1;
                            i < input.Length &&
                            (char.IsLetter(input[i]) || char.IsDigit(input[i]));
                            i++)
                            s += input[i];
                }

                yield return s;
                pos += s.Length;
            }
        }

        private byte GetPriority(string s)
        {
            switch (s)
            {
                case "(":
                case ")":
                    return 0;
                case "+":
                case "-":
                    return 1;
                case "*":
                case "/":
                    return 2;
                default:
                    return 4;
            }
        }

        private IEnumerable<string> ConvertToPostfixNotation(string input)
        {
            var n = new Node();
            var outputSeparated = new List<string>();
            var stack = new Stack<string>();
            foreach (var c in Separate(input))
            {
                if (_operators.Contains(c))
                {
                    if (stack.Count > 0 && !c.Equals("("))
                    {
                        if (c.Equals(")"))
                        {
                            var s = stack.Pop();
                            while (s != "(")
                            {
                                outputSeparated.Add(s);
                                s = stack.Pop();
                            }
                        }
                        else if (GetPriority(c) > GetPriority(stack.Peek()))
                            stack.Push(c);
                        else
                        {
                            while (stack.Count > 0 && GetPriority(c) <= GetPriority(stack.Peek()))
                                outputSeparated.Add(stack.Pop());
                            stack.Push(c);
                        }
                    }
                    else
                        stack.Push(c);
                }
                else
                    outputSeparated.Add(c);
            }

            if (stack.Count <= 0) return outputSeparated.ToArray();
            {
                outputSeparated.AddRange(stack);
            }

            return outputSeparated.ToArray();
        }

        public decimal Result(string input)
        {
            var stack = new Stack<string>();
            var queue = new Queue<string>(ConvertToPostfixNotation(input));
            var str = queue.Dequeue();
            while (queue.Count >= 0)
            {
                if (!_operators.Contains(str))
                {
                    stack.Push(str);
                    str = queue.Dequeue();
                }
                else
                {
                    decimal summ = 0;
                    try
                    {

                        switch (str)
                        {

                            case "+":
                            {
                                decimal a = Convert.ToDecimal(stack.Pop());
                                decimal b = Convert.ToDecimal(stack.Pop());
                                summ = a + b;
                                break;
                            }
                            case "-":
                            {
                                decimal a = Convert.ToDecimal(stack.Pop());
                                decimal b = Convert.ToDecimal(stack.Pop());
                                summ = b - a;
                                break;
                            }
                            case "*":
                            {
                                var a = short.Parse(stack.Pop());
                                var b = short.Parse(stack.Pop());
                                summ = b * a;
                                break;
                            }
                            case "/":
                            {
                                var a = short.Parse(stack.Pop());
                                var b = short.Parse(stack.Pop());
                                summ = b / a;
                                break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show(ex.Message);
                    }

                    stack.Push(summ.ToString());
                    if (queue.Count > 0)
                        str = queue.Dequeue();
                    else
                        break;
                }

            }

            return Convert.ToDecimal(stack.Pop());
        }
    }
}