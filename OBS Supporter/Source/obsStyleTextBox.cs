using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace OBS_Supporter
{
    public partial class obsStyleTextBox : UserControl
    {
        public obsStyleTextBox()
        {
            InitializeComponent();
        }

        GraphicsPath GetRoundPath(RectangleF Rect, int radius)
        {
            float r2 = radius / 2f;
            GraphicsPath GraphPath = new GraphicsPath();

            GraphPath.AddArc(Rect.X, Rect.Y, radius, radius, 180, 90);
            // GraphPath.AddLine(Rect.X + r2, Rect.Y, Rect.Width - r2, Rect.Y);

            GraphPath.AddArc(Rect.X + Rect.Width - radius, Rect.Y, radius, radius, 270, 90);
            //GraphPath.AddLine(Rect.Width, Rect.Y + r2, Rect.Width, Rect.Height - r2);

            GraphPath.AddArc(Rect.X + Rect.Width - radius, Rect.Y + Rect.Height - radius, radius, radius, 0, 90);
            //GraphPath.AddLine(Rect.Width - r2, Rect.Height, Rect.X + r2, Rect.Height);

            GraphPath.AddArc(Rect.X, Rect.Y + Rect.Height - radius, radius, radius, 90, 90);
            //GraphPath.AddLine(Rect.X, Rect.Height - r2, Rect.X, Rect.Y + r2);

            GraphPath.CloseFigure();
            return GraphPath;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            SolidBrush myBrush = new SolidBrush(Color.FromArgb(31, 30, 31));
            Graphics formGraphics;
            formGraphics = CreateGraphics();
            RectangleF Rect = new RectangleF(tbx.Location.X - 8, tbx.Location.Y - 3, tbx.Width + 9, tbx.Height + 6);
            formGraphics.FillPath(myBrush, GetRoundPath(Rect, 10));
            myBrush.Dispose();
            formGraphics.Dispose(); 
        }

        public void setText(string text)
        {
            tbx.Text = text;
        }

        public string getText()
        {
            return tbx.Text;
        }

        private void tbx_TextChanged(object sender, EventArgs e)
        {
            OnTextChanged(e);
        }
    }
}
