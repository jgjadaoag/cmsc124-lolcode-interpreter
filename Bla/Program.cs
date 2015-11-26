using System;
using Gtk;
using System.Collections.Generic;

namespace Bla
{
	class MainClass
	{
		public static MainWindow win;

		public static void Main (string[] args)
		{
			Application.Init ();
			win = new MainWindow ();
			win.Show ();
			Application.Run ();
		}

		public static void writeToConsole(string str) {
			win.displayTextToConsole (str);
		}

	

		public static void runInterpreter(string input) {

			/*Parser p = new Parser (input);
			if (p.parse () == false) {
				win.displayTextToConsole ("Syntax error :(");
			}*/

			Interpreter interpret = new Interpreter (input);
			interpret.runProgram ();

		}
	}
}
