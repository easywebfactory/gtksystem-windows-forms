using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace System.Windows.Forms
{
    public sealed class Application
    {
        static Application() {
            Init();
        }

        private static string appDataDirectory { get {
                string[] assemblyFullName = Assembly.GetEntryAssembly().FullName.Split(",");
                string _namespace = assemblyFullName[0];
                AssemblyName assembly = Assembly.GetExecutingAssembly().GetName();
                return Path.Combine(_namespace, assembly.Name, assembly.Version.ToString());
            }
        }

        public static string CommonAppDataPath {
            get {
                return Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),appDataDirectory);
            } 
        }
        public static string UserAppDataPath
        {
            get
            {
                return Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),appDataDirectory);
            }
        }
        public static string LocalUserAppDataPath
        {
            get
            {
                return Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),appDataDirectory);
            }
        }
        public static string ExecutablePath { 
            get {
                System.Diagnostics.ProcessModule module = System.Diagnostics.Process.GetCurrentProcess().MainModule;
                if (module.ModuleName.ToLower() == "dotnet" || module.ModuleName.ToLower() == "dotnet.exe")
                    return Assembly.GetEntryAssembly().Location;
                else
                    return module.FileName;
            }
        }
        public static string StartupPath { get { return System.IO.Directory.GetCurrentDirectory(); } }

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

        public static Gtk.Application App { get; private set; }
        public static Gtk.Application Init()
        {
            if (App == null)
            {
                Gtk.Application.Init();
                App = new Gtk.Application("GtkSystem.Windows.Forms", GLib.ApplicationFlags.IsLauncher);
                //App = new Gtk.Application("GtkSystem.Windows.Forms", GLib.ApplicationFlags.None);
                //App.Register(GLib.Cancellable.Current);
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
.DefaultThemeStyle button{padding:0px;border-radius:0px;}
.DefaultThemeStyle border{border:solid 1px #cccccc;}
.DefaultThemeStyle entry{background-color:#ffffff; padding: 0px 6px;border-radius:0px;}

/*
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


.Form {border-width:0px;margin:0px;box-shadow: none;}
.ScrollForm {border-right:solid 6px #e6e5e4;}
.UserControl{border-width:0px;}

.MessageBox{}
.MessageBox button{margin:10px;}
.MessageBox-BarTitle{font-size:20px;padding-bottom:10px;}
.TabControl{} 
.TabControl header.top{background-color:#e7e5e3;} 
.TabControl header.top tab:hover{background-color:#EFEFEF;} 
.TabControl header.top tab:checked{background-color:#f9f9f9;} 

.DataGridView{margin:0px;}
.DataGridView treeview.view{background-color:#ffffff;margin:0px;border-bottom:solid 1px #dddddd;border-left-width:0px;border-top-width:0px;;border-right-width:0px;}
.DataGridView treeview.view:hover{background-color:#ffff88;}
.DataGridView treeview.view:selected{background-color:#5555ff;color:#ffffff;}
.DataGridView treeview.view header{background-color:#EFEFEF;}
.DataGridView treeview.view header button{background-color:#f9f9f9; padding-top:6px;padding-bottom:6px;border-radius:0px;}
.DataGridView treeview.view header button:hover{background-color:#cccccc;}

.DataGridView button{} 
.GridViewCell-Button{ font-size:12px; border:solid 1px #aaaaaa; background:linear-gradient(#e9e9e9,#e0e0e0);}
.GridViewCell-Button:hover{  border:solid 1px #aaaaaa;background:linear-gradient(#eeeeee,#efefef);}
.GridViewCell-Button:selected{ color:blue}
.TreeView{border-bottom-width:0px;border-left-width:0px;border-top-width:1px;;border-right-width:0px;}
.TreeView button{border-left-width:0px;border-right-width:0px;}
.TreeView:selected{ color:blue}

.Button{padding:0px;} 

.TextBox{caret-color:#999999;} 
.RichTextBox {border-width:1px;caret-color:#999999;}
.RichTextBox border.top{border-width:1px;}
.RichTextBox border.left{border-width:1px;}
.RichTextBox border.right{border-width:1px;}
.RichTextBox border.bottom{border-width:1px;}
.CheckBox {border-width:0px;} 
.CheckedListBox {background-color:#ffffff;} 
.RadioButton {border-width:0px;} 

.Label{border-width:0px;background-color:#F6F5F4;} 
.LinkLabel{border-width:0px;} 
.NumericUpDown{min-height:6px;min-width:6px;caret-color:#999999;}
.NumericUpDown button.up{border-width:0px;padding:0px;font-size:6px;min-height:6px;min-width:6px;}
.NumericUpDown button.down{border-width:0px;padding:0px;font-size:6px;min-height:6px;min-width:6px;}
.NumericUpDown entry{border-width:0px;padding:0px 0px 0px 3px;min-height:6px;min-width:6px;} 
.NumericUpDown entry:focus{border-width:0px;}

.ComboBox{border-width:0px;}
.ComboBox button{padding:0px 5px 0px 5px; border-width:1px 0px 1px 0px;border-color:#c9c9c9;}
.ComboBox entry{padding:0px 5px;border:solid 1px #c9c9c9;}

.DropDownList button{padding:0px; border-width:1px 0px 1px 0px;border-color:#c9c9c9;}
.DropDownList entry{padding:0px 5px;background:linear-gradient(#F6F5F3,#F1F0EE,#ECEBE7);border:solid 1px #c9c9c9;}
.DropDownList entry:hover{background:linear-gradient(#F6F5F3,#F6F5F3);}


.Panel{border-width:0px;} 
.SplitContainer.horizontal{border-width:1px;}
.GroupBox{border-color:#DCDCDC;} 
.TableLayoutPanel viewport{border:solid 1px #eeeeee;}
.FlowLayoutPanel{}

.MenuStrip{padding:0px;background-color:#F6F5F4;background-image:none;border-width:0px;}
.MenuStrip viewport{border-width:0px;} 
.ToolStrip{padding:0px;background:linear-gradient(#fefefd,#efefef);border-width:0px;background-color:#F6F5F4;} 
.ToolStrip viewport{border-width:0px;} 
.ToolStripMenuItemNoChecked check{color:transparent;opacity:0;} 
.ToolStripSeparator{background-color:#C9C9C9;border-bottom: 1px inset rgba(250, 250, 250, 1);border-right: 1px inset rgba(250, 250, 250, 1);}

.StatusStrip{padding:0px;background-image:linear-gradient(#ECECEC,#e7e5e3,#e7e5e3); border-width:0px; border-top:solid 1px #c6c6c6;}
.StatusStrip viewport{border-width:0px;} 


.ListBox{border-width:1px;background-color:#ffffff; }
.ListView{background-color:#ffffff; }
.ListView .Label{background-color:transparent;} 
.ListViewHeader { background-color:#eeeeee;}
.ListView .Group{background-color:#fefefe; background-image:linear-gradient(#ffffff 12px,#3333bb 3px,#ffffff 14px);}
.ListView .Title{background-color:#fefefe; padding-left:5px;padding-right:5px; color:#3333bb;}
.ListView .SubTitle{padding-left:5px;padding-right:5px;color:#666666; font-size:14px; }

");

                Gtk.StyleContext.AddProviderForScreen(Gdk.Screen.Default, css, 800);
            }
            return App;
        }
        private static void QuitActivated(object sender, EventArgs e)
        {
            Gtk.Application.Quit();
        }
        public static bool SetHighDpiMode(HighDpiMode highDpiMode) {
             return true;
         }
        public static void EnableVisualStyles() {

        }
        public static void SetCompatibleTextRenderingDefault(bool defaultValue) {
            
        }

        public static void Run(Form mainForm)
        {
            mainForm.Widget.Destroyed += Control_Destroyed;
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

    public static class InitAppliction
    {
       private static Gtk.Application app = Application.Init();
        static InitAppliction()
        {

        }
    }
}
public sealed class ApplicationConfiguration
{
    public static void Initialize()
    {
        global::System.Windows.Forms.Application.EnableVisualStyles();
        global::System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
        global::System.Windows.Forms.Application.SetHighDpiMode(HighDpiMode.SystemAware);
    }
}
