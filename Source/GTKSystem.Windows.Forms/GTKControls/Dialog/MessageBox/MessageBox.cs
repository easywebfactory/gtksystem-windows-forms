/*
 * 基于GTK3.24.24.34版本组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://gitee.com/easywebfactory, https://www.cnblogs.com/easywebfactory
 * author:chenhongjin
 * date: 2024/1/3
 */
using Gtk;

namespace System.Windows.Forms
{
    public class MessageBox
    {
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //START WHIDBEY ADDS                                                                                           //
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Displays a message box with specified text, caption, and style with Help Button.
        /// </summary>
        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon,
                                        MessageBoxDefaultButton defaultButton, MessageBoxOptions options, bool displayHelpButton)
        {
            return ShowCore(null, text, caption, buttons, icon, defaultButton, options, displayHelpButton);
        }

        /// <summary>
        ///  Displays a message box with specified text, caption, style and Help file Path .
        /// </summary>
        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon,
                                        MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath)
        {
            return ShowCore(null, text, caption, buttons, icon, defaultButton, options);
        }

        /// <summary>
        ///  Displays a message box with specified text, caption, style and Help file Path for a IWin32Window.
        /// </summary>
        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon,
                                        MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath)
        {
            return ShowCore(owner, text, caption, buttons, icon, defaultButton, options);
        }

        /// <summary>
        ///  Displays a message box with specified text, caption, style, Help file Path and keyword.
        /// </summary>
        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon,
                                        MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, string keyword)
        {
            return ShowCore(null, text, caption, buttons, icon, defaultButton, options);
        }

        /// <summary>
        ///  Displays a message box with specified text, caption, style, Help file Path and keyword for a IWin32Window.
        /// </summary>
        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon,
                                        MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, string keyword)
        {
            return ShowCore(owner, text, caption, buttons, icon, defaultButton, options);
        }

        /// <summary>
        ///  Displays a message box with specified text, caption, style, Help file Path and HelpNavigator.
        /// </summary>
        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon,
                                        MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, HelpNavigator navigator)
        {
            return ShowCore(null, text, caption, buttons, icon, defaultButton, options);
        }

        /// <summary>
        ///  Displays a message box with specified text, caption, style, Help file Path and HelpNavigator for IWin32Window.
        /// </summary>
        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon,
                                        MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, HelpNavigator navigator)
        {
            return ShowCore(owner, text, caption, buttons, icon, defaultButton, options);
        }

        /// <summary>
        ///  Displays a message box with specified text, caption, style, Help file Path ,HelpNavigator and object.
        /// </summary>
        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon,
                                        MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, HelpNavigator navigator, object param)
        {
            return ShowCore(null, text, caption, buttons, icon, defaultButton, options);
        }

        /// <summary>
        ///  Displays a message box with specified text, caption, style, Help file Path ,HelpNavigator and object for a IWin32Window.
        /// </summary>
        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon,
                                        MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, HelpNavigator navigator, object param)
        {
            return ShowCore(owner, text, caption, buttons, icon, defaultButton, options);
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //END ADD                                                                                                      //
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Displays a message box with specified text, caption, and style.
        /// </summary>
        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon,
                                        MessageBoxDefaultButton defaultButton, MessageBoxOptions options)
        {
            return ShowCore(null, text, caption, buttons, icon, defaultButton, options, false);
        }

        /// <summary>
        ///  Displays a message box with specified text, caption, and style.
        /// </summary>
        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon,
                                        MessageBoxDefaultButton defaultButton)
        {
            return ShowCore(null, text, caption, buttons, icon, defaultButton, 0, false);
        }

        /// <summary>
        ///  Displays a message box with specified text, caption, and style.
        /// </summary>
        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            return ShowCore(null, text, caption, buttons, icon, MessageBoxDefaultButton.Button1, 0, false);
        }

        /// <summary>
        ///  Displays a message box with specified text, caption, and style.
        /// </summary>
        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons)
        {
            return ShowCore(null, text, caption, buttons, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, 0, false);
        }

        /// <summary>
        ///  Displays a message box with specified text and caption.
        /// </summary>
        public static DialogResult Show(string text, string caption)
        {
            return ShowCore(null, text, caption, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, 0, false);
        }

        /// <summary>
        ///  Displays a message box with specified text.
        /// </summary>
        public static DialogResult Show(string text)
        {
            return ShowCore(null, text, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, 0, false);
        }

        /// <summary>
        ///  Displays a message box with specified text, caption, and style.
        /// </summary>
        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon,
                                        MessageBoxDefaultButton defaultButton, MessageBoxOptions options)
        {
            return ShowCore(owner, text, caption, buttons, icon, defaultButton, options, false);
        }

        /// <summary>
        ///  Displays a message box with specified text, caption, and style.
        /// </summary>
        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon,
                                        MessageBoxDefaultButton defaultButton)
        {
            return ShowCore(owner, text, caption, buttons, icon, defaultButton, 0, false);
        }

        /// <summary>
        ///  Displays a message box with specified text, caption, and style.
        /// </summary>
        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            return ShowCore(owner, text, caption, buttons, icon, MessageBoxDefaultButton.Button1, 0, false);
        }

        /// <summary>
        ///  Displays a message box with specified text, caption, and style.
        /// </summary>
        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons)
        {
            return ShowCore(owner, text, caption, buttons, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, 0, false);
        }

        /// <summary>
        ///  Displays a message box with specified text and caption.
        /// </summary>
        public static DialogResult Show(IWin32Window owner, string text, string caption)
        {
            return ShowCore(owner, text, caption, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, 0, false);
        }

        /// <summary>
        ///  Displays a message box with specified text.
        /// </summary>
        public static DialogResult Show(IWin32Window owner, string text)
        {
            return ShowCore(owner, text, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, 0, false);
        }

        private static DialogResult ShowCore(IWin32Window owner, string text, string caption,
                                     MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton,
                                     MessageBoxOptions options)
        {
            DialogResult result = DialogResult.None;
            try
            {
                
                result = ShowCore(owner, text, caption, buttons, icon, defaultButton, options, true);
            }
            finally
            {
              
            }
            return result;
        }

        private static DialogResult ShowCore(IWin32Window owner, string text, string caption,
                                             MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton,
                                             MessageBoxOptions options, bool showHelp)
        {
            int irun = 0;
            if (owner is System.Windows.Forms.Form control)
            {
                //irun = ShowMessageDialogCore(control.Control, text, caption, buttons, icon, defaultButton, options, showHelp);
                irun = ShowCore(control.Control, Gtk.WindowPosition.CenterOnParent, text, caption, buttons, icon);
            }
            else
            {
                //irun = ShowMessageDialogCore(new Gtk.Window(Gtk.WindowType.Popup), text, caption, buttons, icon, defaultButton, options, showHelp);
                irun = ShowCore(new Gtk.Window(Gtk.WindowType.Popup), Gtk.WindowPosition.Center, text, caption, buttons, icon);
            }

            Gtk.ResponseType resp = Enum.Parse<Gtk.ResponseType>(irun.ToString());
            if (resp == Gtk.ResponseType.Yes)
                return DialogResult.Yes;
            else if (resp == Gtk.ResponseType.No)
                return DialogResult.No;
            else if (resp == Gtk.ResponseType.Ok)
                return DialogResult.OK;
            else if (resp == Gtk.ResponseType.Cancel)
                return DialogResult.Cancel;
            else if (resp == Gtk.ResponseType.Reject)
                return DialogResult.Abort;
            else if (resp == Gtk.ResponseType.Help)
                return DialogResult.Retry;
            else if (resp == Gtk.ResponseType.Close)
                return DialogResult.Ignore;
            else if (resp == Gtk.ResponseType.None)
                return DialogResult.None;
            else if (resp == Gtk.ResponseType.DeleteEvent)
                return DialogResult.None;
            else
                return DialogResult.None;
        }

        private static int ShowMessageDialogCore(Gtk.Window owner, string text, string caption, MessageBoxButtons buttons, params object[] icon)
        {
            Gtk.ButtonsType buttonsType = Gtk.ButtonsType.Close;
            if (buttons == MessageBoxButtons.OK)
                buttonsType = Gtk.ButtonsType.Ok;
            else if (buttons == MessageBoxButtons.OKCancel)
                buttonsType = Gtk.ButtonsType.OkCancel;
            else if (buttons == MessageBoxButtons.YesNo)
                buttonsType = Gtk.ButtonsType.YesNo;
            else if (buttons == MessageBoxButtons.YesNoCancel)
                buttonsType = Gtk.ButtonsType.YesNo;
            else if (buttons == MessageBoxButtons.AbortRetryIgnore)
                buttonsType = Gtk.ButtonsType.OkCancel;
            else if (buttons == MessageBoxButtons.RetryCancel)
                buttonsType = Gtk.ButtonsType.YesNo;


            Gtk.MessageDialog dia = new Gtk.MessageDialog(owner, Gtk.DialogFlags.DestroyWithParent | Gtk.DialogFlags.UseHeaderBar, Gtk.MessageType.Warning, buttonsType, "");
            dia.SetPosition(Gtk.WindowPosition.Center);
            dia.StyleContext.AddClass("MessageBox");
            dia.StyleContext.AddClass("BorderRadiusStyle");
            dia.BorderWidth = 10;
            dia.Title = caption;
            var content = new Gtk.Label(text);
            content.MarginBottom = 20;
            dia.ContentArea.Add(content);
            dia.Response += Dia_Response;
            dia.ShowAll();
            return dia.Run();
        }

        private static int ShowCore(Gtk.Window owner, Gtk.WindowPosition position, string text, string caption, MessageBoxButtons buttons, params object[] icon)
        {
            Gtk.Dialog dia = new Gtk.Dialog(caption, owner, Gtk.DialogFlags.DestroyWithParent | DialogFlags.Modal);
            dia.SetPosition(position);
            dia.StyleContext.AddClass("MessageBox");
            dia.StyleContext.AddClass("BorderRadiusStyle");
            // dia.SetSizeRequest(300, 160);
            dia.BorderWidth = 10;
            dia.Response += Dia_Response;

            var content = new Gtk.Label(text);
            content.MarginBottom = 20;
            dia.ContentArea.Add(content);
            
            if (buttons == MessageBoxButtons.OK)
            {
                dia.AddButton("确定", Gtk.ResponseType.Ok);
            }
            else if (buttons == MessageBoxButtons.OKCancel)
            {
                dia.AddButton("确定", Gtk.ResponseType.Ok);
                dia.AddButton("取消", Gtk.ResponseType.Cancel);
            }
            else if (buttons == MessageBoxButtons.YesNo)
            {
                dia.AddButton("是", Gtk.ResponseType.Yes);
                dia.AddButton("否", Gtk.ResponseType.No);
            }
            else if (buttons == MessageBoxButtons.YesNoCancel)
            {
                dia.AddButton("是", Gtk.ResponseType.Yes);
                dia.AddButton("否", Gtk.ResponseType.No);
                dia.AddButton("取消", Gtk.ResponseType.Cancel);
            }
            else if (buttons == MessageBoxButtons.AbortRetryIgnore)
            {
                dia.AddButton("放弃", Gtk.ResponseType.Reject);
                dia.AddButton("重试", Gtk.ResponseType.Help);
                dia.AddButton("忽略", Gtk.ResponseType.Close);
            }
            else if (buttons == MessageBoxButtons.RetryCancel)
            {
                dia.AddButton("重试", Gtk.ResponseType.Help);
                dia.AddButton("取消", Gtk.ResponseType.Cancel);
            }
            dia.ShowAll();
            return dia.Run();
        }
        private static void Dia_Response(object o, Gtk.ResponseArgs args)
        {
            Gtk.Dialog dia = o as Gtk.Dialog;
            dia.PangoContext.Dispose();
            dia.Dispose();
        }
    }
}
