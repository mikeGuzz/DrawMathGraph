using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DrawMathGraph
{
    public partial class FunctionEditor : Form
    {
        public DrawableFunction Function
        {
            get
            {
                var tmp = new DrawableFunction(f_textBox.Text, bodyColor_pictureBox.BackColor);
                tmp.DashStyle = (DashStyle)dash_comboBox.SelectedIndex;
                return tmp;
            }
        }

        public FunctionEditor()
        {
            InitializeComponent();
            Setup();

            dash_comboBox.SelectedIndex = 0;
            ok_button.Enabled = false;
        }

        public FunctionEditor(DrawableFunction func)
        {
            InitializeComponent();
            Setup();

            dash_comboBox.SelectedIndex = func.DashStyle == DashStyle.Custom ? 0 : (int)func.DashStyle;
            f_textBox.Text = func.Function;
            f_textBox.Select(f_textBox.Text.Length, 0);
            bodyColor_pictureBox.BackColor = func.Color;
        }

        private void Setup()
        {
            var coll = Enum.GetNames(typeof(DashStyle));
            for(int i = 0; i < coll.Length - 1; i++)
            {
                dash_comboBox.Items.Add(coll[i]);
            }

            //tooltips
            var tip = new ToolTip();
            tip.SetToolTip(sin_button, MathFunction.GetFunction("sin").ShortDescription);
            tip.SetToolTip(cos_button, MathFunction.GetFunction("cos").ShortDescription);
            tip.SetToolTip(tan_button, MathFunction.GetFunction("tan").ShortDescription);
            tip.SetToolTip(log_button, MathFunction.GetFunction("log").ShortDescription);
            tip.SetToolTip(ln_button, MathFunction.GetFunction("ln").ShortDescription);
            tip.SetToolTip(abs_button, MathFunction.GetFunction("abs").ShortDescription);
            tip.SetToolTip(tan_button, MathFunction.GetFunction("tan").ShortDescription);
            tip.SetToolTip(sqrt_button, MathFunction.GetFunction("sqrt").ShortDescription);
            tip.SetToolTip(ceil_button, MathFunction.GetFunction("ceil").ShortDescription);
            tip.SetToolTip(floor_button, MathFunction.GetFunction("floor").ShortDescription);
            tip.SetToolTip(pow_button, MathFunction.GetFunction("pow").ShortDescription);
            tip.SetToolTip(pow3_button, "pow(n, 3)");
            tip.SetToolTip(pow2_button, "pow(n, 2)");
            tip.SetToolTip(standart_button, "n*pow(10, 1) - standard number form.");
        }

        private void pickColor_button_Click(object sender, EventArgs e)
        {
            var dialog = new ColorDialog();
            dialog.Color = bodyColor_pictureBox.BackColor;
            dialog.FullOpen = true;
            if(dialog.ShowDialog() == DialogResult.OK)
            {
                bodyColor_pictureBox.BackColor = dialog.Color;
            }
        }

        //num-panel buttons

        private void abs_button_Click(object sender, EventArgs e)
        {
            var start = f_textBox.SelectionStart;
            if (ModifierKeys.HasFlag(Keys.Control))
            {
                f_textBox.Text = f_textBox.Text.Insert(f_textBox.SelectionStart, "abs(x)");
                f_textBox.Select(start + 6, 0);
            }
            else
            {
                f_textBox.Text = f_textBox.Text.Insert(f_textBox.SelectionStart, "abs()");
                f_textBox.Select(start + 4, 0);
            }
            f_textBox.Focus();
        }

        private void log_button_Click(object sender, EventArgs e)
        {
            var start = f_textBox.SelectionStart;
            if (ModifierKeys.HasFlag(Keys.Control))
            {
                f_textBox.Text = f_textBox.Text.Insert(f_textBox.SelectionStart, "log(,x)");
                f_textBox.Select(start + 4, 0);
            }
            else
            {
                f_textBox.Text = f_textBox.Text.Insert(f_textBox.SelectionStart, "log(,)");
                f_textBox.Select(start + 4, 0);
            }
            f_textBox.Focus();
        }

        private void sqrt_button_Click(object sender, EventArgs e)
        {
            var start = f_textBox.SelectionStart;
            if (ModifierKeys.HasFlag(Keys.Control))
            {
                f_textBox.Text = f_textBox.Text.Insert(f_textBox.SelectionStart, "sqrt(x)");
                f_textBox.Select(start + 7, 0);
            }
            else
            {
                f_textBox.Text = f_textBox.Text.Insert(f_textBox.SelectionStart, "sqrt()");
                f_textBox.Select(start + 5, 0);
            }
            f_textBox.Focus();
        }

        private void ln_button_Click(object sender, EventArgs e)
        {
            var start = f_textBox.SelectionStart;
            if (ModifierKeys.HasFlag(Keys.Control))
            {
                f_textBox.Text = f_textBox.Text.Insert(f_textBox.SelectionStart, "ln(x)");
                f_textBox.Select(start + 5, 0);
            }
            else
            {
                f_textBox.Text = f_textBox.Text.Insert(f_textBox.SelectionStart, "ln()");
                f_textBox.Select(start + 3, 0);
            }
            f_textBox.Focus();
        }

        private void pow_button_Click(object sender, EventArgs e)
        {
            var start = f_textBox.SelectionStart;
            if (ModifierKeys.HasFlag(Keys.Control))
            {
                f_textBox.Text = f_textBox.Text.Insert(f_textBox.SelectionStart, "pow(x,)");
                f_textBox.Select(start + 6, 0);
            }
            else
            {
                f_textBox.Text = f_textBox.Text.Insert(f_textBox.SelectionStart, "pow(,)");
                f_textBox.Select(start + 4, 0);
            }
            f_textBox.Focus();
        }

        private void tan_button_Click(object sender, EventArgs e)
        {
            var start = f_textBox.SelectionStart;
            if (ModifierKeys.HasFlag(Keys.Control))
            {
                f_textBox.Text = f_textBox.Text.Insert(f_textBox.SelectionStart, "tan(x)");
                f_textBox.Select(start + 6, 0);
            }
            else
            {
                f_textBox.Text = f_textBox.Text.Insert(f_textBox.SelectionStart, "tan()");
                f_textBox.Select(start + 4, 0);
            }
            f_textBox.Focus();
        }

        private void sin_button_Click(object sender, EventArgs e)
        {
            var start = f_textBox.SelectionStart;
            if (ModifierKeys.HasFlag(Keys.Control))
            {
                f_textBox.Text = f_textBox.Text.Insert(f_textBox.SelectionStart, "sin(x)");
                f_textBox.Select(start + 6, 0);
            }
            else
            {
                f_textBox.Text = f_textBox.Text.Insert(f_textBox.SelectionStart, "sin()");
                f_textBox.Select(start + 4, 0);
            }
            f_textBox.Focus();
        }

        private void cos_button_Click(object sender, EventArgs e)
        {
            var start = f_textBox.SelectionStart;
            if (ModifierKeys.HasFlag(Keys.Control))
            {
                f_textBox.Text = f_textBox.Text.Insert(f_textBox.SelectionStart, "cos(x)");
                f_textBox.Select(start + 6, 0);
            }
            else
            {
                f_textBox.Text = f_textBox.Text.Insert(f_textBox.SelectionStart, "cos()");
                f_textBox.Select(start + 4, 0);
            }
            f_textBox.Focus();
        }

        private void ceil_button_Click(object sender, EventArgs e)
        {
            var start = f_textBox.SelectionStart;
            if (ModifierKeys.HasFlag(Keys.Control))
            {
                f_textBox.Text = f_textBox.Text.Insert(f_textBox.SelectionStart, "ceil(x)");
                f_textBox.Select(start + 7, 0);
            }
            else
            {
                f_textBox.Text = f_textBox.Text.Insert(f_textBox.SelectionStart, "ceil()");
                f_textBox.Select(start + 5, 0);
            }
            f_textBox.Focus();
        }

        private void floor_button_Click(object sender, EventArgs e)
        {
            var start = f_textBox.SelectionStart;
            if (ModifierKeys.HasFlag(Keys.Control))
            {
                f_textBox.Text = f_textBox.Text.Insert(f_textBox.SelectionStart, "floor(x)");
                f_textBox.Select(start + 8, 0);
            }
            else
            {
                f_textBox.Text = f_textBox.Text.Insert(f_textBox.SelectionStart, "floor()");
                f_textBox.Select(start + 6, 0);
            }
            f_textBox.Focus();
        }

        private void mult_button_Click(object sender, EventArgs e)
        {
            var start = f_textBox.SelectionStart;
            if (ModifierKeys.HasFlag(Keys.Control))
            {
                f_textBox.Text = f_textBox.Text.Insert(f_textBox.SelectionStart, "()()");
                f_textBox.Select(start + 1, 0);
            }
            else
            {
                f_textBox.Text = f_textBox.Text.Insert(f_textBox.SelectionStart, "*");
                f_textBox.Select(start + 1, 0);
            }
            f_textBox.Focus();
        }

        private void div_button_Click(object sender, EventArgs e)
        {
            var start = f_textBox.SelectionStart;
            if (ModifierKeys.HasFlag(Keys.Control))
            {
                f_textBox.Text = f_textBox.Text.Insert(f_textBox.SelectionStart, "()/()");
                f_textBox.Select(start + 1, 0);
            }
            else
            {
                f_textBox.Text = f_textBox.Text.Insert(f_textBox.SelectionStart, "/");
                f_textBox.Select(start + 1, 0);
            }
            f_textBox.Focus();
        }

        private void plus_button_Click(object sender, EventArgs e)
        {
            var start = f_textBox.SelectionStart;
            if (ModifierKeys.HasFlag(Keys.Control))
            {
                f_textBox.Text = f_textBox.Text.Insert(f_textBox.SelectionStart, "()+()");
                f_textBox.Select(start + 1, 0);
            }
            else
            {
                f_textBox.Text = f_textBox.Text.Insert(f_textBox.SelectionStart, "+");
                f_textBox.Select(start + 1, 0);
            }
            f_textBox.Focus();
        }

        private void min_button_Click(object sender, EventArgs e)
        {
            var start = f_textBox.SelectionStart;
            if (ModifierKeys.HasFlag(Keys.Control))
            {
                f_textBox.Text = f_textBox.Text.Insert(f_textBox.SelectionStart, "()-()");
                f_textBox.Select(start + 1, 0);
            }
            else
            {
                f_textBox.Text = f_textBox.Text.Insert(f_textBox.SelectionStart, "-");
                f_textBox.Select(start + 1, 0);
            }
            f_textBox.Focus();
        }

        private void brackets_button_Click(object sender, EventArgs e)
        {
            var start = f_textBox.SelectionStart;
            f_textBox.Text = f_textBox.Text.Insert(f_textBox.SelectionStart, "()");
            f_textBox.Select(start + 1, 0);
            f_textBox.Focus();
        }

        private void pow3_button_Click(object sender, EventArgs e)
        {
            var start = f_textBox.SelectionStart;
            if (ModifierKeys.HasFlag(Keys.Control))
            {
                f_textBox.Text = f_textBox.Text.Insert(f_textBox.SelectionStart, "pow(x,3)");
                f_textBox.Select(start + 8, 0);
            }
            else
            {
                f_textBox.Text = f_textBox.Text.Insert(f_textBox.SelectionStart, "pow(,3)");
                f_textBox.Select(start + 4, 0);
            }
            f_textBox.Focus();
        }

        private void pow2_button_Click(object sender, EventArgs e)
        {
            var start = f_textBox.SelectionStart;
            if (ModifierKeys.HasFlag(Keys.Control))
            {
                f_textBox.Text = f_textBox.Text.Insert(f_textBox.SelectionStart, "pow(x,2)");
                f_textBox.Select(start + 8, 0);
            }
            else
            {
                f_textBox.Text = f_textBox.Text.Insert(f_textBox.SelectionStart, "pow(,2)");
                f_textBox.Select(start + 4, 0);
            }
            f_textBox.Focus();
        }

        private void powMin1_button_Click(object sender, EventArgs e)
        {
            var start = f_textBox.SelectionStart;
            if (ModifierKeys.HasFlag(Keys.Control))
            {
                f_textBox.Text = f_textBox.Text.Insert(f_textBox.SelectionStart, "pow(x,-1)");
                f_textBox.Select(start + 9, 0);
            }
            else
            {
                f_textBox.Text = f_textBox.Text.Insert(f_textBox.SelectionStart, "pow(,-1)");
                f_textBox.Select(start + 4, 0);
            }
            f_textBox.Focus();
        }

        private void lBracket_button_Click(object sender, EventArgs e)
        {
            var start = f_textBox.SelectionStart;
            f_textBox.Text = f_textBox.Text.Insert(f_textBox.SelectionStart, "(");
            f_textBox.Select(start + 1, 0);
            f_textBox.Focus();
        }

        private void rBracket_button_Click(object sender, EventArgs e)
        {
            var start = f_textBox.SelectionStart;
            f_textBox.Text = f_textBox.Text.Insert(f_textBox.SelectionStart, ")");
            f_textBox.Select(start + 1, 0);
            f_textBox.Focus();
        }

        private void enum9_button_Click(object sender, EventArgs e)
        {
            var start = f_textBox.SelectionStart;
            f_textBox.Text = f_textBox.Text.Insert(f_textBox.SelectionStart, "9");
            f_textBox.Select(start + 1, 0);
            f_textBox.Focus();
        }

        private void enum8_button_Click(object sender, EventArgs e)
        {
            var start = f_textBox.SelectionStart;
            f_textBox.Text = f_textBox.Text.Insert(f_textBox.SelectionStart, "8");
            f_textBox.Select(start + 1, 0);
            f_textBox.Focus();
        }

        private void enum7_button_Click(object sender, EventArgs e)
        {
            var start = f_textBox.SelectionStart;
            f_textBox.Text = f_textBox.Text.Insert(f_textBox.SelectionStart, "7");
            f_textBox.Select(start + 1, 0);
            f_textBox.Focus();
        }

        private void enum6_button_Click(object sender, EventArgs e)
        {
            var start = f_textBox.SelectionStart;
            f_textBox.Text = f_textBox.Text.Insert(f_textBox.SelectionStart, "6");
            f_textBox.Select(start + 1, 0);
            f_textBox.Focus();
        }

        private void enum5_button_Click(object sender, EventArgs e)
        {
            var start = f_textBox.SelectionStart;
            f_textBox.Text = f_textBox.Text.Insert(f_textBox.SelectionStart, "5");
            f_textBox.Select(start + 1, 0);
            f_textBox.Focus();
        }

        private void enum4_button_Click(object sender, EventArgs e)
        {
            var start = f_textBox.SelectionStart;
            f_textBox.Text = f_textBox.Text.Insert(f_textBox.SelectionStart, "4");
            f_textBox.Select(start + 1, 0);
            f_textBox.Focus();
        }

        private void enum3_button_Click(object sender, EventArgs e)
        {
            var start = f_textBox.SelectionStart;
            f_textBox.Text = f_textBox.Text.Insert(f_textBox.SelectionStart, "3");
            f_textBox.Select(start + 1, 0);
            f_textBox.Focus();
        }

        private void enum2_button_Click(object sender, EventArgs e)
        {
            var start = f_textBox.SelectionStart;
            f_textBox.Text = f_textBox.Text.Insert(f_textBox.SelectionStart, "2");
            f_textBox.Select(start + 1, 0);
            f_textBox.Focus();
        }

        private void enum1_button_Click(object sender, EventArgs e)
        {
            var start = f_textBox.SelectionStart;
            f_textBox.Text = f_textBox.Text.Insert(f_textBox.SelectionStart, "1");
            f_textBox.Select(start + 1, 0);
            f_textBox.Focus();
        }

        private void enum0_button_Click(object sender, EventArgs e)
        {
            var start = f_textBox.SelectionStart;
            f_textBox.Text = f_textBox.Text.Insert(f_textBox.SelectionStart, "0");
            f_textBox.Select(start + 1, 0);
            f_textBox.Focus();
        }

        private void point_button_Click(object sender, EventArgs e)
        {
            var start = f_textBox.SelectionStart;
            f_textBox.Text = f_textBox.Text.Insert(f_textBox.SelectionStart, ".");
            f_textBox.Select(start - Convert.ToInt32(ModifierKeys.HasFlag(Keys.Control)), 0);
            f_textBox.Focus();
        }

        private void standart_button_Click(object sender, EventArgs e)
        {
            var start = f_textBox.SelectionStart;
            if (ModifierKeys.HasFlag(Keys.Control))
            {
                f_textBox.Text = f_textBox.Text.Insert(f_textBox.SelectionStart, "(*pow(10,1))");
                f_textBox.Select(start + 1, 0);
            }
            else
            {
                f_textBox.Text = f_textBox.Text.Insert(f_textBox.SelectionStart, "*pow(10,)");
                f_textBox.Select(start + 8, 0);
            }
            f_textBox.Focus();
        }

        private void x_button_Click(object sender, EventArgs e)
        {
            var start = f_textBox.SelectionStart;
            f_textBox.Text = f_textBox.Text.Insert(f_textBox.SelectionStart, "x");
            f_textBox.Select(start + 1, 0);
            f_textBox.Focus();
        }

        private void pi_button_Click(object sender, EventArgs e)
        {

        }

        private void e_button_Click(object sender, EventArgs e)
        {

        }

        private void f_textBox_TextChanged(object sender, EventArgs e)
        {
            ok_button.Enabled = !string.IsNullOrEmpty(f_textBox.Text);
        }

        private void ok_button_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
