using ConditionalStringReversal.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ConditionalStringReversal
{
    public class ConditionalStringReversalHandler
    {
        private readonly MortgageConnectDbContext _dbContext;

        public ConditionalStringReversalHandler(MortgageConnectDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public string Handle(int databaseId)
        {
            var pattern = new Regex("\\w");
            var inputString = _dbContext.StringToReverse.FirstOrDefault(s => s.Id == databaseId)?.DataValue;

            if (inputString == null)
                throw new ArgumentException($"Unable to locate database record id {databaseId}");

            var answer = new string[inputString.Length];

            var itemsToRearrange = GetStackOfItemsToRearrange(inputString, pattern);
            
            for (int i = 0; i < inputString.Length; i++)
            {
                var currentValue = inputString[i].ToString();
                
                if (pattern.IsMatch(currentValue))
                {
                    answer[i] = itemsToRearrange.Pop();
                }

                else
                {
                    answer[i] = currentValue;
                }
            }

            return String.Join(" ", answer);
        }

        private static Stack<string> GetStackOfItemsToRearrange(string inputString, Regex pattern)
        {
            var stack = new Stack<string>();

            foreach (var c in inputString)
            {
                var currentValue = c.ToString();
                if (pattern.IsMatch(currentValue))
                {
                    stack.Push(currentValue);
                }
            }

            return stack;
        }
    }
}
