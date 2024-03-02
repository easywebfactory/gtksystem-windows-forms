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
            ListViewGroup listViewGroup1 = new ListViewGroup("ListViewGroup1", HorizontalAlignment.Left);
            ListViewGroup listViewGroup2 = new ListViewGroup("ListViewGroup2", HorizontalAlignment.Left);
            ListViewItem listViewItem1 = new ListViewItem("test1ddddddddddddddddddddddddddddddddddddddddddddddddddddd", 1);
            ListViewItem listViewItem2 = new ListViewItem(new string[] { "特别说明：本框架不支持ImageList使用窗口设计器（资源数据反序列化加解密没解决", "sssss特别说明：本框架不支持ImageList使用窗口设计器（资源数据反序列化加解密没解决", "ffffffff特别说明：本框架不支持ImageList使用窗口设计器（资源数据反序列化加解密没解决" }, 0);
            ListViewItem listViewItem3 = new ListViewItem("ttt3", 1);
            ListViewItem listViewItem4 = new ListViewItem("ttt31", 1);
            ListViewItem listViewItem5 = new ListViewItem("ttt32", 1);
            ListViewItem listViewItem6 = new ListViewItem("ttt33", 1);
            ListViewItem listViewItem7 = new ListViewItem("ttt34", 1);
            imageList1 = new ImageList(components);
            listView1 = new ListView();
            columnHeader1 = new ColumnHeader();
            columnHeader2 = new ColumnHeader();
            columnHeader3 = new ColumnHeader();
            listBox1 = new ListBox();
            richTextBox1 = new RichTextBox();
            label1 = new Label();
            SuspendLayout();
            // 
            // imageList1
            // 
            imageList1.ColorDepth = ColorDepth.Depth32Bit;
            imageList1.ImageStream = (ImageListStreamer)resources.GetObject("imageList1.ImageStream");
            imageList1.TransparentColor = System.Drawing.Color.Transparent;
            imageList1.Images.SetKeyName(0, "010.jpg");
            imageList1.Images.SetKeyName(1, "timg2.jpg");
            imageList1.Images.SetKeyName(2, "0062a864_E964096_2d030bca.jpg");
            // 
            // listView1
            // 
            listView1.CheckBoxes = true;
            listView1.Columns.AddRange(new ColumnHeader[] { columnHeader1, columnHeader2, columnHeader3 });
            listView1.FullRowSelect = true;
            listView1.GridLines = true;
            listViewGroup1.Footer = "ggg1";
            listViewGroup1.Header = "ListViewGroup1";
            listViewGroup1.Name = "listViewGroup1";
            listViewGroup1.Subtitle = "group1";
            listViewGroup2.Header = "ListViewGroup2";
            listViewGroup2.Name = "listViewGroup2";
            listViewGroup2.Subtitle = "group2";
            listView1.Groups.AddRange(new ListViewGroup[] { listViewGroup1, listViewGroup2 });
            listViewItem1.Group = listViewGroup1;
            listViewItem1.StateImageIndex = 0;
            listViewItem2.Group = listViewGroup2;
            listViewItem2.StateImageIndex = 0;
            listViewItem3.Group = listViewGroup1;
            listViewItem3.StateImageIndex = 0;
            listViewItem4.Group = listViewGroup1;
            listViewItem4.StateImageIndex = 0;
            listViewItem5.Group = listViewGroup1;
            listViewItem5.StateImageIndex = 0;
            listViewItem6.Group = listViewGroup1;
            listViewItem6.StateImageIndex = 0;
            listViewItem7.Group = listViewGroup1;
            listViewItem7.StateImageIndex = 0;
            listView1.Items.AddRange(new ListViewItem[] { listViewItem1, listViewItem2, listViewItem3, listViewItem4, listViewItem5, listViewItem6, listViewItem7 });
            listView1.LargeImageList = imageList1;
            listView1.Location = new System.Drawing.Point(105, 12);
            listView1.Name = "listView1";
            listView1.Size = new System.Drawing.Size(419, 184);
            listView1.SmallImageList = imageList1;
            listView1.TabIndex = 13;
            listView1.UseCompatibleStateImageBehavior = false;
            listView1.View = View.SmallIcon;
            listView1.ColumnClick += listView1_ColumnClick;
            listView1.ItemCheck += listView1_ItemCheck;
            listView1.ItemChecked += listView1_ItemChecked;
            listView1.ItemSelectionChanged += listView1_ItemSelectionChanged;
            listView1.SelectedIndexChanged += listView1_SelectedIndexChanged;
            listView1.Click += listView1_Click;
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "组1";
            columnHeader1.Width = 200;
            // 
            // columnHeader2
            // 
            columnHeader2.Text = "组2";
            // 
            // columnHeader3
            // 
            columnHeader3.Text = "组3";
            // 
            // listBox1
            // 
            listBox1.FormattingEnabled = true;
            listBox1.Items.AddRange(new object[] { "sdfdfdf", "ssss", "fffff", "ddddddddd", "ddddddd" });
            listBox1.Location = new System.Drawing.Point(671, 41);
            listBox1.Name = "listBox1";
            listBox1.Size = new System.Drawing.Size(150, 104);
            listBox1.TabIndex = 3;
            // 
            // richTextBox1
            // 
            richTextBox1.BackColor = System.Drawing.Color.Yellow;
            richTextBox1.Location = new System.Drawing.Point(105, 380);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new System.Drawing.Size(549, 120);
            richTextBox1.TabIndex = 14;
            richTextBox1.Text = "";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F);
            label1.Location = new System.Drawing.Point(48, 214);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(1032, 135);
            label1.TabIndex = 15;
            label1.Text = resources.GetString("label1.Text");
            // 
            // Form2
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = (System.Drawing.Image)resources.GetObject("$this.BackgroundImage");
            ClientSize = new System.Drawing.Size(1136, 658);
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
    }
}