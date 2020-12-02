using System;
using System.Collections.Generic;
using System.ComponentModel;
using System;
using KGySoft.Drawing;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace OBS_Supporter
{
    public partial class OBS_StyleTextBox : UserControl
    {
        private TextBox textBox = new TextBox();

        public OBS_StyleTextBox()
        {
            InitializeComponent();
            // 
            // tbxObsPath
            // 
            textBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            textBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(30)))), ((int)(((byte)(31)))));
            textBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            textBox.ForeColor = System.Drawing.SystemColors.ControlLight;
            //textBox.Location = new System.Drawing.Point(78, 11);
            textBox.Name = "tbxObsPath";
            textBox.Size = new System.Drawing.Size(532, 13);
            textBox.TabIndex = 1;


            GraphicsPath path = new GraphicsPath();
            path.AddRectangle(new Rectangle(textBox.Location, textBox.Size));

            Rectangle arc = new Rectangle(textBox.Location, new Size(532, 45));
            int diameter = 20 * 2;

            //// top left arc  
            //path.AddArc(arc, 180, 90);

            //// top right arc  
            //arc.X = arc.Right - diameter;
            //path.AddArc(arc, 270, 90);

            //// bottom right arc  
            //arc.Y = arc.Bottom - diameter;
            //path.AddArc(arc, 0, 90);

            //// bottom left arc 
            //arc.X = arc.Left;
            //path.AddArc(arc, 90, 90);

            path.CloseFigure();

            //Graphics g = this.CreateGraphics();
            
           // g.DrawRectangle(Pens.White, new Rectangle(10, 40 , 50, 50));

            Controls.Add(textBox);

            GraphicsExtensions.DrawRoundedRectangle(textBox.CreateGraphics(), new Pen(Color.White, 5), new Rectangle(10, 40, 50, 50), 5);
            
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public override string Text
        {
            get { return textBox.Text; }
            set { textBox.Text = value; }
        }
    }
}
