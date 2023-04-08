using System.Data;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Text;
using System.Timers;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Linq;

namespace DrawMathGraph
{
    public enum ToolType { Selection, Move, Zoom };

    public partial class Form1 : Form
    {
        private bool resize;

        private SizeF offset = new Size(-0, 0);
        private double stepX = 0.05d;
        private SizeF tileSize = new SizeF(15, 15);

        private ToolType toolType = ToolType.Move;
        private Point startSelection;
        private SizeF startOffset;
        private bool inToolMode;

        private double zoom = 1d;
        private const double zoomPch = 0.35d;
        private double maxZoom = 5d;
        private double minZoom = 0.35d;
        private readonly double zoomIncreaser, zoomDecreaser;
        private bool CanIncreaseZoom => zoomIncreaser * zoom <= maxZoom;
        private bool CanDecreaseZoom => zoomDecreaser * zoom >= minZoom;

        public Form1()
        {
            InitializeComponent();
            Setup();

            zoomIncreaser = 1d + zoomPch;
            zoomDecreaser = 1d - zoomPch;
        }

        private void Setup()
        {
            KeyPreview = true;

            int index = 0;
            foreach(var ob in Enum.GetNames(typeof(ToolType)))
            {
                ToolStripMenuItem item = (ToolStripMenuItem)toolsToolStripMenuItem.DropDownItems.Add(ob);
                var type = (ToolType)index++;
                item.Tag = type;
                item.Click += ToolMenuItemClick;
                if (type == toolType)
                    item.CheckState = CheckState.Indeterminate;
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;

            var sclaedTileSize = new SizeF((float)(tileSize.Width * zoom), (float)(tileSize.Height * zoom));

            DrawableFunction.DrawGraphBackground(g, sclaedTileSize, pictureBox1.ClientSize, offset);

            if (!resize && checkedListBox1.CheckedItems.Count != 0)
            {
                foreach (DrawableFunction ob in checkedListBox1.CheckedItems)
                {
                    ob.Draw(g, stepX, sclaedTileSize, pictureBox1.ClientSize, offset);
                }
            }
            //sqrt(cos(x))*cos(200x)+sqrt(abs(x))-(3.14/4)*pow((2-pow(x, 2)), 0.01)
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (!e.Button.HasFlag(MouseButtons.Left))
                return;
            inToolMode = true;
            startSelection = e.Location;
            switch (toolType)
            {
                case ToolType.Move:
                    startOffset = offset;
                    break;
                case ToolType.Zoom:
                    Cursor = Cursors.Cross;
                    break;
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (!e.Button.HasFlag(MouseButtons.Left))
                return;
            switch (toolType)
            {
                case ToolType.Move:
                    Cursor = Cursors.SizeAll;
                    offset = new SizeF(startOffset.Width + (e.X - startSelection.X), startOffset.Height + (e.Y - startSelection.Y));
                    pictureBox1.Invalidate();
                    break;
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (!e.Button.HasFlag(MouseButtons.Left))
                return;
            inToolMode = false;
            switch (toolType)
            {
                case ToolType.Move:
                    Cursor = Cursors.Default;
                    if (!ModifierKeys.HasFlag(Keys.Control))
                        break;
                    offset = new SizeF((float)pictureBox1.ClientSize.Width / 2 + offset.Width - e.X, (float)pictureBox1.ClientSize.Height / 2 + offset.Height - e.Y);
                    pictureBox1.Invalidate();
                    break;
                case ToolType.Zoom:
                    Cursor = Cursors.Default;
                    if (ModifierKeys.HasFlag(Keys.Control))
                    {
                        if (!CanDecreaseZoom)
                            return;
                        zoom *= zoomDecreaser;
                        offset = new SizeF((float)((pictureBox1.ClientSize.Width / 2 + offset.Width - e.X) * zoomDecreaser), (float)((pictureBox1.ClientSize.Height / 2 + offset.Height - e.Y) * zoomDecreaser));
                        pictureBox1.Invalidate();
                    }
                    else
                    {
                        if (!CanIncreaseZoom)
                            return;
                        zoom *= zoomIncreaser;
                        offset = new SizeF((float)((pictureBox1.ClientSize.Width / 2 + offset.Width - e.X) * zoomIncreaser), (float)((pictureBox1.ClientSize.Height / 2 + offset.Height - e.Y) * zoomIncreaser));
                        pictureBox1.Invalidate();
                    }
                    break;
            }
        }

        private void ToolMenuItemClick(object? sender, EventArgs e)
        {
            if(sender is ToolStripMenuItem item && item.Tag is ToolType type)
            {
                toolType = type;
                foreach (ToolStripMenuItem ob in toolsToolStripMenuItem.DropDownItems)
                    ob.CheckState = CheckState.Unchecked;
                item.CheckState = CheckState.Indeterminate;
            }
        }

        private void Form1_ResizeBegin(object sender, EventArgs e)
        {
            resize = true;
        }

        private void Form1_ResizeEnd(object sender, EventArgs e)
        {
            resize = false;
            pictureBox1.Invalidate();
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            pictureBox1.Invalidate();
        }

        private void zoomInToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CanIncreaseZoom)
                zoom *= zoomIncreaser;
            else return;
            offset = new SizeF((float)(offset.Width * zoomIncreaser), (float)(offset.Height * zoomIncreaser));
            pictureBox1.Invalidate();
        }

        private void zoomOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CanDecreaseZoom)
                zoom *= zoomDecreaser;
            else return;
            offset = new SizeF((float)(offset.Width * zoomDecreaser), (float)(offset.Height * zoomDecreaser));
            pictureBox1.Invalidate();
        }

        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (inToolMode)
                return;
            zoom = 1;
            pictureBox1.Invalidate();
        }

        private void add_button_Click(object sender, EventArgs e)
        {
            var dialog = new FunctionEditor();
            if(dialog.ShowDialog() == DialogResult.OK)
            {
                checkedListBox1.Items.Add(dialog.Function, true);
                pictureBox1.Invalidate();
            }
        }

        private void edit_button_Click(object sender, EventArgs e)
        {
            if (checkedListBox1.SelectedIndex == ListBox.NoMatches)
                return;
            var dialog = new FunctionEditor((DrawableFunction)checkedListBox1.SelectedItem);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                checkedListBox1.Items[checkedListBox1.SelectedIndex] = dialog.Function;
                pictureBox1.Invalidate();
            }
        }
        
        private void remove_button_Click(object sender, EventArgs e)
        {
            if (checkedListBox1.Items.Count > 0 && ModifierKeys.HasFlag(Keys.Control)
                && MessageBox.Show($"{checkedListBox1.Items.Count} list items will be permanently deleted.", ProductName, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                checkedListBox1.Items.Clear();
                pictureBox1.Invalidate();
                return;
            }
            if (checkedListBox1.SelectedIndex == ListBox.NoMatches)
                return;
            checkedListBox1.Items.RemoveAt(checkedListBox1.SelectedIndex);
            pictureBox1.Invalidate();
        }

        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            pictureBox1.Invalidate();
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            edit_button.Enabled = checkedListBox1.SelectedIndex != ListBox.NoMatches;
        }

        private void resetToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (inToolMode)
                return;
            offset = Size.Empty;
            pictureBox1.Invalidate();
        }
    }
}