/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */

using System.ComponentModel;

namespace System.Windows.Forms
{
    [DefaultEvent("Tick")]
    [DefaultProperty("Interval")]
    [ToolboxItemFilter("System.Windows.Forms")]
    public class Timer : Component
    {
        private System.Timers.Timer TimersTimer;
        private protected EventHandler? _onTimer;
        private int _interval = 100;
        private bool _enabled;
        private readonly object _syncObj = new();
        public Timer() : base()
        {
            TimersTimer = new Timers.Timer(_interval);
            TimersTimer.AutoReset = true;
            TimersTimer.Elapsed += TimersTimer_Elapsed;
        }

        public Timer(IContainer container) : this()
        {
            container.Add(this);
        }
        private void TimersTimer_Elapsed(object sender, Timers.ElapsedEventArgs e)
        {
            OnTick(EventArgs.Empty);
        }
        public event EventHandler Tick
        {
            add => _onTimer += value;
            remove => _onTimer -= value;
        }
        protected virtual void OnTick(EventArgs e)
        {
            if (_onTimer != null)
            {
                lock (_syncObj)
                    Gtk.Application.Invoke(_onTimer);
            }
        }
        public object Tag { get; set; }

        [DefaultValue(false)]
        public virtual bool Enabled { get => TimersTimer.Enabled; set => TimersTimer.Enabled = value; }

        [DefaultValue(100)]
        public int Interval { get => (int)TimersTimer.Interval; set => TimersTimer.Interval = value; }

        public void Start()
        {
            TimersTimer.Start();
        }

        public void Stop()
        {
            TimersTimer.Stop();
        }

        public override string ToString() => $"{base.ToString()}, Interval: {Interval}";

        protected override void Dispose(bool disposing)
        {
            TimersTimer.Stop();
            TimersTimer.Enabled = false;
            TimersTimer.Dispose();
            base.Dispose(disposing);
        }
    }
}