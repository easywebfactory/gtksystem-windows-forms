/*
 * A cross-platform interface component developed based on GTK components and compatible with the native C# control winform interface.
 * Use this component GTKSystem.Windows.Forms instead of Microsoft.WindowsDesktop.App.WindowsForms, compile once, run across platforms windows, linux, macos
 * Technical support 438865652@qq.com, https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */

namespace System.Windows.Forms;

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