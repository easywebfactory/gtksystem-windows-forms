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

namespace GTKWinFormsApp
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 f = new Form1();
            f.ShowDialog();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            label1.Text = trackBar1.Value.ToString();
        }

        private void button1_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
             //g.DrawEllipse(new Pen(new SolidBrush(Color.Red), 2), 0, 0, 30, 20);
            //g.FillEllipse(new SolidBrush(Color.Red), 40, 25, 30, 20);
            // g.DrawEllipse(new Pen(new SolidBrush(Color.Red), 0), 40, 25, 40, 40);

            GraphicsPath path = new GraphicsPath();
            path.AddEllipse(40, 25, 40, 20);

            path.StartFigure();
            path.AddArc(5, 5, 6, 6, 180, 90);
            path.AddArc(20, 5, 6, 6, 270, 90);
            path.AddArc(20, 18, 6, 6, 0, 90);
            path.AddArc(5, 18, 6, 6, 90, 90);
            path.CloseFigure();

            //path.StartFigure();
            path.AddPie(-10, 20, 60, 60, 10, 100);
            path.AddLine(new Point(20, 20), new Point(60, 30));

            //path.StartFigure();
            path.AddBezier(new Point(40, 40), new Point(50, 90), new Point(30, -20), new Point(100, 30));
            path.AddRectangle(new Rectangle(30, 10, 190, 30));
            //path.CloseFigure();


            path.AddString("test文本", new FontFamily(GenericFontFamilies.Serif), (int)FontStyle.Italic, 16, new Point(10, 10), new StringFormat(StringFormatFlags.NoWrap));

            //path.CloseAllFigures();

            LinearGradientBrush brush = new LinearGradientBrush(new Point(0, 0), new Point(100, 30), Color.Red, Color.Blue);
            //g.TranslateTransform(30, 0);
            //g.RotateTransform(20);
            g.DrawPath(new Pen(brush, 2), path);

            //PathGradientBrush gradientBrush = new PathGradientBrush(path);
            //gradientBrush.CenterColor = Color.Red;
            //gradientBrush.SurroundColors = new Color[] { Color.Yellow, Color.Blue };
            //g.DrawPath(new Pen(gradientBrush, 2), path);

            //g.FillPath(brush, path);
        }

        private void Form3_Load(object sender, EventArgs e)
        {
           

        }
    }
}
