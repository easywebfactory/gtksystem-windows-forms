/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */
using System.Windows.Forms;

namespace GTKSystem.Windows.Forms.GTKControls.ControlBase
{
    public sealed class PropertyGridBase:Gtk.Paned, IControlGtk
    {
        public GtkControlOverride Override { get; set; }
        public Gtk.Box child2 = new Gtk.Box(Gtk.Orientation.Vertical,2);
        public Gtk.ScrolledWindow child1 = new Gtk.ScrolledWindow();
        Gtk.Label titleLabel = new Gtk.Label("name") { Xalign = 0, Yalign = 0 };
        Gtk.Label descriptionLabel = new Gtk.Label("description") { Xalign = 0, Yalign = 0 };
        public PropertyGridBase() : base(Gtk.Orientation.Vertical)
        {
            this.Override = new GtkControlOverride(this);
            this.Override.AddClass("PropertyGrid");
            this.Valign = Gtk.Align.Start;
            this.Halign = Gtk.Align.Start;
            this.WideHandle = true;
             child2.BorderWidth = 1;
            Pango.AttrList attrList = new Pango.AttrList();
            attrList.Insert(new Pango.AttrWeight(Pango.Weight.Bold));
            titleLabel.Attributes = attrList;
            child2.PackStart(titleLabel, false, true, 2);
            child2.PackStart(descriptionLabel, true, true, 2);

            this.Pack1(child1, true, true);
            this.Pack2(child2, false, true);
        }
         
        public void ShowDescription(string title,string description)
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
}
