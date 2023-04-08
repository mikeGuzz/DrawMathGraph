using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Drawing2D;
using System.Windows.Forms.VisualStyles;

namespace DrawMathGraph
{
    public class DrawableFunction
    {
        public static int SeparateN { get; set; } = 4;
        public static Color BackgroundColor { get; set; } = Color.White;
        public static int SeparatorSize { get; set; } = 3;
        public static int MarksDistance { get; set; } = 90;

        public string Function { get; set; }
        public Color Color { get; set; } = Color.Red;
        public DashStyle DashStyle { get; set; }

        public List<FunctionValue> Table { get; private set; } = new List<FunctionValue>();

        public DrawableFunction(string function)
        {
            Function = function;
        }

        public DrawableFunction(string function, Color color)
        {
            Function = function;
            Color = color;
        }

        public void Draw(Graphics g, double stepX, SizeF tileSize, Size clientSize, SizeF offset)
        {
            Table.Clear();

            PointF center = new PointF(clientSize.Width / 2f, clientSize.Height / 2f);
            int sizeN = (int)Math.Ceiling(clientSize.Width / (stepX * tileSize.Width)) + 1;
            var min = ((double)center.X - offset.Width - clientSize.Width) / tileSize.Width;

            for (int i = -1; i < sizeN; i++)
            {
                double x = i * stepX + min;
                double y;
                try
                {
                    y = MathFunction.ComputeEquation(Function, x);
                    if (double.IsInfinity(y))
                        y = double.NaN;
                    else if (!double.IsNaN(y))
                        y = Math.Round(y, 3);
                }
                catch (Exception ex) when (ex is SyntaxException)
                {
                    y = double.NaN;
                }

                Table.Add(new FunctionValue(x, -y));
            }

            g.SmoothingMode = SmoothingMode.HighSpeed;

            var pen = new Pen(Color, 2f);
            pen.DashStyle = DashStyle;
            var getX_Px = (int i) => i * (tileSize.Width * stepX);
            var getY_Px = (int i) => Math.Round(Table[i].Y * tileSize.Height) + center.Y + offset.Height;

            for (int i = 0; i < Table.Count - 1; i++)
            {
                if (double.IsNaN(Table[i].Y))
                    continue;
                if (double.IsNaN(Table[i + 1].Y))
                {
                    i++;
                    continue;
                }
                float x1 = (float)getX_Px(i);
                float x2 = (float)getX_Px(i + 1);
                var y1 = getY_Px(i);
                var y2 = getY_Px(i + 1);

                if ((y1 < 0 && y2 < 0) || (y1 > clientSize.Height && y2 > clientSize.Height))
                    continue;

                if (y1 < 0)
                    y1 = 0;
                else if (y1 > clientSize.Height)
                    y1 = clientSize.Height;
                if (y2 < 0)
                    y2 = 0;
                else if (y2 > clientSize.Height)
                    y2 = clientSize.Height;

                try
                {
                    g.DrawLine(pen, new PointF(x1, (float)y1), new PointF(x2, (float)y2));
                }
                catch { }
            }
        }

        public static void DrawGraphBackground(Graphics g, SizeF tileSize, Size clientSize, SizeF offset)
        {
            g.Clear(BackgroundColor);

            PointF center = new PointF(clientSize.Width / 2f, clientSize.Height / 2f);
            var axisPen = new Pen(Color.Black, 2f);
            var gridPen = new Pen(Color.FromArgb(55, Color.Black), 1.5f);
            var separatorGridPen = new Pen(Color.FromArgb(105, Color.Black), 1.5f);
            var xAxisP = center.Y + offset.Height;
            var yAxisP = center.X + offset.Width;

            //x and y axis
            g.DrawLine(axisPen, new PointF(0, xAxisP), new PointF(clientSize.Width, xAxisP));//x-axis
            g.DrawLine(axisPen, new PointF(yAxisP, 0), new PointF(yAxisP, clientSize.Height));//y-axis

            //origin mark
            var sepSize = SeparatorSize;
            var multSepSize = (int)(sepSize * 1.75);
            var valuesFont = new Font("Consolas", 9);
            g.DrawString("0", valuesFont, Brushes.Black, new PointF(center.X + offset.Width + multSepSize, center.Y + offset.Height + multSepSize));

            //grid
            var markDist = MarksDistance;
            var getMarkX = (float fontHeight) =>
            {
                float markPos;
                if (xAxisP < 0)//top edge
                    markPos = multSepSize;
                else if ((xAxisP + multSepSize) > (clientSize.Height - markDist))//bottom edge
                    markPos = clientSize.Height - fontHeight - multSepSize - (xAxisP < clientSize.Height ? clientSize.Height - xAxisP : 0);
                else//none
                    markPos = xAxisP + multSepSize;
                return markPos;
            };
            var getMarkY = (float fontWidth) =>
            {
                float markPos;
                if (yAxisP < 0)//left edge
                    markPos = multSepSize;
                else if ((yAxisP + multSepSize) > (clientSize.Width - markDist))//right edge
                    markPos = clientSize.Width - fontWidth - multSepSize - (yAxisP < clientSize.Width ? clientSize.Width - yAxisP : 0);
                else//none
                    markPos = yAxisP + multSepSize;
                return markPos;
            };

            var end = (int)Math.Ceiling((double)clientSize.Width / tileSize.Width) + 1;
            var min = (int)((center.X - offset.Width - clientSize.Width) / tileSize.Width);
            for (int i = -1; i < end; i++)//vertical
            {
                var xPx = i * tileSize.Width + ((offset.Width + center.X) % tileSize.Width);
                g.DrawLine(gridPen, new PointF(xPx, 0), new PointF(xPx, clientSize.Height));

                //x separator
                var x = i + min;
                if (x % SeparateN != 0 || x == 0)
                {
                    g.DrawLine(axisPen, new PointF(xPx, center.Y - sepSize + offset.Height), new PointF(xPx, center.Y + sepSize + offset.Height));
                    continue;
                }
                else
                    g.DrawLine(axisPen, new PointF(xPx, center.Y - multSepSize + offset.Height), new PointF(xPx, center.Y + multSepSize + offset.Height));
                
                //x value mark
                g.DrawLine(separatorGridPen, new PointF(xPx, 0), new PointF(xPx, clientSize.Height));
                var strX = x.ToString();
                var fontSize = g.MeasureString(strX, valuesFont);
                g.DrawString(strX, valuesFont, Brushes.Black, new PointF(xPx - fontSize.Width / 2, getMarkX(fontSize.Height)));
            }
            end = (int)Math.Ceiling((double)clientSize.Height / tileSize.Height) + 1;
            min = (int)((center.Y - offset.Height - clientSize.Height) / tileSize.Height);
            for (int i = -1; i < end; i++)//horizontal
            {
                float yPx = i * tileSize.Height + ((offset.Height + center.Y) % tileSize.Height);
                g.DrawLine(gridPen, new PointF(0, yPx), new PointF(clientSize.Width, yPx));

                //y separator
                var y = -(i + min);
                if (y % SeparateN != 0 || y == 0)
                {
                    g.DrawLine(axisPen, new PointF(center.X - sepSize + offset.Width, yPx), new PointF(center.X + sepSize + offset.Width, yPx));
                    continue;
                }
                else
                    g.DrawLine(axisPen, new PointF(center.X - multSepSize + offset.Width, yPx), new PointF(center.X + multSepSize + offset.Width, yPx));


                //y value mark

                g.DrawLine(separatorGridPen, new PointF(0, yPx), new PointF(clientSize.Width, yPx));
                var strY = y.ToString();
                var fontSize = g.MeasureString(strY, valuesFont);
                g.DrawString(strY, valuesFont, Brushes.Black, new PointF(getMarkY(fontSize.Width), yPx - fontSize.Height / 2));
            }
            
            var beautifulFont = new Font("Times New Roman", 16, FontStyle.Italic);
            //x mark
            {
                markDist -= (multSepSize + 15) - multSepSize;
                multSepSize = multSepSize + 15;

                var strX = "x";
                var fontSize = g.MeasureString(strX, beautifulFont);
                g.DrawString(strX, beautifulFont, Brushes.Black, new PointF(center.X + markDist, getMarkX(fontSize.Height)));
            }
            //y mark
            {
                markDist -= (multSepSize + 10) - multSepSize;
                multSepSize = multSepSize + 10;

                var strY = "y";
                var fontSize = g.MeasureString(strY, beautifulFont);
                g.DrawString(strY, beautifulFont, Brushes.Black, new PointF(getMarkY(fontSize.Width), center.Y - markDist));
            }
        }

        public override string ToString()
        {
            return $"y={Function}";
        }
    }
}
