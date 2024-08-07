﻿using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
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
                if (!Directory.Exists("Resources"))
                {
                    Directory.CreateDirectory("Resources");
                }
                Gtk.Application.Init();
                App = new Gtk.Application("GtkSystem.Windows.Forms", GLib.ApplicationFlags.None);
                //App.Register(GLib.Cancellable.Current);
                App.Shutdown += App_Shutdown;
                var quitAction = new GLib.SimpleAction("quit", null);
                quitAction.Activated += QuitActivated;
                App.AddAction(quitAction);
                Gtk.Settings.Default.SplitCursor = true;
                Gtk.CssProvider css = new Gtk.CssProvider();
                string css_style = @"

/* 定义控件样式*/

@define-color frame_color  alpha(@theme_fg_color, 0.2);
@define-color frame3d_color  alpha(@theme_fg_color, 0.2);

@define-color line_color #ECECEC;
@define-color separator_color1 #C6C5C4;
@define-color separator_color2 #D6D7D8;

.DefaultThemeStyle{padding: 0px 2px; border-style:solid;}
.DefaultThemeStyle entry{
   padding: 0px 4px; border-width: 1px; border-style: solid; 
   background-color: @theme_base_color; color: @theme_text_color;
}
.DefaultThemeStyle entry.flat{
    border-width: 1px; border-style: solid; border-color:@frame_color;
}
.DefaultThemeStyle button{padding:4px 3px;}
.BorderNone{border-style:none;box-shadow:none;}

.BorderFixedSingle{border-width:0px;border-style:none;padding:1px;box-shadow: inset 0px 0px 0px 1px @frame_color;}
.BorderFixed3D{border-width:0px;border-style:none; padding:2px; box-shadow: inset 1px 1px 1px 2px @frame3d_color;}

.DataGridView {border-width:1px;margin:-3px;}
.GridViewCell-Button{ color:@theme_text_color; border:solid 1px @frame_color; background-color: shade(@theme_bg_color, 0.7);}
.GridViewCell-Button:hover{background-color: shade(@theme_bg_color, 0.8);}
.GridViewCell-Button:selected{ color:blue}

.LinkLabel{border-style:none;}
.TextBox{}
.ComboBox entry.flat{border-right-width:0px;}
.DropDownList button{padding:3px;}

.SplitContainer1 > separator {border-style:solid;border-width:2px;}
.TableLayoutPanel {box-shadow: 1px 1px 1px 0px @frame_color;}
.TableLayoutPanel viewport.frame {box-shadow: inset 1px 1px 1px 0 @frame_color;}
.ListView{}
.ListView checkbutton {padding:0px;}
.ListView .Label{background-color:transparent;} 
.ListViewHeader {background-color:@theme_bg_color; opacity:0.88; }
.ListView .GroupLine{border-top:inset 1px #6677bb;}
.ListView .GroupTitle{padding-left:5px;padding-right:5px; }
.ListView .GroupSubTitle{padding-left:5px;padding-right:5px; }
.StatusStrip{padding:0px; border-width:1px 0px 0px 0px; border-top:solid 1px @frame_color;}
.ToolStrip button{padding:0px;}

.NumericUpDown{border-width:1px;padding:3px; min-height:6px;min-width:6px;}
.NumericUpDown button.up{border-width:0px;padding:0px;min-height:6px;min-width:6px;}
.NumericUpDown button.down{border-width:0px;padding:0px;min-height:6px;min-width:6px;}
.NumericUpDown.horizontal entry{border-width:0px;padding:1px;min-height:6px;min-width:6px;} 
.NumericUpDown.vertical entry{border-width:0px;padding:1px;min-height:6px;min-width:6px;} 
.PrintPreviewBack{background-color:#cccccc; border-radius:0px;}
.Paper{box-shadow: 0px 0px 3px 1px #999999;background:#ffffff; border-radius:0px;}
                ";

                string defaulttheme = "theme/default/style/style.css";
                if (File.Exists(defaulttheme))
                    css_style = $"@import url(\"{defaulttheme}\");\n";
                
                string customstyle = $"theme/style_{typeof(Application).Assembly.GetName().Version}.css";
                if (File.Exists(customstyle))
                    css.LoadFromPath(customstyle);
                else
                {
                    if (!Directory.Exists("theme"))
                        Directory.CreateDirectory("theme");
                    File.WriteAllText(customstyle, css_style);
                    css.LoadFromData(css_style);
                }  
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
