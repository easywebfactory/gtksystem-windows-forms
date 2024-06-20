/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://gitee.com/easywebfactory, https://github.com/easywebfactory, https://www.cnblogs.com/easywebfactory
 * author:chenhongjin
 */

using GTKSystem.Windows.Forms.GTKControls.ControlBase;
using System.ComponentModel;


namespace System.Windows.Forms
{
    [DesignerCategory("Component")]
    public partial class SplitContainer : ContainerControl
    {
        public readonly SplitContainerBase self = new SplitContainerBase();
        public override object GtkControl => self;
        public SplitContainer() : base()
        {
            _panel1 = new SplitterPanel(this);
            _panel2 = new SplitterPanel(this);
            self.Add1(_panel1.Widget);
            self.Add2(_panel2.Widget);
            self.Realized += Control_Realized;
            self.WidgetEvent += Self_WidgetEvent;
            self.ResizeChecked += Self_ResizeChecked;
        }
        private void Self_ResizeChecked(object sender, EventArgs e)
        {
            ResizeControls();
        }

        int position = 0;
        private void Self_WidgetEvent(object o, Gtk.WidgetEventArgs args)
        {
            if (self.IsRealized == true && self.IsMapped)
            {
                if (position < 1)
                {
                    position = self.Position;
                }
                else if (position != self.Position)
                {
                    ResizeControls();
                }
            }

        }

        private void ResizeControls()
        {
            Gtk.Application.Invoke(new EventHandler((o, e) =>
            {
                if (self.IsRealized == true && self.IsMapped)
                {
                    if (self.Toplevel is FormBase form)
                    {
                        try
                        {
                            int widthIncrement = 0;
                            int heightIncrement = 0;
                            int currposition = self.Position;
                            if (self.Orientation == Gtk.Orientation.Horizontal)
                            {
                                widthIncrement = currposition - _SplitterDistance;
                                heightIncrement -= self.HeightRequest - (int)self.Data["Height"];
                                form.ResizeControls(widthIncrement, heightIncrement, _panel1.self);
                                widthIncrement -= self.WidthRequest - (int)self.Data["Width"];
                                form.ResizeControls(-widthIncrement, -heightIncrement, _panel2.self);
                            }
                            else
                            {
                                heightIncrement = currposition - _SplitterDistance;
                                widthIncrement -= self.WidthRequest - (int)self.Data["Width"];
                                form.ResizeControls(-widthIncrement, heightIncrement, _panel1.self);
                                heightIncrement -= self.HeightRequest - (int)self.Data["Height"];
                                form.ResizeControls(-widthIncrement, -heightIncrement, _panel2.self);
                            }
                            position = currposition;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("splitcontainer resize:" + ex.Message);
                        }
                    }
                }
            }));
        }
        private void Control_Realized(object sender, EventArgs e)
        {
            if (self.Orientation == Gtk.Orientation.Horizontal)
            {
                _panel1.Width = self.Position;
                _panel1.Height = self.HeightRequest;
                _panel2.Width = self.WidthRequest - self.Position - 16;
                _panel2.Height = self.HeightRequest;
            }
            else
            {
                _panel1.Height = self.Position;
                _panel1.Width = self.WidthRequest;
                _panel2.Height = self.HeightRequest - self.Position - 16;
                _panel2.Width = self.WidthRequest;
            } 
        }

        private SplitterPanel _panel1;
        private SplitterPanel _panel2;
        public SplitterPanel Panel1
        {
            get
            {
                return _panel1;
            }
            set
            {
                _panel1 = value;
            }
        }
        public SplitterPanel Panel2 {
            get
            {
                return _panel2;
            }
            set
            {
                _panel2 = value;
            }
        }
        public int _SplitterDistance;
        public int SplitterDistance { get => self.Position + 5; set { _SplitterDistance = Math.Max(1, value - 5); self.Position = _SplitterDistance;  } }
        private int _SplitterWidth;
        public int SplitterWidth { get { return _SplitterWidth; } set { _SplitterWidth = value; self.WideHandle = value > 2; } }
        public int SplitterIncrement { get; set; }
        public Orientation Orientation {
            get { return self.Orientation == Gtk.Orientation.Horizontal ? Orientation.Vertical : Orientation.Horizontal; }
            set {
                self.Orientation = value == Orientation.Horizontal ? Gtk.Orientation.Vertical : Gtk.Orientation.Horizontal;
            }
        }
    }
}
