
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
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

            UserControl11 userControl11 = new UserControl11();
            //userControl11.Location = new Point(300, 300);
            //userControl11.Size = new Size(200,500);
            panel5.Controls.Add(userControl11);
            // Controls.Add(userControl11);
            this.SizeChanged += Form3_SizeChanged;

            this.Shown += Form3_Shown;
            panel1.Click += Panel1_Click;
            panel1.DoubleClick += Panel1_DoubleClick;
            button5.Click += Button5_Click;


            toolStripMenuItem2.Click += ToolStripMenuItem2_Click;
            toolStripMenuItem7.Click += ToolStripMenuItem7_Click;
            toolStripMenuItem8.Click += ToolStripMenuItem8_Click;

            this.FormClosing += Form3_FormClosing;

            this.Load += Form3_Load;

            ddddToolStripMenuItem1.Click += DdddToolStripMenuItem1_Click;
        }

        private void DdddToolStripMenuItem1_Click(object? sender, EventArgs e)
        {
            var p = sender as ToolStripMenuItem;

        }

        private void Button5_Click(object? sender, EventArgs e)
        {
            Test1 test1 = new Test1();
            test1.Show();
        }

        private void Form3_FormClosing(object sender, FormClosingEventArgs e)
        {
            //this.Hide();
            //this.Visible = false;
            //e.Cancel = true;

        }

        private void ToolStripMenuItem8_Click(object? sender, EventArgs e)
        {
            this.ShowInTaskbar = false;
            //this.Visible = false;
            this.Hide();
        }

        private void ToolStripMenuItem7_Click(object? sender, EventArgs e)
        {
            this.Close();
        }

        private void ToolStripMenuItem2_Click(object? sender, EventArgs e)
        {
            notifyIcon1.ShowBalloonTip(10000, "C# 桌面应用程序跨平台界面框架", "一次编译，跨平台运行，支持Windows、Linux、MacOS \n便于开发跨平台winform软件，便于将C# winform升级为跨平台软件", ToolTipIcon.Warning);
            this.ShowInTaskbar = true;
            this.WindowState = FormWindowState.Normal;
            this.Show();
        }

        NotifyIcon notifyIcon;
        private void NotifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            notifyIcon.ShowBalloonTip(20000);
        }

        private void Panel1_DoubleClick(object? sender, EventArgs e)
        {
            Console.WriteLine("Panel1_DoubleClick");
        }

        private void Panel1_Click(object? sender, EventArgs e)
        {
            Console.WriteLine("Panel1_Click");
            Graphics g1 = panel5.CreateGraphics();
            g1.DrawRectangle(new Pen(Brushes.Red, 2), new Rectangle(10, 10, 200, 60));
            g1.FillRectangle(Brushes.AliceBlue, new RectangleF(10, 10, 200, 60));
            g1.Dispose();
            panel5.Refresh();
        }

        private void Form3_Shown(object sender, EventArgs e)
        {

            // SwitchBox switchBox = new SwitchBox();
            //switchBox.Location = new Point(100, 100);
            //panel1.Controls.Add(switchBox);
        }

        private void Form3_SizeChanged(object sender, EventArgs e)
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
                System.Threading.Thread.Sleep(1000);
                for (int i = 1; i < 101; i++)
                {
                    progressBar1.Invoke(new MethodInvoker(() =>
                    {
                        progressBar1.Value = i;
                    }));
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
            Image image = new Bitmap(500,300);
            Graphics gg = Graphics.FromImage(image);
            gg.DrawString("test文本gggggggg", new Font(GenericFontFamilies.Serif.ToString(), 20), new SolidBrush(Color.Red), new PointF(10, 50));
            gg.Flush();

            g.DrawImageUnscaled(image, 0, 0);
        }

        private void ssssToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Form2 f1 = new Form2();
            DialogResult res = f1.ShowDialog(this);
            Console.WriteLine(res);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form4 f = new Form4();
            f.Show(this);
        }

        private void panel5_Scroll(object sender, System.Windows.Forms.ScrollEventArgs e)
        {
            Console.WriteLine($"panel5_Scroll:{e.OldValue},{e.NewValue};{e.ScrollOrientation}");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //打印
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form2 f1 = new Form2();
            f1.ShowDialog(this);
        }

        private void toolStripSplitButton1_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("ddddddd");
            Console.WriteLine("toolStripSplitButton1_Click");
        }

        private void toolStripSplitButton1_DoubleClick(object sender, EventArgs e)
        {
            Console.WriteLine("toolStripSplitButton1_DoubleClick");
            //MessageBox.Show("toolStripSplitButton1_DoubleClick");
        }

        private void toolStripSplitButton1_MouseUp(object sender, MouseEventArgs e)
        {
            //MessageBox.Show("toolStripSplitButton1_MouseUp");
        }
    }
}
