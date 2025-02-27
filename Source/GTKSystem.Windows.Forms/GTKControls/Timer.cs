/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */

using System.ComponentModel;

namespace System.Windows.Forms;

[DefaultEvent("Tick")]
[DefaultProperty("Interval")]
[ToolboxItemFilter("System.Windows.Forms")]
public class Timer : Component
{
    readonly System.Timers.Timer timersTimer = new();
    public Timer()
    {
        timersTimer.Elapsed += TimersTimer_Elapsed;
    }

    public Timer(IContainer container) : this()
    {
    }
    private void TimersTimer_Elapsed(object? sender, Timers.ElapsedEventArgs e)
    {
        if (Tick != null)
            Gtk.Application.Invoke(Tick);
    }

    [Bindable(true)]
    [DefaultValue(null)]
    [Localizable(false)]
    [TypeConverter(typeof(StringConverter))]
    public object? Tag { get; set; }

    [DefaultValue(false)]
    public virtual bool Enabled { get => timersTimer.Enabled; set => timersTimer.Enabled = value; }

    [DefaultValue(100)]
    public int Interval
    {
        get => (int)timersTimer.Interval;
        set
        {
            if (value <= 0 || value== int.MaxValue)
            {
                throw new ArgumentOutOfRangeException();
            }
            timersTimer.Interval = value;
        }
    }

    public event EventHandler? Tick;
    public void Start()
    {
        timersTimer.Start();
    }

    public void Stop()
    {
        timersTimer.Stop();
    }

    public override string ToString() { return "System.Windows.Forms.Timer"; }

    protected override void Dispose(bool disposing)
    {
        timersTimer.Dispose();
        base.Dispose();
    }
    protected new void Dispose()
    {
        Dispose(true);
    }
}