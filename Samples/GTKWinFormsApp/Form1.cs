
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Windows.Forms;

namespace GTKWinFormsApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {

            InitializeComponent();
            this.Load += Form1_Load;
        }

        private void Form1_Load(object? sender, EventArgs e)
        {
            
            treeView1.Nodes.Clear();
            //treeView1.CheckBoxes = true;

            string jsontext = File.ReadAllText("TestData1.json");
            using (FileStream reader = new FileStream("TestData1.json", FileMode.Open, FileAccess.Read))
            {
                DataContractJsonSerializer dataContractJson = new DataContractJsonSerializer(typeof(List<TestDataMode>));
                List<TestDataMode>? json = dataContractJson.ReadObject(reader) as List<TestDataMode>;
                IEnumerable<TreeNode> childs = GetChild(null, json);
                treeView1.Nodes.AddRange(childs);
                foreach (TreeNode child in treeView1.Nodes)
                    child.Expand();

                treeView1.Nodes[0].Nodes[2].Nodes[3].Checked = true;
                treeView1.SelectedNode = treeView1.Nodes[0].Nodes[2];
            }
        }
        private IEnumerable<TreeNode> GetChild(string treeID, IEnumerable<TestDataMode> data)
        {
            List<TreeNode> children = new List<TreeNode>();
            var list = data.Where(w => w.parent == treeID);
            foreach (TestDataMode d in list)
            {
                var node = new TreeNode(d.name) { Name = d.treeID };
                IEnumerable<TreeNode> childs = GetChild(d.treeID, data);
                if (childs.Count() > 0)
                    node.Nodes.AddRange(childs);
                children.Add(node);
            }
            return children;
        }
        public class TestDataMode
        {
            public string name { get; set; }
            public string treeID { get; set; }
            public string parent { get; set; }
            public string treeName { get; set; }
        }

        TestEntity b = new TestEntity();
        private void button1_Click(object sender, EventArgs e)
        {
            Console.WriteLine(treeView1.SelectedNode?.Text);
            // b.Title = "test2";
            DialogResult result = MessageBox.Show("1、加载数据点yes \n2、不加载数据点no", "加载数据提示", MessageBoxButtons.YesNo);
            if (result == DialogResult.No)
            {
                return;
            }
            //1、数据集列表数据源
            List<TestEntity> data = new List<TestEntity>();
            var createdate = DateTime.Now;
            data.Add(new TestEntity() { ID = 0, Title = "加载数据点yes加载数据\n点yes加载数据点yes加载数据点yes", Info = "sdfdf", State = true, CreateDate = createdate, Operate = "编辑", PIC = "face-smile-big" });
            data.Add(new TestEntity() { ID = 1, Title = "test2", Info = " 3234fdf", State = true, CreateDate = createdate, Operate = "编辑", PIC = "Resources\\timg2.jpg" });
            data.Add(new TestEntity() { ID = 3, Title = "test3", Info = "ddds", State = false, CreateDate = createdate, Operate = "编辑", PIC = "Resources\\BindingNavigator.Delete.ico" });
            data.Add(new TestEntity() { ID = 4, Title = "test4", Info = "yyyy", State = true, CreateDate = createdate, Operate = "编辑", PIC = "" });

            data.Add(new TestEntity() { ID = 5, Title = "网络图片异步加载", Info = "ddds", State = false, CreateDate = createdate, Operate = "编辑", PIC = "https://gitlab.gnome.org/uploads/-/system/project/avatar/13319/gi-docgen.png?width=48" });
            data.Add(new TestEntity() { ID = 6, Title = "test4", Info = "yyyy", State = true, CreateDate = createdate, Operate = "编辑", PIC = "" });
            for (int i = 0; i < 10; i++)
                data.Add(new TestEntity() { ID = i + 7, Title = "网络图片异步加载" + i.ToString(), Info = "ddds", State = false, CreateDate = createdate, Operate = "编辑", PIC = "https://www.baidu.com/img/flexible/logo/pc/result.png?" + i.ToString() });



            this.dataGridView1.DataSource = data;
            //var s=this.dataGridView1.Rows[0].Cells[0];

            //2、datatable数据源
            //DataTable dt = new DataTable();
            //dt.Columns.Add("ID", typeof(string));
            //dt.Columns.Add("CreateDate", typeof(DateTime));
            //dt.Columns.Add("State", typeof(bool));
            //dt.Rows.Add("test1dddd", DateTime.Now, true);
            //dt.Rows.Add("test2", DateTime.Now.AddDays(5), false);
            //this.dataGridView1.Columns.Clear();
            // this.dataGridView1.DataSource = dt;
        }
        public class TestEntity : INotifyPropertyChanged
        {
            public int ID { get; set; }
            public string title;
            public string Title { get { return title; } set { title = value; OnPropertyChangedEventHandler(); } }
            public string Info { get; set; }
            public bool State { get; set; }
            public DateTime CreateDate { get; set; }
            public string Operate { get; set; }
            public string PIC { get; set; }

            public event PropertyChangedEventHandler? PropertyChanged;
            protected void OnPropertyChangedEventHandler([CallerMemberName] string propertyName = null)
            {
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
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
                textBox1.BackColor = ColorTranslator.FromHtml(textBox1.Text);
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
            //6
            Console.WriteLine("dataGridView1_SelectionChanged");
        }

        private void dataGridView1_MultiSelectChanged(object sender, EventArgs e)
        { //1
            Console.WriteLine("dataGridView1_MultiSelectChanged");
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            Console.WriteLine("dataGridView1_CellValueChanged");
            DataGridViewCell cell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
            Console.WriteLine($"{cell.Value},{cell.Selected}");

            //foreach (DataGridViewRow row in dataGridView1.Rows)
            //{
            //    Console.WriteLine(row.Cells[1].Value);
            //}
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            //3
            Console.WriteLine("dataGridView1_CellEnter");
        }

        private void dataGridView1_CellLeave(object sender, DataGridViewCellEventArgs e)
        {//4
            Console.WriteLine("dataGridView1_CellLeave");
        }

        private void dataGridView1_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            //7
            Console.WriteLine("dataGridView1_CellValidated");

            Console.WriteLine(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
        }

        private void dataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            //5
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
            //2
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

            Console.WriteLine("treeView1_BeforeSelect：" + e.Node?.Text);
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            Console.WriteLine("treeView1_AfterSelect：" + treeView1.SelectedValuePath);
            Console.WriteLine("treeView1_AfterSelect：" + e.Node?.Text);
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

            if (GTKWinFormsApp.Properties.Resources.timg6 != null)
            {
                MemoryStream mem = new MemoryStream(GTKWinFormsApp.Properties.Resources.timg6);
                
                    //g.DrawImage(new Bitmap(mem), new Point(0, 0));
                    g.DrawImage(new Bitmap(mem), new Rectangle(0, 0, 192, 108), new Rectangle(0, 0, 1920, 1080), GraphicsUnit.Pixel);
                
            }

            g.FillRectangle(new SolidBrush(Color.AliceBlue), new Rectangle(0, 0, 100, 50));
            // g.DrawLine(new Pen(new SolidBrush(Color.Blue), 2), new Point(10, 10), new Point(50, 30));
            List<PointF> Rps = new List<PointF>();
            List<PointF> rps = new List<PointF>();
            float R = 50;
            double rad = Math.PI / 180;
            float r = (float)(R * Math.Sin(18 * R) / Math.Cos(36 * R));
            float x = pictureBox2.Width / 2;
            float y = pictureBox2.Height / 2;
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

                g.DrawLines(new Pen(new SolidBrush(Color.Red), 2), new PointF[] { Rps[i], rps[i], new PointF(x, y), Rps[i] });
            }

            g.DrawString("这是Paint Graphics示例效果", new Font(FontFamily.GenericSansSerif, 12, FontStyle.Regular), new SolidBrush(Color.Red), 0, 60);
            g.DrawArc(new Pen(new SolidBrush(Color.Blue), 2), new Rectangle(pictureBox2.Width / 2, pictureBox2.Height / 2, pictureBox2.Width, pictureBox2.Height), 0, 270);

            g.DrawCurve(new Pen(new SolidBrush(Color.Blue), 2), new PointF[] { new PointF(50, 60), new PointF(100, 80), new PointF(75, 100) });
            g.DrawCurve(new Pen(new SolidBrush(Color.Blue), 2), new PointF[] { new PointF(75, 100), new PointF(100, 120), new PointF(120, 100) });
            g.DrawRectangle(new Pen((Color)Color.Red), new Rectangle(10, 10, 20, 20));
        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            Form2 form = new Form2();
            DialogResult result = form.ShowDialog(this);
            if (result == DialogResult.None || result == DialogResult.Cancel)
            {
                // MessageBox.Show("关闭窗口返回");
            }
            //form.Show();
        }

        private void tabControl1_DrawItem(object sender, DrawItemEventArgs e)
        {
            var rect = tabControl1.GetTabRect(e.Index);
            //e.Graphics.FillRectangle(new SolidBrush(Color.Gray), new Rectangle(rect.X, rect.Y, rect.Width, rect.Height));
            e.Graphics.FillRectangle(new SolidBrush(Color.DarkBlue), e.Bounds);
            var font = new Font(FontFamily.GenericSansSerif, 12);
            e.Graphics.DrawString($"tab组{e.Index}", font, new SolidBrush(Color.Red), new PointF(0, 0));
            e.Graphics.DrawImage(Image.FromFile("Resources\\BindingNavigator.Delete.ico"), new Point(e.Bounds.Width - 16, 0));
        }

        private void button1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            
            Console.WriteLine("textBox1_KeyDown");
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            Console.WriteLine("textBox1_KeyPress");
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            Console.WriteLine("textBox1_KeyUp");
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {
            // MessageBox.Show("ffsssssss");
        }

        private void comboBox1_DropDown(object sender, EventArgs e)
        {
            Console.WriteLine("comboBox1_DropDown");
        }
    }

}
