using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawMathGraph
{
    public class FunctionVariable
    {
        public readonly string Name;
        public double Value { get; set; }

        public FunctionVariable(string name)
        {
            Name = name;
        }

        public FunctionVariable(string name, double value)
        {
            Name = name;
            Value = value;
        }
    }
}
