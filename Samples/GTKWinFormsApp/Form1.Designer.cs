
using System.Runtime.InteropServices;
using System;
using System.Windows.Forms;
using System.IO;

namespace GTKWinFormsApp
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            TreeNode treeNode1 = new TreeNode("Node21");
            TreeNode treeNode2 = new TreeNode("Node22");
            TreeNode treeNode3 = new TreeNode("Node2", new TreeNode[] { treeNode1, treeNode2 });
            TreeNode treeNode4 = new TreeNode("Node3");
            TreeNode treeNode5 = new TreeNode("Node0", new TreeNode[] { treeNode3, treeNode4 });
            TreeNode treeNode6 = new TreeNode("Node1");
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            button1 = new Button();
            comboBox1 = new ComboBox();
            textBox1 = new TextBox();
            dataGridView1 = new DataGridView();
            groupBox1 = new GroupBox();
            checkBox2 = new CheckBox();
            linkLabel1 = new LinkLabel();
            button2 = new Button();
            maskedTextBox2 = new MaskedTextBox();
            checkedListBox1 = new CheckedListBox();
            radioButton3 = new RadioButton();
            dateTimePicker1 = new DateTimePicker();
            radioButton2 = new RadioButton();
            label2 = new Label();
            radioButton1 = new RadioButton();
            numericUpDown1 = new NumericUpDown();
            maskedTextBox1 = new MaskedTextBox();
            button7 = new Button();
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            splitContainer1 = new SplitContainer();
            tabPage2 = new TabPage();
            listBox1 = new ListBox();
            tableLayoutPanel1 = new TableLayoutPanel();
            label6 = new Label();
            label5 = new Label();
            button6 = new Button();
            richTextBox1 = new RichTextBox();
            flowLayoutPanel1 = new FlowLayoutPanel();
            button4 = new Button();
            button5 = new Button();
            checkBox3 = new CheckBox();
            label4 = new Label();
            checkBox1 = new CheckBox();
            tabPage3 = new TabPage();
            button3 = new Button();
            pictureBox1 = new PictureBox();
            pictureBox2 = new PictureBox();
            treeView1 = new TreeView();
            contextMenuStrip1 = new ContextMenuStrip(components);
            toolStripMenuItem1 = new ToolStripMenuItem();
            toolStripMenuItem2 = new ToolStripMenuItem();
            菜单三ToolStripMenuItem = new ToolStripMenuItem();
            menuStrip1 = new MenuStrip();
            toolStripMenuItem3 = new ToolStripMenuItem();
            test1ToolStripMenuItem = new ToolStripMenuItem();
            三级菜单1ToolStripMenuItem = new ToolStripMenuItem();
            test2ToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem4 = new ToolStripMenuItem();
            二级菜单1ToolStripMenuItem = new ToolStripMenuItem();
            splitContainer2 = new SplitContainer();
            ID = new DataGridViewTextBoxColumn();
            State = new DataGridViewCheckBoxColumn();
            Title = new DataGridViewTextBoxColumn();
            CreateDate = new DataGridViewComboBoxColumn();
            Operate = new DataGridViewButtonColumn();
            PIC = new DataGridViewImageColumn();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).BeginInit();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            tabPage2.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            contextMenuStrip1.SuspendLayout();
            menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
            splitContainer2.Panel1.SuspendLayout();
            splitContainer2.Panel2.SuspendLayout();
            splitContainer2.SuspendLayout();
            SuspendLayout();
            // 
            // button1
            // 
            button1.BackgroundImageLayout = ImageLayout.Stretch;
            button1.Font = new System.Drawing.Font("Microsoft YaHei UI", 11F);
            button1.Location = new System.Drawing.Point(669, 16);
            button1.Margin = new Padding(4);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(175, 50);
            button1.TabIndex = 0;
            button1.Text = "加载数据表数据";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            button1.Paint += button1_Paint;
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Items.AddRange(new object[] { "test1", "test2", "test3333333333333333333", "这是下拉列表控件数据测试" });
            comboBox1.Location = new System.Drawing.Point(267, 29);
            comboBox1.Margin = new Padding(4);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new System.Drawing.Size(154, 28);
            comboBox1.TabIndex = 1;
            comboBox1.DropDown += comboBox1_DropDown;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            comboBox1.SelectedValueChanged += comboBox1_SelectedValueChanged;
            // 
            // textBox1
            // 
            textBox1.BackColor = System.Drawing.SystemColors.Info;
            textBox1.Location = new System.Drawing.Point(63, 31);
            textBox1.Margin = new Padding(4);
            textBox1.Name = "textBox1";
            textBox1.PlaceholderText = "选择颜色";
            textBox1.Size = new System.Drawing.Size(256, 27);
            textBox1.TabIndex = 2;
            textBox1.TextChanged += textBox1_TextChanged;
            textBox1.Enter += textBox1_Enter;
            textBox1.KeyDown += textBox1_KeyDown;
            textBox1.KeyPress += textBox1_KeyPress;
            textBox1.KeyUp += textBox1_KeyUp;
            textBox1.Validating += textBox1_Validating;
            // 
            // dataGridView1
            // 
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.LightCyan;
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Red;
            dataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dataGridView1.ColumnHeadersHeight = 29;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { ID, State, Title, CreateDate, Operate, PIC });
            dataGridView1.DataMember = "ID,State,Title";
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.Honeydew;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.True;
            dataGridView1.DefaultCellStyle = dataGridViewCellStyle4;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.Location = new System.Drawing.Point(0, 0);
            dataGridView1.Margin = new Padding(4);
            dataGridView1.Name = "dataGridView1";
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle5;
            dataGridView1.RowHeadersWidth = 51;
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.LightGoldenrodYellow;
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.FromArgb(0, 0, 192);
            dataGridView1.RowsDefaultCellStyle = dataGridViewCellStyle6;
            dataGridView1.RowTemplate.Height = 46;
            dataGridView1.RowTemplate.Resizable = DataGridViewTriState.True;
            dataGridView1.Size = new System.Drawing.Size(1171, 333);
            dataGridView1.TabIndex = 2;
            dataGridView1.MultiSelectChanged += dataGridView1_MultiSelectChanged;
            dataGridView1.CellClick += dataGridView1_CellClick;
            dataGridView1.CellEnter += dataGridView1_CellEnter;
            dataGridView1.CellLeave += dataGridView1_CellLeave;
            dataGridView1.CellValidated += dataGridView1_CellValidated;
            dataGridView1.CellValidating += dataGridView1_CellValidating;
            dataGridView1.CellValueChanged += dataGridView1_CellValueChanged;
            dataGridView1.RowEnter += dataGridView1_RowEnter;
            dataGridView1.RowLeave += dataGridView1_RowLeave;
            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(checkBox2);
            groupBox1.Controls.Add(textBox1);
            groupBox1.Controls.Add(linkLabel1);
            groupBox1.Controls.Add(button2);
            groupBox1.Controls.Add(maskedTextBox2);
            groupBox1.Controls.Add(checkedListBox1);
            groupBox1.Controls.Add(radioButton3);
            groupBox1.Controls.Add(dateTimePicker1);
            groupBox1.Controls.Add(radioButton2);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(radioButton1);
            groupBox1.Location = new System.Drawing.Point(8, 12);
            groupBox1.Margin = new Padding(4);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(4);
            groupBox1.Size = new System.Drawing.Size(1171, 162);
            groupBox1.TabIndex = 3;
            groupBox1.TabStop = false;
            groupBox1.Text = "分组框标题";
            // 
            // checkBox2
            // 
            checkBox2.AutoSize = true;
            checkBox2.Checked = true;
            checkBox2.CheckState = CheckState.Indeterminate;
            checkBox2.Location = new System.Drawing.Point(950, 114);
            checkBox2.Margin = new Padding(4);
            checkBox2.Name = "checkBox2";
            checkBox2.Size = new System.Drawing.Size(91, 24);
            checkBox2.TabIndex = 6;
            checkBox2.Text = "全部勾选";
            checkBox2.UseVisualStyleBackColor = true;
            checkBox2.CheckedChanged += checkBox2_CheckedChanged;
            checkBox2.CheckStateChanged += checkBox2_CheckStateChanged;
            // 
            // linkLabel1
            // 
            linkLabel1.ActiveLinkColor = System.Drawing.Color.Blue;
            linkLabel1.AutoSize = true;
            linkLabel1.LinkColor = System.Drawing.Color.Red;
            linkLabel1.Location = new System.Drawing.Point(461, 115);
            linkLabel1.Margin = new Padding(4, 0, 4, 0);
            linkLabel1.Name = "linkLabel1";
            linkLabel1.Size = new System.Drawing.Size(191, 20);
            linkLabel1.TabIndex = 6;
            linkLabel1.TabStop = true;
            linkLabel1.Text = "https://www.gtkapp.com";
            // 
            // button2
            // 
            button2.BackColor = System.Drawing.Color.FromArgb(255, 128, 255);
            button2.BackgroundImageLayout = ImageLayout.Center;
            button2.Font = new System.Drawing.Font("Microsoft YaHei UI", 15F);
            button2.ForeColor = System.Drawing.Color.Blue;
            button2.Location = new System.Drawing.Point(372, 28);
            button2.Margin = new Padding(4);
            button2.Name = "button2";
            button2.Size = new System.Drawing.Size(217, 51);
            button2.TabIndex = 0;
            button2.Text = "选择颜色按钮";
            button2.UseVisualStyleBackColor = false;
            button2.Click += button2_Click;
            // 
            // maskedTextBox2
            // 
            maskedTextBox2.Location = new System.Drawing.Point(63, 72);
            maskedTextBox2.Margin = new Padding(4);
            maskedTextBox2.Name = "maskedTextBox2";
            maskedTextBox2.PasswordChar = '*';
            maskedTextBox2.Size = new System.Drawing.Size(256, 27);
            maskedTextBox2.TabIndex = 5;
            maskedTextBox2.Text = "sdfdf43";
            maskedTextBox2.Validated += maskedTextBox2_Validated;
            // 
            // checkedListBox1
            // 
            checkedListBox1.CheckOnClick = true;
            checkedListBox1.ColumnWidth = 100;
            checkedListBox1.FormattingEnabled = true;
            checkedListBox1.Items.AddRange(new object[] { "北京市", "天津市", "河南省", "山西省", "内蒙古自治区", "辽宁省", "吉林省", "黑龙江省", "上海市", "江苏省", "浙江省", "安徽省", "福建省" });
            checkedListBox1.Location = new System.Drawing.Point(677, 22);
            checkedListBox1.Margin = new Padding(4);
            checkedListBox1.MultiColumn = true;
            checkedListBox1.Name = "checkedListBox1";
            checkedListBox1.Size = new System.Drawing.Size(241, 114);
            checkedListBox1.TabIndex = 13;
            checkedListBox1.ItemCheck += checkedListBox1_ItemCheck;
            checkedListBox1.SelectedValueChanged += checkedListBox1_SelectedValueChanged;
            // 
            // radioButton3
            // 
            radioButton3.AutoSize = true;
            radioButton3.Location = new System.Drawing.Point(950, 83);
            radioButton3.Margin = new Padding(4);
            radioButton3.Name = "radioButton3";
            radioButton3.Size = new System.Drawing.Size(90, 24);
            radioButton3.TabIndex = 12;
            radioButton3.Text = "经济特区";
            radioButton3.UseVisualStyleBackColor = true;
            radioButton3.CheckedChanged += radioButton3_CheckedChanged;
            // 
            // dateTimePicker1
            // 
            dateTimePicker1.CustomFormat = "yyyy年MM年dd HH时mm分ss秒";
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.Location = new System.Drawing.Point(63, 112);
            dateTimePicker1.Margin = new Padding(4);
            dateTimePicker1.MaxDate = new DateTime(2029, 5, 17, 15, 58, 47, 534);
            dateTimePicker1.MinDate = new DateTime(2024, 3, 17, 15, 58, 47, 535);
            dateTimePicker1.Name = "dateTimePicker1";
            dateTimePicker1.Size = new System.Drawing.Size(256, 27);
            dateTimePicker1.TabIndex = 8;
            dateTimePicker1.ValueChanged += dateTimePicker1_ValueChanged;
            // 
            // radioButton2
            // 
            radioButton2.AutoSize = true;
            radioButton2.Checked = true;
            radioButton2.Location = new System.Drawing.Point(950, 52);
            radioButton2.Margin = new Padding(4);
            radioButton2.Name = "radioButton2";
            radioButton2.Size = new System.Drawing.Size(75, 24);
            radioButton2.TabIndex = 11;
            radioButton2.TabStop = true;
            radioButton2.Text = "直辖市";
            radioButton2.UseVisualStyleBackColor = true;
            radioButton2.CheckedChanged += radioButton2_CheckedChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(372, 115);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(84, 20);
            label2.TabIndex = 3;
            label2.Text = "打开网站：";
            // 
            // radioButton1
            // 
            radioButton1.AutoSize = true;
            radioButton1.Location = new System.Drawing.Point(950, 21);
            radioButton1.Margin = new Padding(4);
            radioButton1.Name = "radioButton1";
            radioButton1.Size = new System.Drawing.Size(60, 24);
            radioButton1.TabIndex = 10;
            radioButton1.Text = "全国";
            radioButton1.UseVisualStyleBackColor = true;
            radioButton1.CheckedChanged += radioButton1_CheckedChanged;
            // 
            // numericUpDown1
            // 
            numericUpDown1.Location = new System.Drawing.Point(458, 31);
            numericUpDown1.Margin = new Padding(4);
            numericUpDown1.Name = "numericUpDown1";
            numericUpDown1.Size = new System.Drawing.Size(154, 27);
            numericUpDown1.TabIndex = 9;
            numericUpDown1.ValueChanged += numericUpDown1_ValueChanged;
            // 
            // maskedTextBox1
            // 
            maskedTextBox1.Location = new System.Drawing.Point(20, 30);
            maskedTextBox1.Margin = new Padding(4);
            maskedTextBox1.Mask = "0000年90月90日 90时00分";
            maskedTextBox1.Name = "maskedTextBox1";
            maskedTextBox1.Size = new System.Drawing.Size(223, 27);
            maskedTextBox1.TabIndex = 4;
            maskedTextBox1.ValidatingType = typeof(DateTime);
            // 
            // button7
            // 
            button7.BackColor = System.Drawing.Color.LightCoral;
            button7.Location = new System.Drawing.Point(47, 18);
            button7.Name = "button7";
            button7.Size = new System.Drawing.Size(154, 51);
            button7.TabIndex = 14;
            button7.Text = "listview演示";
            button7.UseVisualStyleBackColor = false;
            button7.Click += button7_Click;
            // 
            // tabControl1
            // 
            tabControl1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Controls.Add(tabPage3);
            tabControl1.ItemSize = new System.Drawing.Size(181, 25);
            tabControl1.Location = new System.Drawing.Point(4, 197);
            tabControl1.Margin = new Padding(4);
            tabControl1.Multiline = true;
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new System.Drawing.Size(1187, 459);
            tabControl1.TabIndex = 4;
            tabControl1.DrawItem += tabControl1_DrawItem;
            tabControl1.SelectedIndexChanged += tabControl1_SelectedIndexChanged;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(splitContainer1);
            tabPage1.Location = new System.Drawing.Point(4, 29);
            tabPage1.Margin = new Padding(4);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(4);
            tabPage1.Size = new System.Drawing.Size(1179, 426);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "数据表格示例";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.FixedPanel = FixedPanel.Panel1;
            splitContainer1.Location = new System.Drawing.Point(4, 4);
            splitContainer1.Margin = new Padding(4);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.AutoScroll = true;
            splitContainer1.Panel1.Controls.Add(button1);
            splitContainer1.Panel1.Controls.Add(maskedTextBox1);
            splitContainer1.Panel1.Controls.Add(comboBox1);
            splitContainer1.Panel1.Controls.Add(numericUpDown1);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(dataGridView1);
            splitContainer1.Size = new System.Drawing.Size(1171, 418);
            splitContainer1.SplitterDistance = 80;
            splitContainer1.SplitterWidth = 5;
            splitContainer1.TabIndex = 14;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(button7);
            tabPage2.Controls.Add(listBox1);
            tabPage2.Controls.Add(tableLayoutPanel1);
            tabPage2.Controls.Add(richTextBox1);
            tabPage2.Controls.Add(flowLayoutPanel1);
            tabPage2.Controls.Add(checkBox1);
            tabPage2.Location = new System.Drawing.Point(4, 29);
            tabPage2.Margin = new Padding(4);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(4);
            tabPage2.Size = new System.Drawing.Size(1179, 426);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "容器类表格示例";
            tabPage2.UseVisualStyleBackColor = true;
            tabPage2.Click += tabPage2_Click;
            // 
            // listBox1
            // 
            listBox1.ColumnWidth = 100;
            listBox1.DisplayMember = "Title";
            listBox1.FormattingEnabled = true;
            listBox1.HorizontalScrollbar = true;
            listBox1.Items.AddRange(new object[] { "listbox item1", "listbox item2", "listbox item3" });
            listBox1.Location = new System.Drawing.Point(47, 102);
            listBox1.MultiColumn = true;
            listBox1.Name = "listBox1";
            listBox1.ScrollAlwaysVisible = true;
            listBox1.SelectionMode = SelectionMode.MultiSimple;
            listBox1.Size = new System.Drawing.Size(154, 244);
            listBox1.Sorted = true;
            listBox1.TabIndex = 4;
            listBox1.ValueMember = "Title";
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(label6, 0, 1);
            tableLayoutPanel1.Controls.Add(label5, 0, 0);
            tableLayoutPanel1.Controls.Add(button6, 1, 1);
            tableLayoutPanel1.Location = new System.Drawing.Point(795, 26);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 66.29214F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 33.7078667F));
            tableLayoutPanel1.Size = new System.Drawing.Size(291, 181);
            tableLayoutPanel1.TabIndex = 7;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new System.Drawing.Point(4, 120);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(129, 60);
            label6.TabIndex = 0;
            label6.Text = "\r\nddddddddddddddddddddddddddddddddddddd";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(4, 1);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(131, 20);
            label5.TabIndex = 0;
            label5.Text = "tablelayoutpanel";
            // 
            // button6
            // 
            button6.Location = new System.Drawing.Point(149, 123);
            button6.Name = "button6";
            button6.Size = new System.Drawing.Size(94, 29);
            button6.TabIndex = 1;
            button6.Text = "button6";
            button6.UseVisualStyleBackColor = true;
            button6.Click += button6_Click;
            // 
            // richTextBox1
            // 
            richTextBox1.Location = new System.Drawing.Point(308, 189);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new System.Drawing.Size(406, 153);
            richTextBox1.TabIndex = 6;
            richTextBox1.Text = "这是richtextbox，这是richtextbox，这是richtextbox，这是richtextbox，这是richtextbox，这是richtextbox，这是richtextbox，这是richtextbox，这是richtextbox，这是richtextbox，这是richtextbox，这是richtextbox，这是richtextbox，这是richtextbox";
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Controls.Add(button4);
            flowLayoutPanel1.Controls.Add(button5);
            flowLayoutPanel1.Controls.Add(checkBox3);
            flowLayoutPanel1.Controls.Add(label4);
            flowLayoutPanel1.Location = new System.Drawing.Point(308, 26);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new System.Drawing.Size(379, 131);
            flowLayoutPanel1.TabIndex = 5;
            // 
            // button4
            // 
            button4.Location = new System.Drawing.Point(3, 3);
            button4.Name = "button4";
            button4.Size = new System.Drawing.Size(94, 29);
            button4.TabIndex = 0;
            button4.Text = "button4";
            button4.UseVisualStyleBackColor = true;
            // 
            // button5
            // 
            button5.Location = new System.Drawing.Point(103, 3);
            button5.Name = "button5";
            button5.Size = new System.Drawing.Size(249, 29);
            button5.TabIndex = 1;
            button5.Text = "button5";
            button5.UseVisualStyleBackColor = true;
            // 
            // checkBox3
            // 
            checkBox3.AutoSize = true;
            checkBox3.Location = new System.Drawing.Point(3, 38);
            checkBox3.Name = "checkBox3";
            checkBox3.Size = new System.Drawing.Size(109, 24);
            checkBox3.TabIndex = 2;
            checkBox3.Text = "checkBox3";
            checkBox3.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(118, 35);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(125, 20);
            label4.TabIndex = 3;
            label4.Text = "flowlayoutpanel";
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new System.Drawing.Point(1293, 318);
            checkBox1.Margin = new Padding(4);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new System.Drawing.Size(109, 24);
            checkBox1.TabIndex = 1;
            checkBox1.Text = "checkBox1";
            checkBox1.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            tabPage3.BackColor = System.Drawing.Color.LightGray;
            tabPage3.BackgroundImage = Properties.Resources.timg;
            tabPage3.BackgroundImageLayout = ImageLayout.Zoom;
            tabPage3.Controls.Add(button3);
            tabPage3.Controls.Add(pictureBox1);
            tabPage3.Controls.Add(pictureBox2);
            tabPage3.Location = new System.Drawing.Point(4, 29);
            tabPage3.Name = "tabPage3";
            tabPage3.Padding = new Padding(3);
            tabPage3.Size = new System.Drawing.Size(1179, 426);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "绘图和图片示例";
            // 
            // button3
            // 
            button3.Image = Properties.Resources.timg;
            button3.Location = new System.Drawing.Point(774, 27);
            button3.Name = "button3";
            button3.Size = new System.Drawing.Size(343, 152);
            button3.TabIndex = 15;
            button3.Text = "button3";
            button3.UseVisualStyleBackColor = true;
            // 
            // pictureBox1
            // 
            pictureBox1.BackgroundImage = Properties.Resources.timg;
            pictureBox1.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBox1.Location = new System.Drawing.Point(401, 28);
            pictureBox1.Margin = new Padding(4);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new System.Drawing.Size(333, 220);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 5;
            pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            pictureBox2.ImageLocation = "";
            pictureBox2.Location = new System.Drawing.Point(68, 27);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new System.Drawing.Size(286, 221);
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.TabIndex = 14;
            pictureBox2.TabStop = false;
            pictureBox2.Paint += pictureBox2_Paint;
            // 
            // treeView1
            // 
            treeView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            treeView1.Location = new System.Drawing.Point(13, 0);
            treeView1.Margin = new Padding(4);
            treeView1.Name = "treeView1";
            treeNode1.Name = "";
            treeNode1.Text = "Node21";
            treeNode2.Name = "";
            treeNode2.Text = "Node22";
            treeNode3.Name = "Node2";
            treeNode3.Text = "Node2";
            treeNode4.Name = "Node3";
            treeNode4.Text = "Node3";
            treeNode5.Name = "Node0";
            treeNode5.Text = "Node0";
            treeNode6.Name = "Node1";
            treeNode6.Text = "Node1";
            treeView1.Nodes.AddRange(new TreeNode[] { treeNode5, treeNode6 });
            treeView1.Size = new System.Drawing.Size(197, 660);
            treeView1.TabIndex = 4;
            treeView1.AfterCollapse += treeView1_AfterCollapse;
            treeView1.AfterExpand += treeView1_AfterExpand;
            treeView1.BeforeSelect += treeView1_BeforeSelect;
            treeView1.AfterSelect += treeView1_AfterSelect;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { toolStripMenuItem1, toolStripMenuItem2 });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new System.Drawing.Size(124, 52);
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new System.Drawing.Size(123, 24);
            toolStripMenuItem1.Text = "菜单一";
            toolStripMenuItem1.Click += toolStripMenuItem1_Click;
            // 
            // toolStripMenuItem2
            // 
            toolStripMenuItem2.DropDownItems.AddRange(new ToolStripItem[] { 菜单三ToolStripMenuItem });
            toolStripMenuItem2.Name = "toolStripMenuItem2";
            toolStripMenuItem2.Size = new System.Drawing.Size(123, 24);
            toolStripMenuItem2.Text = "菜单二";
            // 
            // 菜单三ToolStripMenuItem
            // 
            菜单三ToolStripMenuItem.Name = "菜单三ToolStripMenuItem";
            菜单三ToolStripMenuItem.Size = new System.Drawing.Size(137, 26);
            菜单三ToolStripMenuItem.Text = "菜单三";
            // 
            // menuStrip1
            // 
            menuStrip1.AutoSize = false;
            menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { toolStripMenuItem3, toolStripMenuItem4 });
            menuStrip1.Location = new System.Drawing.Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Padding = new Padding(8, 2, 0, 2);
            menuStrip1.Size = new System.Drawing.Size(1409, 29);
            menuStrip1.TabIndex = 7;
            menuStrip1.Text = "menuStrip1";
            // 
            // toolStripMenuItem3
            // 
            toolStripMenuItem3.DropDownItems.AddRange(new ToolStripItem[] { test1ToolStripMenuItem, test2ToolStripMenuItem });
            toolStripMenuItem3.Name = "toolStripMenuItem3";
            toolStripMenuItem3.Size = new System.Drawing.Size(92, 25);
            toolStripMenuItem3.Text = "一级菜单1";
            toolStripMenuItem3.DropDownItemClicked += toolStripMenuItem3_DropDownItemClicked;
            toolStripMenuItem3.Click += toolStripMenuItem3_Click;
            // 
            // test1ToolStripMenuItem
            // 
            test1ToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { 三级菜单1ToolStripMenuItem });
            test1ToolStripMenuItem.Name = "test1ToolStripMenuItem";
            test1ToolStripMenuItem.Size = new System.Drawing.Size(161, 26);
            test1ToolStripMenuItem.Text = "二级菜单1";
            // 
            // 三级菜单1ToolStripMenuItem
            // 
            三级菜单1ToolStripMenuItem.Name = "三级菜单1ToolStripMenuItem";
            三级菜单1ToolStripMenuItem.Size = new System.Drawing.Size(161, 26);
            三级菜单1ToolStripMenuItem.Text = "三级菜单1";
            // 
            // test2ToolStripMenuItem
            // 
            test2ToolStripMenuItem.Checked = true;
            test2ToolStripMenuItem.CheckState = CheckState.Checked;
            test2ToolStripMenuItem.Name = "test2ToolStripMenuItem";
            test2ToolStripMenuItem.Size = new System.Drawing.Size(161, 26);
            test2ToolStripMenuItem.Text = "二级菜单2";
            test2ToolStripMenuItem.CheckedChanged += test2ToolStripMenuItem_CheckedChanged;
            test2ToolStripMenuItem.CheckStateChanged += test2ToolStripMenuItem_CheckStateChanged;
            test2ToolStripMenuItem.DropDownItemClicked += test2ToolStripMenuItem_DropDownItemClicked;
            // 
            // toolStripMenuItem4
            // 
            toolStripMenuItem4.DropDownItems.AddRange(new ToolStripItem[] { 二级菜单1ToolStripMenuItem });
            toolStripMenuItem4.Image = Properties.Resources.timg;
            toolStripMenuItem4.Name = "toolStripMenuItem4";
            toolStripMenuItem4.Size = new System.Drawing.Size(112, 25);
            toolStripMenuItem4.Text = "一级菜单2";
            // 
            // 二级菜单1ToolStripMenuItem
            // 
            二级菜单1ToolStripMenuItem.Name = "二级菜单1ToolStripMenuItem";
            二级菜单1ToolStripMenuItem.Size = new System.Drawing.Size(161, 26);
            二级菜单1ToolStripMenuItem.Text = "二级菜单1";
            // 
            // splitContainer2
            // 
            splitContainer2.Dock = DockStyle.Fill;
            splitContainer2.Location = new System.Drawing.Point(0, 29);
            splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            splitContainer2.Panel1.Controls.Add(treeView1);
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.Controls.Add(tabControl1);
            splitContainer2.Panel2.Controls.Add(groupBox1);
            splitContainer2.Size = new System.Drawing.Size(1409, 660);
            splitContainer2.SplitterDistance = 210;
            splitContainer2.TabIndex = 15;
            // 
            // ID
            // 
            ID.HeaderText = "序号";
            ID.MinimumWidth = 6;
            ID.Name = "ID";
            ID.SortMode = DataGridViewColumnSortMode.NotSortable;
            ID.Width = 160;
            // 
            // State
            // 
            State.HeaderText = "状态";
            State.MinimumWidth = 6;
            State.Name = "State";
            State.Resizable = DataGridViewTriState.False;
            State.Width = 125;
            // 
            // Title
            // 
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(192, 255, 192);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(192, 0, 0);
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
            Title.DefaultCellStyle = dataGridViewCellStyle3;
            Title.HeaderText = "标题";
            Title.MinimumWidth = 6;
            Title.Name = "Title";
            Title.Resizable = DataGridViewTriState.True;
            Title.Width = 250;
            // 
            // CreateDate
            // 
            CreateDate.HeaderText = "日期时间";
            CreateDate.Items.AddRange(new object[] { "2012-09-12 12:32:11", "2012-09-13 12:32:22", "2012-09-14 12:32:33" });
            CreateDate.MinimumWidth = 6;
            CreateDate.Name = "CreateDate";
            CreateDate.Resizable = DataGridViewTriState.True;
            CreateDate.SortMode = DataGridViewColumnSortMode.Automatic;
            CreateDate.Width = 180;
            // 
            // Operate
            // 
            Operate.HeaderText = "操作";
            Operate.MinimumWidth = 6;
            Operate.Name = "Operate";
            Operate.Width = 160;
            // 
            // PIC
            // 
            PIC.HeaderText = "图片";
            PIC.MinimumWidth = 6;
            PIC.Name = "PIC";
            PIC.Width = 225;
            // 
            // Form1
            // 
            AutoScaleMode = AutoScaleMode.None;
            AutoScroll = true;
            ClientSize = new System.Drawing.Size(1409, 689);
            ContextMenuStrip = contextMenuStrip1;
            Controls.Add(splitContainer2);
            Controls.Add(menuStrip1);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip1;
            Margin = new Padding(4);
            Name = "Form1";
            Text = "默认风格界面";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).EndInit();
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel1.PerformLayout();
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            tabPage2.ResumeLayout(false);
            tabPage2.PerformLayout();
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            flowLayoutPanel1.ResumeLayout(false);
            flowLayoutPanel1.PerformLayout();
            tabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            contextMenuStrip1.ResumeLayout(false);
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            splitContainer2.Panel1.ResumeLayout(false);
            splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
            splitContainer2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.TextBox textBox1;

        private System.Windows.Forms.DataGridView dataGridView1;

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button2;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private System.Windows.Forms.Label label2;
        private CheckBox checkBox1;
        private MaskedTextBox maskedTextBox1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private MaskedTextBox maskedTextBox2;
        private LinkLabel linkLabel1;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem toolStripMenuItem1;
        private ToolStripMenuItem toolStripMenuItem2;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem toolStripMenuItem3;
        private ToolStripMenuItem test1ToolStripMenuItem;
        private ToolStripMenuItem test2ToolStripMenuItem;
        private ToolStripMenuItem toolStripMenuItem4;
        private ToolStripMenuItem 菜单三ToolStripMenuItem;
        private CheckBox checkBox2;
        private DateTimePicker dateTimePicker1;
        private NumericUpDown numericUpDown1;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton3;
        private CheckedListBox checkedListBox1;
        private System.Windows.Forms.TreeView treeView1;
        private ToolStripMenuItem 三级菜单1ToolStripMenuItem;
        private ToolStripMenuItem 二级菜单1ToolStripMenuItem;
        private SplitContainer splitContainer1;
        private PictureBox pictureBox2;
        private ListBox listBox1;
        private FlowLayoutPanel flowLayoutPanel1;
        private Button button4;
        private Button button5;
        private CheckBox checkBox3;
        private Label label4;
        private RichTextBox richTextBox1;
        private TableLayoutPanel tableLayoutPanel1;
        private Label label5;
        private Button button6;
        private Label label6;
        private Button button7;
        private TabPage tabPage3;
        private SplitContainer splitContainer2;
        private Button button3;
        private DataGridViewTextBoxColumn ID;
        private DataGridViewCheckBoxColumn State;
        private DataGridViewTextBoxColumn Title;
        private DataGridViewComboBoxColumn CreateDate;
        private DataGridViewButtonColumn Operate;
        private DataGridViewImageColumn PIC;
    }
}

