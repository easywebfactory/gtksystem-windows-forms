﻿/*
 * 基于GTK组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://www.gtkapp.com, https://gitee.com/easywebfactory, https://github.com/easywebfactory
 * author:chenhongjin
 */

using GTKSystem.Windows.Forms.GTKControls.ControlBase;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;

namespace System.Windows.Forms
{
    public class ColorDialog : CommonDialog
    {
        public Gtk.ColorChooserDialog colorChooserDialog;
        public ColorDialog() : base()
        {

        }

        [DefaultValue(true)]
        public virtual bool AllowFullOpen { get; set; }

        [DefaultValue(false)]
        public virtual bool AnyColor { get; set; }
        public Color Color { get; set; }
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int[] CustomColors { get; set; }

        [DefaultValue(false)]
        public virtual bool FullOpen { get; set; }

        [DefaultValue(false)]
        public virtual bool ShowHelp { get; set; }

        [DefaultValue(false)]
        public virtual bool SolidColorOnly { get; set; }

        protected virtual IntPtr Instance { get; }

        protected virtual int Options { get; }


        public override void Reset()
        {
            Color = Color.Black;
        }
        private static Gtk.Window ActiveWindow = null;
        protected override bool RunDialog(IWin32Window owner)
        {
            if (owner != null && owner is Form ownerform)
            {
                colorChooserDialog = new Gtk.ColorChooserDialog("选择颜色", ownerform.self);
                colorChooserDialog.WindowPosition = Gtk.WindowPosition.CenterOnParent;
            }
            else
            {
                Gtk.Window window = Gtk.Window.ListToplevels().LastOrDefault(o => o is FormBase && o.IsActive);
                if (window != null)
                {
                    ActiveWindow = window;
                }
                colorChooserDialog = new Gtk.ColorChooserDialog("选择颜色", ActiveWindow);
                colorChooserDialog.WindowPosition = window == null ? Gtk.WindowPosition.Center : Gtk.WindowPosition.CenterOnParent;
            }
            colorChooserDialog.TypeHint = Gdk.WindowTypeHint.Dialog;
            if (Color.Name != "0")
                colorChooserDialog.Rgba = new Gdk.RGBA() { Alpha = (double)Color.A / 255, Red = (double)Color.R / 255, Green = (double)Color.G / 255, Blue = (double)Color.B / 255 };
            if (FullOpen && AllowFullOpen)
                colorChooserDialog.Fullscreen();
            int res = colorChooserDialog.Run();
            colorChooserDialog.ParentWindow = null;
            colorChooserDialog.HideOnDelete();
            Gdk.RGBA colorSelection = colorChooserDialog.Rgba;
            this.Color = Color.FromArgb((int)(colorSelection.Alpha * 255), (int)Math.Round(colorSelection.Red * 255, 0), (int)Math.Round(colorSelection.Green * 255, 0), (int)Math.Round(colorSelection.Blue * 255, 0));
            return res == -5;
        }
 
        public override string ToString() { return this.Color.Name; }
        protected override void Dispose(bool disposing)
        {
            if (colorChooserDialog != null)
            {
                colorChooserDialog.Dispose();
                colorChooserDialog = null;
            }
            base.Dispose(disposing);
        }
    }
}
