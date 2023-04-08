using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawMathGraph
{
    public delegate double MathFunctionDelegate(params double[] args);

    public static class MathFunction
    {
        public static readonly FunctionHolder[] Functions = new FunctionHolder[]
        {
            new FunctionHolder("abs", 1, i =>
            {
                if (i.Length != 1)
                    throw new SyntaxException($"No overload for method '{nameof(Math.Abs).ToLower()}' takes {i.Length} arguments");
                return Math.Abs(i[0]);
            }, "Returns the absolute value of a double-precision floating-point number."),
            new FunctionHolder("cos", 1, i =>
            {
                if (i.Length != 1)
                    throw new SyntaxException($"No overload for method '{nameof(Math.Cos).ToLower()}' takes {i.Length} arguments");
                return Math.Cos(i[0]);
            }, "Returns the cosine of the specified angle."),
            new FunctionHolder("sin", 1, i =>
            {
                if (i.Length != 1)
                    throw new SyntaxException($"No overload for method '{nameof(Math.Sin).ToLower()}' takes {i.Length} arguments");
                return Math.Sin(i[0]);
            }, "Returns the sine of the specified angle."),
            new FunctionHolder("tan", 1, i =>
            {
                if (i.Length != 1)
                    throw new SyntaxException($"No overload for method '{nameof(Math.Tan).ToLower()}' takes {i.Length} arguments");
                return Math.Tan(i[0]);
            }, "Returns the tangent of the specified angle."),
            new FunctionHolder("tanh", 1, i =>
            {
                if (i.Length != 1)
                    throw new SyntaxException($"No overload for method '{nameof(Math.Tanh).ToLower()}' takes {i.Length} arguments");
                return Math.Tanh(i[0]);
            }, "Returns the hyperbolic tangent of the specified angle."),
            new FunctionHolder("round", 2, i =>
            {
                if (i.Length != 2)
                    throw new SyntaxException($"No overload for method '{nameof(Math.Round).ToLower()}' takes {i.Length} arguments");
                if(i[1].ToString().Split('.').Length > 1 || (int)i[1] < 0)
                    throw new SyntaxException("Invalid argument.");
                return Math.Round(i[0], (int)i[1]);
            }, "Rounds a decimal value to a specified number of fractional digits, and rounds midpoint values to the nearest even number."),
            new FunctionHolder("sqrt", 1, i =>
            {
                if (i.Length != 1)
                    throw new SyntaxException($"No overload for method '{nameof(Math.Sqrt).ToLower()}' takes {i.Length} arguments");
                return Math.Sqrt(i[0]);
            }, "Returns the square root of a specified number."),
            new FunctionHolder("log", 2, i =>
            {
                if (i.Length != 2)
                    throw new SyntaxException($"No overload for method '{nameof(Math.Log).ToLower()}' takes {i.Length} arguments");
                return Math.Log(i[0], i[1]);
            }, "Returns the logarithm of a specified number in a specified base."),
            new FunctionHolder("pow", 2, i =>
            {
                if (i.Length != 2)
                    throw new SyntaxException($"No overload for method '{nameof(Math.Pow).ToLower()}' takes {i.Length} arguments");
                return Math.Pow(i[0], i[1]);
            }, "Returns a specified number raised to the specified power."),
             new FunctionHolder("ln", 1, i =>
            {
                if (i.Length != 1)
                    throw new SyntaxException($"No overload for method 'ln' takes {i.Length} arguments");
                return Math.Log(i[0]);
            }, $"Returns the natural (base {nameof(Math.E).ToLower()}) logarithm of a specified number."),
            new FunctionHolder("ceil", 1, i =>
            {
                if (i.Length != 1)
                    throw new SyntaxException($"No overload for method 'ceil' takes {i.Length} arguments");
                return Math.Ceiling(i[0]);
            }, "Returns the smallest integral value greater than or equal to the specified number."),
            new FunctionHolder("floor", 1, i =>
            {
                if (i.Length != 1)
                    throw new SyntaxException($"No overload for method '{nameof(Math.Floor).ToLower()}' takes {i.Length} arguments");
                return Math.Ceiling(i[0]);
            }, "Returns the largest integral value less than or equal to the specified number."),
        };

        public static double CallFunction(string name, params double[] args) => GetFunctionDelegate(name)(args);

        public static bool IsFunction(string? name)
        {
            if (string.IsNullOrEmpty(name))
                return false;
            if (name[0] == '-')
                name = name.Remove(0, 1);
            return Functions.Any(ob => name == ob.Name);
        }

        public static MathFunctionDelegate GetFunctionDelegate(string name)
        {
            return GetFunction(name).Delegate;
        }

        public static FunctionHolder GetFunction(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new SyntaxException("Empty function name");
            if (name[0] == '-')
                name = name.Remove(0, 1);
            foreach (var ob in Functions)
                if (name == ob.Name)
                    return ob;
            throw new SyntaxException($"There is no method named {name}");
        }

        public static string SimplifyEquation(string str)
        {
            int opened = 0;
            int flag = 0;
            StringBuilder build = new StringBuilder(str);
            StringBuilder args = new StringBuilder();
            StringBuilder methodName = new StringBuilder();

            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == '(' && flag != 0) opened++;
                else if (str[i] == ')' && flag != 0) opened--;

                if (str[i] == '(' && i > 0)
                    if (str[i - 1] == ')' || char.IsNumber(str[i - 1]))
                        build.Insert(i, '*');// adding * where it is set implicitly

                if ((char.IsLetter(str[i]) || str[i] == '_') && flag == 0)// if there is a possible beginning of the function name
                    flag = 1;
                else if (str[i] == '(' && flag == 1)
                {
                    if (methodName.ToString().ToLower() == "x")
                    {
                        methodName.Clear();
                        if (args.Length > 0)
                            args.Clear();
                    }

                    if (!IsFunction(methodName.ToString()))
                        throw new SyntaxException($"Method '{methodName}' is not available or does not exist");
                    flag = 2;
                }
                else if (str[i] == ')' && flag == 2 && opened == 0)
                {
                    string method = methodName.ToString() + args + ')';
                    build.Replace(method, SimplifyFunction(method).ToString());
                    flag = 0;
                    methodName.Clear();
                    args.Clear();
                }

                switch (flag)
                {
                    case 1:
                        methodName.Append(str[i]);
                        break;
                    case 2:
                        args.Append(str[i]);
                        break;
                }
            }

            return build.ToString();
        }

        public static double ComputeEquation(string str, double x)
        {
            if (str == string.Empty)
                throw new SyntaxException("Empty equation.");

            StringBuilder build = new StringBuilder();
            StringBuilder methodStr = new StringBuilder();
            int lastXIndex = 0;
            int flag = 0;
            int intend = 0;

            for (int i = 0; i < str.Length; i++) //replace all x on values
            {
                build.Append(str[i]);

                if (char.ToLower(str[i]) == 'x' && flag == 0)// if you find x, but are not sure that this is not a method name
                {
                    lastXIndex = i;
                    flag = 1;
                    if (i > 0)
                        if (char.IsNumber(str[i - 1]))
                            flag = 2;// if there is an implicit *
                }

                if (flag != 0)
                    methodStr.Append(str[i]);

                if ((!char.IsNumber(str[i]) && !char.IsLetter(str[i]) && str[i] != '_' && flag != 0) || i == str.Length - 1)
                {
                    if (!string.IsNullOrEmpty(methodStr.ToString()) && !methodStr.ToString().Any(ob => (char.IsLetter(ob) && ob != 'x') || ob == '_'))
                    {
                        if (flag == 2)
                            build.Insert(lastXIndex++ + intend, '*');
                        build.Remove(lastXIndex + intend, 1);
                        build.Insert(lastXIndex + intend, x);
                        intend += Convert.ToString(x).Length - 1 + Convert.ToInt32(flag == 2);
                        flag = 0;
                        methodStr.Clear();
                    }
                }
            }
            try
            {
                var res = Convert.ToDouble(new DataTable().Compute(SimplifyEquation(build.ToString()), null));
                return res;
            }
            catch
            {
                return double.NaN;
            }
        }

        public static double SimplifyFunction(string str)
        {
            if (string.IsNullOrEmpty(str))
                throw new SyntaxException("Empty function.");

            StringBuilder name = new StringBuilder();
            int index;
            int multiplier = 1;

            for (index = 0; index < str.Length; index++)// name reading
            {
                if (str[index] != '(')
                    name.Append(str[index]);
                else break;
            }

            if (name[0] == '-')
            {
                multiplier = -1;
                name.Remove(0, 1);
            }

            if (!IsFunction(name.ToString()))
                throw new SyntaxException($"Method '{name}' is not available or does not exist");

            StringBuilder argsLine = new StringBuilder();
            index++;

            for (; index < str.Length - (str.Last() == ')' ? 1 : 0); index++)
                argsLine.Append(str[index]);// reading arguments, e.g. "Log(8, 2)" = { 8, 2 }

            List<StringBuilder> args = new List<StringBuilder>() { new StringBuilder() };
            index = 0;
            int opened = 0;

            for (int i = 0; i < argsLine.Length; i++)// parsing the argument string into individual arguments
            {
                switch (argsLine[i])
                {
                    case '(':
                        opened++;
                        break;
                    case ')':
                        opened--;
                        break;
                    case ',':
                        if (opened == 0)
                        {
                            index++;
                            args.Add(new StringBuilder());
                        }
                        break;
                }

                if (argsLine[i] != ' ')
                    if (!(argsLine[i] == ',' && opened == 0))
                        args[index].Append(argsLine[i]);
            }

            index = 0;
            for (; index < args.Count; index++)
            {
                if (string.IsNullOrEmpty(args[index].ToString()))
                    throw new SyntaxException($"{index + 1}'st argument in method '{name}' is invalid");
                StringBuilder superName = new StringBuilder();
                bool flag = false;
                for (int i = 0; i < args[index].Length; i++)
                {
                    if (char.IsLetter(args[index][i]) || args[index][i] == '_' && !flag)
                        flag = true;
                    else if (flag && args[index][i] == '(')
                    {
                        if (!string.IsNullOrEmpty(superName.ToString()))
                        {
                            if (IsFunction(superName.ToString()))
                            {
                                double res = ComputeEquation(args[index].ToString(), 1);
                                args[index].Clear();
                                args[index].Append(res);
                            }
                        }

                        break;
                    }

                    if (flag)
                        superName.Append(args[index][i]);
                }

                try
                {
                    var tempRes = Convert.ToDouble(new DataTable().Compute(args[index].ToString(), null));
                    args[index].Clear();
                    args[index].Append(tempRes);
                }
                catch (Exception e)
                {
                    throw new SyntaxException($"Argument '{args[index]}' in method '{name}' is invalid.\nDetails:\n{e.Message}", e);
                }
            }

            return CallFunction(name.ToString(), args.Select(i => Convert.ToDouble(i.ToString())).ToArray()) * multiplier;
        }
    }
}
