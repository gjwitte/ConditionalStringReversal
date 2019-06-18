using System;
using System.Text.RegularExpressions;
using ConditionalStringReversal.Entities;

namespace ConditionalStringReversal
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            

            var dbContext = new MortgageConnectDbContext();
            var handler = new ConditionalStringReversalHandler(dbContext);

            var answer = handler.Handle(1);

            Console.WriteLine($"The conditionally reversed string is: {answer}");
            Console.ReadKey();
        }
    }
}
