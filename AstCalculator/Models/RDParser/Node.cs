using System;
using ConsoleAppAst.Models.Enums;

namespace ConsoleAppAst.Models.RDParser
{
    public class Node : IEquatable<Node>
    {
        public Node()
        {
            Token = LexemeType.Unknown;
        }
        
        public Node(LexemeType token)
        {
            Token = token;
        }
        
        public Node(int value, LexemeType token)
        {
            Value = value;
            Token = token;
        }

        private LexemeType Token { get; set; }
        
        public Node Left { get; set; }
        
        public Node Right { get; set; }
        
        public int Value { get; set; }
        
        public string PrintedForm { get; set; }

        public bool IsMultDiv => Token is LexemeType.Mul or LexemeType.Div;
        
        public bool IsPlusMinus => Token is LexemeType.Plus or LexemeType.Minus;

        public void SetToken(LexemeType token)
        {
            Token = token;
            switch (token)
            {
                case LexemeType.Plus:
                    PrintedForm = "+";
                    break;
                case LexemeType.Minus:
                    PrintedForm = "-";
                    break;
                case LexemeType.Mul:
                    PrintedForm = "*";
                    break;
                case LexemeType.Div:
                    PrintedForm = "/";
                    break;
                case LexemeType.Number:
                    PrintedForm = Value.ToString();
                    break;
                case LexemeType.LBracket:
                case LexemeType.RBracket:
                case LexemeType.Eof:
                case LexemeType.Unknown:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(token), token, null);
            }
        }

        public static Node WithLeft(Node lNode)
        {
            return new Node
            {
                Left = lNode
            };
        }

        public bool IsFull()
        {
            return Token == LexemeType.Number || Right != null && Left != null;
        }

        private bool IsLeaf()
        {
            return Token == LexemeType.Number || Right == null && Left == null;
        }

        public override string ToString()
        {
            if (IsLeaf())
            {
                return Value.ToString();
            }
            return PrintedForm;
        }

        public bool Equals(Node other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Token == other.Token 
                   && Equals(Left, other.Left) 
                   && Equals(Right, other.Right)
                   && Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == this.GetType() && Equals((Node) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine((int) Token, Left, Right, Value);
        }
    }
}