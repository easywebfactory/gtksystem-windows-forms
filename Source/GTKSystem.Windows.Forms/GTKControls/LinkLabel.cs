/*
 * 基于GTK3.24.24.34版本组件开发，兼容原生C#控件winform界面的跨平台界面组件。
 * 使用本组件GTKSystem.Windows.Forms代替Microsoft.WindowsDesktop.App.WindowsForms，一次编译，跨平台跨平台windows、linux、macos运行
 * 技术支持438865652@qq.com，https://gitee.com/easywebfactory, https://www.cnblogs.com/easywebfactory
 * author:chenhongjin
 * date: 2024/1/3
 */
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Linq;

namespace System.Windows.Forms
{
    [DesignerCategory("Component")]
    public partial class LinkLabel: WidgetControl<Gtk.LinkButton>
    {
        public LinkLabel():base("")
        {
            Widget.StyleContext.AddClass("LinkLabel");
            base.Control.Clicked += LinkLabel_Click;
            base.Control.ActivateLink += LinkLabel_ActivateLink;
        }

        private void LinkLabel_ActivateLink(object o, Gtk.ActivateLinkArgs args)
        {
            if (LinkClicked != null)
            {
                LinkClicked(this, new LinkLabelLinkClickedEventArgs(new Link() { Description = base.Control.Label, LinkData = base.Control.Uri }));
            }
        }

        private void LinkLabel_Click(object sender, EventArgs e)
        {
            Console.WriteLine("LinkLabel_Click");
            if (Click != null)
            {
                Click(this, e);
            }
        }
        public override event EventHandler Click;
        public override string Text { get { return string.IsNullOrEmpty(base.Control.Label)? base.Control.Uri : base.Control.Label; } set { base.Control.Label = value; base.Control.Uri = value; } }
         

        public event LinkLabelLinkClickedEventHandler LinkClicked;

        public bool LinkVisited { get; set; }

        public LinkCollection Links { get; }

        public Color LinkColor
        {
            get { return Color.FromArgb(base.Control.LinkColor.Red, base.Control.LinkColor.Green, base.Control.LinkColor.Blue); }
            set
            {

            }
        }


        //public LinkBehavior LinkBehavior { get; set; }

        //public LinkArea LinkArea { get; set; }

        public FlatStyle FlatStyle { get; set; }


        public Color DisabledLinkColor { get; set; }

        public Color ActiveLinkColor { get; set; }

        public Color VisitedLinkColor { 
            get { return Color.FromArgb(base.Control.VisitedLinkColor.Red, base.Control.VisitedLinkColor.Green, base.Control.VisitedLinkColor.Blue); }
            set {
                
            } }
        public bool UseCompatibleTextRendering { get; set; }


        public class Link
        {
            public Link()
            {
            }

            public Link(int start, int length)
            {

            }

            public Link(int start, int length, object linkData)
            {

            }
            public string Description { get; set; }
            [DefaultValue(null)]
            public object LinkData { get; set; }
        }
        public class LinkCollection : IList
        {
            private readonly LinkLabel owner;
            private bool linksAdded = false;   
            private int lastAccessedIndex = -1;

            public LinkCollection(LinkLabel owner)
            {
                this.owner = owner ?? throw new ArgumentNullException(nameof(owner));
            }

            public object this[int index] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

            public bool IsFixedSize => throw new NotImplementedException();

            public bool IsReadOnly => throw new NotImplementedException();

            public int Count => throw new NotImplementedException();

            public bool IsSynchronized => throw new NotImplementedException();

            public object SyncRoot => throw new NotImplementedException();

            public int Add(object value)
            {
                if(value is Label)
                {
                    owner.Text = ((Label)value).Text;
                }
                else
                {
                    owner.Text = value?.ToString();
                }
                return 1;
            }

            public void Clear()
            {
                owner.Text = "";
            }

            public bool Contains(object value)
            {
                return owner.Text.Contains(value.ToString());
            }

            public void CopyTo(Array array, int index)
            {
                throw new NotImplementedException();
            }

            public IEnumerator GetEnumerator()
            {
                throw new NotImplementedException();
            }

            public int IndexOf(object value)
            {
                throw new NotImplementedException();
            }

            public void Insert(int index, object value)
            {
                throw new NotImplementedException();
            }

            public void Remove(object value)
            {
                throw new NotImplementedException();
            }

            public void RemoveAt(int index)
            {
                throw new NotImplementedException();
            }
        }
    }
}
