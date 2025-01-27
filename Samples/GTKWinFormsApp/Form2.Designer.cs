using System.Windows.Forms;

namespace GTKWinFormsApp
{
    partial class Form2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form2));
            ListViewGroup listViewGroup1 = new ListViewGroup("组头标题", HorizontalAlignment.Left);
            ListViewGroup listViewGroup2 = new ListViewGroup("ListViewGroup2", HorizontalAlignment.Left);
            ListViewItem listViewItem1 = new ListViewItem(new string[] { "ListViewItem 是一个 ContentControl 且只能包含单个子元素。 但是，该子元素可以是任何视觉元素。" }, 0, System.Drawing.Color.Blue, System.Drawing.Color.FromArgb(255, 192, 255), null);
            ListViewItem listViewItem2 = new ListViewItem("ListView 派生自 ListBox。 通常，该控件的项为数据集合的成员，并且表示为 ListViewItem 对象", 0);
            ListViewItem listViewItem3 = new ListViewItem("ListView 控件提供了使用不同布局或视图中显示一组数据项的基础结构。 例如，用户可能需要在表格中显示数据项，并同时对表格的列进行排序。", 0);
            ListViewItem listViewItem4 = new ListViewItem(new string[] { "ListView 分组", "listView1.Groups.Add()", "432321" }, 0);
            ListViewItem listViewItem5 = new ListViewItem(new string[] { "ListView 定义视图模式", "View.SmallIcon", "View.LargeIcon", "View.Details", "View.List" }, 1);
            ListViewItem listViewItem6 = new ListViewItem("什么是 ListView？", 0);
            ListViewItem listViewItem7 = new ListViewItem(new string[] { "将数据绑定到 ListView", "listView1.Items.Add();" }, 0);
            imageList1 = new ImageList(components);
            listView1 = new ListView();
            columnHeader1 = new ColumnHeader();
            columnHeader2 = new ColumnHeader();
            columnHeader3 = new ColumnHeader();
            columnHeader4 = new ColumnHeader();
            columnHeader5 = new ColumnHeader();
            listBox1 = new ListBox();
            richTextBox1 = new RichTextBox();
            label1 = new Label();
            button1 = new Button();
            timer1 = new System.Windows.Forms.Timer(components);
            label2 = new Label();
            SuspendLayout();
            // 
            // imageList1
            // 
            imageList1.ColorDepth = ColorDepth.Depth32Bit;
            imageList1.ImageStream = (ImageListStreamer)resources.GetObject("imageList1.ImageStream");
            imageList1.TransparentColor = System.Drawing.Color.Transparent;
            imageList1.Images.SetKeyName(0, "010.jpg");
            imageList1.Images.SetKeyName(1, "timg2.jpg");
            // 
            // listView1
            // 
            listView1.AllowColumnReorder = true;
            listView1.BackColor = System.Drawing.SystemColors.Window;
            listView1.CheckBoxes = true;
            listView1.Columns.AddRange(new ColumnHeader[] { columnHeader1, columnHeader2, columnHeader3, columnHeader4, columnHeader5 });
            listView1.Dock = DockStyle.Top;
            listView1.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            listView1.FullRowSelect = true;
            listView1.GridLines = true;
            listView1.GroupImageList = imageList1;
            listViewGroup1.Footer = "组脚标题";
            listViewGroup1.Header = "组头标题";
            listViewGroup1.Name = "listViewGroup1";
            listViewGroup1.Subtitle = "组头副标题";
            listViewGroup2.Header = "ListViewGroup2";
            listViewGroup2.Name = "listViewGroup2";
            listViewGroup2.Subtitle = "group2";
            listView1.Groups.AddRange(new ListViewGroup[] { listViewGroup1, listViewGroup2 });
            listViewItem1.Group = listViewGroup1;
            listViewItem1.StateImageIndex = 0;
            listViewItem2.Group = listViewGroup1;
            listViewItem2.StateImageIndex = 0;
            listViewItem3.Group = listViewGroup1;
            listViewItem3.StateImageIndex = 0;
            listViewItem4.Group = listViewGroup2;
            listViewItem4.StateImageIndex = 0;
            listViewItem5.Group = listViewGroup2;
            listViewItem5.StateImageIndex = 0;
            listViewItem6.StateImageIndex = 0;
            listViewItem7.Group = listViewGroup2;
            listViewItem7.StateImageIndex = 0;
            listView1.Items.AddRange(new ListViewItem[] { listViewItem1, listViewItem2, listViewItem3, listViewItem4, listViewItem5, listViewItem6, listViewItem7 });
            listView1.LargeImageList = imageList1;
            listView1.Location = new System.Drawing.Point(0, 0);
            listView1.Name = "listView1";
            listView1.Size = new System.Drawing.Size(1303, 260);
            listView1.SmallImageList = imageList1;
            listView1.Sorting = SortOrder.Descending;
            listView1.TabIndex = 13;
            listView1.UseCompatibleStateImageBehavior = false;
            listView1.View = View.Details;
            listView1.ColumnClick += listView1_ColumnClick;
            listView1.ColumnReordered += listView1_ColumnReordered;
            listView1.ItemCheck += listView1_ItemCheck;
            listView1.ItemChecked += listView1_ItemChecked;
            listView1.ItemSelectionChanged += listView1_ItemSelectionChanged;
            listView1.SelectedIndexChanged += listView1_SelectedIndexChanged;
            listView1.Click += listView1_Click;
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "组1";
            columnHeader1.Width = 300;
            // 
            // columnHeader2
            // 
            columnHeader2.DisplayIndex = 2;
            columnHeader2.Text = "组2fffffffffffffffffffffffffffffff";
            columnHeader2.Width = 100;
            // 
            // columnHeader3
            // 
            columnHeader3.DisplayIndex = 1;
            columnHeader3.Text = "组3";
            columnHeader3.Width = 100;
            // 
            // columnHeader4
            // 
            columnHeader4.Text = "4";
            // 
            // columnHeader5
            // 
            columnHeader5.Text = "5";
            // 
            // listBox1
            // 
            listBox1.BackColor = System.Drawing.Color.Yellow;
            listBox1.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            listBox1.FormattingEnabled = true;
            listBox1.Items.AddRange(new object[] { "0-异常警告0 --- 机房空调运行监控事件 --- 638526419125153303 ------ 2024/5/30 4:58:32", "1-异常警告1 --- 机房空调运行监控事件 --- 638526419135198480 ------ 2024/5/30 4:58:33", "2-异常警告2 --- 机房空调运行监控事件 --- 638526419155233741 ------ 2024/5/30 4:58:35", "3-异常警告3 --- 机房空调运行监控事件 --- 638526419495254800 ------ 2024/5/30 4:59:09", "4-异常警告4 --- 机房空调运行监控事件 --- 638526419505198651 ------ 2024/5/30 4:59:10", "5-异常警告5 --- 机房空调运行监控事件 --- 638526419565218661 ------ 2024/5/30 4:59:16" });
            listBox1.Location = new System.Drawing.Point(0, 455);
            listBox1.Name = "listBox1";
            listBox1.Size = new System.Drawing.Size(720, 150);
            listBox1.TabIndex = 3;
            listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged;
            // 
            // richTextBox1
            // 
            richTextBox1.BackColor = System.Drawing.Color.AliceBlue;
            richTextBox1.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F);
            richTextBox1.Location = new System.Drawing.Point(740, 455);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new System.Drawing.Size(551, 150);
            richTextBox1.TabIndex = 14;
            richTextBox1.Text = "rich text";
            richTextBox1.BackgroundImage = (System.Drawing.Image)resources.GetObject("$this.BackgroundImage");
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            label1.Location = new System.Drawing.Point(421, 263);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(882, 115);
            label1.TabIndex = 15;
            label1.Text = resources.GetString("label1.Text");
            // 
            // button1
            // 
            button1.Location = new System.Drawing.Point(45, 277);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(214, 46);
            button1.TabIndex = 16;
            button1.Text = "add listview item";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // timer1
            // 
            timer1.Enabled = true;
            timer1.Interval = 2000;
            timer1.Tick += Timer1_Tick;
            // 
            // label2
            // 
            label2.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold);
            label2.Location = new System.Drawing.Point(0, 429);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(720, 25);
            label2.TabIndex = 17;
            label2.Text = ">监控滚动事件日志";
            // 
            // Form2
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = (System.Drawing.Image)resources.GetObject("$this.BackgroundImage");
            ClientSize = new System.Drawing.Size(1303, 620);
            Controls.Add(label2);
            Controls.Add(button1);
            Controls.Add(label1);
            Controls.Add(richTextBox1);
            Controls.Add(listBox1);
            Controls.Add(listView1);
            Name = "Form2";
            Text = "Form2";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ListBox listBox1;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
        private ColumnHeader columnHeader3;
        private RichTextBox richTextBox1;
        private Label label1;
        private Button button1;
        private ColumnHeader columnHeader4;
        private ColumnHeader columnHeader5;
        private System.Windows.Forms.Timer timer1;
        private Label label2;
    }
}