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
        private protected EventHandler? _onTimer;
        private uint _interval = 100;
        private bool _enabled;
        private uint _timerId = 0;
        private readonly object _syncObj = new();
        public Timer() : base()
        {
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
            _onTimer?.Invoke(this, EventArgs.Empty);
        }
        public object Tag { get; set; }

        [DefaultValue(false)]
        public virtual bool Enabled { get => _enabled;
            set
            {
                lock (_syncObj)
                {
                    if (_enabled != value)
                    {
                        _enabled = value;
                        if (value)
                        {
                            _timerId = Gdk.Threads.AddTimeout(0, _interval, () =>
                            {
                                OnTick(EventArgs.Empty);
                                return true;
                            });
                        }
                        else
                        {
                            if (_timerId != 0)
                                GLib.Timeout.Remove(_timerId);
                        }
                    }
                }
            }

        }

        [DefaultValue(100)]
        public int Interval { get => (int)_interval; 
            set {
                lock (_syncObj)
                {
                    if (_interval != value)
                    {
                        _interval = (uint)value;
                        if (_enabled)
                        {
                            Enabled = false;
                            Enabled = true;
                        }
                    }
                }
            }
        }

        public void Start()
        {
            Enabled = true;
        }

        public void Stop()
        {
            Enabled = false;
        }

        public override string ToString() => $"{base.ToString()}, Interval: {Interval}";

        protected override void Dispose(bool disposing)
        {
            Enabled = false;
            base.Dispose(disposing);
        }
    }
}