using System;
using System.Collections.Generic;

namespace Bla
{
	public enum Statement_Types {
		VARIABLE_DECLARATION_ITZ,
		VARIABLE_DECLARATION,
		VARIABLE_ASSIGNMENT,
		IF_THEN_START,
		IF_THEN_FALSE,
		IF_THEN_TRUE,
		ELSE_IF,
		SWITCH,
		VARIABLE_IDENTIFIER,
		ADDITION,
		SUBTRACTION,
		MULTIPLICATION,
		DIVISION,
		MODULO,
		MAXIMUM,
		MINIMUM,
		EQUALITY,
		INEQUALITY,
		GREATER,
		LESS,
		GREATER_EQUAL,
		LESS_EQUAL,
		LITERAL,
		INPUT,
		OUTPUT,
		CONCAT_MKAY,
		CONCAT,
		AND,
		OR,
		XOR,
		NOT,
		OIC
	}

	public class lolStatement {
		public readonly Statement_Types type;
		public readonly int location;
		public lolStatement(Statement_Types st, int loc) {
			this.type = st;
			this.location = loc;
		}
	}
	public class Interpreter
	{
		delegate void lolAction(int location);
		List <Token> tokenList;
		List <lolStatement> actionList;
		Dictionary <Statement_Types, lolAction> actionMap;
		Dictionary <TokenType, LOLType> tokToLolType;
		int currentPosition;
		SymbolTable variableTable;
		lolValue lolIt;
		bool errorFlag;
		string errorMessage;

		public Interpreter (string input)
		{
			actionMap = new Dictionary<Statement_Types, lolAction> ();
			tokenList = new List<Token> ();
			TokenStream ts = new TokenStream (input);
			Token t;
			currentPosition = 0;
			variableTable = new SymbolTable ();
			lolIt = new lolValue (LOLType.NOOB, "");
			errorFlag = false;

			while(!ts.end()){
				t = ts.get ();
				MainClass.win.addLexemes (t.getValue (), t.getType ().ToString ());
				if (t.getType () == TokenType.UNKNOWN) {
					MainClass.win.displayTextToConsole ("Unknown token: " + t.getValue ());
					tokenList.Add (t);
				} else if ((t.getType () != TokenType.BTW && t.getType () != TokenType.OBTW)) {
					tokenList.Add (t);
				} 
			}
			Parser p = new Parser (tokenList);
			if (!p.parse ()) {
				setError ("Error in parser");
			};

			tokToLolType = new Dictionary<TokenType, LOLType> ();

			tokToLolType.Add (TokenType.NUMBR_LITERAL, LOLType.NUMBR);
			tokToLolType.Add (TokenType.NUMBAR_LITERAL, LOLType.NUMBAR);
			tokToLolType.Add (TokenType.TROOF_LITERAL, LOLType.TROOF);
			tokToLolType.Add (TokenType.YARN_LITERAL, LOLType.YARN);

			addActionDefinitions ();

			actionList = p.getActionOrder ();
		}

		void addActionDefinitions() {
			actionMap.Add (Statement_Types.VARIABLE_DECLARATION_ITZ, variableDeclarationItz);
			actionMap.Add (Statement_Types.VARIABLE_DECLARATION, variableDeclaration);
			actionMap.Add (Statement_Types.VARIABLE_ASSIGNMENT, variableAssignment);
			actionMap.Add (Statement_Types.ADDITION, addition);
			actionMap.Add (Statement_Types.SUBTRACTION, subtraction);
			actionMap.Add (Statement_Types.MULTIPLICATION, multiplication);
			actionMap.Add (Statement_Types.DIVISION, division);
			actionMap.Add (Statement_Types.OUTPUT, output);
			actionMap.Add (Statement_Types.INPUT, input);
			actionMap.Add (Statement_Types.LITERAL, literal);
			actionMap.Add (Statement_Types.VARIABLE_IDENTIFIER, variableIdentifier);
			actionMap.Add (Statement_Types.AND, and);
			actionMap.Add (Statement_Types.OR, or);
			actionMap.Add (Statement_Types.XOR, xor);
			actionMap.Add (Statement_Types.NOT, not);
			actionMap.Add (Statement_Types.EQUALITY, equality);
			actionMap.Add (Statement_Types.INEQUALITY, inequality);
			actionMap.Add (Statement_Types.MODULO, modulo);
			actionMap.Add (Statement_Types.MAXIMUM, maximum);
			actionMap.Add (Statement_Types.IF_THEN_START, ifThenStart);
		}

		public void runProgram() {
			Console.WriteLine ("=============================================");
			Console.WriteLine ("=ACTION=LIST================================");
			Console.WriteLine ("=============================================");
			foreach (lolStatement ls in actionList) {
				Console.WriteLine (ls.type.ToString());
			}
			Console.WriteLine ("=============================================");
			for (currentPosition = 0; currentPosition < actionList.Count && !errorFlag; currentPosition++) {
				actionMap [actionList[currentPosition].type] (actionList[currentPosition].location);
			}
			if (errorFlag) {
				MainClass.win.displayTextToConsole (errorMessage);
			}
		}

		void setError(string message) {
			errorFlag = true;
			errorMessage = message;
		}

		void variableDeclarationItz(int location) {
			//Execute expressions first
			currentPosition++;
			actionMap [actionList[currentPosition].type] (actionList[currentPosition].location);

			if (!variableTable.hasVariable (tokenList [location + 1].getValue ())) {
				variableTable.createVar (tokenList [location + 1].getValue (), 
					lolIt.getType(),
					lolIt.getValue ());
			} else {
				setError ("Error: Variable " + tokenList[location + 1].getValue() + " already declared");
			}
			MainClass.win.refreshSymbol (variableTable);
		}
		void variableDeclaration(int location) {
			if (!variableTable.hasVariable (tokenList [location + 1].getValue ())) {
				variableTable.createVar (tokenList [location + 1].getValue (), 
					LOLType.NOOB,
					"");
			} else {
				setError ("Error: Variable " + tokenList[location + 1].getValue() + " already declared");
			}
			MainClass.win.refreshSymbol (variableTable);
		}

		void variableAssignment(int location){
			if (variableTable.hasVariable (tokenList [location].getValue ())) {
				variableTable.setVar (tokenList [location].getValue (), 
					tokToLolType [tokenList [location + 2].getType ()],
					tokenList [location + 2].getValue ());
			} else {
				setError ("Error: Variable " + tokenList[location].getValue() + " not declared");
			}
			MainClass.win.refreshSymbol (variableTable);
		}

		void variableIdentifier(int location) {
			if (variableTable.hasVariable (tokenList [location].getValue ())) {
				lolValue val = variableTable.getVar (tokenList [location].getValue ());
				lolIt.setValue (val.getType (), val.getValue());
			}
		}

		void addition(int location){
			decimal sum = 0;
			currentPosition++;
			actionMap [actionList[currentPosition].type] (actionList[currentPosition].location);
			lolValue add1 = lolIt.getCopy();

			currentPosition++;
			actionMap [actionList [currentPosition].type] (actionList [currentPosition].location);
			lolValue add2 = lolIt.getCopy();

			LOLType sumType = LOLType.NUMBR;

			if (add1.getType () == LOLType.NUMBAR || add2.getType () == LOLType.NUMBAR) {
				sum = decimal.Parse(add1.getValue ()) + decimal.Parse (add2.getValue ());
				sumType = LOLType.NUMBAR;
			} else {
				sum = int.Parse (add1.getValue ()) + int.Parse (add2.getValue ());
			}

			lolIt.setValue (sumType, sum.ToString());
			MainClass.win.displayTextToConsole (""+sum);
		}

		void subtraction(int location){
			decimal diff = 0;
			currentPosition++;
			actionMap [actionList[currentPosition].type] (actionList[currentPosition].location);
			lolValue d1 = lolIt.getCopy();

			currentPosition++;
			actionMap [actionList [currentPosition].type] (actionList [currentPosition].location);
			lolValue d2 = lolIt.getCopy();

			LOLType sumType = LOLType.NUMBR;

			if (d1.getType () == LOLType.NUMBAR || d2.getType () == LOLType.NUMBAR) {
				diff = decimal.Parse(d1.getValue ()) - decimal.Parse (d2.getValue ());
				sumType = LOLType.NUMBAR;
			} else {
				diff = int.Parse (d1.getValue ()) - int.Parse (d2.getValue ());
			}

			lolIt.setValue (sumType, diff.ToString());
			Console.WriteLine(diff);
			MainClass.win.displayTextToConsole (""+diff);
		}

		void multiplication(int location){
			decimal prod = 0;
			currentPosition++;
			actionMap [actionList[currentPosition].type] (actionList[currentPosition].location);
			lolValue d1 = lolIt.getCopy();

			currentPosition++;
			actionMap [actionList [currentPosition].type] (actionList [currentPosition].location);
			lolValue d2 = lolIt.getCopy();

			LOLType sumType = LOLType.NUMBR;

			if (d1.getType () == LOLType.NUMBAR || d2.getType () == LOLType.NUMBAR) {
				prod = decimal.Parse(d1.getValue ()) * decimal.Parse (d2.getValue ());
				sumType = LOLType.NUMBAR;
			} else {
				prod = int.Parse (d1.getValue ()) * int.Parse (d2.getValue ());
			}

			lolIt.setValue (sumType, prod.ToString());
			MainClass.win.displayTextToConsole (""+prod);
		}

		void division(int location){
			decimal quo = 0;
			currentPosition++;
			actionMap [actionList[currentPosition].type] (actionList[currentPosition].location);
			lolValue d1 = lolIt.getCopy();

			currentPosition++;
			actionMap [actionList [currentPosition].type] (actionList [currentPosition].location);
			lolValue d2 = lolIt.getCopy();

			LOLType sumType = LOLType.NUMBR;

			if (decimal.Parse (d2.getValue ()) != 0) {
				
				quo = decimal.Parse (d1.getValue ()) / decimal.Parse (d2.getValue ());
				sumType = LOLType.NUMBAR;

				lolIt.setValue (sumType, quo.ToString ());
				MainClass.win.displayTextToConsole ("" + quo);
			} else {
				setError ("Invalid operation: Division by zero");
			} 
				
		}

		void modulo(int location){
			decimal quo = 0;
			currentPosition++;
			actionMap [actionList[currentPosition].type] (actionList[currentPosition].location);
			lolValue d1 = lolIt.getCopy();

			currentPosition++;
			actionMap [actionList [currentPosition].type] (actionList [currentPosition].location);
			lolValue d2 = lolIt.getCopy();

			LOLType sumType = LOLType.NUMBR;

			if (decimal.Parse (d2.getValue ()) != 0) {
				
				quo = decimal.Parse (d1.getValue ()) % decimal.Parse (d2.getValue ());
				sumType = LOLType.NUMBAR;

				lolIt.setValue (sumType, quo.ToString ());
			} else {
				setError ("Invalid operation: Division by zero");
			} 
				
		}

		void maximum(int location){
			decimal higher;
			currentPosition++;
			actionMap [actionList[currentPosition].type] (actionList[currentPosition].location);
			lolValue d1 = lolIt.getCopy();

			currentPosition++;
			actionMap [actionList [currentPosition].type] (actionList [currentPosition].location);
			lolValue d2 = lolIt.getCopy();

			LOLType sumType = LOLType.NUMBR;

			/*
			if (isNumberType(d1) || isNumberType(d2)) {
				diff = decimal.Parse(d1.getValue ()) - decimal.Parse (d2.getValue ());
				sumType = LOLType.NUMBAR;
			} else {
				setError("Error comparing non numeric types");
			}

			lolIt.setValue (sumType, diff.ToString());
			Console.WriteLine(diff);
			MainClass.win.displayTextToConsole (""+diff);
			*/
		}

		void output(int location) {
			//Execute expressions first
			currentPosition++;
			actionMap [actionList[currentPosition].type] (actionList[currentPosition].location);
			MainClass.win.displayTextToConsole (lolIt.getValue());
		}
		void input(int location) {
			if (variableTable.hasVariable (tokenList [location + 1].getValue ())) {
				(new Dialog (tokenList [location + 1].getValue (), variableTable)).Run();
			} else {
				setError ("Error: Variable " + tokenList[location + 1].getValue() + " not declared");
			}
		}
		void literal(int location) {
			if (tokenList [location].getType () == TokenType.STRING_DELIMETER)
				location++;
			lolIt.setValue (tokToLolType [tokenList [location].getType ()], tokenList [location].getValue ());
		}
		void and(int location){
			string result = "";
			currentPosition++;
			actionMap [actionList[currentPosition].type] (actionList[currentPosition].location);
			lolValue x = lolIt.getCopy();

			currentPosition++;
			actionMap [actionList [currentPosition].type] (actionList [currentPosition].location);
			lolValue y = lolIt.getCopy();

			if (x.getValue () == "WIN" && y.getValue () == "WIN") {
				result = "WIN";
			} else
				result = "FAIL";

			MainClass.win.displayTextToConsole (result);
		}

		void or(int location){
			string result = "";
			currentPosition++;
			actionMap [actionList[currentPosition].type] (actionList[currentPosition].location);
			lolValue x = lolIt.getCopy();

			currentPosition++;
			actionMap [actionList [currentPosition].type] (actionList [currentPosition].location);
			lolValue y = lolIt.getCopy();

			if (x.getValue () == "FAIL" && y.getValue () == "FAIL") {
				result = "FAIL";
			} else
				result = "WIN";

			MainClass.win.displayTextToConsole (result);
		}

		void xor(int location){
			string result = "";
			currentPosition++;
			actionMap [actionList[currentPosition].type] (actionList[currentPosition].location);
			lolValue x = lolIt.getCopy();

			currentPosition++;
			actionMap [actionList [currentPosition].type] (actionList [currentPosition].location);
			lolValue y = lolIt.getCopy();

			if (x.getValue () == y.getValue()) {
				result = "FAIL";
			} else
				result = "WIN";

			MainClass.win.displayTextToConsole (result);
		}

		void not(int location){
			string result = "";
			currentPosition++;
			actionMap [actionList[currentPosition].type] (actionList[currentPosition].location);
			lolValue x = lolIt.getCopy();

			if (x.getValue () == "WIN") {
				result = "FAIL";
			} else
				result = "WIN";

			MainClass.win.displayTextToConsole (result);
		}

		void equality(int location){
			string result = "";
			currentPosition++;
			actionMap [actionList[currentPosition].type] (actionList[currentPosition].location);
			lolValue x = lolIt.getCopy();

			currentPosition++;
			actionMap [actionList [currentPosition].type] (actionList [currentPosition].location);
			lolValue y = lolIt.getCopy();

			if (x.getValue () == y.getValue()) {
				result = "WIN";
			} else
				result = "FAIL";

			MainClass.win.displayTextToConsole (result);
		}

		void inequality(int location){
			string result = "";
			currentPosition++;
			actionMap [actionList[currentPosition].type] (actionList[currentPosition].location);
			lolValue x = lolIt.getCopy();

			currentPosition++;
			actionMap [actionList [currentPosition].type] (actionList [currentPosition].location);
			lolValue y = lolIt.getCopy();

			if (x.getValue () != y.getValue()) {
				result = "WIN";
			} else
				result = "FAIL";

			MainClass.win.displayTextToConsole (result);
		}

		void ifThenStart(int location) {
			Console.WriteLine ("LOLIT: ", lolIt.getValue());
		}

		bool isNumberType(lolValue lv) {
			return lv.getType() == LOLType.NUMBR || lv.getType() == LOLType.NUMBAR;
		}

		lolValue implicitCast(lolValue lv, LOLType toType) {
			if(lv.getType() == LOLType.NOOB && toType != LOLType.TROOF) {
				setError("Cannot implicitly cast NOOB to any type except TROOF");
				return null;
			}
			return cast(lv, toType);
		}
		lolValue cast(lolValue lv, LOLType toType) {
			string newValue = "";
			switch(toType) {
				case LOLType.NOOB:
					break;
				case LOLType.NUMBAR:
					switch (lv.getType()) {
						case LOLType.NOOB:
							newValue = "0";
							break;
						case LOLType.NUMBAR:
							newValue = lv.getValue();
							break;
						case LOLType.NUMBR:
							newValue = lv.getValue();
							break;
						case LOLType.TROOF:
							newValue = lv.getValue() == "FAIL"? "0": "1";
							break;
						case LOLType.YARN:
							newValue = decimal.Parse(lv.getValue()).ToString();
							break;
					}
					break;
				case LOLType.NUMBR:
					switch (lv.getType()) {
						case LOLType.NOOB:
							newValue = "0";
							break;
						case LOLType.NUMBAR:
							newValue = Math.Floor(decimal.Parse(lv.getValue())).ToString();
							break;
						case LOLType.NUMBR:
							newValue = lv.getValue();
							break;
						case LOLType.TROOF:
							newValue = lv.getValue() == "FAIL"? "0": "1";
							break;
						case LOLType.YARN:
							newValue = int.Parse(lv.getValue()).ToString();
							break;
					}
					break;
				case LOLType.TROOF:
					switch (lv.getType()) {
						case LOLType.NOOB:
							newValue = "FAIL";
							break;
						case LOLType.NUMBAR:
							newValue = decimal.Parse(lv.getValue()) != 0? "WIN": "FAIL";
							break;
						case LOLType.NUMBR:
							newValue = int.Parse(lv.getValue()) != 0? "WIN": "FAIL";
							break;
						case LOLType.TROOF:
							newValue = lv.getValue();
							break;
						case LOLType.YARN:
							newValue = lv.getValue().Length != 0? "WIN": "FAIL";
							break;
					}
					break;
				case LOLType.YARN:
					newValue = lv.getValue();
					break;
			}
			return new lolValue(toType, newValue);
		}
	}
}

