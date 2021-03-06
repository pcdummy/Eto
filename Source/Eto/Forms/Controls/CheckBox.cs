using System;

namespace Eto.Forms
{
	/// <summary>
	/// Handler interface for the <see cref="CheckBox"/> control
	/// </summary>
	public interface ICheckBox : ITextControl
	{
		/// <summary>
		/// Gets or sets the checked state
		/// </summary>
		/// <remarks>
		/// When <see cref="ThreeState"/> is true, null signifies an indeterminate value.
		/// </remarks>
		/// <value>The checked value</value>
		bool? Checked { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this CheckBox allows three states: true, false, or null
		/// </summary>
		/// <value><c>true</c> if three state; otherwise, <c>false</c>.</value>
		bool ThreeState { get; set; }
	}

	public class CheckBox : TextControl
	{
		new ICheckBox Handler { get { return (ICheckBox)base.Handler; } }

		/// <summary>
		/// Occurs when <see cref="Checked"/> property is changed by the user
		/// </summary>
		public event EventHandler<EventArgs> CheckedChanged
		{
			add { Properties.AddEvent(CheckedChangedKey, value); }
			remove { Properties.RemoveEvent(CheckedChangedKey, value); }
		}

		static readonly object CheckedChangedKey = new object();

		/// <summary>
		/// Raises the <see cref="CheckedChanged"/> event.
		/// </summary>
		/// <param name="e">Event arguments</param>
		public virtual void OnCheckedChanged(EventArgs e)
		{
			Properties.TriggerEvent(CheckedChangedKey, this, e);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Eto.Forms.CheckBox"/> class.
		/// </summary>
		public CheckBox()
			: this((Generator)null)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Eto.Forms.CheckBox"/> class.
		/// </summary>
		/// <param name="generator">Generator to create the handler</param>
		public CheckBox(Generator generator) : this(generator, typeof(ICheckBox))
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Eto.Forms.CheckBox"/> class.
		/// </summary>
		/// <param name="generator">Generator to create the handler</param>
		/// <param name="type">Handler type to create, must be an instance of <see cref="ICheckBox"/></param>
		/// <param name="initialize">Initialize the handler if true, false if the caller will initialize</param>
		protected CheckBox(Generator generator, Type type, bool initialize = true)
			: base(generator, type, initialize)
		{
		}

		/// <summary>
		/// Gets or sets the checked state
		/// </summary>
		/// <remarks>
		/// When <see cref="ThreeState"/> is true, null signifies an indeterminate value.
		/// </remarks>
		/// <value>The checked value</value>
		public virtual bool? Checked
		{
			get { return Handler.Checked; }
			set { Handler.Checked = value; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether this CheckBox allows three states: true, false, or null
		/// </summary>
		/// <value><c>true</c> if three state; otherwise, <c>false</c>.</value>
		public bool ThreeState
		{
			get { return Handler.ThreeState; }
			set { Handler.ThreeState = value; }
		}

		/// <summary>
		/// Gets a binding for the <see cref="Checked"/> property
		/// </summary>
		/// <value>The binding for the checked property.</value>
		public ObjectBinding<CheckBox, bool?> CheckedBinding
		{
			get
			{
				return new ObjectBinding<CheckBox, bool?>(
					this, 
					c => c.Checked, 
					(c, v) => c.Checked = v, 
					(c, h) => c.CheckedChanged += h, 
					(c, h) => c.CheckedChanged -= h
				);
			}
		}
	}
}
