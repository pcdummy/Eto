using SD = System.Drawing;
using SWF = System.Windows.Forms;
using Eto.Forms;
using System;

namespace Eto.Platform.Windows
{
	/// <summary>
	/// Summary description for MenuBarHandler.
	/// </summary>
	public class SeparatorMenuItemHandler : MenuHandler<SWF.ToolStripSeparator, SeparatorMenuItem>, ISeparatorMenuItem
	{
		
		public SeparatorMenuItemHandler()
		{
			Control = new SWF.ToolStripSeparator();
		}

		public string Text
		{
			get { return null; }
			set { throw new NotSupportedException(); }
		}

		public string ToolTip
		{
			get { return null; }
			set { throw new NotSupportedException(); }
		}

		public Keys Shortcut
		{
			get { return Keys.None; }
			set { throw new NotSupportedException(); }
		}

		public bool Enabled
		{
			get { return false; }
			set { throw new NotSupportedException(); }
		}
	}
}
