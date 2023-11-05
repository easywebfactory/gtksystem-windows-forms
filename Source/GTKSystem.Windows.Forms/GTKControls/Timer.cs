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
            add { TimersTimer.Elapsed += (object sender, Timers.ElapsedEventArgs e)=> { value.Invoke(sender, e); }; }
            remove { TimersTimer.Elapsed -= (object sender, Timers.ElapsedEventArgs e) => { value.Invoke(sender, e); }; }
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