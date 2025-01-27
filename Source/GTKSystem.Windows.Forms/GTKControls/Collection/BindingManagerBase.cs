using System.Collections;
using System.ComponentModel;

namespace System.Windows.Forms
{
	public abstract class BindingManagerBase
	{
		protected EventHandler onCurrentChangedHandler;

		protected EventHandler onPositionChangedHandler;

		private protected EventHandler _onCurrentItemChangedHandler;

		public BindingsCollection Bindings
		{
			get
			{
				throw null;
			}
		}

		public abstract object Current { get; }

		internal abstract Type BindType { get; }

		public abstract int Position { get; set; }

		internal abstract object DataSource { get; }

		internal abstract bool IsBinding { get; }

		public bool IsBindingSuspended
		{
			get
			{
				throw null;
			}
		}

		public abstract int Count { get; }

		public event BindingCompleteEventHandler BindingComplete
		{
			add
			{
				throw null;
			}
			remove
			{
				throw null;
			}
		}

		public event EventHandler CurrentChanged
		{
			add
			{
				throw null;
			}
			remove
			{
				throw null;
			}
		}

		public event EventHandler CurrentItemChanged
		{
			add
			{
				throw null;
			}
			remove
			{
				throw null;
			}
		}

		public event BindingManagerDataErrorEventHandler DataError
		{
			add
			{
				throw null;
			}
			remove
			{
				throw null;
			}
		}

		public event EventHandler PositionChanged
		{
			add
			{
				throw null;
			}
			remove
			{
				throw null;
			}
		}

		protected internal void OnBindingComplete(BindingCompleteEventArgs args)
		{
			throw null;
		}

		protected internal abstract void OnCurrentChanged(EventArgs e);

		protected internal abstract void OnCurrentItemChanged(EventArgs e);

		protected internal void OnDataError(Exception e)
		{
			throw null;
		}

		private protected abstract void SetDataSource(object dataSource);

		public BindingManagerBase()
		{
			throw null;
		}

		internal BindingManagerBase(object dataSource)
		{
			throw null;
		}

		internal abstract PropertyDescriptorCollection GetItemProperties(PropertyDescriptor[] listAccessors);

		public virtual PropertyDescriptorCollection GetItemProperties()
		{
			throw null;
		}

		protected internal virtual PropertyDescriptorCollection GetItemProperties(ArrayList dataSources, ArrayList listAccessors)
		{
			throw null;
		}

		//protected virtual PropertyDescriptorCollection GetItemProperties([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type listType, int offset, ArrayList dataSources, ArrayList listAccessors)
		//{
		//	throw null;
		//}
        protected virtual PropertyDescriptorCollection GetItemProperties(Type listType, int offset, ArrayList dataSources, ArrayList listAccessors)
        {
            throw null;
        }
        internal abstract string GetListName();

		public abstract void CancelCurrentEdit();

		public abstract void EndCurrentEdit();

		public abstract void AddNew();

		public abstract void RemoveAt(int index);

		protected abstract void UpdateIsBinding();

		protected internal abstract string GetListName(ArrayList listAccessors);

		public abstract void SuspendBinding();

		public abstract void ResumeBinding();

		protected void PullData()
		{
			throw null;
		}

		internal void PullData(out bool success)
		{
			throw null;
		}

		protected void PushData()
		{
			throw null;
		}
	}
}
