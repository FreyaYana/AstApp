using ConsoleAppAst.Models.Enums;

namespace ConsoleAppAst.Models
{
    public class Lexeme
    {
        public LexemeType Type;
        public string Value;
        public string Name;

        public Lexeme(LexemeType type, string value, string name) 
            : this(type, value)
        {
            Name = name;
        }
        
        public Lexeme(LexemeType type, string value)
        {
            this.Type = type;
            this.Value = value;
        }

        public Lexeme(LexemeType type, char value)
        {
            this.Type = type;
            this.Value = value.ToString();
        }
    }
}