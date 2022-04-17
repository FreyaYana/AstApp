using System;
using System.Linq;
using ConsoleAppAst.Folder;
using ConsoleAppAst.Models;
using ConsoleAppAst.Models.Enums;
using ConsoleAppAst.Models.RDParser;

namespace ConsoleAppAst
{
    class Program
    {
        static void Main(string[] args)
        {
            var expressionText = Console.ReadLine();
            var lexemes = RdParser.Analyze(expressionText);
            foreach (var varLexeme in lexemes.Where(w => w.Type == LexemeType.Var))
            {
                Console.WriteLine($"{varLexeme.Name}=");
                var value = Console.ReadLine();
                varLexeme.Value = value;
            }
            var (result, three) = RdParser.Parse(new LexemeBuffer(lexemes));

            var function = Console.ReadLine();
            while (function != "end")
            {
                switch (function)
                {
                    case "print":
                        three.Print();
                        break;
                    case "calculate":
                        Console.WriteLine($"Result - {result}");
                        break;
                }

                function = Console.ReadLine();
            }

            Console.WriteLine(result);
            Console.ReadLine();
        }
    }
}