using System.Collections;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace System.Windows.Forms
{
    [DefaultProperty("Text")]
    public class ColumnHeader : Component, ICloneable
    {
        internal int _index;

        internal string _text;

        internal string _name;

        internal int _width = 120;

        [Localizable(true)]
        public int DisplayIndex { get; set; }

        [Browsable(false)]
        public int Index
        {
            get
            {
                return _index;
            }
        }

        [DefaultValue(-1)]

        public int ImageIndex { get; set; } = -1;

        [Browsable(false)]
        public ImageList ImageList
        {
            get
            {
                throw null;
            }
        }

        public string ImageKey { get; set; }

        [Browsable(false)]
        public ListView ListView
        {
            [CompilerGenerated]
            get;
        }

        [Browsable(false)]

        public string Name
        {
            get;
            set;
        }

        [Localizable(true)]

        public string Text
        {
            get;
            set;
        }


        [Localizable(true)]
        [DefaultValue(HorizontalAlignment.Left)]
        public HorizontalAlignment TextAlign
        {
            get;
            set;
        }


        [Localizable(false)]
        [Bindable(true)]

        [DefaultValue(null)]

        public object Tag
        {
            get;
            set;
        }

        [Localizable(true)]
        [DefaultValue(60)]
        public int Width
        {
            get;
            set;
        } = 100;

        public ColumnHeader()
        {
            
        }

        public ColumnHeader(int imageIndex)
        {
            ImageIndex = imageIndex;
        }

        public ColumnHeader(string imageKey)
        {
            ImageKey = imageKey;
        }

        public ColumnHeader(string text, int width) 
        {
            Text = text;
            Width = width;
        }

        //public void AutoResize(ColumnHeaderAutoResizeStyle headerAutoResize)
        //{
        //	throw null;
        //}

        public object Clone()
        {
            return ((ArrayList)(new ArrayList() { this }).Clone())[0];
            //string data = System.Text.Json.JsonSerializer.Serialize(this,typeof(ColumnHeader));
            //return System.Text.Json.JsonSerializer.Deserialize<ColumnHeader>(data);
        }
        protected override void Dispose(bool disposing)
        {
         
        }
    }
}
