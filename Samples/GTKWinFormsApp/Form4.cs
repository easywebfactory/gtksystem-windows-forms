using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
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
            this.Shown += Form4_Shown;

            button4.Click += Button4_Click;
        }

        private void Button4_Click(object? sender, EventArgs e)
        {
            //propertyGrid1.SelectedObject = sender;
            splitContainer1.Panel1.Controls.Add(new Button() { Location = new Point(200, 100), Size = new Size(160, 30), Text = "testtest", Dock=DockStyle.Fill });
        }

        Point panel1Location = new Point();
        private void Form4_Shown(object? sender, EventArgs e)
        {
           
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "图像文件(.jpg)|*.jpg;图像文件(.png)|*.png";
            ofd.Multiselect = true;
            ofd.Title = "测试打开文件";
            ofd.DefaultExt = ".jpg";
            DialogResult dialogResult = ofd.ShowDialog(this);
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
            ofd.Description = "浏览文件夹 decription";
            DialogResult dialogResult = ofd.ShowDialog();
            Console.WriteLine("dialogResult:" + dialogResult.ToString());
            Console.WriteLine("SelectedPath:" + ofd.SelectedPath);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            colorDialog.ShowDialog();

            //FontDialog fontDialog = new FontDialog();
            //fontDialog.ShowDialog();

            //Graphics g = CreateGraphics();
            //// g.DrawString("ddddddddd", new Font(FontFamily.GenericSansSerif, 16), new SolidBrush(Color.Red), 0, 0);
            //g.DrawRectangle(new Pen(new SolidBrush(Color.Red),2), new Rectangle(110, 110, 200, 200));

        }

        private void button9_Click(object sender, EventArgs e)
        {
            MessageBox.Show("test message test message test messagetest message test message test message test messagetest message test message test message test messagetest message test message test message test messagetest message test message test message test messagetest message", "疑问", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            MessageBox.Show("test message test message \ntest messagetest message", "警告", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
        }

        private void vScrollBar1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void hScrollBar1_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
