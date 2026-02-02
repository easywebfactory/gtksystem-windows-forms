/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 */
using Gtk;
using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms;


[ProvideProperty(nameof(ToolTip), typeof(Control))]
[DefaultEvent(nameof(Popup))]
[ToolboxItemFilter("System.Windows.Forms")]
public partial class ToolTip : Component, IExtenderProvider
{

    internal const int DefaultDelay = 500;
    private const int ReshowRatio = 5;
    private const int AutoPopRatio = 10;
    private const int InfiniteDelay = short.MaxValue;
    private readonly Dictionary<Control, TipInfo> _tools = new();
    private readonly int[] _delayTimes = new int[4];
    private bool _auto = true;
    private bool _showAlways;
    private Control? _topLevelControl;
    private bool active = true;
    private Color _backColor = SystemColors.Info;
    private Color _foreColor = SystemColors.InfoText;
    private bool _isBalloon;
    private bool _isDisposing;
    private string _toolTipTitle = string.Empty;
    private ToolTipIcon _toolTipIcon = ToolTipIcon.None;
    private ToolTipTimer? _timer;
    private bool _stripAmpersands;
    private bool _useAnimation = true;
    private bool _useFading = true;
  
    public ToolTip(IContainer cont)
        : this()
    {
        if (cont == null)
            throw new ArgumentNullException(nameof(cont));

        cont.Add(this);
    }

    public ToolTip()
    {
        InitialDelay = 500;
    }

    [DefaultValue(true)]
    public bool Active
    {
        get => active;
        set
        {
            if (active != value)
            {
                active = value;

            }
        }
    }

    [DefaultValue(DefaultDelay)]
    public int AutomaticDelay
    {
        get => _delayTimes[0] < 10 ? DefaultDelay : _delayTimes[0];
        set
        {
            _delayTimes[0] = value;
        }
    }
    public int AutoPopDelay
    {
        get => Math.Max(1000, _delayTimes[1]);
        set
        {
            _delayTimes[1] = value;
        }
    }

    [DefaultValue(typeof(Color), "Info")]
    public Color BackColor
    {
        get => _backColor;
        set
        {
            _backColor = value;
        }
    }
    public Color ForeColor
    {
        get => _foreColor;
        set
        {
            if (value.IsEmpty)
            {
                throw new ArgumentException(string.Format("ToolTip Empty Color {0}:{1}", nameof(ForeColor)), nameof(value));
            }

            _foreColor = value;
        }
    }

    public bool IsBalloon
    {
        get => _isBalloon;
        set
        {
            if (_isBalloon != value)
            {
                _isBalloon = value;
            }
        }
    }

    public int InitialDelay
    {
        get => _delayTimes[2];
        set
        {
            _delayTimes[0] = value;
            _delayTimes[1] = value * AutoPopRatio;
            _delayTimes[2] = value;
            _delayTimes[3] = value / ReshowRatio;
        }
    }

    public bool OwnerDraw { get; set; }

    public int ReshowDelay
    {
        get => _delayTimes[3];
        set
        {
            _delayTimes[3] = value;
        }
    }

    public bool ShowAlways
    {
        get => _showAlways;
        set
        {
            if (_showAlways != value)
            {
                _showAlways = value;
            }
        }
    }

    public bool StripAmpersands
    {
        get => _stripAmpersands;
        set
        {
            if (_stripAmpersands != value)
            {
                _stripAmpersands = value;
            }
        }
    }

    public object? Tag { get; set; }

    public ToolTipIcon ToolTipIcon
    {
        get => _toolTipIcon;
        set
        {
            if (_toolTipIcon != value)
            {
                _toolTipIcon = value;
            }
        }
    }

    public string ToolTipTitle
    {
        get => _toolTipTitle;
        set
        {
            value ??= string.Empty;

            if (_toolTipTitle != value)
            {
                _toolTipTitle = value;
            }
        }
    }

    public bool UseAnimation
    {
        get => _useAnimation;
        set
        {
            if (_useAnimation != value)
            {
                _useAnimation = value;
            }
        }
    }

    public bool UseFading
    {
        get => _useFading;
        set
        {
            if (_useFading != value)
            {
                _useFading = value;
            }
        }
    }

    public event DrawToolTipEventHandler? Draw;
    public event PopupEventHandler? Popup;
    public bool CanExtend(object target) => target is Control;
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _isDisposing = true;
            try
            {
                RemoveAll();
            }
            finally
            {
                _isDisposing = false;
            }
        }

        base.Dispose(disposing);
    }
    public string? GetToolTip(Control? control)
    {
        if (control is null)
        {
            return string.Empty;
        }

        return _tools.TryGetValue(control, out TipInfo? tipInfo)
            ? tipInfo.Caption
            : string.Empty;
    }

    public void RemoveAll()
    {
        Control[] controls = _tools.Keys.ToArray();
        foreach (Control control in controls)
        {
            control.Widget.TooltipText = string.Empty;
            control.Widget.HasTooltip = false;
        }
        _tools.Clear();
        _topLevelControl = null;
    }
    public void SetToolTip(Control control, string? caption)
    {
        TipInfo info = new(caption, TipInfo.Type.Auto);
        _tools.Add(control, info);
        control.Widget.HasTooltip = true;
        control.Widget.QueryTooltip += Widget_QueryTooltip;
    }

    private void Widget_QueryTooltip(object o, QueryTooltipArgs args)
    {
        foreach (var tip in _tools)
        {
            if (IsBalloon == false)
            {
                TooltipShow(tip.Key.Widget, tip.Value.Caption, this.AutomaticDelay, this.AutoPopDelay);
            }
            else
            {
                TooltipShowBalloon(tip.Key.Widget, ref tip.Value.popover, tip.Value.Caption, this.AutomaticDelay, this.AutoPopDelay);
            }
        }
    }
    private void TooltipShowBalloon(Gtk.Widget widget, ref Gtk.Popover popover, string caption, int delay, int duration)
    {
        if (popover == null)
        {
            popover = new Gtk.Popover(widget);
            Gtk.Box box = new Gtk.Box(Gtk.Orientation.Horizontal, 0);
            box.Margin = 8;
            string tooltiptitle = ToolTipTitle;
            if (ToolTipIcon == ToolTipIcon.Info)
            {
                box.Add(new Gtk.Label() { Markup = "<span foreground=\"#0000ff\" size=\"x-large\">🔔</span>", Yalign = 0.5f });
                tooltiptitle = $"<span foreground=\"#0000ff\" size=\"large\">{tooltiptitle}</span>";
            }
            else if (ToolTipIcon == ToolTipIcon.Warning)
            {
                box.Add(new Gtk.Label() { Markup = "<span foreground=\"#e0a500\" size=\"x-large\">⚡ </span>", Yalign = 0.5f });
                tooltiptitle = $"<span foreground=\"#e0a500\" size=\"large\">{tooltiptitle}</span>";
            }
            else if (ToolTipIcon == ToolTipIcon.Error)
            {
                box.Add(new Gtk.Label() { Markup = "<span foreground=\"#ff0000\" size=\"x-large\">❌</span>", Yalign = 0.5f });
                tooltiptitle = $"<span foreground=\"#ff0000\" size=\"large\">{tooltiptitle}</span>";
            }

            Gtk.Label titlelabel = new Gtk.Label();
            titlelabel.Markup = $"{tooltiptitle}{Environment.NewLine}<span foreground=\"#333333\" >{caption}</span>";
            box.Add(titlelabel);
            box.ShowAll();
            popover.Add(box);
        }
        if (popover.IsVisible == false)
        {
            Gtk.Popover popover1 = popover;
            GLib.Timeout.Add((uint)delay, () =>
            {
                popover1?.ShowAll();
                if (_showAlways == false)
                {
                    GLib.Timeout.Add((uint)duration, () =>
                    {
                        popover1?.Hide();
                        return false;
                    });
                }
                return false;
            });
        }
    }
    private void TooltipShow(Gtk.Widget widget, string caption, int delay, int duration)
    {
        string tooltiptitle = ToolTipTitle;
        if (ToolTipIcon == ToolTipIcon.Info)
            tooltiptitle = $"<span foreground=\"#0000ff\" size=\"large\">🔔 {tooltiptitle}</span>";
        else if (ToolTipIcon == ToolTipIcon.Warning)
            tooltiptitle = $"<span foreground=\"#e0a500\" size=\"large\">⚡ {tooltiptitle}</span>";
        else if (ToolTipIcon == ToolTipIcon.Error)
            tooltiptitle = $"<span foreground=\"#ff0000\" size=\"large\">❌ {tooltiptitle}</span>";

        GLib.Timeout.Add((uint)Math.Max(0, delay - 500), () =>
        {
            widget.TooltipMarkup = $"{tooltiptitle}{Environment.NewLine}<span foreground=\"#333333\" >{caption}</span>";
            return false;
        });
    }
    public void Show(string? text, IWin32Window window)
    {
        ShowTooltip(text, window, 0);
    }

    public void Show(string? text, IWin32Window window, int duration)
    {
        ShowTooltip(text, window, duration);
    }

    public void Show(string? text, IWin32Window window, Point point)
    {
        ShowTooltip(text, window, 0);
    }

    public void Show(string? text, IWin32Window window, Point point, int duration)
    {
        ShowTooltip(text, window, duration);
    }
    public void Show(string? text, IWin32Window window, int x, int y)
    {
        ShowTooltip(text, window, 0);
    }

    public void Show(string? text, IWin32Window window, int x, int y, int duration)
    {
        ShowTooltip(text, window, duration);
    }
    private void ShowTooltip(string? text, IWin32Window window, int duration)
    {
        if(window is Control form && form.Widget.IsFocus)
        {
            foreach (var tip in _tools)
            {
                Control con = tip.Key;
                if (IsBalloon)
                {
                    TooltipShowBalloon(tip.Key.Widget, ref tip.Value.popover, tip.Value.Caption, this.AutomaticDelay, this.AutoPopDelay);
                }
                else
                {
                    TooltipShow(tip.Key.Widget, tip.Value.Caption, this.AutomaticDelay, duration);
                }
            }
        }
    }
    public void Hide(IWin32Window win)
    {
        if (win is Control tool)
        {
            if (_tools.ContainsKey(tool))
            {
                _tools[tool].popover?.Hide();
            }
        }
    }
  
    private void TimerHandler(object? source, EventArgs args)
    {
        Hide(((ToolTipTimer)source!).Host);
    }
    ~ToolTip() { 
    
    }

    public override string ToString()
    {
        string s = base.ToString();
        return $"{s} InitialDelay: {InitialDelay}, ShowAlways: {ShowAlways}";
    }
}
