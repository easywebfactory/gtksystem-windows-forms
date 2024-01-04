/*
 * 基于GTK3.24.24.34版本组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://gitee.com/easywebfactory, https://www.cnblogs.com/easywebfactory
 * author:chenhongjin
 * date: 2024/1/3
 */
using System.ComponentModel;

namespace System.Windows.Forms
{
    [DefaultEvent("Tick")]
    [DefaultProperty("Interval")]
    [ToolboxItemFilter("System.Windows.Forms")]
    public class Timer : Component
    {
        System.Timers.Timer TimersTimer;
        public Timer() {

            TimersTimer = new Timers.Timer();
        }

        public Timer(IContainer container):this() {
           // container.Add(this);
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
            add { TimersTimer.Elapsed += (object sender, Timers.ElapsedEventArgs e)=> { value.Invoke(this, e); }; }
            remove { TimersTimer.Elapsed -= (object sender, Timers.ElapsedEventArgs e) => { value.Invoke(this, e); }; }
        }

        public void Start()
        {
            TimersTimer.Start();
        }

        public void Stop()
        {
            TimersTimer.Stop();
        }

        public override string ToString() { return "Timer"; }

        protected override void Dispose(bool disposing) {
            TimersTimer.Dispose();
            this.Dispose();
        }

        //protected virtual void OnTick(EventArgs e) { 
            
        //}
    }
}