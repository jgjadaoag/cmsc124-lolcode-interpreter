using System;
using Gtk;

public partial class MainWindow: Gtk.Window
{	
	Gtk.ListStore lexemeStore = new Gtk.ListStore (typeof (string), typeof (string));

	public MainWindow (): base (Gtk.WindowType.Toplevel)
	{
		Build ();
		lexemesInit ();
		symbolTableInit ();
	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}

	protected void lexemesInit ()
	{
		Gtk.TreeViewColumn lexemeColumn = new Gtk.TreeViewColumn ();
		Gtk.TreeViewColumn classificationColumn = new Gtk.TreeViewColumn ();

		lexemeColumn.Title = "Lexeme";
		classificationColumn.Title = "Classification";

		lexemesTree.AppendColumn (lexemeColumn);
		lexemesTree.AppendColumn (classificationColumn);

		lexemesTree.Model = lexemeStore;
		Gtk.CellRendererText lexemeCell = new Gtk.CellRendererText ();
		lexemeColumn.PackStart (lexemeCell, true);
		Gtk.CellRendererText classificationCell = new Gtk.CellRendererText ();
		classificationColumn.PackStart (classificationCell, true);

		lexemeColumn.AddAttribute (lexemeCell, "text", 0);
		classificationColumn.AddAttribute (classificationCell, "text", 1);

		lexemeStore.AppendValues ("HAI", "Code Delimiter"); //test sample
	}

	protected void symbolTableInit ()
	{
		Gtk.TreeViewColumn indentifierColumn = new Gtk.TreeViewColumn ();
		Gtk.TreeViewColumn valueColumn = new Gtk.TreeViewColumn ();

		indentifierColumn.Title = "Indentifier";
		valueColumn.Title = "Value";

		symbolTableTree.AppendColumn (indentifierColumn);
		symbolTableTree.AppendColumn (valueColumn);

		Gtk.ListStore symbolTableStore = new Gtk.ListStore (typeof (string), typeof (string));
		symbolTableTree.Model = symbolTableStore;
		Gtk.CellRendererText indentifierCell = new Gtk.CellRendererText ();
		indentifierColumn.PackStart (indentifierCell, true);
		Gtk.CellRendererText classificationCell = new Gtk.CellRendererText ();
		valueColumn.PackStart (classificationCell, true);

		indentifierColumn.AddAttribute (indentifierCell, "text", 0);
		valueColumn.AddAttribute (classificationCell, "text", 1);

		symbolTableStore.AppendValues ("HAI", "KTHXBYE"); //test sample
	}

	protected void executeOnClick ()
	{
		lexemeStore.Clear();

		string str = codeField.Buffer.Text;  // test sample
		console.Buffer.Text = str;
	}
}
