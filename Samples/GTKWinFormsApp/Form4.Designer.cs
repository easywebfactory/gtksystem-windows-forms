using System.Windows.Forms;

namespace GTKWinFormsApp
{
    partial class Form4
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
            TreeNode treeNode1 = new TreeNode("节点3");
            TreeNode treeNode2 = new TreeNode("节点2", new TreeNode[] { treeNode1 });
            TreeNode treeNode3 = new TreeNode("节点4");
            TreeNode treeNode4 = new TreeNode("节点0", new TreeNode[] { treeNode2, treeNode3 });
            TreeNode treeNode5 = new TreeNode("节点1");
            splitContainer1 = new SplitContainer();
            treeView1 = new TreeView();
            hScrollBar1 = new HScrollBar();
            vScrollBar1 = new VScrollBar();
            button1 = new Button();
            button2 = new Button();
            button3 = new Button();
            button4 = new Button();
            button5 = new Button();
            button6 = new Button();
            button7 = new Button();
            button8 = new Button();
            button9 = new Button();
            button10 = new Button();
            contextMenuStrip1 = new ContextMenuStrip(components);
            test1ToolStripMenuItem = new ToolStripMenuItem();
            test12ToolStripMenuItem = new ToolStripMenuItem();
            test21ToolStripMenuItem = new ToolStripMenuItem();
            test22ToolStripMenuItem = new ToolStripMenuItem();
            test13ToolStripMenuItem = new ToolStripMenuItem();
            panel1 = new Panel();
            label1 = new Label();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            contextMenuStrip1.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.FixedPanel = FixedPanel.Panel1;
            splitContainer1.Location = new System.Drawing.Point(0, 51);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.AutoScroll = true;
            splitContainer1.Panel1.BackColor = System.Drawing.Color.Transparent;
            splitContainer1.Panel1.Controls.Add(button2);
            splitContainer1.Panel1.Controls.Add(button1);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(hScrollBar1);
            splitContainer1.Panel2.Controls.Add(vScrollBar1);
            splitContainer1.Panel2.Controls.Add(button10);
            splitContainer1.Panel2.Controls.Add(button9);
            splitContainer1.Panel2.Controls.Add(button8);
            splitContainer1.Panel2.Controls.Add(button7);
            splitContainer1.Panel2.Controls.Add(button6);
            splitContainer1.Panel2.Controls.Add(treeView1);
            splitContainer1.Panel2.Controls.Add(button5);
            splitContainer1.Panel2.Controls.Add(panel1);
            splitContainer1.Panel2.Controls.Add(button3);
            splitContainer1.Size = new System.Drawing.Size(895, 441);
            splitContainer1.SplitterDistance = 265;
            splitContainer1.TabIndex = 0;
            // 
            // button2
            // 
            button2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            button2.Location = new System.Drawing.Point(24, 222);
            button2.Name = "button2";
            button2.Size = new System.Drawing.Size(93, 25);
            button2.TabIndex = 1;
            button2.Text = "button2";
            button2.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            button1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right| AnchorStyles.Bottom;
            button1.Location = new System.Drawing.Point(24, 46);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(120, 53);
            button1.TabIndex = 0;
            button1.Text = "button1";
            button1.UseVisualStyleBackColor = true;
            // 
            // button10
            // 
            button10.Location = new System.Drawing.Point(49, 273);
            button10.Name = "button10";
            button10.Size = new System.Drawing.Size(171, 69);
            button10.TabIndex = 2;
            button10.Text = "选择字体";
            button10.UseVisualStyleBackColor = true;
            button10.Click += button10_Click;
            // 
            // hScrollBar1
            // 
            hScrollBar1.Location = new System.Drawing.Point(409, 343);
            hScrollBar1.Name = "hScrollBar1";
            hScrollBar1.Size = new System.Drawing.Size(260, 26);
            hScrollBar1.TabIndex = 8;
            hScrollBar1.ValueChanged += hScrollBar1_ValueChanged;
            // 
            // vScrollBar1
            // 
            vScrollBar1.Location = new System.Drawing.Point(239, 177);
            vScrollBar1.Name = "vScrollBar1";
            vScrollBar1.Size = new System.Drawing.Size(26, 216);
            vScrollBar1.TabIndex = 7;
            vScrollBar1.ValueChanged += vScrollBar1_ValueChanged;
            // 
            // button9
            // 
            button9.Location = new System.Drawing.Point(464, 72);
            button9.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            button9.Name = "button9";
            button9.Size = new System.Drawing.Size(73, 25);
            button9.TabIndex = 6;
            button9.Text = "警告消息";
            button9.UseVisualStyleBackColor = true;
            button9.Click += button9_Click;
            // 
            // button8
            // 
            button8.Location = new System.Drawing.Point(359, 72);
            button8.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            button8.Name = "button8";
            button8.Size = new System.Drawing.Size(73, 25);
            button8.TabIndex = 5;
            button8.Text = "选择颜色";
            button8.UseVisualStyleBackColor = true;
            button8.Click += button8_Click;
            // 
            // button7
            // 
            button7.Location = new System.Drawing.Point(249, 72);
            button7.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            button7.Name = "button7";
            button7.Size = new System.Drawing.Size(73, 25);
            button7.TabIndex = 4;
            button7.Text = "浏览文件夹";
            button7.UseVisualStyleBackColor = true;
            button7.Click += button7_Click;
            // 
            // button6
            // 
            button6.Location = new System.Drawing.Point(147, 72);
            button6.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            button6.Name = "button6";
            button6.Size = new System.Drawing.Size(73, 25);
            button6.TabIndex = 3;
            button6.Text = "保存文件";
            button6.UseVisualStyleBackColor = true;
            button6.Click += button6_Click;
            // 
            // treeView1
            // 
            treeView1.Location = new System.Drawing.Point(49, 154);
            treeView1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            treeView1.Name = "treeView1";
            treeNode1.Name = "节点3";
            treeNode1.Text = "节点3";
            treeNode2.Name = "节点2";
            treeNode2.Text = "节点2";
            treeNode3.Name = "节点4";
            treeNode3.Text = "节点4";
            treeNode4.Name = "节点0";
            treeNode4.Text = "节点0";
            treeNode5.Name = "节点1";
            treeNode5.Text = "节点1";
            treeView1.Nodes.AddRange(new TreeNode[] { treeNode4, treeNode5 });
            treeView1.Size = new System.Drawing.Size(151, 121);
            treeView1.TabIndex = 2;
            // 
            // button5
            // 
            button5.ContextMenuStrip = contextMenuStrip1;
            button5.Dock = DockStyle.Top;
            button5.Location = new System.Drawing.Point(0, 0);
            button5.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            button5.Name = "button5";
            button5.Size = new System.Drawing.Size(626, 50);
            button5.TabIndex = 2;
            button5.Text = "button5";
            button5.UseVisualStyleBackColor = true;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { test1ToolStripMenuItem, test12ToolStripMenuItem, test13ToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new System.Drawing.Size(125, 76);
            // 
            // test1ToolStripMenuItem
            // 
            test1ToolStripMenuItem.Name = "test1ToolStripMenuItem";
            test1ToolStripMenuItem.Size = new System.Drawing.Size(124, 24);
            test1ToolStripMenuItem.Text = "test1";
            // 
            // test12ToolStripMenuItem
            // 
            test12ToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { test21ToolStripMenuItem, test22ToolStripMenuItem });
            test12ToolStripMenuItem.Name = "test12ToolStripMenuItem";
            test12ToolStripMenuItem.Size = new System.Drawing.Size(124, 24);
            test12ToolStripMenuItem.Text = "test12";
            // 
            // test21ToolStripMenuItem
            // 
            test21ToolStripMenuItem.Name = "test21ToolStripMenuItem";
            test21ToolStripMenuItem.Size = new System.Drawing.Size(138, 26);
            test21ToolStripMenuItem.Text = "test21";
            // 
            // test22ToolStripMenuItem
            // 
            test22ToolStripMenuItem.Name = "test22ToolStripMenuItem";
            test22ToolStripMenuItem.Size = new System.Drawing.Size(138, 26);
            test22ToolStripMenuItem.Text = "test22";
            // 
            // test13ToolStripMenuItem
            // 
            test13ToolStripMenuItem.Name = "test13ToolStripMenuItem";
            test13ToolStripMenuItem.Size = new System.Drawing.Size(124, 24);
            test13ToolStripMenuItem.Text = "test13";
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(button4);
            panel1.Location = new System.Drawing.Point(232, 150);
            panel1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(224, 101);
            panel1.TabIndex = 1;
            // 
            // button4
            // 
            button4.Location = new System.Drawing.Point(48, 17);
            button4.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            button4.Name = "button4";
            button4.Size = new System.Drawing.Size(73, 25);
            button4.TabIndex = 0;
            button4.Text = "button4";
            button4.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            button3.Location = new System.Drawing.Point(49, 72);
            button3.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            button3.Name = "button3";
            button3.Size = new System.Drawing.Size(73, 25);
            button3.TabIndex = 0;
            button3.Text = "打开文件";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // label1
            // 
            label1.Dock = DockStyle.Top;
            label1.Font = new System.Drawing.Font("Microsoft YaHei UI", 18F);
            label1.Location = new System.Drawing.Point(0, 0);
            label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(696, 43);
            label1.TabIndex = 1;
            label1.Text = "横幅栏";
            // 
            // Form4
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(895, 492);
            Controls.Add(splitContainer1);
            Controls.Add(label1);
            Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            Name = "Form4";
            Text = "Form4";
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            contextMenuStrip1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem test1ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem test12ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem test21ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem test22ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem test13ToolStripMenuItem;
        
        private System.Windows.Forms.VScrollBar vScrollBar1;
        private System.Windows.Forms.HScrollBar hScrollBar1;
    }
}