using ConsoleAppAst.Exceptions;
using ConsoleAppAst.Models;
using ConsoleAppAst.Models.RDParser;
using Xunit;

namespace ConsoleTests
{
    public class InputDataTests
    {
        [Theory]
        [InlineData("1+=2")]
        [InlineData("!1+2")]
        [InlineData("1+2?")]
        [InlineData("<>")]
        public void UnknownSymbolsTest(string s)
        {
            var lexemes = RdParser.Analyze(s);
            Assert.Throws<ParseException>(() 
                => RdParser.Parse(new LexemeBuffer(lexemes)));
        }

        [Theory]
        [InlineData("1++2")]
        [InlineData("12+")]
        [InlineData("+12")]
        [InlineData("1a + 2")]
        [InlineData("a! + 2")]
        [InlineData("a_ + 2")]
        public void UnexpectedSymbolsOrderTest(string s)
        {
            var lexemes = RdParser.Analyze(s);
            Assert.Throws<UnexpectedSymbolOrderParseException>(() 
                => RdParser.Parse(new LexemeBuffer(lexemes)));
        }
    }
}