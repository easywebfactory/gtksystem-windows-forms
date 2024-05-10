using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace System.Windows.Forms
{

	public class BaseCollection : MarshalByRefObject, ICollection, IEnumerable
	{

        [Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public virtual int Count
		{
			get;
        }

		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public bool IsSynchronized
		{
			get
			{
				return true;
			}
		}

		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		protected virtual ArrayList List
		{
			get;
		}

		public void CopyTo(Array ar, int index)
		{

		}

		public virtual IEnumerator GetEnumerator()
		{
			 throw new NotImplementedException();
		}

		public BaseCollection()
		{
			
		}
	}
}
