using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace System.Windows.Forms
{
	public class BindingCompleteEventArgs : CancelEventArgs
	{
		public Binding Binding
		{
			[CompilerGenerated]
			get
			{
				throw null;
			}
		}

		public BindingCompleteState BindingCompleteState
		{
			[CompilerGenerated]
			get
			{
				throw null;
			}
		}

		public BindingCompleteContext BindingCompleteContext
		{
			[CompilerGenerated]
			get
			{
				throw null;
			}
		}

		public string ErrorText
		{
			[CompilerGenerated]
			get
			{
				throw null;
			}
		}

		public Exception Exception
		{
			[CompilerGenerated]
			get
			{
				throw null;
			}
		}

		public BindingCompleteEventArgs(Binding binding, BindingCompleteState state, BindingCompleteContext context, string errorText, Exception exception, bool cancel)
		{
			throw null;
		}

		public BindingCompleteEventArgs(Binding binding, BindingCompleteState state, BindingCompleteContext context, string errorText, Exception exception)
		{
			throw null;
		}

		public BindingCompleteEventArgs(Binding binding, BindingCompleteState state, BindingCompleteContext context, string errorText)
		{
			throw null;
		}

		public BindingCompleteEventArgs(Binding binding, BindingCompleteState state, BindingCompleteContext context)
		{
			throw null;
		}
	}
}
