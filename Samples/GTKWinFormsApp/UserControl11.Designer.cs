using System.Collections;
using System.Windows.Forms;

namespace GTKWinFormsApp
{
    partial class UserControl11
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            button1 = new Button();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new System.Drawing.Point(3, 42);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(153, 34);
            button1.TabIndex = 0;
            button1.Text = "这是自定义控件";
            button1.UseVisualStyleBackColor = true;
            // 
            // UserControl11
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = System.Drawing.Color.LightGreen;
            Controls.Add(button1);
            Name = "UserControl11";
            Size = new System.Drawing.Size(190, 103);
            Load += UserControl11_Load;
            Paint += UserControl11_Paint;
            MouseEnter += UserControl11_MouseEnter;
            MouseLeave += UserControl11_MouseLeave;
            MouseHover += UserControl11_MouseHover;
            MouseMove += UserControl11_MouseMove;
            MouseUp += UserControl11_MouseUp;
            ParentChanged += UserControl11_ParentChanged;
            ResumeLayout(false);
        }

        #endregion

        private Button button1;
    }
}
