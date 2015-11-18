using System;
using Gtk;
using System.Collections.Generic;

namespace Bla
{
	class MainClass
	{
		public static MainWindow win;
		public static SymbolTable st;

		public static void Main (string[] args)
		{
			Application.Init ();
			win = new MainWindow ();
			win.Show ();
			st = new SymbolTable ();
			st.createVar ("a", new Tuple<LOLType, string> (LOLType.YARN, "hello"));
			win.refreshSymbol (st);
			Application.Run ();
		}

		public static void writeToConsole(string str) {
			win.displayTextToConsole (str);
		}

	

		public static void runInterpreter(string input) {
			TokenStream ts = new TokenStream (input);
			Token t;
			while (!ts.end ()) {
				//try {
					t = ts.get ();
				win.addLexemes (t.getValue (), t.getType ().ToString ());
				/**if (t.getType() == TokenType.I_HAS_A) {
					t = ts.get ();
					if (!variableList.ContainsKey (t.getValue ())) {
						variableList.Add (t.getValue (), "NOOB");
						win.addSymbol (t.getValue (), "NOOB");		
						win.addLexemes (t.getValue (), t.getType ().ToString ());					
					} else {
						win.displayTextToConsole (t.getValue()+" is already declared.");
						return;
					}

				}**/
					string rex = ts.tokenDetails[t.getType()].ToString();
					
					Console.WriteLine(rex[rex.Length - 1]);
				/*} catch(Exception e) {	
					win.displayTextToConsole (e.Source);
					win.displayTextToConsole (e.Message);
					break;
				}*/
			}

			Parser p = new Parser (input);
			if (p.parse () == false) {
				win.displayTextToConsole ("Syntax error :(");
			}

		}
	}
}
