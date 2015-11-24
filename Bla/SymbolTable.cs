using System;
using System.Collections.Generic;

namespace Bla
{
	public class SymbolTable
	{
		Dictionary <string, Tuple<LOLType, string>> variableList;
		public SymbolTable ()
		{
			variableList = new Dictionary <string, Tuple<LOLType, string>> ();
		}

		public void setVar(string name, LOLType type, string value)
		{
			variableList [name] = new Tuple<LOLType, string> (type, value);;
		}

		public void createVar(string name, LOLType type, string value) {
			variableList.Add (name, new Tuple<LOLType, string> (type, value));
		}

		public Dictionary<string, Tuple<LOLType, string>> getVariableList() {
			return variableList;
		}

		public bool hasVariable(string name) {
			return variableList.ContainsKey (name);
		}
	}
}

