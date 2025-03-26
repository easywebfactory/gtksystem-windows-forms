using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading.Tasks;
using System.Windows.Forms;
using GTKWinFormsApp.Properties;

namespace GTKWinFormsApp;

public partial class GtkMainForm : Form
{
    public GtkMainForm()
    {
        InitializeComponent();

        toolStripMenuItem1.Text = Resources.GtkMainForm_GtkMainForm_Menu_1;
        ddddToolStripMenuItem.Text = Resources.GtkMainForm_GtkMainForm_a_Menu;
        ssssToolStripMenuItem.Text = Resources.GtkMainForm_GtkMainForm_b_Menu;
        bbMenuToolStripMenuItem.Text = Resources.GtkMainForm_GtkMainForm_bb_Menu;
        bbMenu2ToolStripMenuItem.Text = Resources.GtkMainForm_GtkMainForm_bb_Menu_2;
        cMenuToolStripMenuItem.Text = Resources.GtkMainForm_GtkMainForm_c_Menu;
        ssssToolStripMenuItem1.Text = Resources.GtkMainForm_GtkMainForm_Menu_2;
        toolStripDropDownButton1.Text = Resources.GtkMainForm_GtkMainForm_Dropdown_List_1;
        memnuToolStripMenuItem.Text = Resources.GtkMainForm_GtkMainForm_Item_1;
        fffffffToolStripMenuItem.Text = Resources.GtkMainForm_GtkMainForm_Item_2;
        button1.Text = Resources.GtkMainForm_GtkMainForm_Open_Main_Window;
        toolStripStatusLabel1.Text = Resources.GtkMainForm_GtkMainForm_Status_Text;
        toolStripSplitButton2.Text = Resources.GtkMainForm_GtkMainForm_Dropdown_Menu;
        label1.Text = Resources.GtkMainForm_GtkMainForm_Slider_Value;
        button4.Text = Resources.GtkMainForm_GtkMainForm_List_View;
        button2.Text = Resources.GtkMainForm_GtkMainForm_Split_Container_Layout;
        button3.Text = Resources.GtkMainForm_GtkMainForm_Print;
        label2.Text = Resources.GtkMainForm_GtkMainForm_This_is_a_UserControl_placeholder;
        Text = Resources.GtkMainForm_GtkMainForm_Default_Style_Interface;
        PreLoad += (_, _) =>
        {
            Text = Resources.GtkMainForm_GtkMainForm_Default_Style_Interface;
        };

        // Currently, custom controls cannot be visualized in the form designer.
        // It is recommended to add them programmatically, as shown in the following example:
        var userControl11 = new UserControl11();
        panel5.Controls.Add(userControl11);

        AddHandlers();
    }

    private void AddHandlers()
    {
        ssssToolStripMenuItem1.Click -= ssssToolStripMenuItem1_Click;
        ssssToolStripMenuItem1.Click += ShowListViewFormToolStripMenuItem_Click;
        button1.Click += button1_Click;
        trackBar1.Scroll += trackBar1_Scroll;
        button4.Click += ButtonShowListViewForm_Click;
        button2.Click += ButtonShowCommonDialogsForm_Click;
        button3.Click += ButtonPrint_Click;
        panel3.Paint += panel3_Paint;
        panel5.Scroll += panel5_Scroll;
        Load += GtkMainForm_Load;
        SizeChanged += GtkMainForm_SizeChanged;
        Shown += GtkMainForm_Shown;
    }

    private void ssssToolStripMenuItem1_Click(object? sender, EventArgs e)
    {
        ShowListViewFormToolStripMenuItem_Click(sender, e);
    }

    private void GtkMainForm_Shown(object? sender, EventArgs e)
    {
        // SwitchBox switchBox = new SwitchBox();
        //switchBox.Location = new Point(100, 100);
        //panel1.Controls.Add(switchBox);
    }

    private void GtkMainForm_SizeChanged(object? sender, EventArgs e)
    {
        panel1.Refresh();
        //Console.WriteLine(Width);
        // panel1.Refresh();
        this.Refresh();
    }

    private void button1_Click(object? sender, EventArgs e)
    {
        //button1.ForeColor=Color.Red;
        //button1.BackColor=Color.Green;
        var f = new TestDataForm();
        f.Show();
    }

    private void trackBar1_Scroll(object? sender, EventArgs e)
    {
        label1.Text = trackBar1.Value.ToString();
    }

    private void GtkMainForm_Load(object? sender, EventArgs e)
    {
        void MethodInvoker()
        {
            while (!progressBar1.IsHandleCreated)
            {
                System.Threading.Thread.Sleep(100);
            }

            for (var i = 1; i < 101; i++)
            {
                progressBar1.Invoke(new MethodInvoker(() => { progressBar1.Value = i; }));
                System.Threading.Thread.Sleep(20);
            }
        }

        var result = this.BeginInvoke(MethodInvoker);
    }

    private void panel3_Paint(object? sender, PaintEventArgs e)
    {
        var g = e.Graphics;

        var path = new GraphicsPath();
        path.AddEllipse(90, 25, 40, 20);

        path.StartFigure();
        path.AddArc(5, 5, 6, 6, 180, 90);
        path.AddArc(20, 5, 6, 6, 270, 90);
        path.AddArc(20, 18, 6, 6, 0, 90);
        path.AddArc(5, 18, 6, 6, 90, 90);
        path.CloseFigure();

        path.StartFigure();

        path.AddLine(new Point(20, 40), new Point(60, 30));
        path.AddPie(-10, 20, 60, 60, 10, 100);
        //path.StartFigure();
        path.AddBezier(new Point(60, 70), new Point(80, 120), new Point(110, 20), new Point(160, 80));
        path.AddRectangle(new Rectangle(30, 10, 190, 30));
        //path.CloseFigure();

        path.AddString(Resources.GtkMainForm_panel3_Paint_testText, new FontFamily(GenericFontFamilies.Serif), (int)FontStyle.Italic, 16, new Point(1, 1), new StringFormat(StringFormatFlags.NoWrap));

        //path.CloseAllFigures();

        var brush = new LinearGradientBrush(new Point(0, 0), new Point(100, 30), Color.Red, Color.Blue);
        //g.TranslateTransform(30, 0);
        //g.RotateTransform(20);
        g.DrawPath(new Pen(brush, 2), path);

        //PathGradientBrush gradientBrush = new PathGradientBrush(path);
        //gradientBrush.CenterColor = Color.Red;
        //gradientBrush.SurroundColors = new Color[] { Color.Yellow, Color.Blue };
        //g.DrawPath(new Pen(gradientBrush, 2), path);

        //g.FillPath(brush, path);
    }

    private void ShowListViewFormToolStripMenuItem_Click(object? sender, EventArgs e)
    {
        var f1 = new ListViewForm();
        f1.Show(this);
    }

    private void ButtonShowCommonDialogsForm_Click(object? sender, EventArgs e)
    {
        var f = new CommonDialogsForm();
        var res = f.ShowDialog();
        Console.WriteLine(res);
    }

    private void panel5_Scroll(object? sender, System.Windows.Forms.ScrollEventArgs e)
    {
        Console.WriteLine($"panel5_Scroll:{e.OldValue},{e.NewValue};{e.ScrollOrientation}");
    }

    private void ButtonPrint_Click(object? sender, EventArgs e)
    {
        // Print
        AutoClosingMessageBox.Instance.Show("ToDo");
    }

    private void ButtonShowListViewForm_Click(object? sender, EventArgs e)
    {
        var f1 = new ListViewForm();
        f1.Show(this);
    }
}