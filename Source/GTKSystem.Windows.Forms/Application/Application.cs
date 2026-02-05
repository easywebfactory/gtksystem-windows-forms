using Gtk;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace System.Windows.Forms
{
    public sealed class Application
    {
        static Application() {
            Init();
        }

        private static string appDataDirectory {
            get {
                AssemblyName assembly = Assembly.GetEntryAssembly().GetName();
                string[] assemblyFullName = assembly.FullName.Split(',');
                string _namespace = assemblyFullName[0];
                return Path.Combine(_namespace, assembly.Name, assembly.Version.ToString());
            }
        }

        public static string CommonAppDataPath {
            get {
                string dir = Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),appDataDirectory);
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
                return dir;
            } 
        }
        public static string UserAppDataPath
        {
            get
            {
                string dir = Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), appDataDirectory);
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
                return dir;
            }
        }
        public static string LocalUserAppDataPath
        {
            get
            {
                string dir = Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),appDataDirectory);
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
                return dir;
            }
        }
        private static string _excutablepath;
        public static string ExecutablePath { 
            get {
                if (_excutablepath == null)
                {
                    var args = Environment.GetCommandLineArgs();
                    if (args.Length > 0)
                    {
                        _excutablepath = args[0];
                    }
                    else
                    {
#if NET6_0_OR_GREATER
                        _excutablepath = Assembly.GetEntryAssembly().Location;
                        if (string.IsNullOrWhiteSpace(_excutablepath))
                            _excutablepath = Environment.ProcessPath;
#else
                        _excutablepath = Assembly.GetEntryAssembly().Location;
                        if (string.IsNullOrWhiteSpace(_excutablepath))
                            _excutablepath = Path.Combine(AppContext.BaseDirectory, Assembly.GetEntryAssembly()?.GetName().Name, ".dll");
#endif
                    }
                }
                return _excutablepath;
            }
        }
        private static string _startuppath;
        public static string StartupPath { get {
                if (_startuppath == null)
                {
                    string appstart = AppContext.BaseDirectory;
                    if (string.IsNullOrWhiteSpace(appstart))
                        appstart = Directory.GetCurrentDirectory();
                    else
                        Directory.SetCurrentDirectory(appstart);
                    _startuppath = appstart;
                }
                return _startuppath;
            } 
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
        public static FormCollection OpenForms
        {
            get
            {
                FormCollection forms = new FormCollection();
                var windows = Gtk.Window.ListToplevels().Where(w => w.IsVisible == true);
                foreach (Gtk.Window w in windows)
                    if (w.Data.ContainsKey("Control") && w.Data["Control"] is Form form)
                        forms.Add(form);
                return forms;
            }
        }
        public static void DoEvents()
        {
            while(Gtk.Application.EventsPending())
                Gtk.Application.RunIteration(false);
        }
        public static Gtk.Application App { get; private set; }
        public static Gtk.Application Init()
        {
            if (App == null)
            {
                string css_style = @"

/* 定义控件样式*/

@define-color frame_color  alpha(@theme_fg_color, 0.2);
@define-color frame3d_color  alpha(@theme_fg_color, 0.2);

@define-color line_color #ECECEC;
@define-color separator_color1 #C6C5C4;
@define-color separator_color2 #D6D7D8;

.DefaultThemeStyle{padding: 0px 2px; border-style:solid;min-height:6px;min-width:6px;}
.DefaultThemeStyle entry{
   padding: 4px 5px; border-width: 1px; border-style: solid; border-color:@frame_color;
   background-color: @theme_base_color; color: @theme_text_color;
}
.DefaultThemeStyle entry.flat{
   padding: 4px 5px; border-width: 1px; border-style: solid; border-color:@frame_color;
   background-color: @theme_base_color; color: @theme_text_color;
}
.DefaultThemeStyle button{padding:4px 3px;}
.titlebar{border-radius: 0px; border-bottom: solid 1px #cfcfcf;}
.ButtonNone{padding:0px;margin:0px;border-radius:0px;border-style:none;box-shadow:none;}
.ButtonNone.check,.ButtonNone check{padding-top:0px;padding-bottom:0px;;margin-top:0px;margin-bottom:0px;}
.BorderNone{border-style:none;box-shadow:none;}
.BorderFixedSingle{border-width:0px;border-style:none;padding:1px;box-shadow: inset 0px 0px 0px 1px @frame_color;}
.BorderFixed3D{border-width:0px;border-style:none; padding:2px; box-shadow: inset 1px 1px 1px 2px @frame3d_color;}

.DataGridView {border-width:1px;margin:-3px;}
.GridViewCell-Button{ color:@theme_text_color; border:solid 1px @frame_color; background-color: shade(@theme_bg_color, 0.7);}
.GridViewCell-Button:hover{background-color: shade(@theme_bg_color, 0.8);}
.GridViewCell-Button:selected{ color:blue}

.LinkLabel{border-style:none;}
.TextBox{}
.ComboBox{padding:0px;}
.ComboBox entry{border-right-width:0px;  }
.ComboBox entry.flat{border-right-width:0px;  }
.ComboBox entry:focus{border-right-width:0px; box-shadow: inset 0px 0px 0px 1px #62a0ea;}
.ComboBox button{padding-top:1px;padding-bottom:1px;border-width: 1px 1px 1px 1px; border-style: solid; border-color:@frame_color;}

.DropDownList button{padding:2px;}
.SplitContainer{padding:0px;border:0px;box-shadow:none;}
.SplitterPanel{padding:0px;margin:0px;border:0px;box-shadow:none;min-width:20px;min-height:20px;}
.SplitterPanel .frame{padding:0px;margin:0px;border:0px;box-shadow:none;}
.SplitterPanel .flat{padding:0px;margin:0px;border:0px;box-shadow:none;}

.TableLayoutPanel {box-shadow: inset -1px -1px 1px 0px @frame_color;padding:0px;}
.TableLayoutPanel>viewport.frame {box-shadow: inset 1px 1px 1px 0 @frame_color;border-width:0px;}
.ListView{border:inset 1px @frame_color;}
.ListView .Label{background-color:transparent;} 
.ListView checkbutton {padding:0px;}
.ListView .Label{background-color:transparent;} 
.ListView flowboxchild {padding:0px;}
.ListView flowboxchild viewport{padding:2px 0px;}
.ListView .GridBorder viewport{box-shadow: inset -1px -1px #eeeeee;}
.ListViewHeader {background-color:@theme_bg_color; padding:0px;min-height:6px; }
.ListViewHeader button{box-shadow:inset -1px 0px @frame_color;border-width:0px; border-radius:0px; padding:0px 0px 5px 0px;min-height:20px;min-width:6px;margin:0px; }
.ListViewHeader button.frame{border-width:0px;padding:0px 0px 0px 0px;margin:0px; }
.ListView .GroupLine{border-top:inset 1px #6677bb;}
.ListView .GroupTitle{padding-left:5px;padding-right:5px; color:#6677bb; }
.ListView .GroupSubTitle{padding-left:5px;padding-right:5px; }
.GroupBox>border{border: solid 1px rgba(76, 66, 66, 0.3);}

.ToolStrip,.MenuStrip{padding:0px;border-radius:0px;min-height:6px;min-width:6px;}
.ToolStrip>toolbutton,.ToolStrip>toolbutton label{padding:0px;margin:0px;}
.MenuStrip>menuitem,.MenuStrip>menuitem label{padding:0px;margin:0px;}
.ToolStrip button,.MenuStrip button{padding-top:0px;padding-bottom:0px; min-height:6px;min-width:6px;border-radius:0px;border-width:1px;}
.ToolStrip combox,.MenuStrip combox{padding-top:0px;padding-bottom:0px; min-height:6px;min-width:6px;border-radius:0px;border-width:0px;}
.ToolStrip entry,.MenuStrip entry{padding-top:0px;padding-bottom:0px; min-height:6px;min-width:6px;border-radius:0px;border-width:1px;}
.ToolStrip levelbar,.MenuStrip levelbar{padding:0px;min-height:6px;min-width:6px;border-radius:0px;border-width:0px;}
.StatusStrip {border-top:solid 1px @frame_color;}
.MenuItemButton {background-color:transparent;background:none; border:none; border-radius:0px;border-width:0px; padding:0px;box-shadow:none; min-width:1px;min-height:1px;}
.ToolStrip checkbutton,.MenuStrip checkbutton{margin-left:-20px; padding:0px; } 
.ToolStrip menu button image,.MenuStrip menu button image{margin-left:-22px;}
.ToolStrip menu button image+label,.MenuStrip menu button image+label{margin-left:-2px; }
.ToolStrip>separator,.MenuStrip>separator{padding:0px;margin:2px;}

.ContextMenuStrip button image{margin-left:-22px; }
.ContextMenuStrip button image+label{margin-left:-2px; }
.ContextMenuStrip checkbutton{margin-left:-22px;}
.NumericUpDown{padding:1px; min-height:6px;min-width:6px; }
.NumericUpDown button.up{padding:0px;}
.NumericUpDown button.down{padding:0px;}
.NumericUpDown.horizontal entry{margin:1px;border-width:0px;padding:2px 3px; min-height:6px;min-width:6px;} 
.NumericUpDown.vertical entry{margin:1px;border-width:0px;padding:2px 3px; min-height:6px;min-width:6px;} 
.TrackBar {border-width:0px;box-shadow:none;}
.PrintPreviewBack{background-color:#cccccc; border-radius:0px;}
.Paper{box-shadow: 0px 0px 3px 1px #999999;background:#ffffff; border-radius:0px;}
.PropertyGrid {box-shadow:0px 0px 0px 1px @frame_color; background:#eeeeee;}
.PropertyGrid button{background:#eeeeee;}

tooltip { background-color: #ffffff; color: #555555; border-radius: 0px; border: 1px solid #c9c9c9; box-shadow: 3px 3px 2px #787878;}
tooltip window,tooltip.background {background-color: #ffffff;}
tooltip label {padding: 0px 10px; color: #555555;}

";

                string appdirectory = StartupPath;
                string resourcepath = Path.Combine(appdirectory, "Resources");
                string themepath = Path.Combine(appdirectory, "theme");
                string themesetuppath = Path.Combine(themepath, "setup.theme");
                if (!Directory.Exists(resourcepath))
                {
                    Directory.CreateDirectory(resourcepath);
                }
                if (!Directory.Exists(themepath))
                {
                    Directory.CreateDirectory(themepath);
                }

                Gtk.Application.Init();
                App = new Gtk.Application("GtkSystem.Windows.Forms", GLib.ApplicationFlags.None);
                App.Register(GLib.Cancellable.Current);
                App.Shutdown += App_Shutdown;
                var quitAction = new GLib.SimpleAction("quit", null);
                quitAction.Activated += QuitActivated;
                App.AddAction(quitAction);
                Gtk.Settings settings = Gtk.Settings.Default;
                settings.SplitCursor = true;
                settings.EnableAnimations = true;
                settings.EnableTooltips = true;
                string iconpath = Path.Combine(appdirectory, "icon.png");
                if (File.Exists(iconpath))
                    Gtk.Window.SetDefaultIconFromFile(iconpath);

                Gtk.CssProvider css = new Gtk.CssProvider();
                StringBuilder cssBuilder=new StringBuilder();

                if (File.Exists(themesetuppath))
                {
                    string[] setuptheme = File.ReadAllLines(themesetuppath, Text.Encoding.UTF8);
                    Dictionary<string,string> nameValue = setuptheme.Where(w=>w.Contains("=")).ToDictionary(k => k.Split('=')[0],v=>v.Split('=')[1]);
                    nameValue.TryGetValue("UseDefaultStyle", out string usedef);
                    if (usedef != "false")
                        cssBuilder.AppendLine(css_style);

                    nameValue.TryGetValue("AutoTheme", out string autotheme);
                    if (autotheme == "false") {
                        if (nameValue.TryGetValue("DefaultThemeName", out string themename))
                            Gtk.Settings.Default.ThemeName = themename;
                    }
                    if (nameValue.TryGetValue("UseCustomTheme", out string usetheme))
                    {
                        if (usetheme == "true")
                        {
                            if (nameValue.TryGetValue("ThemeFolder", out string themefolder))
                            {
                                if (Directory.Exists(themefolder))
                                {
                                    try
                                    {
                                        string _themefolder = Path.GetFullPath(themefolder);
                                        Environment.SetEnvironmentVariable("GTK_DATA_PREFIX", _themefolder);
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex.Message);
                                    }
                                    finally
                                    {
                                        Gtk.Settings.Default.ThemeName = nameValue["Name"];
                                    }
                                }
                                else
                                {
                                    Console.WriteLine(themefolder + "=》目录不存在");
                                }
                            }
                        }
                    }
                    if (nameValue.TryGetValue("UseCustomStyle", out string customstyle))
                    {
                        if (customstyle == "true" && nameValue.TryGetValue("StylePath", out string stylefile))
                        {
                            string stylepath = Path.Combine(themepath, stylefile);
                            if (File.Exists(stylepath))
                            {
                                string styletext = File.ReadAllText(stylepath, Text.Encoding.UTF8);
                                cssBuilder.AppendLine(styletext);
                            }
                            else
                            {
                                File.WriteAllText(stylepath, "/* 这里可以自定义或调整控件样式 */ ", Text.Encoding.UTF8);
                            }
                        }
                    }
                }
                else
                {
                    cssBuilder.AppendLine(css_style);

                    StringBuilder setupthemecontent = new StringBuilder();
                    setupthemecontent.AppendLine("[setup]");
                    setupthemecontent.AppendLine("/*是否默认跟随系统主题*/");
                    setupthemecontent.AppendLine("AutoTheme=true");
                    setupthemecontent.AppendLine("/*是否应用内置样式*/");
                    setupthemecontent.AppendLine("UseDefaultStyle=true");
                    setupthemecontent.AppendLine("/*指定主题，AutoTheme=false时有效*/");
                    setupthemecontent.AppendLine("DefaultThemeName=Default");
                    setupthemecontent.AppendLine("/*是否使用自定义主题，对应[custom theme]*/");
                    setupthemecontent.AppendLine("UseCustomTheme=false");
                    setupthemecontent.AppendLine("/*是否使用自定义样式，对应[custom style]*/");
                    setupthemecontent.AppendLine("UseCustomStyle=true");

                    setupthemecontent.AppendLine().AppendLine("[custom theme]");
                    setupthemecontent.AppendLine("/* 自定义主题名称 */");
                    setupthemecontent.AppendLine("Name=mytheme");
                    setupthemecontent.AppendLine("/* 主题文件所在文件夹 */");
                    setupthemecontent.AppendLine("ThemeFolder=theme");

                    setupthemecontent.AppendLine().AppendLine("[custom style]");
                    setupthemecontent.AppendLine("/* 自由定义样式文件 */");
                    setupthemecontent.AppendLine("StylePath=style.css");

                    File.WriteAllText(themesetuppath, setupthemecontent.ToString(), Text.Encoding.UTF8);
                }

                css.LoadFromData(cssBuilder.ToString());
                Gtk.StyleContext.AddProviderForScreen(Gdk.Screen.Default, css, StyleProviderPriority.Application);
            }
            return App;
        }

        private static void QuitActivated(object sender, EventArgs e)
        {
            Gtk.Application.Quit();
        }
        private static void App_Shutdown(object sender, EventArgs e)
        {
            Console.WriteLine("App_Shutdown");
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
            if (mainForm.ShowInTaskbar == false)
            {
                Gtk.Window pw = new Gtk.Window(Gtk.WindowType.Toplevel);
                pw.WindowPosition = Gtk.WindowPosition.Center;
                pw.Decorated = false;
                pw.WidthRequest = 1;
                pw.HeightRequest = 1;
                pw.SkipTaskbarHint = true;
                mainForm.self.TransientFor = pw;
                pw.Shown += Pw_Shown;
                pw.Show();
            }
            mainForm.self.Destroyed += Control_Destroyed;
            mainForm.Show();
            Gtk.Application.Run();
        }
        private static void Pw_Shown(object? sender, EventArgs e)
        {
            Gtk.Window pw = (Gtk.Window)sender;
            pw.Window.Hide();
        }
        private static void Control_Destroyed(object sender, EventArgs e)
        {
            ExitThread();
        }

        public static void Exit()
        {
            ExitThread();
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public static void Exit(CancelEventArgs e)
        {
            lock (internalSyncObject)
            {
                try
                {
                    if (e == null)
                    {
                        Gtk.Application.Quit();
                    }
                    else
                    {
                        if (e.Cancel == false)
                        {
                            Gtk.Application.Quit();
                        }
                    }
                }
                finally
                {
                }
            }
        }

        public static void ExitThread()
        {
            lock (internalSyncObject)
            {
                try
                {

                    Gtk.Application.Quit();
                }
                finally { }
            }
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
