/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */
using System.Windows.Forms;

namespace GTKSystem.Windows.Forms.GTKControls.ControlBase;

public sealed class PropertyGridBase:Gtk.Paned, IControlGtk
{
    public IGtkControlOverride Override { get; set; }
    public Gtk.Box child2 = new(Gtk.Orientation.Vertical,2);
    public Gtk.ScrolledWindow child1 = new();
    readonly Gtk.Label titleLabel = new("name") { Xalign = 0, Yalign = 0 };
    readonly Gtk.Label descriptionLabel = new("description") { Xalign = 0, Yalign = 0 };
    public PropertyGridBase() : base(Gtk.Orientation.Vertical)
    {
        Override = new GtkControlOverride(this);
        Override.AddClass("PropertyGrid");
        Valign = Gtk.Align.Start;
        Halign = Gtk.Align.Start;
        WideHandle = true;
        child2.BorderWidth = 1;
        var attrList = new Pango.AttrList();
        attrList.Insert(new Pango.AttrWeight(Pango.Weight.Bold));
        titleLabel.Attributes = attrList;
        child2.PackStart(titleLabel, false, true, 2);
        child2.PackStart(descriptionLabel, true, true, 2);

        Pack1(child1, true, true);
        Pack2(child2, false, true);
    }
         
    public void ShowDescription(string? title,string? description)
    {
        titleLabel.Text = title;
        descriptionLabel.Text = description;
    }
    protected override void OnShown()
    {
        Override.OnAddClass();
        base.OnShown();
    }
}