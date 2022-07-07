using System;

namespace ConsoleAppAst.Exceptions
{
    public class UnexpectedSymbolOrderParseException: Exception
    {
        public UnexpectedSymbolOrderParseException(string s, int getPosition) : base(s)
        {
                
        }

        public UnexpectedSymbolOrderParseException(char c)
        {
            
        }
    }
}