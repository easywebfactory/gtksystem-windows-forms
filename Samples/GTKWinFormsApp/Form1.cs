using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Windows.Forms;
using GTKWinFormsApp.Properties;

namespace GTKWinFormsApp;

public partial class TestDataForm : Form
{
    public TestDataForm()
    {
        InitializeComponent();
        button1.Text = Resources.TestDataForm_TestDataForm_Load_1;
        comboBox1.Items.AddRange("test1", "test2", "test3333333333333333333", Resources.TestDataForm_TestDataForm_Load_2);
        textBox1.PlaceholderText = Resources.TestDataForm_TestDataForm_Load_2B;
        groupBox1.Text = Resources.TestDataForm_TestDataForm_Load_3;
        checkBox2.Text = Resources.TestDataForm_TestDataForm_Load_4;
        button2.Text = Resources.TestDataForm_TestDataForm_Load_5;
        checkedListBox1.Items.Clear();
        checkedListBox1.Items.AddRange(Resources.Resources_TestDataForm_TestDataForm_Load_6.Split('|'));
        radioButton3.Text = Resources.Resources_TestDataForm_TestDataForm_Load_7;
        dateTimePicker1.CustomFormat = Resources.Resources_TestDataForm_TestDataForm_Load_8;
        radioButton2.Text = Resources.Resources_TestDataForm_TestDataForm_Load_9;
        label2.Text = Resources.TestDataForm_TestDataForm_Open_Website_A;
        radioButton1.Text = Resources.Resources_TestDataForm_TestDataForm_Load_B;
        maskedTextBox1.Mask = Resources.Resources_TestDataForm_TestDataForm_Load_C;
        button7.Text = Resources.Resources_TestDataForm_TestDataForm_Load_D;
        tabPage1.Text = Resources.Resources_TestDataForm_TestDataForm_Load_E;
        tabPage2.Text = Resources.Resources_TestDataForm_TestDataForm_Load_F;
        richTextBox1.Text = Resources.Resources_TestDataForm_TestDataForm_Load_G;
        tabPage3.Text = Resources.Resources_TestDataForm_TestDataForm_Load_H;
        toolStripMenuItem1.Text = Resources.Resources_TestDataForm_TestDataForm_Load_I;
        toolStripMenuItem2.Text = Resources.Resources_TestDataForm_TestDataForm_Load_J;
        MenuThreeToolStripMenuItem.Text = Resources.Resources_TestDataForm_TestDataForm_Load_K;
        toolStripMenuItem3.Text = Resources.Resources_TestDataForm_TestDataForm_Load_L;
        test1ToolStripMenuItem.Text = Resources.Resources_TestDataForm_TestDataForm_Load_M;
        ThirdLevelMenu1ToolStripMenuItem.Text = Resources.Resources_TestDataForm_TestDataForm_Load_N;
        test2ToolStripMenuItem.Text = Resources.Resources_TestDataForm_TestDataForm_Load_O;
        toolStripMenuItem4.Text = Resources.Resources_TestDataForm_TestDataForm_Load_P;
        SecondaryMenu1ToolStripMenuItem.Text = Resources.Resources_TestDataForm_TestDataForm_Load_M;
        State.HeaderText = Resources.Resources_TestDataForm_TestDataForm_Load_Q;
        Title.HeaderText = Resources.Resources_TestDataForm_TestDataForm_Load_R;
        CreateDate.HeaderText = Resources.Resources_TestDataForm_TestDataForm_Load_S;
        Operate.HeaderText = Resources.Resources_TestDataForm_TestDataForm_Load_T;
        PIC.HeaderText = Resources.Resources_TestDataForm_TestDataForm_Load_U;
        Text = Resources.GtkMainForm_GtkMainForm_Default_Style_Interface;
        ID.HeaderText = Resources.Resources_TestDataForm_TestDataForm_Load_V;
        Load += TestDataForm_Load;
    }

    private void TestDataForm_Load(object? sender, EventArgs e)
    {

        treeView1.Nodes.Clear();
        treeView1.CheckBoxes = true;

        var testdata1Json = Resources.testdata1Json;
        using (var reader = new FileStream(testdata1Json, FileMode.Open, FileAccess.Read))
        {
            var dataContractJson = new DataContractJsonSerializer(typeof(List<TestDataMode>));
            List<TestDataMode> json = dataContractJson.ReadObject(reader) as List<TestDataMode>;
            IEnumerable<TreeNode> childs = GetChild(null, json);
            treeView1.Nodes.AddRange(childs.ToArray());
            foreach (var child in treeView1.Nodes)
                child.Expand();
            var treeView1SelectedNode = treeView1.Nodes[0].Nodes[2];
            if (treeView1SelectedNode.Nodes.Count >= 3)
            {
                treeView1SelectedNode.Nodes[3].Checked = true;
            }
            treeView1.SelectedNode = treeView1SelectedNode;
            var tabPage = new TabPage();
            tabPage.Location = new Point(4, 29);
            tabPage.Margin = new Padding(4);
            tabPage.Name = "tabPage3";
            tabPage.Padding = new Padding(4);
            tabPage.Size = new Size(1179, 426);
            tabPage.TabIndex = 1;
            tabPage.Text = "test";
            tabPage.UseVisualStyleBackColor = true;

            tabControl1.Controls.Add(tabPage);
        }
    }

    private IEnumerable<TreeNode> GetChild(string treeID, IEnumerable<TestDataMode> data)
    {
        List<TreeNode> children = new();
        var list = data.Where(w => w.parent == treeID);
        foreach (var d in list)
        {
            var node = new TreeNode(d.name) { Name = d.treeID };
            IEnumerable<TreeNode> childs = GetChild(d.treeID, data);
            if (childs.Count() > 0)
                node.Nodes.AddRange(childs.ToArray());
            children.Add(node);
        }
        return children;
    }
    public class TestDataMode
    {
        public string? name { get; set; }
        public string treeID { get; set; }
        public string parent { get; set; }
        public string treeName { get; set; }
    }

    TestEntity b = new();
    private void button1_Click(object? sender, EventArgs e)
    {
        Console.WriteLine(treeView1.SelectedNode?.Text);
        // b.Title = "test2";
        var result = MessageBox.Show(Resources.TestDataForm_button1_1, Resources.TestDataForm_button1_2, MessageBoxButtons.YesNo);
        if (result == DialogResult.No)
        {
            return;
        }
        //1、Dataset list data source
        List<TestEntity> data = new();
        var createdate = DateTime.Now;
        data.Add(new TestEntity()
        {
            ID = 0,
            Title = Resources.TestDataForm_button1_3,
            Info = "sdfdf",
            State = true,
            CreateDate = createdate,
            Operate = Resources.TestDataForm_button1_4,
            PIC1 = "face-smile-big",
            PIC = new Bitmap(10, 10)
        });
        data.Add(new TestEntity()
        {
            ID = 1,
            Title = "test2",
            Info = "yyyy2",
            State = true,
            CreateDate = createdate,
            Operate = Resources.TestDataForm_button1_4,
            PIC1 = "",
            PIC = Image.FromFile("Resources/img11.jpg")
        });
        data.Add(new TestEntity()
        {
            ID = 3,
            Title = "test3",
            Info = "ddds",
            State = false,
            CreateDate = createdate,
            Operate = Resources.TestDataForm_button1_4,
            PIC1 = "Resources/BindingNavigator.Delete.ico",
            PIC = Image.FromFile("Resources/timg2.jpg")
        });
        data.Add(new TestEntity()
        {
            ID = 4,
            Title = "test4",
            Info = "yyyy",
            State = true,
            CreateDate = createdate,
            Operate = Resources.TestDataForm_button1_4,
            PIC1 = "",
            PIC = Image.FromFile("Resources/timg2.jpg")
        });

        data.Add(new TestEntity() { ID = 5, Title = Resources.TestDataForm_button1_Click_Asynchronous_loading_of_network_images, Info = "ddds", State = false, CreateDate = createdate, Operate = Properties.Resources.TestDataForm_button1_4, PIC1 = "https://gitlab.gnome.org/uploads/-/system/project/avatar/13319/gi-docgen.png?width=48", PIC = Image.FromFile("./Resources/timg2.jpg") });
        data.Add(new TestEntity() { ID = 6, Title = "test4", Info = "yyyy", State = true, CreateDate = createdate, Operate = Properties.Resources.TestDataForm_button1_4, PIC1 = "", PIC = Image.FromFile("./Resources/timg2.jpg") });
        for (var i = 0; i < 10; i++)
            data.Add(new TestEntity() { ID = i + 7, Title = Resources.TestDataForm_button1_Click_Asynchronous_loading_of_network_images + i.ToString(), Info = "ddds", State = false, CreateDate = createdate, Operate = Properties.Resources.TestDataForm_button1_4, PIC1 = "https://www.baidu.com/img/flexible/logo/pc/result.png?" + i.ToString(), PIC = Image.FromFile("./Resources/timg2.jpg") });


        this.dataGridView1.DataSource = data;
        this.comboBox1.DisplayMember = "Title";
        this.comboBox1.ValueMember = "ID";
        this.comboBox1.DataSource = data;
        // dataGridView1.Columns[1].Visible = false;

        ////2、datatable data source
        //DataTable dt = new DataTable();
        //dt.Columns.Add("ID", typeof(string));
        //dt.Columns.Add("CreateDate1", typeof(DateTime));
        //dt.Columns.Add("State", typeof(bool));
        //dt.Rows.Add("test1dddd", DateTime.Now, true);
        //dt.Rows.Add("test2", DateTime.Now.AddDays(5), false);
        ////  this.dataGridView1.Columns.Clear();
        //this.dataGridView1.DataSource = dt;
    }
    public class TestEntity
    {
        public int ID { get; set; }
        public string title;
        public string Title { get { return title; } set { title = value; } }
        public string Info { get; set; }
        public bool State { get; set; }
        public DateTime CreateDate { get; set; }
        public string Operate { get; set; }
        public string PIC1 { get; set; }
        public Image PIC { get; set; }
    }


    private void button2_Click(object? sender, EventArgs e)
    {
        var column = new DataGridViewTextBoxColumn();
        column.HeaderText = "test1";
        column.MinimumWidth = 6;
        column.Name = "test1";
        column.Width = 225;
        column.DataPropertyName = "test1";

        dataGridView1.Columns.Add(column);

        var cd = new ColorDialog();
        if (textBox1.Text.Length >= 6)
        {
            try
            {
                cd.Color = ColorTranslator.FromHtml(textBox1.Text);
            }
            catch { }
        }
        var result = cd.ShowDialog(this);

        if (result == DialogResult.OK)
        {
            textBox1.Text = "#" + cd.Color.Name;
            textBox1.BackColor = ColorTranslator.FromHtml(textBox1.Text);
        }

        //DialogResult result = MessageBox.Show(this, "Pop-up window test", "Information prompt", MessageBoxButtons.OKCancel);
        //Console.WriteLine(result.ToString());
        //if (result == DialogResult.OK)
        //{
        //    Console.WriteLine("DialogResult.OK");
        //    label2.Text = "You selected OK";
        //}
        //if (result == DialogResult.Cancel)
        //{
        //    Console.WriteLine("DialogResult.Cancel");
        //    label2.Text = "You chose Cancel";
        //}
    }

    private void toolStripMenuItem1_Click(object? sender, EventArgs e)
    {
        var menu = sender as ToolStripItem;
        Console.WriteLine(menu.Text);
    }

    private void textBox1_Validating(object? sender, CancelEventArgs e)
    {
        Console.WriteLine("textBox1_Validating");
    }

    private void textBox1_Enter(object? sender, EventArgs e)
    {
        Console.WriteLine("textBox1_Enter");
    }

    private void maskedTextBox2_Validated(object? sender, EventArgs e)
    {
        Console.WriteLine("maskedTextBox2_Validated");
    }

    private void checkBox2_CheckedChanged(object? sender, EventArgs e)
    {

    }

    private void checkBox2_CheckStateChanged(object? sender, EventArgs e)
    {

    }

    private void numericUpDown1_ValueChanged(object? sender, EventArgs e)
    {
        Console.WriteLine("numericUpDown1_ValueChanged");
    }

    private void radioButton3_CheckedChanged(object? sender, EventArgs e)
    {
        Console.WriteLine("radioButton3_CheckedChanged");
    }

    private void radioButton2_CheckedChanged(object? sender, EventArgs e)
    {
        Console.WriteLine("radioButton2_CheckedChanged");
    }

    private void radioButton1_CheckedChanged(object? sender, EventArgs e)
    {
        Console.WriteLine("radioButton1_CheckedChanged");
    }

    private void textBox1_TextChanged(object? sender, EventArgs e)
    {
        Console.WriteLine("textBox1_TextChanged");
    }

    private void comboBox1_SelectedIndexChanged(object? sender, EventArgs e)
    {
        Console.WriteLine($"comboBox1_SelectedIndexChanged {comboBox1.SelectedIndex},{comboBox1.SelectedValue},{comboBox1.Text}");
    }

    private void comboBox1_SelectedValueChanged(object? sender, EventArgs e)
    {
        var i = comboBox1.SelectedIndex;
        var o = comboBox1.SelectedItem;
        Console.WriteLine("comboBox1_SelectedValueChanged");
    }

    private void dateTimePicker1_ValueChanged(object? sender, EventArgs e)
    {
        Console.WriteLine("dateTimePicker1_ValueChanged");
    }

    private void dataGridView1_SelectionChanged(object? sender, EventArgs e)
    {
        //6
        //if(dataGridView1.SelectedRows.Count > 0) 
        //    dataGridView1.SelectedRows[0].Cells[3].Value = DateTime.Now;
        Console.WriteLine("dataGridView1_SelectionChanged");
    }

    private void dataGridView1_MultiSelectChanged(object? sender, EventArgs e)
    { //1
        Console.WriteLine("dataGridView1_MultiSelectChanged");
    }

    private void dataGridView1_CellValueChanged(object? sender, DataGridViewCellEventArgs e)
    {
        Console.WriteLine("dataGridView1_CellValueChanged");
        if (e.RowIndex > -1)
        //    if (dataGridView1.Rows.Count > 0 && dataGridView1.Rows[e.RowIndex].Cells.Count>0)
        {
            var cell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
            Console.WriteLine($"{cell.Value},{cell.Selected}");
        }
        //foreach (DataGridViewRow row in dataGridView1.Rows)
        //{
        //    Console.WriteLine(row.Cells[1].Value);
        //}
    }

    private void dataGridView1_CellEnter(object? sender, DataGridViewCellEventArgs e)
    {
        //3
        Console.WriteLine("dataGridView1_CellEnter");
    }

    private void dataGridView1_CellLeave(object? sender, DataGridViewCellEventArgs e)
    {//4
        Console.WriteLine("dataGridView1_CellLeave");
    }

    private void dataGridView1_CellValidated(object? sender, DataGridViewCellEventArgs e)
    {
        //7
        Console.WriteLine("dataGridView1_CellValidated");

        Console.WriteLine(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
    }

    private void dataGridView1_CellValidating(object? sender, DataGridViewCellValidatingEventArgs e)
    {
        //5
        Console.WriteLine("dataGridView1_CellValidating" + e.FormattedValue);
    }

    private void dataGridView1_RowEnter(object? sender, DataGridViewCellEventArgs e)
    {
        Console.WriteLine("dataGridView1_RowEnter");
    }

    private void dataGridView1_RowLeave(object? sender, DataGridViewCellEventArgs e)
    {
        Console.WriteLine("dataGridView1_RowLeave");
    }

    private void dataGridView1_CellClick(object? sender, DataGridViewCellEventArgs e)
    {
        //2
        Console.WriteLine("dataGridView1_CellClick");
    }

    private void richTextBox1_TextChanged(object? sender, EventArgs e)
    {
        Console.WriteLine("richTextBox1_TextChanged");
    }

    private void tabControl1_SelectedIndexChanged(object? sender, EventArgs e)
    {
        Console.WriteLine("tabControl1_SelectedIndexChanged");
    }

    private void treeView1_BeforeSelect(object? sender, TreeViewCancelEventArgs e)
    {

        Console.WriteLine("treeView1_BeforeSelect：" + e.Node?.Text);
    }

    private void treeView1_AfterSelect(object? sender, TreeViewEventArgs e)
    {
        Console.WriteLine("treeView1_AfterSelect：" + treeView1.SelectedNode.FullPath);
        Console.WriteLine("treeView1_AfterSelect：" + e.Node?.Text);
    }

    private void treeView1_AfterCollapse(object? sender, TreeViewEventArgs e)
    {

    }

    private void treeView1_AfterExpand(object? sender, TreeViewEventArgs e)
    {

    }

    private void test2ToolStripMenuItem_CheckedChanged(object? sender, EventArgs e)
    {
        Console.WriteLine("test2ToolStripMenuItem_CheckedChanged");
    }

    private void test2ToolStripMenuItem_CheckStateChanged(object? sender, EventArgs e)
    {
        Console.WriteLine("test2ToolStripMenuItem_CheckedChanged");
    }

    private void test2ToolStripMenuItem_DropDownItemClicked(object? sender, ToolStripItemClickedEventArgs e)
    {
        Console.WriteLine("test2ToolStripMenuItem_DropDownItemClicked");
    }

    private void toolStripMenuItem3_Click(object? sender, EventArgs e)
    {
        Console.WriteLine("toolStripMenuItem3_Click");
    }

    private void toolStripMenuItem3_DropDownItemClicked(object? sender, ToolStripItemClickedEventArgs e)
    {
        Console.WriteLine("toolStripMenuItem3_DropDownItemClicked");
    }

    private void checkedListBox1_SelectedValueChanged(object? sender, EventArgs e)
    {
        Console.WriteLine($"checkedListBox1_SelectedValueChanged:{sender}");
    }

    private void checkedListBox1_ItemCheck(object? sender, ItemCheckEventArgs e)
    {
        (sender as CheckBox).Text = "1234";
        checkedListBox1.Items[0] = DateTime.Now.ToString();
        Console.WriteLine($"checkedListBox1_ItemCheck，{sender}: newvalue:{e.NewValue}-oldvalue:{e.CurrentValue}");
        if (e.Index == 2)
        {
            checkedListBox1.SetItemChecked(3, true);
            foreach (var o in checkedListBox1.CheckedItems)
            {
                Console.WriteLine("ItemCheck，" + o.ToString());
            }
        }
    }

    private void pictureBox2_Paint(object? sender, PaintEventArgs e)
    {
        var g = e.Graphics;
        g.Clear(Color.White);

        if (GTKWinFormsApp.Properties.Resources.timg6 != null)
        {
            //g.DrawImage(new Bitmap(mem), new Point(0, 0));
            g.DrawImage(GTKWinFormsApp.Properties.Resources.timg6, new Rectangle(0, 0, 192, 108), new Rectangle(0, 0, 1920, 1080), GraphicsUnit.Pixel);

        }

        g.FillRectangle(new SolidBrush(Color.AliceBlue), new Rectangle(0, 0, 100, 50));
        // g.DrawLine(new Pen(new SolidBrush(Color.Blue), 2), new Point(10, 10), new Point(50, 30));
        var Rps = new List<PointF>();
        var rps = new List<PointF>();
        float R = 50;
        var rad = Math.PI / 180;
        var r = (float)(R * Math.Sin(18 * R) / Math.Cos(36 * R));
        float x = pictureBox2.Width / 2;
        float y = pictureBox2.Height / 2;
        for (var k = 0; k < 5; k++)
        {
            Rps.Add(new PointF(x - (R * (float)Math.Cos((90 + k * 72) * rad)), y - (R * (float)Math.Sin((90 + k * 72) * rad))));
            rps.Add(new PointF(x - (r * (float)Math.Cos((90 + k * 72 + 36) * rad)), y - (r * (float)Math.Sin((90 + k * 72 + 36) * rad))));
        }
        for (var i = 0; i < 5; i++)
        {
            //g.DrawLine(new Pen(new SolidBrush(Color.Blue), 2), Rps[i], rps[i]);
            //g.DrawLine(new Pen(new SolidBrush(Color.Blue), 2), rps[i], new PointF(x, y));
            //g.DrawLine(new Pen(new SolidBrush(Color.Blue), 2), new PointF(x, y), Rps[i]);

            g.DrawLines(new Pen(new SolidBrush(Color.Red), 2), new PointF[] { Rps[i], rps[i], new(x, y), Rps[i] });
        }

        g.DrawString(Resources.TestDataForm_pictureBox2_Paint_This_is_the_Paint_Graphics_sample_effect, new Font(FontFamily.GenericSansSerif, 12, FontStyle.Regular), new SolidBrush(Color.Red), 0, 60);

        g.DrawArc(new Pen(new SolidBrush(Color.Blue), 2), new Rectangle(pictureBox2.Width / 2, pictureBox2.Height / 2, pictureBox2.Width, pictureBox2.Height), 0, 270);

        g.DrawCurve(new Pen(new SolidBrush(Color.Blue), 2), new PointF[] { new(50, 60), new(100, 80), new(75, 100) });
        g.DrawCurve(new Pen(new SolidBrush(Color.Blue), 2), new PointF[] { new(75, 100), new(100, 120), new(120, 100) });
        g.DrawRectangle(new Pen((Color)Color.Red), new Rectangle(10, 10, 20, 20));
    }

    private void button6_Click(object? sender, EventArgs e)
    {
        // textBox1.InsertTextAtCursor("666 slip");
        Console.WriteLine(textBox1.SelectionStart);
        Console.WriteLine(textBox1.SelectionLength);
        textBox1.SelectionLength = 20;
        // richTextBox1.InsertTextAtCursor("666 slip");
        // Console.WriteLine(richTextBox1.SelectionStart);
        //// Console.WriteLine(richTextBox1.SelectionStart1);
        // Console.WriteLine(richTextBox1.SelectionLength);
        richTextBox1.SelectionLength = 50;
    }

    private void button7_Click(object? sender, EventArgs e)
    {
        var form = new ListViewForm();
        var result = form.ShowDialog(this);
        if (result == DialogResult.None || result == DialogResult.Cancel)
        {
            // MessageBox.Show("Close window return");
        }
        //form.Show();
    }

    private void tabControl1_DrawItem(object? sender, DrawItemEventArgs e)
    {
        var rect = tabControl1.GetTabRect(e.Index);
        //e.Graphics.FillRectangle(new SolidBrush(Color.Gray), new Rectangle(rect.X, rect.Y, rect.Width, rect.Height));
        e.Graphics.FillRectangle(new SolidBrush(Color.DarkBlue), e.Bounds);
        var font = new Font(FontFamily.GenericSansSerif, 12);
        e.Graphics.DrawString($"tabGroup{e.Index}", font, new SolidBrush(Color.Red), new PointF(0, 0));
        e.Graphics.DrawImage(Image.FromFile("./Resources/BindingNavigator.Delete.ico"), new Point(e.Bounds.Width - 16, 0));
    }

    private void button1_Paint(object? sender, PaintEventArgs e)
    {

    }

    private void textBox1_KeyDown(object? sender, KeyEventArgs e)
    {

        Console.WriteLine("textBox1_KeyDown");
    }

    private void textBox1_KeyPress(object? sender, KeyPressEventArgs e)
    {
        Console.WriteLine("textBox1_KeyPress");
    }

    private void textBox1_KeyUp(object? sender, KeyEventArgs e)
    {
        Console.WriteLine("textBox1_KeyUp");
    }

    private void tabPage2_Click(object? sender, EventArgs e)
    {
        // MessageBox.Show("ffsssssss");
    }

    private void comboBox1_DropDown(object? sender, EventArgs e)
    {
        Console.WriteLine("comboBox1_DropDown");
    }
}