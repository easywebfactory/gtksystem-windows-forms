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
            // listView1.Items.Add(new ListViewItem("test1", new ListViewGroup("ListViewGroup1", "ListViewGroup1")) { });

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

        private void button1_Click(object sender, EventArgs e)
        {
            listView1.Items.Add(new ListViewItem("同时添加分组和数据") { ForeColor=Color.Red, BackColor=Color.Yellow, Group = new ListViewGroup("listViewGroup1", "ListViewGroup1") });
            listView1.Items.Add(new ListViewItem("向指定分组添加数据") { ForeColor = Color.Red, BackColor = Color.Yellow, Group = listView1.Groups[1] });

            ListViewItem m = new ListViewItem("这是一种添加多栏数据的方法", 0);
            m.Checked = true;
            m.Selected = true;
            m.ForeColor = Color.Green;
            m.BackColor = Color.Yellow;
            m.SubItems.Add(new ListViewItem.ListViewSubItem(m, "子列数据1", Color.HotPink, Color.Gray, new Font(FontFamily.GenericSansSerif, 16)));
            m.SubItems.Add(new ListViewItem.ListViewSubItem(m, "子列数据2"));
            m.SubItems.Add(new ListViewItem.ListViewSubItem(m, "子列数据3子列数据3子列数据3"));
            m.SubItems.Add(new ListViewItem.ListViewSubItem(m, "子列数据1"));
            listView1.Items.Add(m);
        }

        private void listView1_ColumnReordered(object sender, ColumnReorderedEventArgs e)
        {

        }
    }
} 
