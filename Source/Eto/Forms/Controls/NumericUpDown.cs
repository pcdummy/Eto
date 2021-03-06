using System;

namespace Eto.Forms
{
	public interface INumericUpDown : ICommonControl
	{
		bool ReadOnly { get; set; }

		double Value { get; set; }

		double MinValue { get; set; }

		double MaxValue { get; set; }
	}

	public class NumericUpDown : CommonControl
	{
		new INumericUpDown Handler { get { return (INumericUpDown)base.Handler; } }

		public event EventHandler<EventArgs> ValueChanged;

		public virtual void OnValueChanged(EventArgs e)
		{
			if (ValueChanged != null)
				ValueChanged(this, e);
		}

		public NumericUpDown()
			: this((Generator)null)
		{
		}

		public NumericUpDown(Generator generator) : this(generator, typeof(INumericUpDown))
		{
		}

		protected NumericUpDown(Generator generator, Type type, bool initialize = true)
			: base(generator, type, initialize)
		{
		}

		public bool ReadOnly
		{
			get { return Handler.ReadOnly; }
			set { Handler.ReadOnly = value; }
		}

		public double Value
		{
			get { return Handler.Value; }
			set { Handler.Value = value; }
		}

		public double MinValue
		{
			get { return Handler.MinValue; }
			set { Handler.MinValue = value; }
		}

		public double MaxValue
		{
			get { return Handler.MaxValue; }
			set { Handler.MaxValue = value; }
		}

		public ObjectBinding<NumericUpDown, double> ValueBinding
		{
			get
			{
				return new ObjectBinding<NumericUpDown, double>(
					this, 
					c => c.Value, 
					(c, v) => c.Value = v, 
					(c, h) => c.ValueChanged += h, 
					(c, h) => c.ValueChanged -= h
				)
				{
					SettingNullValue = 0
				};
			}
		}
	}
}
