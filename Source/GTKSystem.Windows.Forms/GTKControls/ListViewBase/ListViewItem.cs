using System.Collections;
using System.Drawing;
using System.Runtime.Serialization;
using Gtk;

namespace System.Windows.Forms;

public class ListViewItem : ICloneable, ISerializable, IKeyboardToolTip
{
    private ListViewSubItemCollection? _subitems;
    public class ListViewSubItem
    {
        internal Gtk.Label? _label;
        public Color? BackColor
        {
            get;
            set;
        }

			
        public Rectangle Bounds
        {
            get;
            set;
        }

        public Font? Font
        {
            get;
            set;
        }

        public Color? ForeColor
        {
            get;
            set;
        }
			 
        public object? Tag
        {
            get;
            set;
        }


        internal string _text = string.Empty;
        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                if (_label != null)
                    _label.Text = value;
            }
        }


        public string Name
        {
            get;
            set;
        } = string.Empty;
        public ListViewItem? ListViewItem
        {
            get;
            set;
        }
        public ListViewSubItem()
        {
				 
        }

        public ListViewSubItem(ListViewItem? owner, string text)
        {
            ListViewItem = owner;
            Text = text;
        }

        public ListViewSubItem(ListViewItem? owner, string text, Color foreColor, Color backColor, Font? font)
        {
            ListViewItem = owner;
            Text= text;
            ForeColor = foreColor;
            BackColor = backColor;
            Font = font;
        }
			 
        public void ResetStyle()
        {
				 
        }
			 
    }

    public class ListViewSubItemCollection : List<ListViewSubItem>
    {
        private readonly ListViewItem? _owner;
        public virtual ListViewSubItem this[string key]
        {
            get
            {
                return Find(w => w.Name == key);
            }
        }

        public ListViewSubItemCollection(ListViewItem? owner)
        {
            _owner = owner;
        }

        public ListViewSubItem Add(string text)
        {
            var sub = new ListViewSubItem(_owner, text);
            Add(sub);
            return sub;
        }

        public ListViewSubItem Add(string text, Color foreColor, Color backColor, Font? font)
        {
            var sub = new ListViewSubItem(_owner, text, foreColor, backColor, font);
            Add(sub);
            return sub;
        }

        public void AddRange(ListViewSubItem[] items)
        {
            foreach(var item in items)
                Add(item);
        }

        public void AddRange(string[] items)
        {
            foreach(var item in items)
                Add(item);
        }

        public void AddRange(string[] items, Color foreColor, Color backColor, Font? font)
        {
            foreach (var item in items)
                Add(item, foreColor, backColor, font);
        }
			 
        public virtual bool ContainsKey(string key)
        {
            return FindIndex(w => w.Name == key) > -1;

        }

        public virtual int IndexOfKey(string key)
        {
            return FindIndex(w => w.Name == key);

        }

        public virtual void RemoveByKey(string key)
        {
            RemoveAt(FindIndex(w => w.Name == key));
        }
    }

    internal ListView? _listView;

    internal ListViewGroup? _group;

    internal int id;
    internal FlowBoxChild? _flowBoxChild;
    internal virtual AccessibleObject? AccessibilityObject
    {
        get;
        set;
    }


    internal string? ItemType
    {
        get;
        set;
    }
    public Color? BackColor
    {
        get;
        set;
    }

		
    public Rectangle Bounds
    {
        get;
        set;
    }

		
		
    internal bool _checked;
    public bool Checked
    {
        get => _checked;

        set {
            _checked = value;
            _listView?.NativeCheckItem(this, value);
        }
    }

		
		
    public bool Focused
    {
        get;
        set;
    }

		
		
		
    public Font? Font
    {
        get;
        set;
    }

		
		
    public Color? ForeColor
    {
        get;
        set;
    }

    public ListViewGroup? Group
    {
        get => _group;
        set => _group = value;
    }

    public int ImageIndex
    {
        get;
        set;
    }

    internal ListViewItemImageIndexer? ImageIndexer
    {
        get;
        set;
    }

		
		
		
		
		
		
		
    public string? ImageKey
    {
        get;
        set;
    }

		
    public ImageList? ImageList
    {
        get;
        internal set;
    }

		
		
		
    public int IndentCount
    {
        get;
        set;
    }

		
    public int Index
    {
        get;
        internal set;
    }

		
    public ListView? ListView
    {
        get;
        internal set;
    }




    public string Name
    {
        get;
        set;
    } = string.Empty;

		
		
		
    public Point Position
    {
        get;
        set;
    }
    internal bool _selected;
    public bool Selected
    {
        get => _selected;
        set { 
            _selected = value;
            _listView?.NativeSelectItem(this, value);
        }
    }









    public int StateImageIndex
    {
        get;
        set;
    }

    internal bool StateImageSet
    {
        get; set;
    }

    internal bool StateSelected
    {
        get;
        set;
    }

		
		
		
		
    public ListViewSubItemCollection? SubItems
    {
        get;
        internal set;
    }

		
		
		
		
		
		
    public object? Tag
    {
        get;
        set;
    }

    internal string _text = string.Empty;
    public string Text
    {
        get => _text;
        set
        {
            _text = value;
            _listView?.NativeUpdateText(this, value);

        }
    }

    public string? ToolTipText
    {
        get;
        set;
    }

		
		
    public bool UseItemStyleForSubItems
    {
        get;
        set;
    }

    public ListViewItem()
    {
        InitListViewItem("", -1, "", null, null, null, null);
    }

    protected ListViewItem(SerializationInfo info, StreamingContext context)
    {
			 
    }

    public ListViewItem(string? text)
    {
        InitListViewItem(text, -1, "", null, null, null, null);
    }

    public ListViewItem(string? text, int imageIndex)
    {
        InitListViewItem(text, imageIndex, "", null, null, null, null);
    }

    public ListViewItem(string?[] items)
    {
        InitListViewItem(items, -1, "", null, null, null, null);
    }

    public ListViewItem(string?[] items, int imageIndex)
    {
        InitListViewItem(items, imageIndex, "", null, null, null, null);
    }

    public ListViewItem(string?[] items, int imageIndex, Color foreColor, Color backColor, Font? font)
    {
        InitListViewItem(items, imageIndex, "", foreColor, backColor, font, null);
    }

    public ListViewItem(ListViewSubItem[] subItems, int imageIndex)
    {
        foreach (var item in subItems)
            _subitems?.Add(item);
        InitListViewItem("", imageIndex, "", null, null, null, null);
    }

    public ListViewItem(ListViewGroup? group)
    {
        InitListViewItem("", -1, "", null, null, null, group);
    }

    public ListViewItem(string? text, ListViewGroup? group)
    {
        InitListViewItem(text, -1, null, null, null, null, group);
    }

    public ListViewItem(string? text, int imageIndex, ListViewGroup? group)
    {
        InitListViewItem(text, imageIndex, null, null, null, null, group);
    }

    public ListViewItem(string?[] items, ListViewGroup? group)
    {
        InitListViewItem(items, -1, "", null, null, null, group);
    }

    public ListViewItem(string?[] items, int imageIndex, ListViewGroup? group)
    {
        InitListViewItem(items, imageIndex, "", null, null, null, group);
    }

    public ListViewItem(string?[] items, int imageIndex, Color foreColor, Color backColor, Font? font, ListViewGroup? group)
    {
        InitListViewItem(items, imageIndex, "", foreColor, backColor, font, group);
    }

    public ListViewItem(ListViewSubItem[] subItems, int imageIndex, ListViewGroup? group)
    {
        foreach (var item in subItems)
            _subitems?.Add(item);
        InitListViewItem([], imageIndex, "", null, null, null, group);
    }

    public ListViewItem(string? text, string? imageKey)
    {
        InitListViewItem(text, -1, imageKey, null, null, null, null);
    }

    public ListViewItem(string?[] items, string? imageKey) 
    {
        InitListViewItem(items, -1, imageKey, null, null, null, null);
    }

    public ListViewItem(string?[] items, string? imageKey, Color foreColor, Color backColor, Font? font)
    {
        InitListViewItem(items, -1, imageKey, foreColor, backColor, font, null);
    }

    public ListViewItem(ListViewSubItem[] subItems, string? imageKey)
    {
        foreach (var item in subItems)
            _subitems?.Add(item);
        InitListViewItem([], -1, imageKey, null, null, null, null);
    }

    public ListViewItem(string? text, string? imageKey, ListViewGroup? group)
    {
        InitListViewItem(text, -1, imageKey, null, null, null, group);
    }

    public ListViewItem(string?[] items, string? imageKey, ListViewGroup? group)
    {
        InitListViewItem(items, -1, imageKey, null, null, null, group);
    }

    public ListViewItem(string?[] items, string? imageKey, Color foreColor, Color backColor, Font? font, ListViewGroup? group)
    {
        InitListViewItem(items, -1, imageKey, foreColor, backColor, font, group);
    }

    public ListViewItem(ListViewSubItem[] subItems, string? imageKey, ListViewGroup? group)
    {
        foreach(var item in subItems)
            _subitems?.Add(item);
        InitListViewItem("", -1, imageKey, null, null, null, group);
    }
    internal void InitListViewItem(string? text, int imageIndex, string? imageKey, Color? foreColor, Color? backColor, Font? font, ListViewGroup? group)
    {
        _subitems = new ListViewSubItemCollection(this);
        SubItems = _subitems;
        Text = text??string.Empty;
        ImageIndex = imageIndex;
        ImageKey = imageKey;
        ForeColor = foreColor;
        BackColor = backColor;
        Font = font;
        Group = group;
    }
    internal void InitListViewItem(string?[] items, int imageIndex, string? imageKey, Color? foreColor, Color? backColor, Font? font, ListViewGroup? group)
    {
        InitListViewItem(items[0], imageIndex, imageKey, foreColor, backColor, font, group);
        foreach (var item in items.Skip(1))
            if (item != null)
            {
                _subitems?.Add(item);
            }
    }
    //internal string RelateFlowBoxChildKey { get; set; }
    public void BeginEdit()
    {
			 
    }

    public virtual object Clone()
    {
        return ((ArrayList)new ArrayList { this }.Clone())[0];
    }

    public virtual void EnsureVisible()
    {
			 
    }
		 
    public ListViewSubItem? GetSubItemAt(int x, int y)
    {
        return _subitems?[x];
    }

    internal void Host(ListView parent, int id, int index)
    {
			
    }
		  
    public virtual void Remove()
    {
			
    }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
           
    }
}