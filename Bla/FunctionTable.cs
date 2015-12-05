using System;
using System.Collections.Generic;

namespace Bla
{
	public class LolFunction {
		public readonly int startLocation;
		public readonly int endLocation;
		public readonly List<string> parameters;

		public LolFunction(int s, int e, List<string> p) {
			startLocation = s;
			endLocation = e;
			parameters = p;
		}

	}
	public class FunctionTable
	{
		Dictionary<string, LolFunction> functionList;
		
		public FunctionTable ()
		{
			functionList = new Dictionary<string, LolFunction> ();
		}

		public bool addFunction(string n, LolFunction func) {
			if (!functionList.ContainsKey (n)) {
				Console.WriteLine("Adding function " + n );
				functionList.Add (n, func);
				return true;
			}

			return false;
		}

		public LolFunction getFunction(string n) {
			if (functionList.ContainsKey (n)) {
				return functionList [n];
			}
			return null;
		}
	}
}

