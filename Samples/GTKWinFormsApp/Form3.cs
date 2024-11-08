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

            this.Shown += Form3_Shown;
        }

        private void Form3_Shown(object? sender, EventArgs e)
        {

            Image m = new Bitmap(500, 300);
            // Graphics g = Graphics.FromImage(m);
            // g.Clear(Color.Red);
            // g.DrawRectangle(new Pen(Color.Blue), new Rectangle(60, 60, 200, 100));
            //// g.Dispose();
            // panel1.Controls.Add(new Gtk.Image(m.Pixbuf));
            //using (Graphics g = Graphics.FromImage(m))
            //{
            //   // m.Pixbuf = new Gdk.Pixbuf(Graphics.surface, 0, 0, 500, 300);
            //    g.Clear(Color.White);
            //    g.DrawString(DateTime.Now.ToString(), new Font(FontFamily.GenericSansSerif, 16), new SolidBrush(Color.Red), 200, 200);
            //    g.DrawRectangle(new Pen((Color)Color.Red, 5), new Rectangle(0, 0, 200, 200));
            //    // g.Dispose();

                 
            //   // panel1.Controls.Add(m);
            //   // panel1.Show();
            //    panel1.Refresh();
            //}
        }

        private void Form3_SizeChanged(object? sender, EventArgs e)
        {
            panel1.Refresh();
            //Console.WriteLine(Width);
            // panel1.Refresh();
            this.Refresh();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //button1.ForeColor=Color.Red;
            //button1.BackColor=Color.Green;
            Form1 f = new Form1();
            f.Show();
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

            //using (Graphics g = CreateGraphics())
            //{
            //    g.DrawString(DateTime.Now.ToString(), new Font(FontFamily.GenericSansSerif, 16), new SolidBrush(Color.Red), 200, 200);
            //    g.DrawRectangle(new Pen((Color)Color.Red, 5), new Rectangle(110, 110, 200, 200));
            //    // g.Dispose();
            //    this.Refresh();
            //}


            Image m = new Bitmap(200, 200);
            using (Graphics gg = Graphics.FromImage(m))
            {
                //gg.Clear(Color.White);
                gg.DrawString(DateTime.Now.ToString(), new Font(FontFamily.GenericSansSerif, 16), new SolidBrush(Color.Red), 20, 120);
                gg.DrawRectangle(new Pen((Color)Color.Red, 5), new Rectangle(10, 10, 100, 100));
                gg.Flush();
            }
            PictureBox pic = new PictureBox();
            pic.Location = new Point(20, 50);
            pic.Width = 300;
            pic.Height = 200;
            pic.Image = m;
            //pic.Dock = DockStyle.Right;
            pic.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            panel1.Controls.Add(pic);
            panel1.Show();
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
