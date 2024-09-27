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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("节点3");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("节点2", new System.Windows.Forms.TreeNode[] { treeNode1 });
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("节点4");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("节点0", new System.Windows.Forms.TreeNode[] { treeNode2, treeNode3 });
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("节点1");
            splitContainer1 = new System.Windows.Forms.SplitContainer();
            button2 = new System.Windows.Forms.Button();
            button1 = new System.Windows.Forms.Button();
            button10 = new System.Windows.Forms.Button();
            button9 = new System.Windows.Forms.Button();
            button8 = new System.Windows.Forms.Button();
            button7 = new System.Windows.Forms.Button();
            button6 = new System.Windows.Forms.Button();
            treeView1 = new System.Windows.Forms.TreeView();
            button5 = new System.Windows.Forms.Button();
            panel1 = new System.Windows.Forms.Panel();
            button4 = new System.Windows.Forms.Button();
            button3 = new System.Windows.Forms.Button();
            label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            splitContainer1.Location = new System.Drawing.Point(0, 43);
            splitContainer1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            splitContainer1.Panel1.Controls.Add(button2);
            splitContainer1.Panel1.Controls.Add(button1);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(button10);
            splitContainer1.Panel2.Controls.Add(button9);
            splitContainer1.Panel2.Controls.Add(button8);
            splitContainer1.Panel2.Controls.Add(button7);
            splitContainer1.Panel2.Controls.Add(button6);
            splitContainer1.Panel2.Controls.Add(treeView1);
            splitContainer1.Panel2.Controls.Add(button5);
            splitContainer1.Panel2.Controls.Add(panel1);
            splitContainer1.Panel2.Controls.Add(button3);
            splitContainer1.Size = new System.Drawing.Size(696, 375);
            splitContainer1.SplitterDistance = 128;
            splitContainer1.SplitterWidth = 3;
            splitContainer1.TabIndex = 0;
            // 
            // button2
            // 
            button2.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            button2.Location = new System.Drawing.Point(19, 189);
            button2.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            button2.Name = "button2";
            button2.Size = new System.Drawing.Size(93, 25);
            button2.TabIndex = 1;
            button2.Text = "button2";
            button2.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            button1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            button1.Location = new System.Drawing.Point(19, 39);
            button1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(93, 130);
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
            treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] { treeNode4, treeNode5 });
            treeView1.Size = new System.Drawing.Size(118, 103);
            treeView1.TabIndex = 2;
            // 
            // button5
            // 
            button5.Dock = System.Windows.Forms.DockStyle.Top;
            button5.Location = new System.Drawing.Point(0, 0);
            button5.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            button5.Name = "button5";
            button5.Size = new System.Drawing.Size(565, 42);
            button5.TabIndex = 2;
            button5.Text = "button5";
            button5.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            panel1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
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
            label1.Dock = System.Windows.Forms.DockStyle.Top;
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
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(696, 418);
            Controls.Add(splitContainer1);
            Controls.Add(label1);
            Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            Name = "Form4";
            Text = "Form4";
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
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
    }
}