using Gtk;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Linq;

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
                //App = new Gtk.Application("GtkSystem.Windows.Forms", GLib.ApplicationFlags.IsLauncher);
                App = new Gtk.Application("GtkSystem.Windows.Forms", GLib.ApplicationFlags.None);
                App.Register(GLib.Cancellable.Current);
                App.Shutdown += App_Shutdown;
                var quitAction = new GLib.SimpleAction("quit", null);
                quitAction.Activated += QuitActivated;
                App.AddAction(quitAction);

                Gtk.CssProvider css = new Gtk.CssProvider();

                string defaulttheme = "theme/default/style/style.css";
                string styletheme = "";
                if(File.Exists(defaulttheme))
                    styletheme = $"@import url(\"{defaulttheme}\");\n";

                styletheme += @"
                @define-color frame_color #C6C6C6;
                @define-color line_color #E9E9E9;
                @define-color separator_color1 #C6C5C4;
                @define-color separator_color2 #D6D7D8;

                .DefaultThemeStyle{border-color:@frame_color;}
                .DefaultThemeStyle button{}
                .DefaultThemeStyle border{border:solid 1px @frame_color;}
                .DefaultThemeStyle entry{background-color:#ffffff; padding: 0px 6px;}
                .DefaultThemeStyle entry.flat{border:solid 1px @frame_color;padding: 4px 4px;}
                .DefaultThemeStyle entry.flat:focus{border:solid 1px @frame_color;padding: 4px 4px;}
                .DefaultThemeStyle .frame{border-width:0px;border-color:@frame_color;border-style:solid; box-shadow:0px 0px 0px 1px @frame_color;}
                 
                .Form {border-width:0px;margin:0px;}
                .UserControl{ }

                .MessageBox{}
                .MessageBox button{margin:10px;}
                .MessageBox-BarTitle{font-size:20px;padding-bottom:10px;}

                .TabControl{margin-top:3px;} 
                .TabControl tab{margin-left:0px;margin-right:0px;margin-top:3px;padding-top:5px;padding-bottom:5px;} 

                .DataGridView {border-width:1px;margin:-3px;}
                .DataGridView button{} 
                .DataGridView treeview.view{margin:0px; border-bottom:solid 1px @line_color;border-left-width:0px;border-top-width:0px;border-right-width:0px;}
                .GridViewCell-Button{ font-size:12px; background:linear-gradient(#e9e9e9,#e0e0e0);}
                .GridViewCell-Button:hover{background:linear-gradient(#eeeeee,#efefef);}
                .GridViewCell-Button:selected{ color:blue}
 
                .TreeView .frame{}
                .TextBox{caret-color:#999999;box-shadow:none; } 

                .RichTextBox text{background-color:transparent;background:transparent;}
                .RichTextBox .view{background-color:transparent;background:transparent;}

                .CheckBox {border-width:0px;} 
                .CheckedListBox {background-color:#ffffff;} 
                .RadioButton {border-width:0px;} 
                .Button{padding:0px;} 
                .Label{border-width:0px; border-style:none;} 
                .LinkLabel{border-width:0px;} 

                .NumericUpDown{border-width:1px;padding:2px; min-height:6px;min-width:6px;}
                .NumericUpDown button.up{border-width:0px;padding:0px;font-size:6px;min-height:6px;min-width:6px;}
                .NumericUpDown button.down{border-width:0px;padding:0px;font-size:6px;min-height:6px;min-width:6px;}
                .NumericUpDown.horizontal entry{border-width:0px;padding:2px;min-height:6px;min-width:6px;} 
                .NumericUpDown.vertical entry{border-width:0px;padding:2px;min-height:6px;min-width:6px;} 

                .ComboBox{border-width:0px;}
                .ComboBox button{padding:0px 5px 0px 5px;}
                .ComboBox entry{padding:0px 5px;}

                .DropDownList button{padding:3px;box-shadow:none;}
                .DropDownList button.frame{box-shadow:none;}
                .DropDownList entry{padding:0px 5px;background:linear-gradient(#F6F5F3,#F1F0EE,#ECEBE7);border-right-width:0px;box-shadow:none;}
                .DropDownList entry.flat{border-right-width:0px;box-shadow:none;}
                .DropDownList entry:hover{background:linear-gradient(#F6F5F3,#F6F5F3);}

                .Panel{border-width:0px;} 
                .SplitContainer {border:solid 1px @frame_color;}
                .SplitContainer > separator {border-top:solid 4px @separator_color1;border-bottom:solid 1px @separator_color2;}

                .GroupBox{} 
                .TableLayoutPanel {box-shadow: 1px 1px 1px 0px #C6C6C6;}
                .TableLayoutPanel viewport.frame {box-shadow: inset 1px 1px 1px 0 @frame_color;}
                .FlowLayoutPanel{}

                .MenuStrip{padding:0px;border-width:0px;}
                .MenuStrip .frame{border-width:0px;}
                .ToolStrip{padding:0px;border-width:0px;} 
                .ToolStrip button{padding:0px 5px 0px 5px;}
                .ToolStrip entry{padding:0px 5px;}
                .ToolStrip .frame{border-width:0px;}
                 menu .MenuItem{padding:0px;margin-left:-23px;}
                 menu menuitem .MenuCheck{padding:0px;margin:0px;}
                .ToolStripSeparator{border-bottom: 1px inset rgba(250, 250, 250, 1);border-right: 1px inset rgba(250, 250, 250, 1);}
                .StatusStrip{padding:0px;border-width:0px;}
                .StatusStrip .frame{border-width:0px;}

                .ListBox {}
                .ListView{}
                .ListView checkbutton {padding:0px;}
                .ListView .Label{background-color:transparent;} 
                .ListViewHeader {background-color:#EFEEEE; opacity:0.88; }
                .ListView .Group{background-image:linear-gradient(#ffffff 12px,#3333bb 3px,#ffffff 14px);}
                .ListView .GroupLine{border-top:inset 1px #6677bb;}
                .ListView .Title{padding-left:5px;padding-right:5px; }
                .ListView .SubTitle{padding-left:5px;padding-right:5px; }
                ";
                string customstyle = "theme/default.css";
                if (File.Exists(customstyle))
                    styletheme += $"\n@import url(\"{customstyle}\");\n";

                css.LoadFromData(styletheme);
                Gtk.StyleContext.AddProviderForScreen(Gdk.Screen.Default, css, 600);
            }
            return App;
        }
        private static void QuitActivated(object sender, EventArgs e)
        {
            Gtk.Application.Quit();
        }
        private static void App_Shutdown(object sender, EventArgs e)
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
