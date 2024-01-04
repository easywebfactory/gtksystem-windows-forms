/*
 * 基于GTK3.24.24.34版本组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://gitee.com/easywebfactory, https://www.cnblogs.com/easywebfactory
 * author:chenhongjin
 * date: 2024/1/3
 */
using Gtk;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;

namespace System.Windows.Forms
{
    public class ColorDialog
    {
        public Gtk.ColorSelectionDialog colorSelectionDialog;
        public ColorDialog()
        {
            colorSelectionDialog = new Gtk.ColorSelectionDialog("选择颜色");
        }

        [DefaultValue(true)]
        public virtual bool AllowFullOpen { get; set; }

        [DefaultValue(false)]
        public virtual bool AnyColor { get; set; }

        public Color Color { 
            get {
                ColorSelection colorSelection = colorSelectionDialog.ColorSelection;
                return Color.FromArgb((int)(colorSelection.CurrentRgba.Alpha), (int)Math.Round(colorSelection.CurrentRgba.Red * 255, 0), (int)Math.Round(colorSelection.CurrentRgba.Green * 255, 0), (int)Math.Round(colorSelection.CurrentRgba.Blue * 255, 0));
            }
            set {  colorSelectionDialog.ColorSelection.CurrentRgba = new Gdk.RGBA() { Alpha = (double)value.A / 255, Red = (double)value.R/255, Green = (double)value.G / 255, Blue = (double)value.B / 255 }; } }
 
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

        public void Reset()
        {

        }

        public object Tag { get; set; }

        public event EventHandler HelpRequest;

        public DialogResult ShowDialog()
        {
            return ShowDialog(null);
        }
        
        public DialogResult ShowDialog(IWin32Window owner)
        {
            if (FullOpen && AllowFullOpen)
                colorSelectionDialog.Fullscreen();

            colorSelectionDialog.WindowPosition = Gtk.WindowPosition.Center;
           // colorSelectionDialog.OkButton.Clicked += OkButton_Clicked;
            colorSelectionDialog.CancelButton.Clicked += CancelButton_Clicked;
            int res = colorSelectionDialog.Run();

            ColorSelection colorSelection = colorSelectionDialog.ColorSelection;
            this.Color = Color.FromArgb((int)(colorSelection.CurrentRgba.Alpha), (int)Math.Round(colorSelection.CurrentRgba.Red * 255, 0), (int)Math.Round(colorSelection.CurrentRgba.Green * 255, 0), (int)Math.Round(colorSelection.CurrentRgba.Blue * 255, 0));

            colorSelectionDialog.HideOnDelete();


            Gtk.ResponseType resp = Enum.Parse<Gtk.ResponseType>(res.ToString());
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

        private void CancelButton_Clicked(object sender, EventArgs e)
        {
            colorSelectionDialog.HideOnDelete();
        }

        private void OkButton_Clicked(object sender, EventArgs e)
        {
        
        }

        public override string ToString() { return this.Color.Name; }

        internal class ZeroWindow : IWin32Window
        {
            public IntPtr Handle => IntPtr.Zero;
        }
    }
}
