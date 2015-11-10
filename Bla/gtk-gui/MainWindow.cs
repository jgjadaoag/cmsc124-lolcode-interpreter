
// This file has been generated by the GUI designer. Do not modify.
public partial class MainWindow
{
	private global::Gtk.UIManager UIManager;
	private global::Gtk.Action OpenFileAction;
	private global::Gtk.Action OpenFileAction1;
	private global::Gtk.Fixed fixed1;
	private global::Gtk.Fixed fixed3;
	private global::Gtk.ScrolledWindow GtkScrolledWindow;
	private global::Gtk.TextView codeField;
	private global::Gtk.ScrolledWindow GtkScrolledWindow1;
	private global::Gtk.TreeView lexemesTree;
	private global::Gtk.ScrolledWindow GtkScrolledWindow2;
	private global::Gtk.TreeView symbolTableTree;
	private global::Gtk.Label symbolTable;
	private global::Gtk.ScrolledWindow GtkScrolledWindow3;
	private global::Gtk.TextView console;
	private global::Gtk.Label label4;
	private global::Gtk.Label lexemes;
	private global::Gtk.Label label2;
	private global::Gtk.Button OpenFileButton;
	private global::Gtk.Label label1;
	private global::Gtk.Button executeButton;

	protected virtual void Build ()
	{
		global::Stetic.Gui.Initialize (this);
		// Widget MainWindow
		this.UIManager = new global::Gtk.UIManager ();
		global::Gtk.ActionGroup w1 = new global::Gtk.ActionGroup ("Default");
		this.OpenFileAction = new global::Gtk.Action ("OpenFileAction", global::Mono.Unix.Catalog.GetString ("Open File..."), null, null);
		this.OpenFileAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("Open File");
		w1.Add (this.OpenFileAction, null);
		this.OpenFileAction1 = new global::Gtk.Action ("OpenFileAction1", global::Mono.Unix.Catalog.GetString ("Open File"), null, null);
		this.OpenFileAction1.ShortLabel = global::Mono.Unix.Catalog.GetString ("Open File");
		w1.Add (this.OpenFileAction1, null);
		this.UIManager.InsertActionGroup (w1, 0);
		this.AddAccelGroup (this.UIManager.AccelGroup);
		this.Name = "MainWindow";
		this.Title = global::Mono.Unix.Catalog.GetString ("ʕノ•ᴥ•ʔノ ︵ ┻━┻");
		this.Icon = global::Stetic.IconLoader.LoadIcon (this, "gtk-about", global::Gtk.IconSize.Menu);
		this.WindowPosition = ((global::Gtk.WindowPosition)(4));
		// Container child MainWindow.Gtk.Container+ContainerChild
		this.fixed1 = new global::Gtk.Fixed ();
		this.fixed1.HasWindow = true;
		// Container child fixed1.Gtk.Fixed+FixedChild
		this.fixed3 = new global::Gtk.Fixed ();
		this.fixed3.Name = "fixed3";
		this.fixed3.HasWindow = false;
		this.fixed1.Add (this.fixed3);
		// Container child fixed1.Gtk.Fixed+FixedChild
		this.GtkScrolledWindow = new global::Gtk.ScrolledWindow ();
		this.GtkScrolledWindow.WidthRequest = 250;
		this.GtkScrolledWindow.HeightRequest = 250;
		this.GtkScrolledWindow.Name = "GtkScrolledWindow";
		this.GtkScrolledWindow.ShadowType = ((global::Gtk.ShadowType)(1));
		// Container child GtkScrolledWindow.Gtk.Container+ContainerChild
		this.codeField = new global::Gtk.TextView ();
		this.codeField.CanFocus = true;
		this.codeField.Name = "codeField";
		this.GtkScrolledWindow.Add (this.codeField);
		this.fixed1.Add (this.GtkScrolledWindow);
		global::Gtk.Fixed.FixedChild w4 = ((global::Gtk.Fixed.FixedChild)(this.fixed1 [this.GtkScrolledWindow]));
		w4.X = 30;
		w4.Y = 91;
		// Container child fixed1.Gtk.Fixed+FixedChild
		this.GtkScrolledWindow1 = new global::Gtk.ScrolledWindow ();
		this.GtkScrolledWindow1.WidthRequest = 250;
		this.GtkScrolledWindow1.HeightRequest = 250;
		this.GtkScrolledWindow1.Name = "GtkScrolledWindow1";
		this.GtkScrolledWindow1.ShadowType = ((global::Gtk.ShadowType)(1));
		// Container child GtkScrolledWindow1.Gtk.Container+ContainerChild
		this.lexemesTree = new global::Gtk.TreeView ();
		this.lexemesTree.CanFocus = true;
		this.lexemesTree.Name = "lexemesTree";
		this.GtkScrolledWindow1.Add (this.lexemesTree);
		this.fixed1.Add (this.GtkScrolledWindow1);
		global::Gtk.Fixed.FixedChild w6 = ((global::Gtk.Fixed.FixedChild)(this.fixed1 [this.GtkScrolledWindow1]));
		w6.X = 325;
		w6.Y = 92;
		// Container child fixed1.Gtk.Fixed+FixedChild
		this.GtkScrolledWindow2 = new global::Gtk.ScrolledWindow ();
		this.GtkScrolledWindow2.WidthRequest = 250;
		this.GtkScrolledWindow2.HeightRequest = 250;
		this.GtkScrolledWindow2.Name = "GtkScrolledWindow2";
		this.GtkScrolledWindow2.ShadowType = ((global::Gtk.ShadowType)(1));
		// Container child GtkScrolledWindow2.Gtk.Container+ContainerChild
		this.symbolTableTree = new global::Gtk.TreeView ();
		this.symbolTableTree.CanFocus = true;
		this.symbolTableTree.Name = "symbolTableTree";
		this.GtkScrolledWindow2.Add (this.symbolTableTree);
		this.fixed1.Add (this.GtkScrolledWindow2);
		global::Gtk.Fixed.FixedChild w8 = ((global::Gtk.Fixed.FixedChild)(this.fixed1 [this.GtkScrolledWindow2]));
		w8.X = 617;
		w8.Y = 89;
		// Container child fixed1.Gtk.Fixed+FixedChild
		this.symbolTable = new global::Gtk.Label ();
		this.symbolTable.Name = "symbolTable";
		this.symbolTable.LabelProp = global::Mono.Unix.Catalog.GetString ("Symbol Table");
		this.fixed1.Add (this.symbolTable);
		global::Gtk.Fixed.FixedChild w9 = ((global::Gtk.Fixed.FixedChild)(this.fixed1 [this.symbolTable]));
		w9.X = 707;
		w9.Y = 59;
		// Container child fixed1.Gtk.Fixed+FixedChild
		this.GtkScrolledWindow3 = new global::Gtk.ScrolledWindow ();
		this.GtkScrolledWindow3.WidthRequest = 800;
		this.GtkScrolledWindow3.HeightRequest = 200;
		this.GtkScrolledWindow3.Name = "GtkScrolledWindow3";
		this.GtkScrolledWindow3.ShadowType = ((global::Gtk.ShadowType)(1));
		// Container child GtkScrolledWindow3.Gtk.Container+ContainerChild
		this.console = new global::Gtk.TextView ();
		this.console.CanFocus = true;
		this.console.Name = "console";
		this.GtkScrolledWindow3.Add (this.console);
		this.fixed1.Add (this.GtkScrolledWindow3);
		global::Gtk.Fixed.FixedChild w11 = ((global::Gtk.Fixed.FixedChild)(this.fixed1 [this.GtkScrolledWindow3]));
		w11.X = 52;
		w11.Y = 440;
		// Container child fixed1.Gtk.Fixed+FixedChild
		this.label4 = new global::Gtk.Label ();
		this.label4.Name = "label4";
		this.label4.LabelProp = global::Mono.Unix.Catalog.GetString ("Console");
		this.fixed1.Add (this.label4);
		global::Gtk.Fixed.FixedChild w12 = ((global::Gtk.Fixed.FixedChild)(this.fixed1 [this.label4]));
		w12.X = 54;
		w12.Y = 409;
		// Container child fixed1.Gtk.Fixed+FixedChild
		this.lexemes = new global::Gtk.Label ();
		this.lexemes.Name = "lexemes";
		this.lexemes.LabelProp = global::Mono.Unix.Catalog.GetString ("Lexemes");
		this.fixed1.Add (this.lexemes);
		global::Gtk.Fixed.FixedChild w13 = ((global::Gtk.Fixed.FixedChild)(this.fixed1 [this.lexemes]));
		w13.X = 427;
		w13.Y = 62;
		// Container child fixed1.Gtk.Fixed+FixedChild
		this.label2 = new global::Gtk.Label ();
		this.label2.Name = "label2";
		this.label2.LabelProp = global::Mono.Unix.Catalog.GetString ("Code");
		this.fixed1.Add (this.label2);
		global::Gtk.Fixed.FixedChild w14 = ((global::Gtk.Fixed.FixedChild)(this.fixed1 [this.label2]));
		w14.X = 40;
		w14.Y = 61;
		// Container child fixed1.Gtk.Fixed+FixedChild
		this.OpenFileButton = new global::Gtk.Button ();
		this.OpenFileButton.CanFocus = true;
		this.OpenFileButton.Name = "OpenFileButton";
		this.OpenFileButton.UseUnderline = true;
		this.OpenFileButton.Label = global::Mono.Unix.Catalog.GetString ("Open File...");
		this.fixed1.Add (this.OpenFileButton);
		global::Gtk.Fixed.FixedChild w15 = ((global::Gtk.Fixed.FixedChild)(this.fixed1 [this.OpenFileButton]));
		w15.X = 116;
		w15.Y = 56;
		// Container child fixed1.Gtk.Fixed+FixedChild
		this.label1 = new global::Gtk.Label ();
		this.label1.Name = "label1";
		this.label1.LabelProp = global::Mono.Unix.Catalog.GetString ("ANG GANDA NI MA'AM KAT LOLTERPRETER");
		this.fixed1.Add (this.label1);
		global::Gtk.Fixed.FixedChild w16 = ((global::Gtk.Fixed.FixedChild)(this.fixed1 [this.label1]));
		w16.X = 17;
		w16.Y = 14;
		// Container child fixed1.Gtk.Fixed+FixedChild
		this.executeButton = new global::Gtk.Button ();
		this.executeButton.WidthRequest = 800;
		this.executeButton.CanFocus = true;
		this.executeButton.Name = "executeButton";
		this.executeButton.UseUnderline = true;
		this.executeButton.Label = global::Mono.Unix.Catalog.GetString ("EXECUTE");
		this.fixed1.Add (this.executeButton);
		global::Gtk.Fixed.FixedChild w17 = ((global::Gtk.Fixed.FixedChild)(this.fixed1 [this.executeButton]));
		w17.X = 47;
		w17.Y = 368;
		this.Add (this.fixed1);
		if ((this.Child != null)) {
			this.Child.ShowAll ();
		}
		this.DefaultWidth = 906;
		this.DefaultHeight = 667;
		this.Show ();
		this.DeleteEvent += new global::Gtk.DeleteEventHandler (this.OnDeleteEvent);
		//this.OpenFileAction1.Activated += new global::System.EventHandler (this.OnOpenFileAction1Activated);
		this.OpenFileButton.Clicked += new global::System.EventHandler (this.OnOpenFileButtonClicked);
		this.executeButton.Clicked += new global::System.EventHandler (this.OnButton3Clicked);
	}
}
