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

		public void setVar(string name, Tuple<LOLType, string> value)
		{
			variableList [name] = value;
		}

		public void createVar(string name, Tuple<LOLType, string> value) {
			variableList.Add (name, value);
		}

		public Dictionary<string, Tuple<LOLType, string>> getVariableList() {
			return variableList;
		}
	}
}

