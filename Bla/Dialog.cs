using System;

namespace Bla
{
	public partial class Dialog : Gtk.Dialog
	{
		string name;
		SymbolTable st;
		public Dialog (string name, SymbolTable st)
		{
			this.name = name;
			this.st = st;
			this.Build ();

		}

		protected void okButtonClicked (object sender, EventArgs e)
		{
			st.setVar (name, LOLType.YARN, getString());
			MainClass.win.refreshSymbol (st);
			this.Destroy ();
		}

		public string getString()
		{
			return inputField.Buffer.Text;
		}
	}
}

