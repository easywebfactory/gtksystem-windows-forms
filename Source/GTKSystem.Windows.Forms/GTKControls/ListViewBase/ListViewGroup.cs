
using Gtk;
using System.Reflection.PortableExecutable;
using System.Runtime.Serialization;

namespace System.Windows.Forms
{
	public sealed class ListViewGroup : ISerializable
	{
        public static readonly string defaultListViewGroupKey = "00000defaultListViewGroup";
        ListView.ListViewItemCollection _items;

        public static ListViewGroup GetDefaultListViewGroup() {
            ListViewGroup defaultGroup = new ListViewGroup("defaultListViewGroup", HorizontalAlignment.Left);
            defaultGroup.Header = "default";
            defaultGroup.Name = ListViewGroup.defaultListViewGroupKey;
            defaultGroup.Subtitle = "";
            return defaultGroup;
        }
        public ListViewGroup() : this("", "")
        {
        }

        public ListViewGroup(string key, string headerText)
        {
            _items = new ListView.ListViewItemCollection(ListView);
            this.Name = string.IsNullOrWhiteSpace(key) ? headerText : key;
            this.Header = headerText;
            this.FlowBox = new Gtk.FlowBox();
            this.FlowBox.Orientation = Gtk.Orientation.Horizontal;
            this.FlowBox.Name = this.Name;
        }

        public ListViewGroup(string header) : this(header, header)
        {

        }

        public ListViewGroup(string header, HorizontalAlignment headerAlignment) : this(header, header)
        {
			this.HeaderAlignment = headerAlignment;

        }
        internal Gtk.FlowBox FlowBox
        {
            get;
            set;
        } = new Gtk.FlowBox() { Orientation = Gtk.Orientation.Horizontal };
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

		public string Footer
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

		
		
		public string Subtitle
		{
            get;
            set;
        }

		
		
		public string TaskLink
		{
            get;
            set;
        }
 
		public int TitleImageIndex
		{
            get;
            set;
        }
 
		public string TitleImageKey
		{
            get;
            set;
        }
		 
		public ListView.ListViewItemCollection Items
		{
			get
			{
				return _items;
			}
		}
 
		public ListView ListView
		{
			get;
			internal set;
		}

		public string Name
		{
            get;
            set;
        }

		public object Tag
		{
			get;
			set;
		}

		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			
		}
	}
}
