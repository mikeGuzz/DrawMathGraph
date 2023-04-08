using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawMathGraph
{
    public sealed class FunctionValue
    {
        public readonly double X;
        public readonly double Y;

        public FunctionValue(double x, double y)
        {
            X = x;
            Y = y;
        }

        public FunctionValue(double x, double y, bool isValid)
        {
            X = x;
            Y = y;
        }
    }
}
