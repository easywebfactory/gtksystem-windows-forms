using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using Gdk;
using Gtk;
using GTKSystemWinFormsApp11.Properties;

namespace GTKSystemWinFormsApp11;

public partial class GtkForm : Form
{
    public GtkForm()
    {
        InitializeComponent();
        toolStripMenuItem1.Text = $"{Resources.GtkForm_InitializeComponent_Menu} 1";
        ddddToolStripMenuItem.Text = $"a {Resources.GtkForm_InitializeComponent_Menu}";
        ssssToolStripMenuItem.Text = $"b {Resources.GtkForm_InitializeComponent_Menu}";
        bbMenuToolStripMenuItem.Text = $"bb {Resources.GtkForm_InitializeComponent_Menu}";
        bbMenu2ToolStripMenuItem.Text = $"bb {Resources.GtkForm_InitializeComponent_Menu} 2";
        ssssToolStripMenuItem1.Text = $"{Resources.GtkForm_InitializeComponent_Menu} 2";
        toolStripDropDownButton1.Text = Resources.GtkForm_GtkForm_dropdown_list_1;
        memnuToolStripMenuItem.Text = $"{Resources.GtkForm_InitializeComponent_Item} 1";
        fffffffToolStripMenuItem.Text = $"{Resources.GtkForm_InitializeComponent_Item} 2";
        button1.Text = Resources.GtkForm_InitializeComponent_Open_main_window;
        toolStripStatusLabel1.Text = Resources.GtkForm_InitializeComponent_Open_Status_Text;
        toolStripSplitButton2.Text = Resources.GtkForm_InitializeComponent_DropSownMenu;

        SizeChanged += Form1_SizeChanged;
    }

    public LinkLabel LinkLabel1 => linkLabel1;

    private void Form1_SizeChanged(object? sender, EventArgs e)
    {
        Console.WriteLine($"{Width},{Height}");
    }

    private void button1_Click(object? sender, EventArgs e)
    {

    }

    private void trackBar1_Scroll(object? sender, EventArgs e)
    {
        label1.Text = trackBar1.Value.ToString();
    }

    private void button1_Paint(object? sender, PaintEventArgs e)
    {
        var g = e.Graphics;
        g.DrawEllipse(new Pen(new SolidBrush(System.Drawing.Color.Red), 2), 80, 25, 30, 20);
        //g.FillEllipse(new SolidBrush(Color.Red), 40, 25, 30, 20);
        // g.DrawEllipse(new Pen(new SolidBrush(Color.Red), 0), 40, 25, 40, 40);

    }


    private async void LinkLabel1_LinkClicked(object? sender, LinkLabelLinkClickedEventArgs e)
    {
        AutoClosingMessageBox.Instance.MessageBoxTimeout = 1000;
        AutoClosingMessageBox.Instance.Show(linkLabel1.Text);
    }
}