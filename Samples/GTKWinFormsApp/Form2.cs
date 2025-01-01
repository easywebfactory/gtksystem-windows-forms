using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace GTKWinFormsApp
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            // listView1.Items.Add(new ListViewItem("test1", new ListViewGroup("ListViewGroup1", "ListViewGroup1")) { });

          DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(string));
            dt.Columns.Add("CreateDate", typeof(DateTime));
            dt.Rows.Add("user1", DateTime.Now);
            dt.Rows.Add("user2", DateTime.Now.AddDays(1));
            dt.Rows.Add("user3", DateTime.Now.AddDays(2));
            dt.Rows.Add("user4", DateTime.Now.AddDays(3));
            //Dictionary<int, string> dic = new Dictionary<int, string>();
            //dic.Add(0, "111");
            //dic.Add(3, "333");
            //List<string> list = new List<string>();
            //list.Add("ddd");
            //list.Add("rrrr");
            //Hashtable hashtable = new Hashtable();
            //hashtable.Add("11", "ddd");
            //hashtable.Add("22", "rrrr");

            listBox1.DisplayMember = "CreateDate";
            listBox1.DataSource = dt;

            this.FormClosing += Form2_FormClosing;
            this.FormClosed += Form2_FormClosed;
        }
        int i = 4;
        private void Timer1_Tick(object? sender, System.EventArgs e)
        {
            // listBox1.Items.Clear();
            //for (int i = 0; i < 10; i++)
            //{
            i++;
                listBox1.Items.Add($"异常警告{i} --- 机房空调运行监控事件 --- {DateTime.Now.Ticks} ------ {DateTime.Now.ToString()}");
                listBox1.TopIndex = i;
           // }
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
           // listView1.Clear();
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

        private void listView1_ColumnReordered(object sender, ColumnReorderedEventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            richTextBox1.Text += $"{listBox1.SelectedIndex}-{listBox1.SelectedValue}\n";
        }
    }
} 
