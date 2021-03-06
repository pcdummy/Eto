using Eto.Forms;
using swc = System.Windows.Controls;
using swm = System.Windows.Media;
using swi = System.Windows.Input;

namespace Eto.Platform.Wpf.Forms.Menu
{
	public class ButtonMenuItemHandler : MenuItemHandler<swc.MenuItem, ButtonMenuItem>, IButtonMenuItem
	{
		public ButtonMenuItemHandler ()
		{
			Control = new swc.MenuItem();
			Setup ();
		}
	}
}
