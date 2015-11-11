using System;
using Gtk;
using Bla;

public partial class MainWindow: Gtk.Window
{	
	Gtk.ListStore lexemeStore = new Gtk.ListStore (typeof (string), typeof (string));
	Gtk.ListStore symbolTableStore = new Gtk.ListStore (typeof (string), typeof (string));

	public MainWindow (): base (Gtk.WindowType.Toplevel)
	{
		Build ();
		lexemesInit ();
		symbolTableInit ();

		//change console background color
		console.ModifyBase(StateType.Normal, new Gdk.Color(0000,0000,0000));
		//change window color
		fixed1.ModifyBg (StateType.Normal, new Gdk.Color (0240, 0240, 0240));
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

		//lexemeStore.AppendValues ("HAI", "Code Delimiter"); //test sample
	}

	public void addLexemes(string value, string classification) {
		lexemeStore.AppendValues (value, classification);
	}

	protected void symbolTableInit ()
	{
		Gtk.TreeViewColumn indentifierColumn = new Gtk.TreeViewColumn ();
		Gtk.TreeViewColumn valueColumn = new Gtk.TreeViewColumn ();

		indentifierColumn.Title = "Indentifier";
		valueColumn.Title = "Value";

		symbolTableTree.AppendColumn (indentifierColumn);
		symbolTableTree.AppendColumn (valueColumn);

		symbolTableTree.Model = symbolTableStore;
		Gtk.CellRendererText indentifierCell = new Gtk.CellRendererText ();
		indentifierColumn.PackStart (indentifierCell, true);
		Gtk.CellRendererText classificationCell = new Gtk.CellRendererText ();
		valueColumn.PackStart (classificationCell, true);

		indentifierColumn.AddAttribute (indentifierCell, "text", 0);
		valueColumn.AddAttribute (classificationCell, "text", 1);

		//symbolTableStore.AppendValues ("HAI", "KTHXBYE"); //test sample
	}

	public void addSymbol(string value, string classification) {
		symbolTableStore.AppendValues (value, classification);

	}

	protected void OnButton3Clicked (object sender, EventArgs e)
	{
		lexemeStore.Clear();
		
		string str = codeField.Buffer.Text;
		//displayTextToConsole (str);
		MainClass.runInterpreter(str);
	}

	public void displayTextToConsole (String stringText)
	{
		//change console text color
		var consoleTextColorTag = new TextTag ("colorTag");
		consoleTextColorTag.Foreground = "white";
		console.Buffer.TagTable.Add (consoleTextColorTag);

		TextIter insertIter = console.Buffer.EndIter;
		console.Buffer.InsertWithTagsByName (ref insertIter, stringText + "\n", "colorTag");
		console.Buffer.ApplyTag ("word_wrap", console.Buffer.StartIter, console.Buffer.EndIter);
	}

	protected void OnOpenFileButtonClicked (object sender, EventArgs e)
	{
		using (FileChooserDialog fileChooser = new FileChooserDialog (null,"Open File", // pang- gawa nung pang open na window
		                                                              null, FileChooserAction.Open,"Cancel", ResponseType.Cancel, 
		                                                              "Open", ResponseType.Accept)) {
			if (fileChooser.Run () == (int)ResponseType.Accept) {
				System.IO.StreamReader file = System.IO.File.OpenText (fileChooser.Filename);
				codeField.Buffer.Text = "  " + file.ReadToEnd ();		// pag lagay dun sa text view
				file.Close ();
				fileChooser.Destroy ();		//pang close nung window
			}
		}
	}
}
