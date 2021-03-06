using System;
using Eto.Forms;

namespace Eto.Platform.GtkSharp.Forms.Controls
{
	public class SliderHandler : GtkControl<Gtk.EventBox, Slider>, ISlider
	{
		int min;
		int max = 100;
		int tick = 1;
		Gtk.Scale scale;

		public SliderHandler()
		{
			this.Control = new Gtk.EventBox();
			//Control.VisibleWindow = false;
			scale = new Gtk.HScale(min, max, 1);
			this.Control.Child = scale;
		}

		protected override void Initialize()
		{
			base.Initialize();
			scale.ValueChanged += Connector.HandleScaleValueChanged;
		}

		protected new SliderConnector Connector { get { return (SliderConnector)base.Connector; } }

		protected override WeakConnector CreateConnector()
		{
			return new SliderConnector();
		}

		protected class SliderConnector : GtkControlConnector
		{
			int? lastValue;

			public new SliderHandler Handler { get { return (SliderHandler)base.Handler; } }

			public void HandleScaleValueChanged(object sender, EventArgs e)
			{
				var scale = Handler.scale;
				var tick = Handler.tick;
				var value = (int)scale.Value;
				var offset = value % tick;
				if (Handler.SnapToTick && offset != 0)
				{
					if (offset > tick / 2)
						scale.Value = value - offset + tick;
					else
						scale.Value -= offset;
				}
				else if (lastValue == null || lastValue.Value != value)
				{
					Handler.Widget.OnValueChanged(EventArgs.Empty);
					lastValue = value;
				}
			}
		}

		public int MaxValue
		{
			get { return max; }
			set
			{
				max = value;
				scale.SetRange(min, max);
			}
		}

		public int MinValue
		{
			get { return min; }
			set
			{
				min = value;
				scale.SetRange(min, max);
			}
		}

		public int Value
		{
			get { return (int)scale.Value; }
			set { scale.Value = value; }
		}

		public bool SnapToTick { get; set; }

		public int TickFrequency
		{
			get
			{
				return tick;
			}
			set
			{
				tick = value;
				// TODO: Only supported from GTK 2.16
			}
		}

		public SliderOrientation Orientation
		{
			get
			{
				return (scale is Gtk.HScale) ? SliderOrientation.Horizontal : SliderOrientation.Vertical;
			}
			set
			{
				if (Orientation != value)
				{
					scale.ValueChanged -= Connector.HandleScaleValueChanged;
					Control.Remove(scale);
					scale.Destroy();
					scale.Dispose();
					if (value == SliderOrientation.Horizontal)
						scale = new Gtk.HScale(min, max, 1);
					else
						scale = new Gtk.VScale(min, max, 1);
					scale.ValueChanged += Connector.HandleScaleValueChanged;
					Control.Child = scale;
					scale.ShowAll();
				}
			}
		}
	}
}

