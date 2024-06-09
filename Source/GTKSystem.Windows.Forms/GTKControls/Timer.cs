/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://gitee.com/easywebfactory, https://github.com/easywebfactory, https://www.cnblogs.com/easywebfactory
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
        readonly System.Timers.Timer TimersTimer = new Timers.Timer();
        private const string TICK_EVENT_KEY = "Form_Timer_Tick";
        public Timer()
        {
        }

        public Timer(IContainer container) : this()
        {
        }

        [Bindable(true)]
        [DefaultValue(null)]
        [Localizable(false)]
        [TypeConverter(typeof(StringConverter))]
        public object Tag { get; set; }

        [DefaultValue(false)]
        public virtual bool Enabled { get => TimersTimer.Enabled; set => TimersTimer.Enabled = value; }

        [DefaultValue(100)]
        public int Interval { get => (int)TimersTimer.Interval; set => TimersTimer.Interval = value; }

        public event EventHandler Tick
        {
            add
            {
                Events.AddHandler(TICK_EVENT_KEY, value);
                TimersTimer.Elapsed += TimersTimer_Elapsed;
            }
            remove
            {
                if (Events[TICK_EVENT_KEY] != null)
                    Events.RemoveHandler(TICK_EVENT_KEY, value);
                TimersTimer.Elapsed -= TimersTimer_Elapsed;
            }
        }

        private void TimersTimer_Elapsed(object sender, Timers.ElapsedEventArgs e)
        {
            if (Events[TICK_EVENT_KEY] != null)
                Gtk.Application.Invoke((EventHandler)Events[TICK_EVENT_KEY]);
        }

        public void Start()
        {
            TimersTimer.Start();
        }

        public void Stop()
        {
            TimersTimer.Stop();
        }

        public override string ToString() { return "System.Windows.Forms.Timer"; }

        protected override void Dispose(bool disposing)
        {
            TimersTimer.Dispose();
            base.Dispose();
        }
        protected new void Dispose()
        {
            Dispose(true);
        }
    }
}