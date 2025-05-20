using Gtk;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace GTKWinFormsApp
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
 
            this.FormClosing += Form2_FormClosing;
            this.FormClosed += Form2_FormClosed;
            listView1.MouseDown += ListView1_MouseDown;
            
        }

        private void ListView1_MouseDown(object sender, MouseEventArgs e)
        {
            ListViewItem item = listView1.GetItemAt(e.X, e.Y);
            if (item != null)
            {
                Console.WriteLine(item.Text);
            }
            listView1.Items.Remove(item);
        }

        int i = 4;
        private void Timer1_Tick(object sender, System.EventArgs e)
        {
            i++;
            listBox1.Items.Add($"异常警告{i} --- 机房空调运行监控事件 --- {DateTime.Now.Ticks} ------ {DateTime.Now.ToString()}");
            listBox1.TopIndex = i;


            richTextBox1.AppendText($"异常警告{i} --- 机房空调运行监控事件 --- {DateTime.Now.Ticks} ------ {DateTime.Now.ToString()}\n");
            richTextBox1.SelectionStart = richTextBox1.Text.Length;
           // richTextBox1.SelectionLength = 0;
            richTextBox1.Focus();
        }
        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            MessageBox.Show("Form2_FormClosed");
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult res = MessageBox.Show("Form2_FormClosing");
            e.Cancel = res != DialogResult.OK;
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
            //listView1.Clear();
            listView1.Groups.Add("listViewGroup11", "listViewGroup11");
            listView1.Groups.Add("listViewGroup21", "listViewGroup21");
            listView1.Items.Add(new ListViewItem("同时添加分组和数据") { ForeColor = Color.Red, BackColor = Color.Yellow, Group = listView1.Groups[0] });
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
 
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            richTextBox1.Text += $"{listBox1.SelectedIndex}-{listBox1.SelectedValue}\n";
        }
    }
} 
