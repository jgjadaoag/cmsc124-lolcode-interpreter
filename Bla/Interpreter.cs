using System;
using System.Collections.Generic;

namespace Bla
{
	public enum Statement_Types {
		VARIABLE_DECLARATION_ITZ, //I HAS A * ITZ
		VARIABLE_DECLARATION,// I HAS A
		VARIABLE_ASSIGNMENT,
		IF_THEN_ELSE, //O RLY? YA RLY, NO WAI..
		IF_THEN, //O RLY?
		SWITCH, //WTF?
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
		CONCAT
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
				if (t.getType() != TokenType.UNKNOWN && (t.getType() != TokenType.BTW && t.getType() != TokenType.OBTW)) {
					tokenList.Add (t);
				}
			}
			Parser p = new Parser (tokenList);
			p.parse ();

			tokToLolType = new Dictionary<TokenType, LOLType> ();

			tokToLolType.Add (TokenType.NUMBR_LITERAL, LOLType.NUMBR);
			tokToLolType.Add (TokenType.NUMBAR_LITERAL, LOLType.NUMBAR);
			tokToLolType.Add (TokenType.TROOF_LITERAL, LOLType.TROOF);
			tokToLolType.Add (TokenType.YARN_LITERAL, LOLType.YARN);

			addActionDefinitions ();

			actionList = p.getActionOrder ();
		}

		public void runProgram() {
			for (currentPosition = 0; currentPosition < actionList.Count && !errorFlag; currentPosition++) {
				actionMap [actionList[currentPosition].type] (actionList[currentPosition].location);
				Console.WriteLine (actionList[currentPosition].type);
			}
			if (errorFlag) {
				MainClass.win.displayTextToConsole (errorMessage);
			}
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
		}

		void setError(string message) {
			errorFlag = true;
			errorMessage = message;
		}

		void variableDeclarationItz(int location) {
			//Execute expressions first
			actionMap [actionList[currentPosition + 1].type] (actionList[currentPosition + 1].location);

			if (!variableTable.hasVariable (tokenList [location + 1].getValue ())) {
				variableTable.createVar (tokenList [location + 1].getValue (), 
					lolIt.getType(),
					lolIt.getValue ());
			} else {
				setError ("Error: Variable " + tokenList[location + 1].getValue() + " already declared");
			}
			MainClass.win.refreshSymbol (variableTable);
			currentPosition++;
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
			float sum = 0;
			currentPosition++;
			actionMap [actionList[currentPosition].type] (actionList[currentPosition].location);
			lolValue add1 = lolIt.getCopy();

			currentPosition++;
			actionMap [actionList [currentPosition].type] (actionList [currentPosition].location);
			lolValue add2 = lolIt.getCopy();

			LOLType sumType = LOLType.NUMBR;

			if (add1.getType () == LOLType.NUMBAR || add2.getType () == LOLType.NUMBAR) {
				sum = float.Parse(add1.getValue ()) + float.Parse (add2.getValue ());
				sumType = LOLType.NUMBAR;
			} else {
				sum = int.Parse (add1.getValue ()) + int.Parse (add2.getValue ());
			}

			lolIt.setValue (sumType, sum.ToString());
			MainClass.win.displayTextToConsole (""+sum);
		}

		void subtraction(int location){
			float diff = 0;
			currentPosition++;
			actionMap [actionList[currentPosition].type] (actionList[currentPosition].location);
			lolValue d1 = lolIt.getCopy();

			currentPosition++;
			actionMap [actionList [currentPosition].type] (actionList [currentPosition].location);
			lolValue d2 = lolIt.getCopy();

			LOLType sumType = LOLType.NUMBR;

			if (d1.getType () == LOLType.NUMBAR || d2.getType () == LOLType.NUMBAR) {
				diff = float.Parse(d1.getValue ()) - float.Parse (d2.getValue ());
				sumType = LOLType.NUMBAR;
			} else {
				diff = int.Parse (d1.getValue ()) - int.Parse (d2.getValue ());
			}

			lolIt.setValue (sumType, diff.ToString());
			MainClass.win.displayTextToConsole (""+diff);
		}

		void multiplication(int location){
			float prod = 0;
			currentPosition++;
			actionMap [actionList[currentPosition].type] (actionList[currentPosition].location);
			lolValue d1 = lolIt.getCopy();

			currentPosition++;
			actionMap [actionList [currentPosition].type] (actionList [currentPosition].location);
			lolValue d2 = lolIt.getCopy();

			LOLType sumType = LOLType.NUMBR;

			if (d1.getType () == LOLType.NUMBAR || d2.getType () == LOLType.NUMBAR) {
				prod = float.Parse(d1.getValue ()) * float.Parse (d2.getValue ());
				sumType = LOLType.NUMBAR;
			} else {
				prod = int.Parse (d1.getValue ()) * int.Parse (d2.getValue ());
			}

			lolIt.setValue (sumType, prod.ToString());
			MainClass.win.displayTextToConsole (""+prod);
		}

		void division(int location){
			float quo = 0;
			currentPosition++;
			actionMap [actionList[currentPosition].type] (actionList[currentPosition].location);
			lolValue d1 = lolIt.getCopy();

			currentPosition++;
			actionMap [actionList [currentPosition].type] (actionList [currentPosition].location);
			lolValue d2 = lolIt.getCopy();

			LOLType sumType = LOLType.NUMBR;

			if (float.Parse (d2.getValue ()) != 0) {
				
				quo = float.Parse (d1.getValue ()) / float.Parse (d2.getValue ());
				sumType = LOLType.NUMBAR;

				lolIt.setValue (sumType, quo.ToString ());
				MainClass.win.displayTextToConsole ("" + quo);
			} else {
				MainClass.win.displayTextToConsole ("Invalid operation: Division by zero");
			} 
				
		}

		void output(int location) {
			//Execute expressions first
			actionMap [actionList[currentPosition + 1].type] (actionList[currentPosition + 1].location);
			currentPosition++;
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
	}
}

