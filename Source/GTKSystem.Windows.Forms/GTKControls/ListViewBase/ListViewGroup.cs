
using Gtk;
using System.Runtime.Serialization;

namespace System.Windows.Forms;

public sealed class ListViewGroup : ISerializable
{
    public readonly string serialGuid = Guid.NewGuid().ToString();
    public static readonly string defaultListViewGroupKey = "_DefaultListViewGroup_";
    internal Box? Groupbox { get; set; }
    public static ListViewGroup CreateDefaultListViewGroup() {
        var _defaultGroup = new ListViewGroup("default", HorizontalAlignment.Left)
        {
            Header = "default",
            Name = defaultListViewGroupKey,
            Subtitle = ""
        };
        return _defaultGroup;
    }
    public ListViewGroup() : this("", "")
    {
    }

    public ListViewGroup(string key, string headerText)
    {
        Name = string.IsNullOrWhiteSpace(key) ? headerText : key;
        Header = headerText;
    }

    public ListViewGroup(string header) : this(header, header)
    {

    }

    public ListViewGroup(string header, HorizontalAlignment headerAlignment) : this(header, header)
    {
        HeaderAlignment = headerAlignment;

    }
    internal readonly FlowBox FlowBox = new() { Orientation = Gtk.Orientation.Horizontal, Name = Guid.NewGuid().ToString() };
    public string Header
    {
        get;
        set;
    }

    public HorizontalAlignment HeaderAlignment
    {
        get;
        set;
    }

    public string? Footer
    {
        get;
        set;
    }

		
		
    public HorizontalAlignment FooterAlignment
    {
        get;
        set;
    }
		 
		
    public ListViewGroupCollapsedState CollapsedState
    {
        get;
        set;
    }

		
		
    public string? Subtitle
    {
        get;
        set;
    }

		
		
    public string? TaskLink
    {
        get;
        set;
    }
 
    public int TitleImageIndex
    {
        get;
        set;
    }
 
    public string? TitleImageKey
    {
        get;
        set;
    }
		 
    public ListView.ListViewItemCollection Items
    {
        get
        {
            var coll = new ListView.ListViewItemCollection(ListView);
            var all = ListView.Items.FindAll(m => m.Group?.serialGuid == serialGuid);
            coll.AddRange(all);
            return coll;
        }
    }

    public ListView ListView
    {
        get;
        internal set;
    } = null!;

    public string Name
    {
        get;
        set;
    }

    public object? Tag
    {
        get;
        set;
    }

    void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
    {
			
    }
}