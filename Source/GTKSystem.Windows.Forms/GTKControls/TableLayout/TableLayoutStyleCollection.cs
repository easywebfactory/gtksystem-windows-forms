using System.Collections;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	public abstract class TableLayoutStyleCollection : IList, ICollection, IEnumerable
	{
		internal IArrangedElement? Owner
		{
			get
			{
				throw null;
			}
		}

		internal virtual string? PropertyName
		{
			get
			{
				throw null;
			}
		}

		object? IList.this[int index]
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

		public TableLayoutStyle this[int index]
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

		bool IList.IsFixedSize
		{
			get
			{
				throw null;
			}
		}

		bool IList.IsReadOnly
		{
			get
			{
				throw null;
			}
		}

		public int Count
		{
			get
			{
				throw null;
			}
		}

		bool ICollection.IsSynchronized
		{
			get
			{
				throw null;
			}
		}

		object ICollection.SyncRoot
		{
			get
			{
				throw null;
			}
		}

		internal TableLayoutStyleCollection(IArrangedElement? owner)
		{
			throw null;
		}

		int IList.Add(object? style)
		{
			throw null;
		}

		public int Add(TableLayoutStyle style)
		{
			throw null;
		}

		void IList.Insert(int index, object? style)
		{
			throw null;
		}

		void IList.Remove(object? style)
		{
			throw null;
		}

		public void Clear()
		{
			throw null;
		}

		public void RemoveAt(int index)
		{
			throw null;
		}

		bool IList.Contains(object? style)
		{
			throw null;
		}

		int IList.IndexOf(object? style)
		{
			throw null;
		}

		void ICollection.CopyTo(Array array, int startIndex)
		{
			throw null;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			throw null;
		}

		internal void EnsureOwnership(IArrangedElement owner)
		{
			throw null;
		}
	}
}
