using System.Drawing;
using System.Windows.Forms;

namespace GTKWinFormsApp
{
    partial class Test1
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Test1));
            dataGridView1 = new DataGridView();
            id = new DataGridViewTextBoxColumn();
            project = new DataGridViewTextBoxColumn();
            state = new DataGridViewTextBoxColumn();
            testnum = new DataGridViewTextBoxColumn();
            panel1 = new Panel();
            tableLayoutPanel1 = new TableLayoutPanel();
            panel2 = new Panel();
            label5 = new Label();
            label4 = new Label();
            pictureBox_CC1 = new PictureBox();
            pictureBox_PC1 = new PictureBox();
            label3 = new Label();
            label2 = new Label();
            panel3 = new Panel();
            label9 = new Label();
            label8 = new Label();
            label7 = new Label();
            label6 = new Label();
            pictureBox_CC2 = new PictureBox();
            pictureBox_PC2 = new PictureBox();
            panel4 = new Panel();
            pictureBox_PC3 = new PictureBox();
            label11 = new Label();
            label10 = new Label();
            panel5 = new Panel();
            label15 = new Label();
            pictureBox_PC4 = new PictureBox();
            label14 = new Label();
            pictureBox_CC4 = new PictureBox();
            label13 = new Label();
            label12 = new Label();
            label1 = new Label();
            pictureBox_chart = new PictureBox();
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
            button11 = new Button();
            button12 = new Button();
            button13 = new Button();
            button14 = new Button();
            button15 = new Button();
            label16 = new Label();
            button16 = new Button();
            button17 = new Button();
            radioButton1 = new RadioButton();
            radioButton2 = new RadioButton();
            radioButton3 = new RadioButton();
            radioButton4 = new RadioButton();
            radioButton5 = new RadioButton();
            radioButton6 = new RadioButton();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            panel1.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox_CC1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox_PC1).BeginInit();
            panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox_CC2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox_PC2).BeginInit();
            panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox_PC3).BeginInit();
            panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox_PC4).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox_CC4).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox_chart).BeginInit();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridViewCellStyle1.BackColor = Color.FromArgb(224, 224, 224);
            dataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(40, 128, 192);
            dataGridViewCellStyle2.Font = new Font("Microsoft YaHei UI", 9F);
            dataGridViewCellStyle2.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { id, project, state, testnum });
            dataGridView1.Location = new Point(33, 240);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.ReadOnly = true;
            dataGridView1.RowHeadersWidth = 51;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.FromArgb(192, 192, 192);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(0, 0, 0);
            dataGridView1.RowsDefaultCellStyle = dataGridViewCellStyle3;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.Size = new Size(291, 598);
            dataGridView1.TabIndex = 0;
            // 
            // id
            // 
            id.DataPropertyName = "id";
            id.HeaderText = "序号";
            id.MinimumWidth = 6;
            id.Name = "id";
            id.ReadOnly = true;
            id.Width = 60;
            // 
            // project
            // 
            project.DataPropertyName = "project";
            project.HeaderText = "项目";
            project.MinimumWidth = 6;
            project.Name = "project";
            project.ReadOnly = true;
            project.Width = 125;
            // 
            // state
            // 
            state.DataPropertyName = "state";
            state.HeaderText = "状态";
            state.MinimumWidth = 6;
            state.Name = "state";
            state.ReadOnly = true;
            state.Width = 60;
            // 
            // testnum
            // 
            testnum.DataPropertyName = "testnum";
            testnum.HeaderText = "测试数";
            testnum.MinimumWidth = 6;
            testnum.Name = "testnum";
            testnum.ReadOnly = true;
            testnum.Width = 60;
            // 
            // panel1
            // 
            panel1.Controls.Add(tableLayoutPanel1);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(pictureBox_chart);
            panel1.Location = new Point(330, 240);
            panel1.Name = "panel1";
            panel1.Size = new Size(777, 598);
            panel1.TabIndex = 1;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(panel2, 0, 0);
            tableLayoutPanel1.Controls.Add(panel3, 1, 0);
            tableLayoutPanel1.Controls.Add(panel4, 0, 1);
            tableLayoutPanel1.Controls.Add(panel5, 1, 1);
            tableLayoutPanel1.Location = new Point(415, 51);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Size = new Size(352, 531);
            tableLayoutPanel1.TabIndex = 2;
            // 
            // panel2
            // 
            panel2.Controls.Add(label5);
            panel2.Controls.Add(label4);
            panel2.Controls.Add(pictureBox_CC1);
            panel2.Controls.Add(pictureBox_PC1);
            panel2.Controls.Add(label3);
            panel2.Controls.Add(label2);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(3, 3);
            panel2.Name = "panel2";
            panel2.Size = new Size(170, 259);
            panel2.TabIndex = 0;
            panel2.BorderStyle = BorderStyle.None;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(102, 208);
            label5.Name = "label5";
            label5.Size = new Size(38, 20);
            label5.TabIndex = 3;
            label5.Text = "CC1";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(17, 208);
            label4.Name = "label4";
            label4.Size = new Size(37, 20);
            label4.TabIndex = 3;
            label4.Text = "PC1";
            // 
            // pictureBox_CC1
            // 
            pictureBox_CC1.BackColor = Color.White;
            pictureBox_CC1.BorderStyle = BorderStyle.FixedSingle;
            pictureBox_CC1.Location = new Point(98, 37);
            pictureBox_CC1.Name = "pictureBox_CC1";
            pictureBox_CC1.Size = new Size(57, 168);
            pictureBox_CC1.TabIndex = 2;
            pictureBox_CC1.TabStop = false;
            pictureBox_CC1.Paint += pictureBox_CC1_Paint;
            // 
            // pictureBox_PC1
            // 
            pictureBox_PC1.BackColor = Color.White;
            pictureBox_PC1.BorderStyle = BorderStyle.FixedSingle;
            pictureBox_PC1.Location = new Point(13, 37);
            pictureBox_PC1.Name = "pictureBox_PC1";
            pictureBox_PC1.Size = new Size(57, 168);
            pictureBox_PC1.TabIndex = 2;
            pictureBox_PC1.TabStop = false;
            pictureBox_PC1.Paint += pictureBox_PC1_Paint;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold);
            label3.Location = new Point(102, 15);
            label3.Name = "label3";
            label3.Size = new Size(41, 19);
            label3.TabIndex = 1;
            label3.Text = "50%";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold);
            label2.Location = new Point(17, 15);
            label2.Name = "label2";
            label2.Size = new Size(41, 19);
            label2.TabIndex = 0;
            label2.Text = "50%";
            // 
            // panel3
            // 
            panel3.Controls.Add(label9);
            panel3.Controls.Add(label8);
            panel3.Controls.Add(label7);
            panel3.Controls.Add(label6);
            panel3.Controls.Add(pictureBox_CC2);
            panel3.Controls.Add(pictureBox_PC2);
            panel3.Dock = DockStyle.Fill;
            panel3.Location = new Point(179, 3);
            panel3.Name = "panel3";
            panel3.Size = new Size(170, 259);
            panel3.TabIndex = 1;
            panel3.BorderStyle = BorderStyle.None;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(109, 208);
            label9.Name = "label9";
            label9.Size = new Size(38, 20);
            label9.TabIndex = 3;
            label9.Text = "CC2";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(24, 208);
            label8.Name = "label8";
            label8.Size = new Size(37, 20);
            label8.TabIndex = 3;
            label8.Text = "PC2";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold);
            label7.Location = new Point(106, 15);
            label7.Name = "label7";
            label7.Size = new Size(41, 19);
            label7.TabIndex = 0;
            label7.Text = "50%";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold);
            label6.Location = new Point(16, 15);
            label6.Name = "label6";
            label6.Size = new Size(41, 19);
            label6.TabIndex = 0;
            label6.Text = "50%";
            // 
            // pictureBox_CC2
            // 
            pictureBox_CC2.BackColor = Color.White;
            pictureBox_CC2.BorderStyle = BorderStyle.FixedSingle;
            pictureBox_CC2.Location = new Point(102, 37);
            pictureBox_CC2.Name = "pictureBox_CC2";
            pictureBox_CC2.Size = new Size(57, 168);
            pictureBox_CC2.TabIndex = 2;
            pictureBox_CC2.TabStop = false;
            pictureBox_CC2.Paint += pictureBox_CC2_Paint;
            // 
            // pictureBox_PC2
            // 
            pictureBox_PC2.BackColor = Color.White;
            pictureBox_PC2.BorderStyle = BorderStyle.FixedSingle;
            pictureBox_PC2.Location = new Point(17, 37);
            pictureBox_PC2.Name = "pictureBox_PC2";
            pictureBox_PC2.Size = new Size(57, 168);
            pictureBox_PC2.TabIndex = 2;
            pictureBox_PC2.TabStop = false;
            pictureBox_PC2.Paint += pictureBox_PC2_Paint;
            // 
            // panel4
            // 
            panel4.Controls.Add(pictureBox_PC3);
            panel4.Controls.Add(label11);
            panel4.Controls.Add(label10);
            panel4.Dock = DockStyle.Fill;
            panel4.Location = new Point(3, 268);
            panel4.Name = "panel4";
            panel4.Size = new Size(170, 260);
            panel4.TabIndex = 2;
            panel4.BorderStyle = BorderStyle.None;
            // 
            // pictureBox_PC3
            // 
            pictureBox_PC3.BackColor = Color.White;
            pictureBox_PC3.BorderStyle = BorderStyle.FixedSingle;
            pictureBox_PC3.Location = new Point(55, 35);
            pictureBox_PC3.Name = "pictureBox_PC3";
            pictureBox_PC3.Size = new Size(57, 168);
            pictureBox_PC3.TabIndex = 2;
            pictureBox_PC3.TabStop = false;
            pictureBox_PC3.Paint += pictureBox_PC3_Paint;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(59, 206);
            label11.Name = "label11";
            label11.Size = new Size(37, 20);
            label11.TabIndex = 3;
            label11.Text = "PC3";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold);
            label10.Location = new Point(59, 13);
            label10.Name = "label10";
            label10.Size = new Size(41, 19);
            label10.TabIndex = 0;
            label10.Text = "50%";
            // 
            // panel5
            // 
            panel5.Controls.Add(label15);
            panel5.Controls.Add(pictureBox_PC4);
            panel5.Controls.Add(label14);
            panel5.Controls.Add(pictureBox_CC4);
            panel5.Controls.Add(label13);
            panel5.Controls.Add(label12);
            panel5.Dock = DockStyle.Fill;
            panel5.Location = new Point(179, 268);
            panel5.Name = "panel5";
            panel5.Size = new Size(170, 260);
            panel5.TabIndex = 3;
            panel5.BorderStyle = BorderStyle.None;
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Location = new Point(116, 206);
            label15.Name = "label15";
            label15.Size = new Size(38, 20);
            label15.TabIndex = 3;
            label15.Text = "CC4";
            // 
            // pictureBox_PC4
            // 
            pictureBox_PC4.BackColor = Color.White;
            pictureBox_PC4.BorderStyle = BorderStyle.FixedSingle;
            pictureBox_PC4.Location = new Point(24, 35);
            pictureBox_PC4.Name = "pictureBox_PC4";
            pictureBox_PC4.Size = new Size(57, 168);
            pictureBox_PC4.TabIndex = 2;
            pictureBox_PC4.TabStop = false;
            pictureBox_PC4.Paint += pictureBox_PC4_Paint;
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Location = new Point(31, 206);
            label14.Name = "label14";
            label14.Size = new Size(37, 20);
            label14.TabIndex = 3;
            label14.Text = "PC4";
            // 
            // pictureBox_CC4
            // 
            pictureBox_CC4.BackColor = Color.White;
            pictureBox_CC4.BorderStyle = BorderStyle.FixedSingle;
            pictureBox_CC4.Location = new Point(109, 35);
            pictureBox_CC4.Name = "pictureBox_CC4";
            pictureBox_CC4.Size = new Size(57, 168);
            pictureBox_CC4.TabIndex = 2;
            pictureBox_CC4.TabStop = false;
            pictureBox_CC4.Paint += pictureBox_CC4_Paint;
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold);
            label13.Location = new Point(113, 13);
            label13.Name = "label13";
            label13.Size = new Size(41, 19);
            label13.TabIndex = 0;
            label13.Text = "50%";
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold);
            label12.Location = new Point(23, 13);
            label12.Name = "label12";
            label12.Size = new Size(41, 19);
            label12.TabIndex = 0;
            label12.Text = "50%";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Microsoft YaHei UI", 11F, FontStyle.Bold);
            label1.Location = new Point(25, 19);
            label1.Name = "label1";
            label1.Size = new Size(161, 26);
            label1.TabIndex = 0;
            label1.Text = "XXX项目分析中...";
            // 
            // pictureBox_chart
            // 
            pictureBox_chart.BackColor = SystemColors.Control;
            pictureBox_chart.Location = new Point(3, 51);
            pictureBox_chart.Name = "pictureBox_chart";
            pictureBox_chart.Size = new Size(404, 528);
            pictureBox_chart.TabIndex = 1;
            pictureBox_chart.TabStop = false;
            pictureBox_chart.Paint += pictureBox_chart_Paint;
            // 
            // button1
            // 
            button1.Location = new Point(33, 881);
            button1.Name = "button1";
            button1.Size = new Size(94, 43);
            button1.TabIndex = 2;
            button1.Text = "打印查看";
            button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            button2.Location = new Point(173, 881);
            button2.Name = "button2";
            button2.Size = new Size(94, 43);
            button2.TabIndex = 2;
            button2.Text = "指标1";
            button2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            button3.Location = new Point(313, 881);
            button3.Name = "button3";
            button3.Size = new Size(94, 43);
            button3.TabIndex = 2;
            button3.Text = "指标2";
            button3.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            button4.Location = new Point(453, 881);
            button4.Name = "button4";
            button4.Size = new Size(94, 43);
            button4.TabIndex = 2;
            button4.Text = "指标3";
            button4.UseVisualStyleBackColor = true;
            // 
            // button5
            // 
            button5.Location = new Point(593, 881);
            button5.Name = "button5";
            button5.Size = new Size(94, 43);
            button5.TabIndex = 2;
            button5.Text = "指标4";
            button5.UseVisualStyleBackColor = true;
            // 
            // button6
            // 
            button6.Location = new Point(733, 881);
            button6.Name = "button6";
            button6.Size = new Size(94, 43);
            button6.TabIndex = 2;
            button6.Text = "指标5";
            button6.UseVisualStyleBackColor = true;
            // 
            // button7
            // 
            button7.Location = new Point(873, 881);
            button7.Name = "button7";
            button7.Size = new Size(94, 43);
            button7.TabIndex = 2;
            button7.Text = "指标6";
            button7.UseVisualStyleBackColor = true;
            // 
            // button8
            // 
            button8.Location = new Point(1013, 881);
            button8.Name = "button8";
            button8.Size = new Size(94, 43);
            button8.TabIndex = 2;
            button8.Text = "指标7";
            button8.UseVisualStyleBackColor = true;
            // 
            // button9
            // 
            button9.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Bold);
            button9.Location = new Point(1156, 123);
            button9.Name = "button9";
            button9.Size = new Size(94, 84);
            button9.TabIndex = 3;
            button9.Text = "停止";
            button9.TextAlign = ContentAlignment.BottomCenter;
            button9.UseVisualStyleBackColor = false;
            // 
            // button10
            // 
            button10.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Bold);
            button10.Location = new Point(1156, 240);
            button10.Name = "button10";
            button10.Size = new Size(94, 84);
            button10.TabIndex = 3;
            button10.Text = "注销";
            button10.TextAlign = ContentAlignment.BottomCenter;
            button10.UseVisualStyleBackColor = true;
            // 
            // button11
            // 
            button11.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Bold);
            button11.Location = new Point(1156, 357);
            button11.Name = "button11";
            button11.Size = new Size(94, 84);
            button11.TabIndex = 3;
            button11.Text = "进样停止";
            button11.TextAlign = ContentAlignment.BottomCenter;
            button11.UseVisualStyleBackColor = true;
            // 
            // button12
            // 
            button12.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Bold);
            button12.Location = new Point(1156, 474);
            button12.Name = "button12";
            button12.Size = new Size(94, 84);
            button12.TabIndex = 3;
            button12.Text = "急诊模式";
            button12.TextAlign = ContentAlignment.BottomCenter;
            button12.UseVisualStyleBackColor = true;
            // 
            // button13
            // 
            button13.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Bold);
            button13.Location = new Point(1156, 591);
            button13.Name = "button13";
            button13.Size = new Size(94, 84);
            button13.TabIndex = 3;
            button13.Text = "报警";
            button13.TextAlign = ContentAlignment.BottomCenter;
            button13.UseVisualStyleBackColor = true;
            // 
            // button14
            // 
            button14.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Bold);
            button14.Location = new Point(1156, 708);
            button14.Name = "button14";
            button14.Size = new Size(94, 84);
            button14.TabIndex = 3;
            button14.Text = "打印";
            button14.TextAlign = ContentAlignment.BottomCenter;
            button14.UseVisualStyleBackColor = true;
            // 
            // button15
            // 
            button15.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Bold);
            button15.Location = new Point(1156, 825);
            button15.Name = "button15";
            button15.Size = new Size(94, 84);
            button15.TabIndex = 3;
            button15.Text = "开始";
            button15.TextAlign = ContentAlignment.BottomCenter;
            button15.UseVisualStyleBackColor = true;
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold);
            label16.Location = new Point(760, 35);
            label16.Name = "label16";
            label16.Size = new Size(191, 19);
            label16.TabIndex = 4;
            label16.Text = "2025/8/10 (星期六) 22:03";
            // 
            // button16
            // 
            button16.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Bold);
            button16.Location = new Point(1042, 25);
            button16.Name = "button16";
            button16.Size = new Size(90, 40);
            button16.TabIndex = 5;
            button16.Text = "帮助";
            button16.TextAlign = ContentAlignment.MiddleRight;
            button16.UseVisualStyleBackColor = true;
            // 
            // button17
            // 
            button17.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Bold);
            button17.Location = new Point(1160, 25);
            button17.Name = "button17";
            button17.Size = new Size(90, 40);
            button17.TabIndex = 5;
            button17.Text = "打印";
            button17.TextAlign = ContentAlignment.MiddleRight;
            button17.UseVisualStyleBackColor = true;
            button17.Click += button17_Click;
            // 
            // radioButton1
            // 
            radioButton1.Appearance = Appearance.Button;
            radioButton1.Font = new Font("Microsoft YaHei UI", 11F, FontStyle.Bold);
            radioButton1.Location = new Point(45, 153);
            radioButton1.Name = "radioButton1";
            radioButton1.Size = new Size(139, 55);
            radioButton1.TabIndex = 7;
            radioButton1.TabStop = true;
            radioButton1.Text = "维护";
            radioButton1.TextAlign = ContentAlignment.MiddleCenter;
            radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            radioButton2.Appearance = Appearance.Button;
            radioButton2.Font = new Font("Microsoft YaHei UI", 11F, FontStyle.Bold);
            radioButton2.Location = new Point(176, 153);
            radioButton2.Name = "radioButton2";
            radioButton2.Size = new Size(190, 55);
            radioButton2.TabIndex = 7;
            radioButton2.TabStop = true;
            radioButton2.Text = "样本数据清洗";
            radioButton2.TextAlign = ContentAlignment.MiddleCenter;
            radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton3
            // 
            radioButton3.Appearance = Appearance.Button;
            radioButton3.Font = new Font("Microsoft YaHei UI", 11F, FontStyle.Bold);
            radioButton3.Location = new Point(364, 153);
            radioButton3.Name = "radioButton3";
            radioButton3.Size = new Size(190, 55);
            radioButton3.TabIndex = 7;
            radioButton3.TabStop = true;
            radioButton3.Text = "试剂装载列表";
            radioButton3.TextAlign = ContentAlignment.MiddleCenter;
            radioButton3.UseVisualStyleBackColor = true;
            // 
            // radioButton4
            // 
            radioButton4.Appearance = Appearance.Button;
            radioButton4.Font = new Font("Microsoft YaHei UI", 11F, FontStyle.Bold);
            radioButton4.Location = new Point(552, 153);
            radioButton4.Name = "radioButton4";
            radioButton4.Size = new Size(245, 55);
            radioButton4.TabIndex = 7;
            radioButton4.TabStop = true;
            radioButton4.Text = "校准品/质控品装载列表";
            radioButton4.TextAlign = ContentAlignment.MiddleCenter;
            radioButton4.UseVisualStyleBackColor = true;
            // 
            // radioButton5
            // 
            radioButton5.Appearance = Appearance.Button;
            radioButton5.Font = new Font("Microsoft YaHei UI", 11F, FontStyle.Bold);
            radioButton5.Location = new Point(790, 153);
            radioButton5.Name = "radioButton5";
            radioButton5.Size = new Size(161, 55);
            radioButton5.TabIndex = 7;
            radioButton5.TabStop = true;
            radioButton5.Text = "参数下载";
            radioButton5.TextAlign = ContentAlignment.MiddleCenter;
            radioButton5.UseVisualStyleBackColor = true;
            // 
            // radioButton6
            // 
            radioButton6.Appearance = Appearance.Button;
            radioButton6.AutoEllipsis = true;
            radioButton6.Font = new Font("Microsoft YaHei UI", 11F, FontStyle.Bold);
            radioButton6.Location = new Point(947, 153);
            radioButton6.Name = "radioButton6";
            radioButton6.Size = new Size(143, 55);
            radioButton6.TabIndex = 7;
            radioButton6.TabStop = true;
            radioButton6.Text = "样本跟踪";
            radioButton6.TextAlign = ContentAlignment.MiddleCenter;
            radioButton6.UseVisualStyleBackColor = true;
            // 
            // Test1
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            ClientSize = new Size(1282, 953);
            Controls.Add(radioButton6);
            Controls.Add(radioButton5);
            Controls.Add(radioButton4);
            Controls.Add(radioButton3);
            Controls.Add(radioButton2);
            Controls.Add(radioButton1);
            Controls.Add(panel1);
            Controls.Add(dataGridView1);
            Controls.Add(button17);
            Controls.Add(button16);
            Controls.Add(label16);
            Controls.Add(button15);
            Controls.Add(button14);
            Controls.Add(button13);
            Controls.Add(button11);
            Controls.Add(button12);
            Controls.Add(button10);
            Controls.Add(button9);
            Controls.Add(button8);
            Controls.Add(button7);
            Controls.Add(button6);
            Controls.Add(button5);
            Controls.Add(button4);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(button1);
            Name = "Test1";
            Text = "Test1";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            tableLayoutPanel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox_CC1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox_PC1).EndInit();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox_CC2).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox_PC2).EndInit();
            panel4.ResumeLayout(false);
            panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox_PC3).EndInit();
            panel5.ResumeLayout(false);
            panel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox_PC4).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox_CC4).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox_chart).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn project;
        private System.Windows.Forms.DataGridViewTextBoxColumn state;
        private System.Windows.Forms.DataGridViewTextBoxColumn testnum;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox_chart;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.PictureBox pictureBox_CC1;
        private System.Windows.Forms.PictureBox pictureBox_PC1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.PictureBox pictureBox_CC2;
        private System.Windows.Forms.PictureBox pictureBox_PC2;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.PictureBox pictureBox_PC3;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.PictureBox pictureBox_PC4;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.PictureBox pictureBox_CC4;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.Button button11;
        private System.Windows.Forms.Button button12;
        private System.Windows.Forms.Button button13;
        private System.Windows.Forms.Button button14;
        private System.Windows.Forms.Button button15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Button button16;
        private System.Windows.Forms.Button button17;
        private RadioButton radioButton1;
        private RadioButton radioButton2;
        private RadioButton radioButton3;
        private RadioButton radioButton4;
        private RadioButton radioButton5;
        private RadioButton radioButton6;
    }
}