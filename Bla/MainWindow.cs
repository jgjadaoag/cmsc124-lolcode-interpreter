using System;
using Gtk;
using Bla;
using System.Collections.Generic;

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

		//change console text color
		var consoleTextColorTag = new TextTag ("colorTag");
		consoleTextColorTag.Foreground = "white";
		console.Buffer.TagTable.Add (consoleTextColorTag);
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

	}

	public void addLexemes(string value, string classification) {
		switch(classification){		//Classification determiner
		case "HAI":
		case "KTHXBYE":
			classification = "Code block delimiter";
			break;
		case "BTW":
		case "OBTW":
		case "TLDR":
			classification = "Comment delimiter";
			break;
		case "I_HAS_A":
			classification = "Variable Declaration";
			break;
		case "ITZ":
		case "R":
			classification = "Variable Assignment";
			break;
		case "SUM_OF":
		case "DIFF_OF":
		case "PRODUKT_OF":
		case "QUOSHUNT_OF":
		case "MOD_OF":
		case "BIGGR_OF":
		case "SMALLR_OF":
			classification = "Math Operator";
			break;
		case "BOTH_OF":
		case "EITHER_OF":
		case "WON_OF":
		case "NOT":
		case "ALL_OF":
		case "ANY_OF":
			classification = "Boolean Operator";
			break;
		case "BOTH_SAEM":
			classification = "Equality Operator";
			break;
		case "DIFFRINT":
			classification = "Inequality Operator";
			break;
		case "UPPIN":
			classification = "Increment Operator";
			break;
		case "NERFIN":
			classification = "Decrement Operator";
			break;
		case "SMOOSH":
			classification = "Concatenation Operator";
			break;
		case "MAEK":
		case "IS_NOW_A":
		case "VISIBLE":
			classification = "Output Keyword";
			break;
		case "GIMMEH":
			classification = "Input Keyword";
			break;
		case "O_RLY":
			classification = "If-Then Delimiter";
			break;
		case "YA_RLY":
			classification = "If Keyword";
			break;
		case "MEBBE":
			classification = "Else-If Keyword";
			break;
		case "NO_WAI":
			classification = "Else Keyword";
			break;
		case "OIC":
			classification = "If-Then/Switch Delimiter";
			break;
		case "WTF":
			classification = "Switch Delimiter";
			break;
		case "OMG":
			classification = "Case Keyword";
			break;
		case "OMGWTF":
			classification = "Default Keyword";
			break;
		case "GTFO":
			classification = "Break Keyword";
			break;
		case "STRING_DELIMETER":
			classification = "String Delimiter";
			break;
		case "A":
		case "AN":
			classification = "Argument Separator";
			break;
		case "YR":
			classification = "Parameter Keyword";
			break;
		case "MKAY":
			classification = "Arity Delimiter";
			break;
		case "VARIABLE_IDENTIFIER":
			classification = "Variable Identifier";
			break;
		case "NUMBR_LITERAL":
		case "NUMBAR_LITERAL":
		case "YARN_LITERAL":
		case "TROOF_LITERAL":
		case "TYPE_LITERAL":
			classification = "Literal";
			break;
		case "STATEMENT_DELIMETER":
			classification = "Statement Delimiter";
			break;
		case "HOW_IZ_I":
		case "IF_U_SAY_SO":
			classification = "Function Delimiter";
			break;
		case "I_IZ":
			classification = "Function Call Keyword";
			break;
		case "FOUND_YR":
			classification = "Return Keyword";
			break;
		case "IM_IN_YR":
		case "IM_OUTTA_YR":
			classification = "Loop Delimiter";
			break;
		case "TIL":
		case "WILE":
			classification = "Condition Keyword";
			break;
		case "EXCLAMATION":
			classification = "New Line Remover";
			break;
		}

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

	}

	public void refreshSymbol(SymbolTable st) {
		symbolTableStore.Clear ();
		foreach(KeyValuePair<string, lolValue> kvp in st.getVariableList()) {
			symbolTableStore.AppendValues (kvp.Key, kvp.Value.getValue());
		}

	}

	protected void OnButton3Clicked (object sender, EventArgs e)
	{
		lexemeStore.Clear();
		symbolTableStore.Clear ();
		console.Buffer.Text = "";

		string str = codeField.Buffer.Text;
		//displayTextToConsole (str);
		MainClass.runInterpreter(str);
	}

	public void displayTextToConsoleNoLine (String stringText)
	{
		TextIter insertIter = console.Buffer.EndIter;
		Console.WriteLine (stringText);
		console.Buffer.InsertWithTagsByName (ref insertIter, stringText, "colorTag");
		//console.Buffer.ApplyTag ("word_wrap", console.Buffer.StartIter, console.Buffer.EndIter);
	}

	public void displayTextToConsole (String stringText)
	{
		TextIter insertIter = console.Buffer.EndIter;
		Console.WriteLine (stringText);
		console.Buffer.InsertWithTagsByName (ref insertIter, stringText + "\n", "colorTag");
		//console.Buffer.ApplyTag ("word_wrap", console.Buffer.StartIter, console.Buffer.EndIter);
	}

	protected void OnOpenFileButtonClicked (object sender, EventArgs e)
	{
		using (FileChooserDialog fileChooser = new FileChooserDialog (null,"Open File",
		                                                              null, FileChooserAction.Open,"Cancel", ResponseType.Cancel, 
		                                                              "Open", ResponseType.Accept)) {
			if (fileChooser.Run () == (int)ResponseType.Accept) {
				System.IO.StreamReader file = System.IO.File.OpenText (fileChooser.Filename);
			/*	codeField.Buffer.Text = file.ReadAllLines ();		// put the file content to codeField

				file.Close ();
				fileChooser.Destroy ();*/

				codeField.Buffer.Text = "";		//to clear code field

				string line = file.ReadLine();
				string nextLine;
				while(line != null)
				{
					nextLine = file.ReadLine ();
					if (nextLine == null)
						codeField.Buffer.Text += line;
					else 
						codeField.Buffer.Text += line + "\n";

					line = nextLine;
				}

				file.Close();
				fileChooser.Destroy ();
			}
		}
	}
}
