using System;
using System.Collections.Generic;

namespace Bla
{
	public class lolValue {
		LOLType type;
		string value;

		public lolValue(LOLType t, string v) {
			type = t;
			value = v;
		}

		public void setValue(LOLType t, string v) {
			type = t;
			value = v;
		}

		public string getValue() {
			return value;
		}
	}
	public class SymbolTable
	{
		Dictionary <string, lolValue> variableList;
		public SymbolTable ()
		{
			variableList = new Dictionary <string, lolValue> ();
		}

		public void setVar(string name, LOLType type, string value)
		{
			variableList [name].setValue(type, value);
		}

		public void createVar(string name, LOLType type, string value) {
			variableList.Add (name, new lolValue (type, value));
		}

		public Dictionary<string, lolValue> getVariableList() {
			return variableList;
		}

		public bool hasVariable(string name) {
			return variableList.ContainsKey (name);
		}
	}
}

