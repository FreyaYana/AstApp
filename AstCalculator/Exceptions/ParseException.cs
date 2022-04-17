using System;

namespace ConsoleAppAst.Exceptions
{
    public class ParseException: Exception
    {
        public ParseException(string s) : base(s)
        {
                
        }

        public ParseException(char c)
        {
            
        }
    }
}