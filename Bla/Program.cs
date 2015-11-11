using System;
using Gtk;
using System.Collections.Generic;

namespace Bla
{
	class MainClass
	{
		static MainWindow win;
		static Dictionary <string, string> variableList;
		public static void Main (string[] args)
		{
			Application.Init ();
			win = new MainWindow ();
			win.Show ();
			Application.Run ();
		}

		public static void runInterpreter(string input) {
			variableList = new Dictionary <string, string> ();
			TokenStream ts = new TokenStream (input);
			Token t;
			while (!ts.end ()) {
				//try {
					t = ts.get ();
				win.addLexemes (t.getValue (), t.getType ().ToString ());
				if (t.getType() == TokenType.I_HAS_A) {
					t = ts.get ();
					if (!variableList.ContainsKey (t.getValue ())) {
						variableList.Add (t.getValue (), "NOOB");
						win.addSymbol (t.getValue (), "NOOB");		
						win.addLexemes (t.getValue (), t.getType ().ToString ());					
					} else {
						win.displayTextToConsole (t.getValue()+" is already declared.");
						return;
					}

				}

					string rex = ts.tokenDetails[t.getType()].ToString();
					Console.WriteLine(rex[rex.Length - 1]);
				/*} catch(Exception e) {	
					win.displayTextToConsole (e.Source);
					win.displayTextToConsole (e.Message);
					break;
				}*/
			}
		}
	}
}
