using System;
using Eto.Forms;
using Eto.Drawing;
using Eto.Platform.GtkSharp.Drawing;

namespace Eto.Platform.GtkSharp
{
	public class CheckBoxHandler : GtkControl<Gtk.CheckButton, CheckBox>, ICheckBox
	{
		Font font;
		readonly Gtk.EventBox box;

		public override Gtk.Widget ContainerControl
		{
			get { return box; }
		}

		public CheckBoxHandler()
		{
			Control = new Gtk.CheckButton();
			box = new Gtk.EventBox { Child = Control };
		}

		protected override void Initialize()
		{
			base.Initialize();
			Control.Toggled += Connector.HandleToggled;
		}

		protected new CheckBoxConnector Connector { get { return (CheckBoxConnector)base.Connector; } }

		protected override WeakConnector CreateConnector()
		{
			return new CheckBoxConnector();
		}

		protected class CheckBoxConnector : GtkControlConnector
		{
			bool toggling;
			public new CheckBoxHandler Handler { get { return (CheckBoxHandler)base.Handler; } }

			public void HandleToggled(object sender, EventArgs e)
			{
				var h = Handler;
				var c = h.Control;
				if (toggling)
					return;

				toggling = true;
				if (h.ThreeState)
				{
					if (!c.Inconsistent && c.Active)
						c.Inconsistent = true;
					else if (c.Inconsistent)
					{
						c.Inconsistent = false;
						c.Active = true;
					}
				}
				h.Widget.OnCheckedChanged(EventArgs.Empty);
				toggling = false;

			}
		}

		public override string Text
		{
			get { return MnuemonicToString(Control.Label); }
			set { Control.Label = StringToMnuemonic(value); }
		}

		public override Font Font
		{
			get
			{
				if (font == null)
					font = new Font(Widget.Generator, new FontHandler(Control.Child));
				return font;
			}
			set
			{
				font = value;
				if (font != null)
					Control.Child.ModifyFont(((FontHandler)font.Handler).Control);
				else
					Control.Child.ModifyFont(null);
			}
		}

		public bool? Checked
		{
			get { return Control.Inconsistent ? null : (bool?)Control.Active; }
			set
			{ 
				if (value == null)
					Control.Inconsistent = true;
				else
				{
					Control.Inconsistent = false;
					Control.Active = value.Value;
				}
			}
		}

		public bool ThreeState
		{
			get;
			set;
		}
	}
}
