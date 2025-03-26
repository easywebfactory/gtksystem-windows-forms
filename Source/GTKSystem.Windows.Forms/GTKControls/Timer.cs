/*
 * A cross-platform interface component developed based on GTK components and compatible with the native C# control winform interface.
 * Use this component GTKSystem.Windows.Forms instead of Microsoft.WindowsDesktop.App.WindowsForms, compile once, run across platforms windows, linux, macos
 * Technical support 438865652@qq.com, https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
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