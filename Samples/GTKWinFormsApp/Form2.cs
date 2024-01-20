using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GTKWinFormsApp
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();

        }

        private void listView1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            MessageBox.Show($"ItemCheck:{e.NewValue.ToString()},{e.CurrentValue.ToString()}");
        }

        private void listView1_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            richTextBox1.Text = "";
            foreach (ListViewItem m in listView1.CheckedItems)
                richTextBox1.Text += $"{m.Text},Selected:{m.Selected},Checked:{m.Checked}；\n";
        }

        private void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            MessageBox.Show($"ItemSelectionChanged:{e.Item.Text},{e.Item.Selected}");
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
            foreach (ListViewItem m in listView1.SelectedItems)
                richTextBox1.Text += $"{m.Text},Selected:{m.Selected},Checked:{m.Checked}；\n";
        }

        private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            MessageBox.Show($"ColumnClick:{e.Column}");
        }

        private void listView1_Click(object sender, EventArgs e)
        {
             
        }
    }
} 
