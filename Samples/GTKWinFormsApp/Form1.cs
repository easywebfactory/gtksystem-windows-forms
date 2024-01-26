
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Windows.Forms;
using System.Windows.Forms;

namespace GTKWinFormsApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(string));
            dt.Columns.Add("CreateDate", typeof(DateTime));
            dt.Columns.Add("State", typeof(bool));
            dt.Rows.Add("user1", DateTime.Now, true);
            dt.Rows.Add("user2", DateTime.Now.AddDays(5), false);

            DataSet dataSet = new DataSet();
            dataSet.Tables.Add(dt);


            listBox1.DataBindings.Add(new Binding("Text", dataSet, "CreateDate"));

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Control p = (Control)this;
            p.Controls.Add(new Button() { Text = "dddd", Location = new Point(681, 156) });

            DialogResult result = MessageBox.Show("1、加载数据点yes \n2、不加载数据点no", "加载数据提示", MessageBoxButtons.YesNo);
            if (result == DialogResult.No)
            {
                return;
            }
            //1、数据集列表数据源
            List<TestEntity> data = new List<TestEntity>();
            var createdate = DateTime.Now;
            data.Add(new TestEntity() { ID = 0, Title = "test1", Info = "sdfdf", State = true, CreateDate = createdate, Operate = "编辑", PIC = "face-smile-big-symbolic.symbolic" });
            data.Add(new TestEntity() { ID = 1, Title = "test2", Info = " 3234fdf", State = true, CreateDate = createdate, Operate = "编辑", PIC = "face-smile" });
            data.Add(new TestEntity() { ID = 3, Title = "test3", Info = "ddds", State = false, CreateDate = createdate, Operate = "编辑", PIC = "" });
            data.Add(new TestEntity() { ID = 4, Title = "test4", Info = "yyyy", State = true, CreateDate = createdate, Operate = "编辑", PIC = "" });
            this.dataGridView1.DataSource = data;
            //2、datatable数据源
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(string));
            dt.Columns.Add("CreateDate", typeof(DateTime));
            dt.Columns.Add("State", typeof(bool));
            dt.Rows.Add("user1", DateTime.Now, true);
            dt.Rows.Add("user2", DateTime.Now.AddDays(5), false);


            DataSet dataSet = new DataSet();
            dataSet.Tables.Add(dt);

            listBox1.DataBindings.Add(new Binding("Text", data, "Title"));


            //3、通过dataviewrow添加数据
            //for (int i = 0; i < 10; i++)
            //{
            //    var cell = new DataGridViewRow();
            //    cell.Cells.AddRange(new List<DataGridViewCell>() { new DataGridViewTextBoxCell() { Value = "user" + i.ToString() }, new DataGridViewCheckBoxCell() { Value=true }, new DataGridViewTextBoxCell() { Value = "title" + i.ToString() }, new DataGridViewTextBoxCell() { Value = DateTime.Now } }.ToArray());
            //    cell.DefaultCellStyle = new DataGridViewCellStyle() { BackColor = Color.Red };
            //    this.dataGridView1.Rows.Add(cell);
            //}

            this.textBox1.Text = this.comboBox1.SelectedItem?.ToString() + "/" + this.comboBox1.SelectedIndex;

        }

        public class TestEntity
        {
            public int ID { get; set; }
            public string Title { get; set; }
            public string Info { get; set; }
            public bool State { get; set; }
            public DateTime CreateDate { get; set; }
            public string Operate { get; set; }
            public string PIC { get; set; }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            if (textBox1.Text.Length >= 6)
            {
                try
                {
                    cd.Color = ColorTranslator.FromHtml(textBox1.Text);
                }
                catch { }
            }
            DialogResult result = cd.ShowDialog(this);

            if (result == DialogResult.OK)
            {
                textBox1.Text = "#" + cd.Color.Name;
            }

            //DialogResult result = MessageBox.Show(this, " 弹窗测试 ", "信息提示", MessageBoxButtons.OKCancel);
            //Console.WriteLine(result.ToString());
            //if (result == DialogResult.OK)
            //{
            //    Console.WriteLine("DialogResult.OK");
            //    label2.Text = "你选择了 确定";
            //}
            //if (result == DialogResult.Cancel)
            //{
            //    Console.WriteLine("DialogResult.Cancel");
            //    label2.Text = "你选择了 取消";
            //}
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ToolStripItem menu = sender as ToolStripItem;
            Console.WriteLine(menu.Text);
        }

        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            Console.WriteLine("textBox1_Validating");
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            Console.WriteLine("textBox1_Enter");
        }

        private void maskedTextBox2_Validated(object sender, EventArgs e)
        {
            Console.WriteLine("maskedTextBox2_Validated");
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox2_CheckStateChanged(object sender, EventArgs e)
        {

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            Console.WriteLine("numericUpDown1_ValueChanged");
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            Console.WriteLine("radioButton3_CheckedChanged");
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            Console.WriteLine("radioButton2_CheckedChanged");
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            Console.WriteLine("radioButton1_CheckedChanged");
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Console.WriteLine("textBox1_TextChanged");
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Console.WriteLine("comboBox1_SelectedIndexChanged");
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            var i = comboBox1.SelectedIndex;
            var o = comboBox1.SelectedItem;
            Console.WriteLine("comboBox1_SelectedValueChanged");
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            Console.WriteLine("dateTimePicker1_ValueChanged");
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            Console.WriteLine("dataGridView1_SelectionChanged");
        }

        private void dataGridView1_MultiSelectChanged(object sender, EventArgs e)
        {
            Console.WriteLine("dataGridView1_MultiSelectChanged");
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            Console.WriteLine("dataGridView1_CellValueChanged");
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            Console.WriteLine("dataGridView1_CellEnter");
        }

        private void dataGridView1_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            Console.WriteLine("dataGridView1_CellLeave");
        }

        private void dataGridView1_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            Console.WriteLine("dataGridView1_CellValidated");
        }

        private void dataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            Console.WriteLine("dataGridView1_CellValidating" + e.FormattedValue);
        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            Console.WriteLine("dataGridView1_RowEnter");
        }

        private void dataGridView1_RowLeave(object sender, DataGridViewCellEventArgs e)
        {
            Console.WriteLine("dataGridView1_RowLeave");
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Console.WriteLine("dataGridView1_CellClick");
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            Console.WriteLine("richTextBox1_TextChanged");
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Console.WriteLine("tabControl1_SelectedIndexChanged");
        }

        private void treeView1_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            Console.WriteLine("treeView1_BeforeSelect");
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            Console.WriteLine("treeView1_AfterSelect");
        }

        private void treeView1_AfterCollapse(object sender, TreeViewEventArgs e)
        {

        }

        private void treeView1_AfterExpand(object sender, TreeViewEventArgs e)
        {

        }

        private void test2ToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            Console.WriteLine("test2ToolStripMenuItem_CheckedChanged");
        }

        private void test2ToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
        {
            Console.WriteLine("test2ToolStripMenuItem_CheckedChanged");
        }

        private void test2ToolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            Console.WriteLine("test2ToolStripMenuItem_DropDownItemClicked");
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            Console.WriteLine("toolStripMenuItem3_Click");
        }

        private void toolStripMenuItem3_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            Console.WriteLine("toolStripMenuItem3_DropDownItemClicked");
        }

        private void checkedListBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            Console.WriteLine("checkedListBox1_SelectedValueChanged");
        }

        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            Console.WriteLine("checkedListBox1_ItemCheck，" + e.NewValue + e.CurrentValue);
            if (e.Index == 2)
            {
                //  checkedListBox1.ClearSelected(); 
            }
        }

        private void pictureBox2_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(Color.White);

            //g.DrawImage(new Bitmap(GTKWinFormsApp.Properties.Resources.timg6), new Point(0, 0));
            g.DrawImage(new Bitmap(GTKWinFormsApp.Properties.Resources.timg6), new Rectangle(0, 0, 192, 108), new Rectangle(0, 0, 1920, 1080), GraphicsUnit.Pixel);
            g.FillRectangle(new SolidBrush(Color.AliceBlue), new Rectangle(0, 0, 100, 50));
           // g.DrawLine(new Pen(new SolidBrush(Color.Blue), 2), new Point(10, 10), new Point(50, 30));
            List<PointF> Rps = new List<PointF>();
            List<PointF> rps = new List<PointF>();
            float R = 50;
            double rad = Math.PI / 180;
            float r = (float)(R * Math.Sin(18*R) / Math.Cos(36 * R));
            float x = pictureBox2.Width/2;
            float y = pictureBox2.Height/2;
            for (int k = 0; k < 5; k++)
            {
                Rps.Add(new PointF(x - (R * (float)Math.Cos((90 + k * 72) * rad)), y - (R * (float)Math.Sin((90 + k * 72) * rad))));
                rps.Add(new PointF(x - (r * (float)Math.Cos((90 + k * 72 + 36) * rad)), y - (r * (float)Math.Sin((90 + k * 72 + 36) * rad))));
            }
            for (int i = 0; i < 5; i++)
            {
                //g.DrawLine(new Pen(new SolidBrush(Color.Blue), 2), Rps[i], rps[i]);
                //g.DrawLine(new Pen(new SolidBrush(Color.Blue), 2), rps[i], new PointF(x, y));
                //g.DrawLine(new Pen(new SolidBrush(Color.Blue), 2), new PointF(x, y), Rps[i]);

                g.DrawLines(new Pen(new SolidBrush(Color.Red), 2), [Rps[i], rps[i],new PointF(x,y), Rps[i]]);
            }

            g.DrawString("这是Paint Graphics示例效果", new Font(new FontFamily(""), 12, FontStyle.Regular), new SolidBrush(Color.Red), 0, 60);
            g.DrawArc(new Pen(new SolidBrush(Color.Blue), 2), new Rectangle(0, 0, pictureBox2.Width, pictureBox2.Height), 60, 190);
        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            Form2 form = new Form2();
            form.Show(this);

        }

        private void tabControl1_DrawItem(object sender, DrawItemEventArgs e)
        {
            var rect = tabControl1.GetTabRect(e.Index);
            //e.Graphics.FillRectangle(new SolidBrush(Color.Gray), new Rectangle(rect.X, rect.Y, rect.Width, rect.Height));
            e.Graphics.FillRectangle(new SolidBrush(Color.DarkBlue), e.Bounds);

            e.Graphics.DrawString($"tab组{e.Index}", new Font(FontFamily.GenericSansSerif, 12), new SolidBrush(Color.Red), new PointF(0, 0));
            e.Graphics.DrawImage(Image.FromFile("F:\\我的项目\\GTK\\Forms_ico\\BindingNavigator.Delete.ico"),new Point(e.Bounds.Width-16, 0));
        }

        private void button1_Paint(object sender, PaintEventArgs e)
        {

        }
    }

}
