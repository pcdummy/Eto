using System;

namespace Eto.Forms
{
	public interface ITextBox : ITextControl
	{
		bool ReadOnly { get; set; }

		int MaxLength { get; set; }
		
		void SelectAll();

		string PlaceholderText { get; set; }
	}
	
	public class TextBox : TextControl
	{
		new ITextBox Handler { get { return (ITextBox)base.Handler; }}
		
		public TextBox()
			: this((Generator)null)
		{
		}

		public TextBox (Generator generator)
			: this(generator, typeof(ITextBox))
		{
			
		}
		protected TextBox (Generator generator, Type type, bool initialize = true)
			: base (generator, type, initialize)
		{
		}

		public bool ReadOnly {
			get { return Handler.ReadOnly; }
			set { Handler.ReadOnly = value; }
		}
		
		public int MaxLength {
			get { return Handler.MaxLength; }
			set { Handler.MaxLength = value; }
		}
		
		public string PlaceholderText {
			get { return Handler.PlaceholderText; }
			set { Handler.PlaceholderText = value; }
		}

		public void SelectAll()
		{
			Handler.SelectAll();
		}
	}
}
