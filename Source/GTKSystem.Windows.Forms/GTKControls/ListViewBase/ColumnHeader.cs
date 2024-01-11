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

        internal int _width;

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
            get
            {
                throw null;
            }
        }

        [Browsable(false)]

        public string Name
        {
            get
            {
                throw null;
            }
            set
            {
                throw null;
            }
        }

        [Localizable(true)]

        public string Text
        {
            get
            {
                throw null;
            }
            [param: AllowNull]
            set
            {
                throw null;
            }
        }


        [Localizable(true)]
        [DefaultValue(HorizontalAlignment.Left)]
        public HorizontalAlignment TextAlign
        {
            get
            {
                throw null;
            }
            set
            {
                throw null;
            }
        }


        [Localizable(false)]
        [Bindable(true)]

        [DefaultValue(null)]

        public object Tag
        {
            [CompilerGenerated]
            get
            {
                throw null;
            }
            [CompilerGenerated]
            set
            {
                throw null;
            }
        }

        [Localizable(true)]
        [DefaultValue(60)]
        public int Width
        {
            get
            {
                throw null;
            }
            set
            {
                throw null;
            }
        }

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

        //public void AutoResize(ColumnHeaderAutoResizeStyle headerAutoResize)
        //{
        //	throw null;
        //}

        public object Clone()
        {
            string data = System.Text.Json.JsonSerializer.Serialize(this,System.Text.Json.Serialization.Metadata.JsonTypeInfo.CreateJsonTypeInfo<ColumnHeader>(System.Text.Json.JsonSerializerOptions.Default));
            return System.Text.Json.JsonSerializer.Deserialize<ColumnHeader>(data);
        }
        protected override void Dispose(bool disposing)
        {
           // ImageList = null;
        }
    }
}
