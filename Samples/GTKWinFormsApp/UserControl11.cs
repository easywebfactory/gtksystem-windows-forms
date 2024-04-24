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

namespace GTKWinFormsApp
{

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
    }
}
