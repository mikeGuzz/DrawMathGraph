using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawMathGraph
{
    public class FunctionHolder
    {
        public readonly string Name;
        public readonly int ArgumentCount;
        public readonly MathFunctionDelegate Delegate;
        public readonly string? ShortDescription;

        public FunctionHolder(string name, int argumentCount, MathFunctionDelegate funcDelegate)
        {
            Name = name;
            Delegate = funcDelegate;
            ArgumentCount = argumentCount;
        }
        public FunctionHolder(string name, int argumentCount, MathFunctionDelegate funcDelegate, string shortDescription)
        {
            Name = name;
            Delegate = funcDelegate;
            ArgumentCount = argumentCount;
            ShortDescription = shortDescription;
        }

        public override string ToString()
        {
            StringBuilder message = new StringBuilder($"{Name}({ArgumentCount} params)\n");
            message.Append(Environment.NewLine);
            if (!ReferenceEquals(null, ShortDescription))
                message.Append($"{ShortDescription}\n");
            return message.ToString();
        }
    }
}
