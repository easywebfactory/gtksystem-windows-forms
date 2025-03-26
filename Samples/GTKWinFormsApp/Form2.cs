using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using GTKWinFormsApp.Properties;

namespace GTKWinFormsApp;

public partial class ListViewForm : Form
{
    public ListViewForm()
    {
        InitializeComponent();
        var listViewGroup1 = new ListViewGroup(Resources.ListViewForm_ListViewForm_Group_Header_Title, HorizontalAlignment.Left);
        var listViewGroup2 = new ListViewGroup(Resources.ListViewForm_ListViewForm_ListViewGroup2, HorizontalAlignment.Left);
        var listViewItem1 = new ListViewItem(new string[]
        {
            Resources.ListViewForm_ListViewForm_ListViewItem_is_a_ContentControl_and_can_only_contain_a_single_child_element__However__this_child_element_can_be_any_visual_element_
        }, 0, Color.Blue, Color.FromArgb(255, 192, 255), null);
        var listViewItem2 = new ListViewItem(Resources.ListViewForm_ListViewForm_ListView_is_derived_from_ListBox__Typically__the_items_of_this_control_are_members_of_a_data_collection_and_are_represented_as_ListViewItem_objects_, 0);
        var listViewItem3 = new ListViewItem(Resources.ListViewForm_ListViewForm_A, 0);
        var listViewItem4 = new ListViewItem(new string[] { Resources.ListViewForm_ListViewForm_ListView_Grouping, Resources.ListViewForm_ListViewForm_listView1_Groups_Add__, Resources.ListViewForm_ListViewForm__432321 }, 0);
        var listViewItem5 = new ListViewItem(new string[] { Resources.ListViewForm_ListViewForm_ListView_Defines_View_Modes, Resources.ViewSmallIcon, Resources.ListViewForm_ListViewForm_View_LargeIcon, Resources.ListViewForm_ListViewForm_View_Details, Resources.ListViewForm_ListViewForm_View_List }, 1);
        var listViewItem6 = new ListViewItem(Resources.ListViewForm_ListViewForm_What_is_ListView_, 0);
        var listViewItem7 = new ListViewItem(new string[] { Resources.ListViewForm_ListViewForm_Bind_Data_to_ListView, Resources.ListViewForm_ListViewForm_listView1_Items_Add___ }, 0);
        listViewGroup1.Footer = Resources.ListViewForm_ListViewForm_Group_Footer_Title;
        listViewGroup1.Header = Resources.ListViewForm_ListViewForm_Group_Header_Title;
        listViewGroup1.Name = Resources.ListViewForm_ListViewForm_listViewGroup1;
        listViewGroup1.Subtitle = Resources.ListViewForm_ListViewForm_Group_Header_Subtitle;
        listViewGroup2.Header = Resources.ListViewForm_ListViewForm_ListViewGroup2;
        listViewGroup2.Name = Resources.ListViewForm_ListViewForm_ListViewGroup2;
        listViewGroup2.Subtitle = Resources.ListViewForm_ListViewForm_group2;
        listView1.Groups.Clear();
        listView1.Groups.AddRange(new ListViewGroup[] { listViewGroup1, listViewGroup2 });

        listViewItem1.Group = listViewGroup1;
        listViewItem1.StateImageIndex = 0;
        listViewItem2.Group = listViewGroup1;
        listViewItem2.StateImageIndex = 0;
        listViewItem3.Group = listViewGroup1;
        listViewItem3.StateImageIndex = 0;
        listViewItem4.Group = listViewGroup2;
        listViewItem4.StateImageIndex = 0;
        listViewItem5.Group = listViewGroup2;
        listViewItem5.StateImageIndex = 0;
        listViewItem6.StateImageIndex = 0;
        listViewItem7.Group = listViewGroup2;
        listViewItem7.StateImageIndex = 0;
        listView1.Items.Clear();
        listView1.Items.AddRange(new ListViewItem[] { listViewItem1, listViewItem2, listViewItem3, listViewItem4, listViewItem5, listViewItem6, listViewItem7 });
        columnHeader1.Text = Resources.ListViewForm_ListViewForm_Group_1;
        columnHeader2.Text = Resources.ListViewForm_ListViewForm_Group_2fffffffffffffffffffffffffffffff;
        columnHeader3.Text = Resources.ListViewForm_ListViewForm_Group_3;

        listBox1.Items.Clear();
        listBox1.Items.AddRange(new object[] {
                Resources.ListViewForm_ListViewForm__0_Exception_Warning_0_____Server_Room_Air_Conditioning_Monitoring_Event_____638526419125153303________2024_5_30_4_58_32,
                Resources.ListViewForm_ListViewForm__1_Exception_Warning_1_____Server_Room_Air_Conditioning_Monitoring_Event_____638526419135198480________2024_5_30_4_58_33,
                Resources.ListViewForm_ListViewForm__2_Exception_Warning_2_____Server_Room_Air_Conditioning_Monitoring_Event_____638526419155233741________2024_5_30_4_58_35,
                Resources.ListViewForm_ListViewForm__3_Exception_Warning_3_____Server_Room_Air_Conditioning_Monitoring_Event_____638526419495254800________2024_5_30_4_59_09,
                Resources.ListViewForm_ListViewForm__4_Exception_Warning_4_____Server_Room_Air_Conditioning_Monitoring_Event_____638526419505198651________2024_5_30_4_59_10,
                Resources.ListViewForm_ListViewForm__5_Exception_Warning_5_____Server_Room_Air_Conditioning_Monitoring_Event_____638526419565218661________2024_5_30_4_59_16
            });
        label1.Text = Resources.label1Text;
        label2.Text = Resources.ListViewForm_ListViewForm___Monitoring_Scroll_Event_Log;
        // listView1.Items.Add(new ListViewItem("test1", new ListViewGroup("ListViewGroup1", "ListViewGroup1")) { });

        var dt = new DataTable();
        dt.Columns.Add("ID", typeof(string));
        dt.Columns.Add("CreateDate", typeof(DateTime));
        dt.Rows.Add("user1", DateTime.Now);
        dt.Rows.Add("user2", DateTime.Now.AddDays(1));
        dt.Rows.Add("user3", DateTime.Now.AddDays(2));
        dt.Rows.Add("user4", DateTime.Now.AddDays(3));
        //Dictionary<int, string> dic = new Dictionary<int, string>();
        //dic.Add(0, "111");
        //dic.Add(3, "333");
        //List<string> list = new List<string>();
        //list.Add("ddd");
        //list.Add("rrrr");
        //Hashtable hashtable = new Hashtable();
        //hashtable.Add("11", "ddd");
        //hashtable.Add("22", "rrrr");

        listBox1.DisplayMember = "CreateDate";
        listBox1.DataSource = dt;

        this.FormClosing += Form2_FormClosing;
        this.FormClosed += Form2_FormClosed;
        listView1.MouseDown += ListView1_MouseDown;
    }

    private void ListView1_MouseDown(object? sender, MouseEventArgs e)
    {
        var item = listView1.GetItemAt(e.X, e.Y);
        if (item != null)
        {
            Console.WriteLine(item.Text);
        }
    }

    int i = 4;
    private void Timer1_Tick(object? sender, System.EventArgs e)
    {
        i++;
        listBox1.Items.Add(string.Format(
            Resources.ListViewForm_Timer1_Tick_Exception_Warning__0______Server_Room_Air_Conditioning_Operation_Monitoring_Event______1__________2_,
            i, DateTime.Now.Ticks, DateTime.Now.ToString(CultureInfo.InvariantCulture)));
        listBox1.TopIndex = i;
    }
    private void Form2_FormClosed(object? sender, FormClosedEventArgs e)
    {
        MessageBox.Show("Form2_FormClosed");
    }

    private void Form2_FormClosing(object? sender, FormClosingEventArgs e)
    {
        var res = MessageBox.Show("Form2_FormClosing");
        e.Cancel = res != DialogResult.OK;
    }
    private void listView1_ItemCheck(object? sender, ItemCheckEventArgs e)
    {
        MessageBox.Show($"ItemCheck:{e.NewValue.ToString()},{e.CurrentValue.ToString()}");
    }

    private void listView1_ItemChecked(object? sender, ItemCheckedEventArgs e)
    {
        richTextBox1.Text = "";
        foreach (var m in listView1.CheckedItems)
            richTextBox1.Text += $"{m.Text},Selected:{m.Selected},Checked:{m.Checked}；\n";
    }

    private void listView1_ItemSelectionChanged(object? sender, ListViewItemSelectionChangedEventArgs e)
    {
        MessageBox.Show($"ItemSelectionChanged:{e.Item.Text},{e.Item.Selected}");
    }

    private void listView1_SelectedIndexChanged(object? sender, EventArgs e)
    {
        richTextBox1.Text = "";
        foreach (var m in listView1.SelectedItems)
            richTextBox1.Text += $"{m.Text},Selected:{m.Selected},Checked:{m.Checked}；\n";
    }

    private void listView1_ColumnClick(object? sender, ColumnClickEventArgs e)
    {
        MessageBox.Show($"ColumnClick:{e.Column}");
    }

    private void listView1_Click(object? sender, EventArgs e)
    {

    }

    private void button1_Click(object? sender, EventArgs e)
    {
        //listView1.Clear();
        listView1.Groups.Add("listViewGroup11", "listViewGroup11");
        listView1.Groups.Add("listViewGroup21", "listViewGroup21");
        listView1.Items.Add(new ListViewItem(Resources.ListViewForm_button1_Click_1_Add_group_and_data_simultaneously)
        { ForeColor = Color.Red, BackColor = Color.Yellow, Group = listView1.Groups[0] });
        listView1.Items.Add(new ListViewItem(Resources.ListViewForm_button1_Click_2_Add_data_to_a_specified_group)
        { ForeColor = Color.Red, BackColor = Color.Yellow, Group = listView1.Groups[1] });

        var m = new ListViewItem(Resources.ListViewForm_button1_Click_3_This_is_a_method_to_add_multi_column_data, 0);
        m.Checked = true;
        m.Selected = true;
        m.ForeColor = Color.Green;
        m.BackColor = Color.Yellow;
        m.SubItems.Add(new ListViewItem.ListViewSubItem(m, Resources.ListViewForm_button1_Click_4_Sub_column_data_1, Color.HotPink, Color.Gray,
            new Font(FontFamily.GenericSansSerif, 16)));
        m.SubItems.Add(new ListViewItem.ListViewSubItem(m, Resources.ListViewForm_button1_Click_5_Sub_column_data_2));
        m.SubItems.Add(new ListViewItem.ListViewSubItem(m, Resources.ListViewForm_button1_Click_6_Sub_column_data_3_Sub_column_data_3_Sub_column_data_3));
        m.SubItems.Add(new ListViewItem.ListViewSubItem(m, Resources.ListViewForm_button1_Click_4_Sub_column_data_1));
        listView1.Items.Add(m);
    }

    private void listView1_ColumnReordered(object? sender, ColumnReorderedEventArgs e)
    {

    }

    private void listBox1_SelectedIndexChanged(object? sender, EventArgs e)
    {
        richTextBox1.Text += $"{listBox1.SelectedIndex}-{listBox1.SelectedValue}\n";
    }
}