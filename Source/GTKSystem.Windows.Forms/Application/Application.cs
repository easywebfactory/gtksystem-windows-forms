using System.ComponentModel;
using System.Globalization;
using System.Threading;

namespace System.Windows.Forms
{
    public sealed class Application
    {
        static Application() {
           // Init();
        }
        private static readonly object internalSyncObject = new object();
        public static CultureInfo CurrentCulture
        {
            get
            {
                return Thread.CurrentThread.CurrentCulture;
            }
            set
            {
                Thread.CurrentThread.CurrentCulture = value;
            }
        }

        public static InputLanguage CurrentInputLanguage
        {
            get
            {
                return InputLanguage.CurrentInputLanguage;
            }
            set
            {
                InputLanguage.CurrentInputLanguage = value;
            }
        }

        public static Gtk.Application App;
        public static void Init()
        {
            Gtk.Application.Init();
            App = new Gtk.Application("GtkSystem.Windows.Forms", GLib.ApplicationFlags.None);
            App.Register(GLib.Cancellable.Current);
            var quitAction = new GLib.SimpleAction("quit", null);
            quitAction.Activated += QuitActivated;
            App.AddAction(quitAction);

            Gtk.CssProvider css = new Gtk.CssProvider();

            css.LoadFromData(@"window{ background:#eeeeee;border-width:0px} 
.NoStyleControl{ border-width:0px;border-style:none;background-color:transparent;border-radius:0px;box-shadow: none;}
.BorderRadiusStyle{ border-radius:0px;}
.BorderRadiusStyle button{ border-radius:0px;}
.BorderRadiusStyle entry{ border-radius:0px;}
.MessageBox{}
.MessageBox button{margin:10px;}
.MessageBox-BarTitle{font-size:20px;padding-bottom:10px;}

.TabControl{border:solid 1px #dddddd;} 
.DataGridView{border:solid 1px #dddddd;}
.DataGridView button{border:solid 1px #dddddd;} 
.GridViewCell-Button{ font-size:12px; border:solid 1px #c0c0c0; border-radius:0px; background:linear-gradient(#eeeeee,#e2e2e2);box-shadow:0px 1px 1px 1px #eeeeee;}
.GridViewCell-Button:hover{ background:linear-gradient(#f6f6f6,#f9f9f9);}
.GridViewCell-Button:selected{ color:blue}
.TreeView{border-bottom:solid 1px #dddddd;}
.TreeView button{border-bottom:solid 1px #dddddd;border-left-width:0px;border-right-width:0px;}

.Button{border:solid 1px #cccccc;padding:0px;} 
.TextBox{background-color:#ffffff;border:solid 1px #acacac;padding:0px 3px 0px 3px;} 
.TextBox:focus{border:solid 1px #0070cc;} 
textview.view{ border:solid 1px #999999;background-color:#999999; } 
.CheckedListBox {border:solid 1px #cccccc;background-color:#ffffff;} 
.Panel{border:solid 1px #cccccc;background-color:#eeeeee;} 
.SplitContainer{border:solid 1px #cccccc;} 
.GroupBox{} 
.LinkLabel{border:none;} 
.NumericUpDown{border-width:1px;padding:0px;min-height:6px;min-width:6px;}
.NumericUpDown button.up{padding:0px;font-size:6px;min-height:6px;min-width:6px;}
.NumericUpDown button.down{padding:0px;font-size:6px;min-height:6px;min-width:6px;}
.NumericUpDown entry{border-width:0px;padding:0px 0px 0px 3px;min-height:6px;min-width:6px;} 
.NumericUpDown entry:focus{border-width:0px;}
spinbutton.horizontal{border-width:1px;}
spinbutton:focus{border-width:1px;}
.progressbar{background:#0055ff;color:#ff5500} 
.ComboBox{border-width:0px;padding:0px;}
.ComboBox button{border:solid 1px #cccccc;padding:0px;}
.ComboBox entry{border:solid 1px #cccccc;padding:0px;}
.ButtonFontStyle{font-size:14px;color:red;}
");

            Gtk.StyleContext.AddProviderForScreen(Gdk.Screen.Default, css, 800);

        }
        private static void QuitActivated(object sender, EventArgs e)
        {
            Gtk.Application.Quit();
        }
        public static bool SetHighDpiMode(HighDpiMode highDpiMode) {
             return false;
         }
        public static void EnableVisualStyles() {

        }
        public static void SetCompatibleTextRenderingDefault(bool defaultValue) {
            Init();
        }

        public static void Run(Form mainForm)
        {
            mainForm.Control.Destroyed += Control_Destroyed;
            mainForm.Show();
            Gtk.Application.Run();
        }

        private static void Control_Destroyed(object sender, EventArgs e)
        {
            Exit(null);
        }

        public static void Exit()
        {
            Exit(null);
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public static void Exit(CancelEventArgs e)
        {
            bool cancelExit = false;
            lock (internalSyncObject)
            {
                try
                {
                    Gtk.Application.Quit();
                    cancelExit = true;
                }
                finally
                {
                 
                }
            }
            if (e != null)
            {
                e.Cancel = cancelExit;
            }
        }

        public static void ExitThread()
        {
            lock (internalSyncObject)
                Gtk.Application.Quit();
        }
    }

}
