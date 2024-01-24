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
            ListViewGroup listViewGroup3 = new ListViewGroup("ListViewGroup1", HorizontalAlignment.Left);
            ListViewGroup listViewGroup4 = new ListViewGroup("ListViewGroup2", HorizontalAlignment.Left);

            ListViewItem listViewItem4 = new ListViewItem("test1ddddddddddddddddddddddddddddddddddddddddddddddddddddd", 1);
            ListViewItem listViewItem5 = new ListViewItem(new string[] { "特别说明：本框架不支持ImageList使用窗口设计器（资源数据反序列化加解密没解决", "sssss特别说明：本框架不支持ImageList使用窗口设计器（资源数据反序列化加解密没解决", "ffffffff特别说明：本框架不支持ImageList使用窗口设计器（资源数据反序列化加解密没解决" }, 0);
            ListViewItem listViewItem6 = new ListViewItem("ttt3", 1);
            ListViewItem listViewItem7 = new ListViewItem("ttt31", 1);
            ListViewItem listViewItem8 = new ListViewItem("ttt32", 1);
            ListViewItem listViewItem9 = new ListViewItem("ttt33", 1);
            ListViewItem listViewItem10 = new ListViewItem("ttt34", 1);
            imageList1 = new ImageList(components);
            listView1 = new ListView();
            columnHeader1 = new ColumnHeader();
            columnHeader1.Width = 200;
            columnHeader1.Text= "组1";
            columnHeader2 = new ColumnHeader();
            columnHeader2.Text = "组2";
            columnHeader3 = new ColumnHeader();
            columnHeader3.Text = "组3";
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
            // 
            // listView1
            // 
            listView1.CheckBoxes = true;
            listView1.Columns.AddRange(new ColumnHeader[] { columnHeader1, columnHeader2, columnHeader3 });
            listView1.FullRowSelect = true;
            listView1.GridLines = true;
            listViewGroup3.Footer = "ggg1";
            listViewGroup3.Header = "ListViewGroup1";
            listViewGroup3.Name = "listViewGroup1";
            listViewGroup3.Subtitle = "group1";
            listViewGroup4.Header = "ListViewGroup2";
            listViewGroup4.Name = "listViewGroup2";
            listViewGroup4.Subtitle = "group2";
            listView1.Groups.AddRange(new ListViewGroup[] { listViewGroup3, listViewGroup4 });
            listViewItem4.Group = listViewGroup3;
            listViewItem4.StateImageIndex = 0;
            listViewItem5.Group = listViewGroup4;
            listViewItem5.StateImageIndex = 0;
            listViewItem6.Group = listViewGroup3;
            listViewItem6.StateImageIndex = 0;

            listViewItem7.Group = listViewGroup3;
            listViewItem7.StateImageIndex = 0;
            listViewItem8.Group = listViewGroup3;
            listViewItem8.StateImageIndex = 0;
            listViewItem9.Group = listViewGroup3;
            listViewItem9.StateImageIndex = 0;
            listViewItem10.Group = listViewGroup3;
            listViewItem10.StateImageIndex = 0;

            listView1.Items.AddRange(new ListViewItem[] { listViewItem4, listViewItem5, listViewItem6, listViewItem7, listViewItem8, listViewItem9, listViewItem10 });
            listView1.LargeImageList = imageList1;
            listView1.Location = new System.Drawing.Point(80, 16);
            listView1.Name = "listView1";
            listView1.Size = new System.Drawing.Size(400, 163);
            listView1.SmallImageList = imageList1;
            listView1.TabIndex = 13;
            listView1.UseCompatibleStateImageBehavior = false;
            listView1.ColumnClick += listView1_ColumnClick;
            listView1.ItemCheck += listView1_ItemCheck;
            listView1.ItemChecked += listView1_ItemChecked;
            listView1.ItemSelectionChanged += listView1_ItemSelectionChanged;
            listView1.SelectedIndexChanged += listView1_SelectedIndexChanged;
            listView1.Click += listView1_Click;
            listView1.View = View.SmallIcon;
            // 
            // listBox1
            // 
            listBox1.FormattingEnabled = true;
            listBox1.Items.AddRange(new object[] { "sdfdfdf", "ssss", "fffff", "ddddddddd", "ddddddd" });
            listBox1.Location = new System.Drawing.Point(580, 31);
            listBox1.Name = "listBox1";
            listBox1.Size = new System.Drawing.Size(150, 104);
            listBox1.TabIndex = 3;
            // 
            // richTextBox1
            // 
            richTextBox1.Location = new System.Drawing.Point(102, 318);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new System.Drawing.Size(549, 120);
            richTextBox1.TabIndex = 14;
            richTextBox1.Text = "";
            richTextBox1.BackColor = System.Drawing.Color.Yellow;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            label1.Location = new System.Drawing.Point(24, 213);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(800, 92);
            label1.TabIndex = 15;
            label1.Text = "特别说明：本框架不支持ImageList使用窗口设计器（资源数据反序列化加解密没解决）\r\n当使用ImageList添加图片时不会保存，当界面设计程序有imageList1.ImageStream时，设计器窗口打不开，\r\n但可以使用程序添加，\r\n所添加的图片必须放到根目录下的Resource目录下（此目录下可进一步增加指定目录(如imageList1)）";
            // 
            // Form2
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = (System.Drawing.Image)resources.GetObject("$this.BackgroundImage");
            ClientSize = new System.Drawing.Size(972, 480);
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