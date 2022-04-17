using System;

namespace ConsoleAppAst.Exceptions
{
    public class UnexpectedSymbolOrderParseException: Exception
    {
        public UnexpectedSymbolOrderParseException(string s) : base(s)
        {
                
        }

        public UnexpectedSymbolOrderParseException(char c)
        {
            
        }
    }
}