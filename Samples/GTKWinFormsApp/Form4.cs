using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GTKWinFormsApp
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
            button5.Click += (_, _) => 
            {
                Debug.Print(Control.ModifierKeys.ToString()); 
            };
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "jpg|*.jpg;png|*.png";
            ofd.Multiselect = true;
            ofd.Title = "测试打开文件";
            ofd.Description = "打开文件 decription";

            DialogResult dialogResult = ofd.ShowDialog();
            Console.WriteLine("dialogResult:" + dialogResult.ToString());
            Console.WriteLine("FileName:" + ofd.FileName);
            foreach (string file in ofd.FileNames)
            {
                Console.WriteLine("FileNames:" + file);
            }
            Console.WriteLine("SafeFileName:" + ofd.SafeFileName);
            foreach (string file in ofd.SafeFileNames)
            {
                Console.WriteLine("SafeFileNames:" + file);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            SaveFileDialog ofd = new SaveFileDialog();
            ofd.Filter = "jpg|*.jpg;png|*.png";
            ofd.Title = "测试保存文件";
            ofd.Description = " 保存文件 decription";

            DialogResult dialogResult = ofd.ShowDialog();
            Console.WriteLine("dialogResult:" + dialogResult.ToString());
            Console.WriteLine("FileName:" + ofd.FileName);
            foreach (string file in ofd.FileNames)
            {
                Console.WriteLine("FileNames:" + file);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog ofd = new FolderBrowserDialog();
            ofd.Title = "测试浏览文件夹";
            ofd.Description = "浏览文件夹 decription";
            ofd.Multiselect = true;
            DialogResult dialogResult = ofd.ShowDialog();
            Console.WriteLine("dialogResult:" + dialogResult.ToString());
            foreach (string file in ofd.SelectedPaths)
            {
                Console.WriteLine("SelectedPaths:" + file);
            }
            Console.WriteLine("SelectedPath:" + ofd.SelectedPath);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            colorDialog.ShowDialog();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            MessageBox.Show("test message test message \ntest messagetest message", "疑问", MessageBoxButtons.RetryCancel, MessageBoxIcon.Question);
            MessageBox.Show("test message test message \ntest messagetest message", "警告", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            FontDialog fontDialog = new FontDialog();
            fontDialog.Font = button10.Font;
            if (fontDialog.ShowDialog() == DialogResult.OK)
                button10.Font = fontDialog.Font;
        }
    }
}
