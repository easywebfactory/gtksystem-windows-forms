using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
            g.DrawEllipse(new Pen(new SolidBrush(Color.Red), 2), 0, 0, 30, 20);
            //g.FillEllipse(new SolidBrush(Color.Red), 40, 25, 30, 20);
            // g.DrawEllipse(new Pen(new SolidBrush(Color.Red), 0), 40, 25, 40, 40);

        }

        private void Form3_Load(object sender, EventArgs e)
        {
           

        }
    }
}
