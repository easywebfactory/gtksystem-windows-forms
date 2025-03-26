using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace GTKWinFormsApp;

[Designer(typeof(ControlDesigner))]
public partial class UserControl11 : UserControl
{
    public UserControl11()
    {
        InitializeComponent();
        this.MouseWheel += UserControl11_MouseWheel;
        button1.Click += (_, _) =>
        {
            AutoClosingMessageBox.Instance.Show(button1.Text);
        };
    }

    private void UserControl11_MouseWheel(object? sender, MouseEventArgs e)
    {
        Console.WriteLine(@"UserControl11_MouseWheel");
    }

    private void UserControl11_Paint(object? sender, PaintEventArgs e)
    {
        var g = e.Graphics;
        g!.DrawEllipse(new Pen(new SolidBrush(Color.Red), 2), 0, 0, 30, 20);
        //g.FillEllipse(new SolidBrush(Color.Red), 40, 25, 30, 20);
    }

    private void UserControl11_ParentChanged(object? sender, EventArgs e)
    {
        // MessageBox.Show("sss");
    }

    private void UserControl11_Load(object? sender, EventArgs e)
    {

    }

    private void UserControl11_MouseEnter(object? sender, EventArgs e)
    {
        Console.WriteLine(@"UserControl11_MouseEnter");
    }

    private void UserControl11_MouseLeave(object? sender, EventArgs e)
    {
        Console.WriteLine(@"UserControl11_MouseLeave");
    }

    private void UserControl11_MouseMove(object? sender, MouseEventArgs e)
    {
        //Console.WriteLine(@"UserControl11_MouseMove");
    }

    private void UserControl11_MouseUp(object? sender, MouseEventArgs e)
    {
        Console.WriteLine(@"UserControl11_MouseUp");
    }

    private void UserControl11_MouseHover(object? sender, EventArgs e)
    {
        Console.WriteLine(@"UserControl11_MouseHover");
    }

    private void UserControl11_MouseDown(object? sender, MouseEventArgs e)
    {
        Console.WriteLine(@"UserControl11_MouseDown");
    }

    private void UserControl11_Click(object? sender, EventArgs e)
    {
        Console.WriteLine(@"UserControl11_Click");
    }

    private void UserControl11_DoubleClick(object? sender, EventArgs e)
    {
        Console.WriteLine(@"UserControl11_DoubleClick");
    }

    private void UserControl11_MouseClick(object? sender, MouseEventArgs e)
    {
        Console.WriteLine(@"UserControl11_MouseClick");
    }

    private void UserControl11_MouseDoubleClick(object? sender, MouseEventArgs e)
    {
        Console.WriteLine(@"UserControl11_MouseDoubleClick");
    }

    private void UserControl11_Scroll(object? sender, ScrollEventArgs e)
    {
        Console.WriteLine($"UserControl11_Scroll:{e.OldValue},{e.NewValue};{e.ScrollOrientation}");
    }
}