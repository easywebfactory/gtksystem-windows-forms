using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GTKSystemWinFormsApp11.Properties;

namespace GTKSystemWinFormsApp11
{
    public partial class GtkForm : Form
    {
        public GtkForm()
        {
            InitializeComponent();
            toolStripMenuItem1.Text = string.Format("{0} 1",Resources.GtkForm_InitializeComponent_Menu);
            ddddToolStripMenuItem.Text = string.Format("a {0}", Resources.GtkForm_InitializeComponent_Menu);
            ssssToolStripMenuItem.Text = string.Format("b {0}", Resources.GtkForm_InitializeComponent_Menu);
            bb菜单ToolStripMenuItem.Text = string.Format("bb {0}", Resources.GtkForm_InitializeComponent_Menu);
            bb菜单2ToolStripMenuItem.Text = string.Format("bb {0} 2", Resources.GtkForm_InitializeComponent_Menu);
            ssssToolStripMenuItem1.Text = string.Format("{0} 2", Resources.GtkForm_InitializeComponent_Menu);
            toolStripDropDownButton1.Text = Resources.GtkForm_GtkForm_dropdown_list_1;
            memnuToolStripMenuItem.Text = string.Format("{0} 1", Resources.GtkForm_InitializeComponent_Item);
            fffffffToolStripMenuItem.Text = string.Format("{0} 2", Resources.GtkForm_InitializeComponent_Item);
            button1.Text =Resources.GtkForm_InitializeComponent_Open_main_window;
            toolStripStatusLabel1.Text = Resources.GtkForm_InitializeComponent_Open_Status_Text;
            toolStripSplitButton2.Text = Resources.GtkForm_InitializeComponent_DropSownMenu;

            SizeChanged += Form1_SizeChanged;
        }

        private void Form1_SizeChanged(object? sender, EventArgs e)
        {
            Console.WriteLine($"{Width},{Height}");
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            label1.Text = trackBar1.Value.ToString();
        }

        private void button1_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.DrawEllipse(new Pen(new SolidBrush(Color.Red), 2), 80, 25, 30, 20);
            //g.FillEllipse(new SolidBrush(Color.Red), 40, 25, 30, 20);
            // g.DrawEllipse(new Pen(new SolidBrush(Color.Red), 0), 40, 25, 40, 40);

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show(linkLabel1.Text);
        }
    }
}
