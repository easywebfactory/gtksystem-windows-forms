using System;
using System.Drawing;
using System.Windows.Forms;
using GTKWinFormsApp.Properties;

namespace GTKWinFormsApp;

public partial class CommonDialogsForm : Form
{
    public CommonDialogsForm()
    {
        InitializeComponent();
        treeNode1.Name = Resources.CommonDialogsForm_CommonDialogsForm_Node_1;
        treeNode1.Text = Resources.CommonDialogsForm_CommonDialogsForm_Node_1;
        treeNode2.ImageIndex = 1;
        treeNode2.Name = Resources.CommonDialogsForm_CommonDialogsForm_Node_2;
        treeNode2.Text = Resources.CommonDialogsForm_CommonDialogsForm_Node_2;
        treeNode3.Name = Resources.CommonDialogsForm_CommonDialogsForm_Node_3;
        treeNode3.Text = Resources.CommonDialogsForm_CommonDialogsForm_Node_3;
        treeNode4.Name = Resources.CommonDialogsForm_CommonDialogsForm_Node_4;
        treeNode4.Text = Resources.CommonDialogsForm_CommonDialogsForm_Node_4;
        treeNode5.ImageIndex = 0;
        treeNode5.ImageKey = "img11.jpg";
        treeNode5.Name = Resources.CommonDialogsForm_CommonDialogsForm_Node_5;
        treeNode5.Text = Resources.CommonDialogsForm_CommonDialogsForm_Node_5;
        treeView1.Nodes.AddRange(new TreeNode[] { treeNode4, treeNode5 });
        button1.Text = Resources.CommonDialogsForm_CommonDialogsForm_0;
        button9.Text = Resources.CommonDialogsForm_CommonDialogsForm_1;
        button8.Text = Resources.TestDataForm_TestDataForm_Load_2B;
        button7.Text = Resources.CommonDialogsForm_CommonDialogsForm_3;
        button6.Text = Resources.CommonDialogsForm_CommonDialogsForm_4;
        button3.Text = Resources.CommonDialogsForm_CommonDialogsForm_5;
        label1.Text = Resources.CommonDialogsForm_CommonDialogsForm_6;

        hScrollBar1.ValueChanged += hScrollBar1_ValueChanged;
        vScrollBar1.ValueChanged += vScrollBar1_ValueChanged;
        button9.Click += ButtonMessageBox_Click;
        button8.Click += ButtonColorDialog_Click;
        button7.Click += ButtonFolderBrowser_Click;
        button6.Click += ButtonSaveFile_Click;
        button3.Click += ButtonOpenFile_Click;
        Shown += Form4_Shown;
        button4.Click += Button4_Click;
        button1.Click += Button_Click;
        button2.Click += Button_Click;
        button5.Click += Button_Click;
    }

    private void Button_Click(object? sender, EventArgs e)
    {
        AutoClosingMessageBox.Instance.Show(((Button)sender!).Text);
    }

    private void Button4_Click(object? sender, EventArgs e)
    {
        var button = new Button() { Location = new Point(200, 100), Size = new Size(160, 30), Text = "testtest", Dock = DockStyle.Fill };
        button.Click += Button_Click;
        splitContainer1.Panel1.Controls.Add(button);
    }

    Point panel1Location = new();
    private void Form4_Shown(object? sender, EventArgs e)
    {

    }

    private void ButtonOpenFile_Click(object? sender, EventArgs e)
    {
        var ofd = new OpenFileDialog();
        ofd.Filter = "jpg|*.jpg|png|*.png";
        ofd.Multiselect = true;
        ofd.Title = Resources.CommonDialogsForm_button3_Click_Test_Open_File;

        var dialogResult = ofd.ShowDialog(this);
        Console.WriteLine(@"dialogResult:" + dialogResult.ToString());
        Console.WriteLine(@"FileName:" + ofd.FileName);
        if (ofd.FileNames != null)
        {
            foreach (var file in ofd.FileNames)
            {
                Console.WriteLine(@"FileNames:" + file);
            }
        }

        Console.WriteLine(@"SafeFileName:" + ofd.SafeFileName);
        foreach (var file in ofd.SafeFileNames)
        {
            Console.WriteLine(@"SafeFileNames:" + file);
        }
    }

    private void ButtonSaveFile_Click(object? sender, EventArgs e)
    {
        var ofd = new SaveFileDialog();
        ofd.Filter = "jpg|*.jpg|png|*.png";
        ofd.Title = Resources.CommonDialogsForm_button6_Click_Test_Save_File;

        var dialogResult = ofd.ShowDialog();
        Console.WriteLine(@"dialogResult:" + dialogResult);
        Console.WriteLine(@"FileName:" + ofd.FileName);
        if (ofd.FileNames != null)
        {
            foreach (var file in ofd.FileNames)
            {
                Console.WriteLine(@"FileNames:" + file);
            }
        }
    }

    private void ButtonFolderBrowser_Click(object? sender, EventArgs e)
    {
        var ofd = new FolderBrowserDialog();
        ofd.Description = Resources.CommonDialogsForm_button7_Click_Browse_Folder_Description;
        var dialogResult = ofd.ShowDialog();
        Console.WriteLine(@"dialogResult:" + dialogResult.ToString());
        Console.WriteLine(@"SelectedPath:" + ofd.SelectedPath);
    }

    private void ButtonColorDialog_Click(object? sender, EventArgs e)
    {
        var colorDialog = new ColorDialog();
        colorDialog.ShowDialog();

        //FontDialog fontDialog = new FontDialog();
        //fontDialog.ShowDialog();

        //Graphics g = CreateGraphics();
        //// g.DrawString("ddddddddd", new Font(FontFamily.GenericSansSerif, 16), new SolidBrush(ColorExtension.Red), 0, 0);
        //g.DrawRectangle(new Pen(new SolidBrush(ColorExtension.Red),2), new Rectangle(110, 110, 200, 200));

    }

    private void ButtonMessageBox_Click(object? sender, EventArgs e)
    {
        MessageBox.Show("test message test message test messagetest message test message test message test messagetest message " +
                        "test message test message test messagetest message test message test message test messagetest message test " +
                        "message test message test messagetest message", Resources.CommonDialogsForm_button9_Click_Doubt,
            MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        MessageBox.Show("test message test message \ntest messagetest message", Resources.CommonDialogsForm_button9_Click_Warn,
            MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
    }

    private void vScrollBar1_ValueChanged(object? sender, EventArgs e)
    {

    }

    private void hScrollBar1_ValueChanged(object? sender, EventArgs e)
    {

    }
}