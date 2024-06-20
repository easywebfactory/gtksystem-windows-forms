using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace GTKWinFormsApp
{
    [Designer(typeof(ControlDesigner))]
    public partial class UserControl11 : UserControl
    {
        public UserControl11()
        {
            InitializeComponent();
        }

        private void UserControl11_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.DrawEllipse(new Pen(new SolidBrush(Color.Red), 2), 0, 0, 30, 20);
            //g.FillEllipse(new SolidBrush(Color.Red), 40, 25, 30, 20);
        }

        private void UserControl11_ParentChanged(object sender, EventArgs e)
        {
            MessageBox.Show("sss");
        }

        private void UserControl11_Load(object sender, EventArgs e)
        {
          
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {

            //using SolidBrush brush = new SolidBrush(_backgroundColor);
            //e.Graphics.FillRectangle(brush, new Rectangle(0, 0, Width, Height));
            //using SolidBrush brush2 = new SolidBrush(Color.Red);
            //e.Graphics.DrawEllipse(new Pen(brush2), new Rectangle(30, 30, Width, Height));
            base.OnPaint(e);
        }

        private void UserControl11_MouseEnter(object sender, EventArgs e)
        {
            Console.WriteLine("UserControl11_MouseEnter");
        }

        private void UserControl11_MouseLeave(object sender, EventArgs e)
        {
            Console.WriteLine("UserControl11_MouseLeave");
        }

        private void UserControl11_MouseMove(object sender, MouseEventArgs e)
        {
            Console.WriteLine("UserControl11_MouseMove");
        }

        private void UserControl11_MouseUp(object sender, MouseEventArgs e)
        {
            Console.WriteLine("UserControl11_MouseUp");
        }

        private void UserControl11_MouseHover(object sender, EventArgs e)
        {
            Console.WriteLine("UserControl11_MouseHover");
        }
    }
}
