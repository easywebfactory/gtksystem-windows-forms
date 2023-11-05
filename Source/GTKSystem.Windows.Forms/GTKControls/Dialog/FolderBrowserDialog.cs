using Cairo;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace System.Windows.Forms
{
    public sealed class FolderBrowserDialog : Gtk.FileChooserDialog
    {
        public FolderBrowserDialog()
        {
            base.Action = Gtk.FileChooserAction.SelectFolder;
            base.AddButton("确定", Gtk.ResponseType.Ok);
            base.AddButton("取消", Gtk.ResponseType.Cancel);
            base.ShowHidden = true;
        }
        public void Reset()
        {
            RootFolder = Environment.SpecialFolder.Desktop;
            Description = string.Empty;
            SelectedPath = string.Empty;
            SelectedPathNeedsCheck = false;
            ShowNewFolderButton = true;
        }
        public Environment.SpecialFolder RootFolder { get; set; }

        public string Description { get { return base.Title; } set { base.Title = value; } }

        public string SelectedPath { get { return base.CurrentFolder; } set { base.SetCurrentFolder(value); } }

        public bool ShowNewFolderButton { get; set; }

        public bool SelectedPathNeedsCheck { get; set; }
        public DialogResult ShowDialog()
        {
            int res = base.Run();
            base.HideOnDelete();
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
    }
}
