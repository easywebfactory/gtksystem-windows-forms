using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Threading;

namespace System.Windows.Forms
{
    public sealed class Application
    {
        static Application() {
        }

        private static string appDataDirectory { get {
                string[] assemblyFullName = Assembly.GetExecutingAssembly().FullName.Split(",");
                string _namespace = assemblyFullName[0];
                string _version = assemblyFullName[1].Split("=")[1];
                return $"{_namespace}\\{Assembly.GetExecutingAssembly().GetName().Name}\\{_version}";
            }
        }

        public static string CommonAppDataPath {
            get {
                string thispath = $"{System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)}\\{appDataDirectory}";
                if(!System.IO.Directory.Exists(thispath))
                    System.IO.Directory.CreateDirectory(thispath);
                return thispath;
            } 
        }
        public static string UserAppDataPath
        {
            get
            {
                string thispath = $"{System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\{appDataDirectory}";
                if (!System.IO.Directory.Exists(thispath))
                    System.IO.Directory.CreateDirectory(thispath);
                return thispath;
            }
        }
        public static string LocalUserAppDataPath
        {
            get
            {
                string thispath = $"{System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\{appDataDirectory}";
                if (!System.IO.Directory.Exists(thispath))
                    System.IO.Directory.CreateDirectory(thispath);
                return thispath;
            }
        }
        public static string ExecutablePath { get { return Environment.ProcessPath; } }
        public static string StartupPath { get { return Environment.CurrentDirectory; } }

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
            //粉红色主题
//.DefaultThemeStyle{border:solid 1px #ddaaaa;border-radius:0px;box-shadow: none;color:#993333;}
//.DefaultThemeStyle.background{background-color:#ffeeee;}
//.DefaultThemeStyle.titlebar{background-color:#996666;}
//.DefaultThemeStyle border{border:solid 1px #ddaaaa;}
//.DefaultThemeStyle button{color:#993333;border-radius:0px;}
//.DefaultThemeStyle entry{border-radius:0px;}
//.DefaultThemeStyle label{color:#993333;}
//.DefaultThemeStyle>button{border:solid 1px #cccccc;}
//.DefaultThemeStyle>entry{border:solid 1px #cccccc;}
//.DefaultThemeStyle header.top{background-color:#ffcccc;} 
//.DefaultThemeStyle header.top tab:hover{background-color:#ffeeee;} 
//.DefaultThemeStyle stack{background-color:#ffeeee;padding:0px;margin:0px;} 
//.DefaultThemeStyle .view button{background-color:#ffcccc;}
//.DefaultThemeStyle:focus{border-color:#000099;}
//.DefaultThemeStyle:active{border-color:#000099;}
            css.LoadFromData(@"
.DefaultThemeStyle{border-radius:0px;border:solid 1px #cccccc;box-shadow: none;color:#000000;}
.DefaultThemeStyle>entry{border:solid 1px #cccccc;}
.DefaultThemeStyle>button{border:solid 1px #cccccc;}
.DefaultThemeStyle button{border-radius:0px;}
.DefaultThemeStyle border{border:solid 1px #cccccc;}
.DefaultThemeStyle entry{border-radius:0px;background-color:#ffffff;}

/*
.DefaultThemeStyle{border:solid 1px #cccccc;border-radius:0px;box-shadow: none;color:#000000;}
.DefaultThemeStyle .background{background-color:#F6F5F4;}
.DefaultThemeStyle .titlebar{background-color:#666655;}
.DefaultThemeStyle .view{background:#ffffff}
.DefaultThemeStyle label{color:#000000;}
.DefaultThemeStyle header.top{background-color:#CFCFCF;} 
.DefaultThemeStyle header.top tab{background-color:#C7C7C7;} 
.DefaultThemeStyle header.top tab:hover{background-color:#EFEFEF;} 

.DefaultThemeStyle stack{background-color:#ffffff;padding:0px;margin:0px;} 
.DefaultThemeStyle .view button{background:linear-gradient(#f6f6f6,#f9f9f9)}
.DefaultThemeStyle:focus{border-color:#eeeeee;}
.DefaultThemeStyle:active{border-color:#cccccc;}
*/

.Form{border-width:0px;}
.BorderRadiusStyle{ ;}
.MessageBox{}
.MessageBox button{margin:10px;border-radius:0px;}
.MessageBox-BarTitle{font-size:20px;padding-bottom:10px;}
.TabControl{padding:0px;} 
.DataGridView{margin:0px;}
.DataGridView treeview.view{margin:0px;padding:0px;border-bottom:solid 1px #dddddd;border-left-width:0px;border-top-width:0px;;border-right-width:0px;}
.DataGridView button{} 
.GridViewCell-Button{ font-size:12px; border:solid 1px #c0c0c0; border-radius:0px; background:linear-gradient(#eeeeee,#e2e2e2);box-shadow:0px 1px 1px 1px #eeeeee;}
.GridViewCell-Button:hover{ background:linear-gradient(#f6f6f6,#f9f9f9);}
.GridViewCell-Button:selected{ color:blue}
.TreeView{border-bottom-width:0px;border-left-width:0px;border-top-width:1px;;border-right-width:0px;}
.TreeView button{border-left-width:0px;border-right-width:0px;}
.TreeView:selected{ color:blue}

.Button{padding:0px;} 
.TextBox{background-color:#ffffff;padding:0px 3px 0px 3px;} 
.RichTextBox {border-width:1px;}
.RichTextBox border.top{border-width:1px;}
.RichTextBox border.left{border-width:1px;}
.RichTextBox border.right{border-width:1px;}
.RichTextBox border.bottom{border-width:1px;}
.CheckBox {border-width:0px;} 
.CheckedListBox {background-color:#ffffff;} 
.RadioButton {border-width:0px;} 
.Label{border-width:0px;} 
.LinkLabel{border-width:0px;} 
.NumericUpDown{padding:0px;min-height:6px;min-width:6px;}
.NumericUpDown button.up{border-width:0px;padding:0px;font-size:6px;min-height:6px;min-width:6px;}
.NumericUpDown button.down{border-width:0px;padding:0px;font-size:6px;min-height:6px;min-width:6px;}
.NumericUpDown entry{border-width:0px;padding:0px 0px 0px 3px;min-height:6px;min-width:6px;} 
.NumericUpDown entry:focus{border-width:0px;}
.ComboBox{border-width:0px;padding:0px;}
.ComboBox button{padding:0px;}
.ComboBox entry{padding:0px;}
.Panel{background-color:#F6F5F4;} 
.SplitContainer.horizontal{border-width:1px;}
.GroupBox{background-color:#F6F5F4;} 
.MenuStrip{background-color:#F6F5F4;}
.ListBox{border-width:1px;background-color:#ffffff; padding:0px;}
.ListView{background-color:#ffffff; padding:0px;}
.ListViewHeader button{padding:0px;border-right:solid 1px #cccccc;border-top-width:0px;border-left-width:0px;border-bottom-width:0px;}
.listviewgroup{background-color:#fefefe; background-image:linear-gradient(#ffffff 12px,#ccccce 3px,#ffffff 14px);}
.listviewtitle{background-color:#fefefe; padding-left:5px;padding-right:5px; }
.listviewsubtitle{padding-left:5px;padding-right:5px;color:#666666; font-size:12px; }
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
