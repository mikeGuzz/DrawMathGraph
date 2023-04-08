using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawMathGraph
{
    public static class MyExtensions
    {
        public static bool IsBinaryOperation(this char c)
        {
            return (c == '+' || c == '-' || c == '*' || c == '/' || c == '^');
        }
    }
}
