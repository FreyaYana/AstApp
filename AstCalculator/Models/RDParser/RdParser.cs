using System;
using System.Collections.Generic;
using System.Text;
using ConsoleAppAst.Exceptions;
using ConsoleAppAst.Models.Enums;

namespace ConsoleAppAst.Models.RDParser
{
    public class RdParser
    {
        // RULES
        //------------------------------------------------------------------
        //    expr : plus* EOF ;
        //    sum: mult [ ( '+' | '-' ) mult ]* ;
        //    mult : factor [ ( '*' | '/' ) factor ]* ;
        //    fact : NUMBER | VAR | '(' expr ')' ;

        public static (short Result, Node Three) Parse(LexemeBuffer lexemeBuffer)
        {
            var node = new Node();
            return Expression(lexemeBuffer, node);
        }
        public static Lexeme[] Analyze(string expText)
        {
            var lexemes = new List<Lexeme>();
            var pos = 0;
            while (pos < expText.Length)
            {
                var c = expText[pos];
                switch (c)
                {
                    case '(':
                        lexemes.Add(new Lexeme(LexemeType.LBracket, c));
                        pos++;
                        continue;
                    case ')':
                        lexemes.Add(new Lexeme(LexemeType.RBracket, c));
                        pos++;
                        continue;
                    case '+':
                        lexemes.Add(new Lexeme(LexemeType.Plus, c));
                        pos++;
                        continue;
                    case '-':
                        lexemes.Add(new Lexeme(LexemeType.Minus, c));
                        pos++;
                        continue;
                    case '*':
                        lexemes.Add(new Lexeme(LexemeType.Mul, c));
                        pos++;
                        continue;
                    case '/':
                        lexemes.Add(new Lexeme(LexemeType.Div, c));
                        pos++;
                        continue;
                    default:
                    {
                        if (char.IsDigit(c))
                        {
                            var sb = new StringBuilder();
                            do
                            {
                                sb.Append(c);
                                pos++;
                                if (pos >= expText.Length)
                                {
                                    break;
                                }
                                c = expText[pos];
                            } 
                            while (char.IsDigit(c));

                            lexemes.Add(new Lexeme(LexemeType.Number, sb.ToString()));
                        }
                        
                        else if (char.IsLetter(c))
                        {
                            var sb = new StringBuilder();
                            do
                            {
                                sb.Append(c);
                                pos++;
                                if (pos >= expText.Length)
                                {
                                    break;
                                }
                                c = expText[pos];
                            } 
                            while (char.IsLetterOrDigit(c));
                            lexemes.Add(new Lexeme(LexemeType.Var, sb.ToString(), sb.ToString()));
                        }
                        else
                        {
                            if (c != ' ')
                            {
                                throw new ParseException(c); //  Runtime
                            }
                            pos++;
                        }
                        break;
                    }

                }
            }

            lexemes.Add(new Lexeme(LexemeType.Eof, ""));
            return lexemes.ToArray();
        }
        private static (short, Node) Expression(LexemeBuffer lexemes, Node node)
        {
            var lexeme = lexemes.Up();
            if (lexeme.Type == LexemeType.Eof)
            {
                return (0, new Node());
            }

            lexemes.Down();
            return SumExpression(lexemes, ref node);
        }
        private static (short, Node) SumExpression(LexemeBuffer lexemes, ref Node node)
        {
            var md = MultExpression(lexemes, ref node);
            var value = md.Result;

            while (true)
            {
                var lexeme = lexemes.Up();
                switch (lexeme.Type)
                {
                    case LexemeType.Plus:
                        if (node.IsPlusMinus || node.IsFull())
                        {
                            node = Node.WithLeft(node);
                        }

                        node.SetToken(LexemeType.Plus);
                        var rn = new Node();
                        var v = MultExpression(lexemes, ref rn);
                        value += v.Result;
                        node.Right = v.Node;
                        break;
                    case LexemeType.Minus:
                        if (node.IsPlusMinus || node.IsFull())
                        {
                            node = Node.WithLeft(node);
                        }

                        node.SetToken(LexemeType.Minus);
                        rn = new Node();
                        v = MultExpression(lexemes, ref rn);
                        value -= v.Result;
                        node.Right = v.Node;
                        break;
                    case LexemeType.Eof:
                    case LexemeType.RBracket:
                        lexemes.Down();
                        return (value, node);
                    default:
                        throw new Exception("Unexpected token: " + lexeme.Value
                                                                 + " at position: " + lexemes.GetPosition()); //Runtime
                }
            }
        }
        private static (short Result, Node Node) MultExpression(LexemeBuffer lexemes, ref Node node)
        {
            var f = Fact(lexemes, ref node);
            var value = f.Result;

            while (true)
            {
                var lexeme = lexemes.Up();
                switch (lexeme.Type)
                {
                    case LexemeType.Mul:
                        if (node.IsMultDiv || node.IsFull())
                        {
                            node = Node.WithLeft(f.Node);
                        }
                        node.SetToken(LexemeType.Mul);
                        var rn = new Node();
                        var v = Fact(lexemes, ref rn);
                        value *= v.Result;
                        node.Right = v.Node;
                        break;
                    case LexemeType.Div:
                        if (node.IsMultDiv || node.IsFull())
                        {
                            node = Node.WithLeft(f.Node);
                        }

                        node.SetToken(LexemeType.Div);
                        rn = new Node();
                        v = Fact(lexemes, ref rn);
                        value /= v.Result;
                        node.Right = v.Node;
                        break;
                    case LexemeType.Eof:
                    case LexemeType.RBracket:
                    case LexemeType.Plus:
                    case LexemeType.Minus:
                        lexemes.Down();
                        return (value, node);
                    default:
                        throw new Exception("Unexpected token: " + lexeme.Value
                                                                 + " at position: " + lexemes.GetPosition()); //Runtime
                }
            }
        }
        private static (short Result, Node Node) Fact(LexemeBuffer lexemes, ref Node node)
        {
            var lexeme = lexemes.Up();
            switch (lexeme.Type)
            {
                case LexemeType.Number:
                    node.Value = short.Parse(lexeme.Value);
                    node.SetToken(LexemeType.Number);
                    return (short.Parse(lexeme.Value), node);
                case LexemeType.Var:
                    node.Value = short.Parse(lexeme.Value);
                    node.SetToken(LexemeType.Number);
                    return (short.Parse(lexeme.Value), node);
                case LexemeType.LBracket:
                    var value = SumExpression(lexemes, ref node);
                    lexeme = lexemes.Up();
                    if (lexeme.Type != LexemeType.RBracket)
                    {
                        throw new Exception("Unexpected token: " + lexeme.Value
                                                                 + " at position: " + lexemes.GetPosition()); // Runtime
                    }

                    return value;
                default:
                    throw new Exception("Unexpected token: " + lexeme.Value
                                                             + " at position: " + lexemes.GetPosition()); //Runtime
            }
        }
    }
}