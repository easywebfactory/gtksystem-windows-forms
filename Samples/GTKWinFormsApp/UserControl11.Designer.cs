using System.Collections;
using System.Windows.Forms;

namespace GTKWinFormsApp
{
    partial class UserControl11
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up all resources in use.
        /// </summary>
        /// <param name="disposing">true if the managed resource should be released; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary> 
        /// Designer supports required methods - do not modify
        /// Use the code editor to modify the contents of this method.
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
            button1.Size = new System.Drawing.Size(153, 71);
            button1.TabIndex = 0;
            button1.Text = "This is a custom control";
            button1.UseVisualStyleBackColor = true;
            // 
            // UserControl11
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = System.Drawing.Color.LightGreen;
            Controls.Add(button1);
            Name = "UserControl11";
            Size = new System.Drawing.Size(186, 143);
            Load += UserControl11_Load;
            Scroll += UserControl11_Scroll;
            Click += UserControl11_Click;
            Paint += UserControl11_Paint;
            DoubleClick += UserControl11_DoubleClick;
            MouseClick += UserControl11_MouseClick;
            MouseDoubleClick += UserControl11_MouseDoubleClick;
            MouseDown += UserControl11_MouseDown;
            MouseEnter += UserControl11_MouseEnter;
            MouseLeave += UserControl11_MouseLeave;
            MouseHover += UserControl11_MouseHover;
            MouseMove += UserControl11_MouseMove;
            MouseUp += UserControl11_MouseUp;
            ParentChanged += UserControl11_ParentChanged;
            ResumeLayout(false);
        }

        private Button button1;
    }
}
