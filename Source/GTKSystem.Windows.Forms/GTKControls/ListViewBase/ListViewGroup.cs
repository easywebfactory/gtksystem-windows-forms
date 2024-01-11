
using System.Runtime.Serialization;

namespace System.Windows.Forms
{
	public sealed class ListViewGroup : ISerializable
	{
		ListView.ListViewItemCollection _items;
        public ListViewGroup()
        {
			_items = new ListView.ListViewItemCollection(ListView);
        }

        public ListViewGroup(string key, string headerText)
        {
            _items = new ListView.ListViewItemCollection(ListView);
            this.Name = key;
			this.Header=headerText;
        }

        public ListViewGroup(string header)
        {
            _items = new ListView.ListViewItemCollection(ListView);
            this.Header = header;
        }

        public ListViewGroup(string header, HorizontalAlignment headerAlignment)
        {
            _items = new ListView.ListViewItemCollection(ListView);
            this.Header = header;
			this.HeaderAlignment = headerAlignment;
        }

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
