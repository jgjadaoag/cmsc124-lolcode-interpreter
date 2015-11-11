using System;
using Gtk;

namespace Bla
{
	class MainClass
	{
		static MainWindow win;
		public static void Main (string[] args)
		{
			Application.Init ();
			win = new MainWindow ();
			win.Show ();
			Application.Run ();
		}

		public static void runInterpreter(string input) {
			TokenStream ts = new TokenStream (input);
			Token t;
			while (!ts.end ()) {
				//try {
					t = ts.get ();
					win.addLexemes (t.getValue (), t.getType ().ToString ());
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
