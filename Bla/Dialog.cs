using System;

namespace Bla
{
	public partial class Dialog : Gtk.Dialog
	{
		string name;
		public Dialog (string name)
		{
			this.name = name;
			this.Build ();
		}

		protected void okButtonClicked (object sender, EventArgs e)
		{
			getString ();
			MainClass.st.setVar (name, LOLType.YARN, getString());
			this.Destroy ();
		}

		public string getString()
		{
			return inputField.Buffer.Text;
		}
	}
}

