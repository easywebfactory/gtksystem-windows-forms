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
            //目前自定义控件无法在窗体设计器中可视化，建议使用程序添加，如下示例：
            UserControl11 userControl11 = new UserControl11();
            panel5.Controls.Add(userControl11);
            this.SizeChanged += Form3_SizeChanged;
        }

        private void Form3_SizeChanged(object? sender, EventArgs e)
        {
            //Console.WriteLine(Width);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //button1.ForeColor=Color.Red;
            //button1.BackColor=Color.Green;
            Form1 f = new Form1();
            f.Show(this);
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            label1.Text = trackBar1.Value.ToString();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            var result = this.BeginInvoke(new MethodInvoker(() =>
            {
                System.Threading.Thread.Sleep(3000);
                for (int i = 0; i < 100; i++)
                {
                    progressBar1.Invoke(() =>
                    {
                        progressBar1.Value = i;
                    });
                    System.Threading.Thread.Sleep(20);
                }
            }));
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;

            GraphicsPath path = new GraphicsPath();
            path.AddEllipse(90, 25, 40, 20);

            path.StartFigure();
            path.AddArc(5, 5, 6, 6, 180, 90);
            path.AddArc(20, 5, 6, 6, 270, 90);
            path.AddArc(20, 18, 6, 6, 0, 90);
            path.AddArc(5, 18, 6, 6, 90, 90);
            path.CloseFigure();

            path.StartFigure();

            path.AddLine(new Point(20, 40), new Point(60, 30));
            path.AddPie(-10, 20, 60, 60, 10, 100);
            //path.StartFigure();
            path.AddBezier(new Point(60, 70), new Point(80, 120), new Point(110, 20), new Point(160, 80));
            path.AddRectangle(new Rectangle(30, 10, 190, 30));
            //path.CloseFigure();


            path.AddString("test文本", new FontFamily(GenericFontFamilies.Serif), (int)FontStyle.Italic, 16, new Point(1, 1), new StringFormat(StringFormatFlags.NoWrap));

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

        private void ssssToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Form2 f1 = new Form2();
            f1.Show(this);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form4 f = new Form4();
            f.ShowDialog();
        }

        private void panel5_Scroll(object sender, System.Windows.Forms.ScrollEventArgs e)
        {
            Console.WriteLine($"panel5_Scroll:{e.OldValue},{e.NewValue};{e.ScrollOrientation}");
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            label1.Text = hScrollBar1.Value.ToString();
        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            label1.Text = vScrollBar1.Value.ToString();
        }
    }
}
