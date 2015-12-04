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
		CASE,
		GTFO,
		DEFAULT_CASE,
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
		LITERAL,
		INPUT,
		OUTPUT,
		CONCAT,
		AND,
		OR,
		XOR,
		NOT,
		OIC,
		ARITY_AND,
		ARITY_OR,
		CAST_MAEK,
		CAST_IS_NOW_A
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

		#region Object Method
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
			actionMap.Add (Statement_Types.MINIMUM, minimum);
			actionMap.Add (Statement_Types.CONCAT, concat);
			actionMap.Add (Statement_Types.IF_THEN_TRUE, ifTrueBlock);
			actionMap.Add (Statement_Types.IF_THEN_FALSE, ifFalseBlock);
			actionMap.Add (Statement_Types.OIC, oic);
			actionMap.Add (Statement_Types.SWITCH, switchBlock);
			actionMap.Add (Statement_Types.CASE, caseBlock);
			actionMap.Add (Statement_Types.DEFAULT_CASE, defaultBlock);
			actionMap.Add (Statement_Types.GTFO, gtfo);
			actionMap.Add (Statement_Types.ARITY_AND, arityAnd);
			actionMap.Add (Statement_Types.ARITY_OR, arityOr);
			actionMap.Add (Statement_Types.CAST_MAEK, castMaek);
			actionMap.Add (Statement_Types.CAST_IS_NOW_A, castIsNowA);
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
		#endregion

		#region Variable Operations
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
			//Execute expressions first
			currentPosition++;
			actionMap [actionList[currentPosition].type] (actionList[currentPosition].location);

			if (variableTable.hasVariable (tokenList [location].getValue ())) {
				variableTable.setVar (tokenList [location].getValue (), 
					lolIt.getType(),
					lolIt.getValue ());
			} else {
				setError ("Error: Variable " + tokenList[location].getValue() + "is not yet declared");
			}
			MainClass.win.refreshSymbol (variableTable);
		}
		#endregion

		#region MATH OPERATORS
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
				add1 = implicitCast (add1, LOLType.NUMBAR);
				add2 = implicitCast (add2, LOLType.NUMBAR);
				sum = decimal.Parse(add1.getValue ()) + decimal.Parse (add2.getValue ());
				sumType = LOLType.NUMBAR;
			} else {
				add1 = implicitCast (add1, LOLType.NUMBR);
				add2 = implicitCast (add2, LOLType.NUMBR);
				sum = int.Parse (add1.getValue ()) + int.Parse (add2.getValue ());
			}

			lolIt.setValue (sumType, sum.ToString());
			//MainClass.win.displayTextToConsole (""+sum);
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
			//MainClass.win.displayTextToConsole (""+diff);
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
			//MainClass.win.displayTextToConsole (""+prod);
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
				if (d1.getType () == LOLType.NUMBAR || d2.getType () == LOLType.NUMBAR) {
					quo = decimal.Parse (d1.getValue ()) / decimal.Parse (d2.getValue ());
					sumType = LOLType.NUMBAR;
				}else {
					quo = int.Parse (d1.getValue ()) / int.Parse (d2.getValue ());
					sumType = LOLType.NUMBAR;
				}

				lolIt.setValue (sumType, quo.ToString ());
				//MainClass.win.displayTextToConsole ("" + quo);
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
			currentPosition++;
			actionMap [actionList[currentPosition].type] (actionList[currentPosition].location);
			lolValue num1 = lolIt.getCopy();

			currentPosition++;
			actionMap [actionList [currentPosition].type] (actionList [currentPosition].location);
			lolValue num2 = lolIt.getCopy();

			if(decimal.Parse (implicitCast(num1, LOLType.NUMBAR).getValue ()) > decimal.Parse (implicitCast(num2, LOLType.NUMBAR).getValue ())){
				lolIt = num1.getCopy();
			}
			else {
				lolIt = num2.getCopy();
			}		
		}

		void minimum(int location){
			currentPosition++;
			actionMap [actionList[currentPosition].type] (actionList[currentPosition].location);
			lolValue num1 = lolIt.getCopy();

			currentPosition++;
			actionMap [actionList [currentPosition].type] (actionList [currentPosition].location);
			lolValue num2 = lolIt.getCopy();

			if(decimal.Parse (implicitCast(num1, LOLType.NUMBAR).getValue ()) > decimal.Parse (implicitCast(num2, LOLType.NUMBAR).getValue ())){
				lolIt = num2.getCopy();
			}
			else {
				lolIt = num1.getCopy();
			}		
		}
		#endregion

		#region Boolean Operations

		void and(int location){
			string result = "";
			currentPosition++;
			actionMap [actionList[currentPosition].type] (actionList[currentPosition].location);
			lolValue x = lolIt.getCopy();
			x = implicitCast (x, LOLType.TROOF);

			currentPosition++;
			actionMap [actionList [currentPosition].type] (actionList [currentPosition].location);
			lolValue y = lolIt.getCopy();
			y = implicitCast (y, LOLType.TROOF);

			if (x.getValue () == "WIN" && y.getValue () == "WIN") {
				result = "WIN";
			} else
				result = "FAIL";

			lolIt.setValue (LOLType.TROOF, result);
			//	MainClass.win.displayTextToConsole (result);
		}

		void or(int location){
			string result = "";
			currentPosition++;
			actionMap [actionList[currentPosition].type] (actionList[currentPosition].location);
			lolValue x = lolIt.getCopy();
			x = implicitCast (x, LOLType.TROOF);

			currentPosition++;
			actionMap [actionList [currentPosition].type] (actionList [currentPosition].location);
			lolValue y = lolIt.getCopy();
			y = implicitCast (y, LOLType.TROOF);

			if (x.getValue () == "FAIL" && y.getValue () == "FAIL") {
				result = "FAIL";
			} else
				result = "WIN";

			lolIt.setValue (LOLType.TROOF, result);
			//MainClass.win.displayTextToConsole (result);
		}

		void xor(int location){
			string result = "";
			currentPosition++;
			actionMap [actionList[currentPosition].type] (actionList[currentPosition].location);
			lolValue x = lolIt.getCopy();
			x = implicitCast (x, LOLType.TROOF);

			currentPosition++;
			actionMap [actionList [currentPosition].type] (actionList [currentPosition].location);
			lolValue y = lolIt.getCopy();
			y = implicitCast (y, LOLType.TROOF);

			if (x.getValue () == y.getValue()) {
				result = "FAIL";
			} else
				result = "WIN";

			lolIt.setValue (LOLType.TROOF, result);
			//MainClass.win.displayTextToConsole (result);
		}

		void not(int location){
			string result = "";
			currentPosition++;
			actionMap [actionList[currentPosition].type] (actionList[currentPosition].location);
			lolValue x = lolIt.getCopy();
			x = implicitCast (x, LOLType.TROOF);

			if (x.getValue () == "WIN") {
				result = "FAIL";
			} else
				result = "WIN";

			lolIt.setValue (LOLType.TROOF, result);
			//	MainClass.win.displayTextToConsole (result);
		}

		void equality(int location){
			string result = "";
			currentPosition++;
			actionMap [actionList[currentPosition].type] (actionList[currentPosition].location);
			lolValue x = lolIt.getCopy();

			currentPosition++;
			actionMap [actionList [currentPosition].type] (actionList [currentPosition].location);
			lolValue y = lolIt.getCopy();

			if (x.getValue () == y.getValue() && (isNumberType(x) && isNumberType(y) || x.getType() == y.getType())) {
				result = "WIN";
			} else
				result = "FAIL";

			lolIt.setValue (LOLType.TROOF, result);
			//	MainClass.win.displayTextToConsole (result);
		}

		void inequality(int location){
			string result = "";
			currentPosition++;
			actionMap [actionList[currentPosition].type] (actionList[currentPosition].location);
			lolValue x = lolIt.getCopy();

			currentPosition++;
			actionMap [actionList [currentPosition].type] (actionList [currentPosition].location);
			lolValue y = lolIt.getCopy();

			if (x.getValue () == y.getValue() && (isNumberType(x) && isNumberType(y) || x.getType() == y.getType())) {
				result = "FAIL";
			} else
				result = "WIN";

			lolIt.setValue (LOLType.TROOF, result);
			//	MainClass.win.displayTextToConsole (result);
		}

		void arityAnd(int location){
			string result = "";
			lolValue x;
			lolValue y;

			int locationEnd = goToMkay (location+1);

			currentPosition++;
			actionMap [actionList[currentPosition].type] (actionList[currentPosition].location);
			x = lolIt.getCopy();
			x = implicitCast (x, LOLType.TROOF);
			result = x.getValue();


			while(currentPosition < actionList.Count - 1 && actionList[currentPosition + 1].location < locationEnd){
				currentPosition++;
				actionMap [actionList [currentPosition].type] (actionList [currentPosition].location);
				y = lolIt.getCopy ();
				y = implicitCast (y, LOLType.TROOF);

				if (result == "WIN" && y.getValue () == "WIN") {
					result = "WIN";
				} else
					result = "FAIL";
			}
		
			lolIt.setValue (LOLType.TROOF, result);
		}

		int goToMkay(int location) {
			int stack = 0;

			while(true){
				if (stack == 0 && tokenList [location].getType () == TokenType.MKAY)
					break;
				if (tokenList [location].getType () == TokenType.ANY_OF || tokenList [location].getType () == TokenType.ALL_OF || tokenList [location].getType () == TokenType.SMOOSH)
					stack++;
				if (tokenList [location].getType () == TokenType.MKAY) 
					stack --;
				location++;
			}

			return location;
		}

		void arityOr(int location){
			string result = "";
			lolValue x;
			lolValue y;

			int locationEnd = goToMkay (location+1);

			currentPosition++;
			actionMap [actionList[currentPosition].type] (actionList[currentPosition].location);
			x = lolIt.getCopy();
			x = implicitCast (x, LOLType.TROOF);
			result = x.getValue();


			while(currentPosition < actionList.Count - 1 && actionList[currentPosition + 1].location < locationEnd){
				currentPosition++;
				actionMap [actionList [currentPosition].type] (actionList [currentPosition].location);
				y = lolIt.getCopy ();
				y = implicitCast (y, LOLType.TROOF);

				if (result == "FAIL" && y.getValue () == "FAIL") {
					result = "FAIL";
				} else
					result = "WIN";
			}

			lolIt.setValue (LOLType.TROOF, result);
		}
		#endregion

		#region I/O
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
		#endregion

		#region Expressions
		void variableIdentifier(int location) {
			if (variableTable.hasVariable (tokenList [location].getValue ())) {
				lolValue val = variableTable.getVar (tokenList [location].getValue ());
				lolIt.setValue (val.getType (), val.getValue ());
			} else {
				setError ("Error: Variable " + tokenList[location].getValue() + " not declared.");
			}
		}

		void literal(int location) {
			if (tokenList [location].getType () == TokenType.STRING_DELIMETER)
				location++;
			lolIt.setValue (tokToLolType [tokenList [location].getType ()], tokenList [location].getValue ());
		}

		void concat(int location){
			string result = "";
			lolValue x;
			lolValue y;

			int locationEnd = location + 1;
			while (locationEnd < tokenList.Count - 1 && tokenList[locationEnd].getType() != TokenType.STATEMENT_DELIMETER)
				locationEnd++;

			currentPosition++;
			actionMap [actionList[currentPosition].type] (actionList[currentPosition].location);
			x = lolIt.getCopy();
			x = implicitCast (x, LOLType.YARN);
			result = x.getValue();


			while(currentPosition < actionList.Count - 1 && actionList[currentPosition + 1].location < locationEnd){
				currentPosition++;
				actionMap [actionList [currentPosition].type] (actionList [currentPosition].location);
				y = lolIt.getCopy ();
				y = implicitCast (y, LOLType.YARN);

				result += y.getValue();
			}

			lolIt.setValue (LOLType.YARN, result);
		}

		void castMaek(int location){	
			Console.WriteLine ("Casting...");

			currentPosition++;
			actionMap [actionList[currentPosition].type] (actionList[currentPosition].location);
			lolValue x = lolIt.getCopy();
		
			Console.WriteLine (x.getValue() + " is a " + x.getType());
				
			location+=3;
			switch (tokenList [location].getValue ()) {
				case "YARN":
				 	x = cast (x, LOLType.YARN);
					break;
				case "NUMBR":
					x = cast (x, LOLType.NUMBR);
					break;
				case "NUMBAR":
					x = cast (x, LOLType.NUMBAR);
					break;
				case "TROOF":
					x = cast (x, LOLType.TROOF);
					break;
				case "NOOB":
					x = cast (x, LOLType.NOOB);
					break;
			}
	
			lolIt.setValue(x.getType(), x.getValue());	
			MainClass.win.refreshSymbol (variableTable);
			Console.WriteLine (x.getValue() + " is now a " + x.getType());
		}

		void castIsNowA(int location){	
			Console.WriteLine ("Casting...");
			lolValue x = null;

			location+=2;
			switch (tokenList [location].getValue ()) {
				case "YARN":
				x = cast (lolIt, LOLType.YARN);
				break;
				case "NUMBR":
				x = cast (lolIt, LOLType.NUMBR);
				break;
				case "NUMBAR":
				x = cast (lolIt, LOLType.NUMBAR);
				break;
				case "TROOF":
				x = cast (lolIt, LOLType.TROOF);
				break;
				case "NOOB":
				x = cast (lolIt, LOLType.NOOB);
				break;
			}

			lolIt.setValue(x.getType(), x.getValue());	
			MainClass.win.refreshSymbol (variableTable);
			Console.WriteLine (x.getValue() + " is now a " + x.getType());
		}
		#endregion

		#region Conditional
		void ifThenStart(int location) {
			Console.WriteLine ("LOLIT: " + lolIt.getValue());
			if (implicitCast (lolIt, LOLType.TROOF).getValue () == "WIN") {
				currentPosition++;
				Console.WriteLine ("if true first statement: " + actionList [currentPosition + 1].type + ", " + actionList [currentPosition].location );
				actionMap [actionList [currentPosition].type] (actionList [currentPosition].location);
			} else {
				currentPosition += 2;
				goToNextIfCondition ();
				Console.WriteLine ("Should be if false: " + actionList [currentPosition].type + ", " + actionList [currentPosition].location );
				Console.WriteLine ("if false first statement: " + actionList [currentPosition + 1].type + ", " + actionList [currentPosition].location );
				actionMap [actionList [currentPosition].type] (actionList [currentPosition].location);
				if (actionList [currentPosition].type == Statement_Types.OIC) {
					return;
				}
			}
			Console.WriteLine ("ifThenStart: Current Statement: " + actionList [currentPosition].type + ", " + actionList [currentPosition].location );
			if(currentPosition + 1 < actionList.Count)
				currentPosition++;
			goToOIC ();
			Console.WriteLine ("ifThenStart: After OIC: " + actionList [currentPosition].type + ", " + actionList [currentPosition].location );
			Console.Write (actionList [currentPosition].type);;
		}

		void goToOIC() {
			int newBlock = 0;

			while (currentPosition < actionList.Count - 1) {
				Console.WriteLine("goToOIC checking statement: "  + actionList [currentPosition].type + ", " + actionList [currentPosition].location );
				switch (actionList [currentPosition].type) {
				case Statement_Types.SWITCH:
				case Statement_Types.IF_THEN_START:
					newBlock++;
					break;
				case Statement_Types.OIC:
					if (newBlock != 0) {
						newBlock--;
						break;
					}
					return;
				}
				currentPosition++;
			}
		}
		void goToNextIfCondition() {
			int newBlock = 0;

			while (currentPosition < actionList.Count) {
				currentPosition++;
				Console.WriteLine ("Checking statement: " + actionList [currentPosition].type + ", " + actionList [currentPosition].location );
				switch (actionList [currentPosition].type) {
				case Statement_Types.SWITCH:
				case Statement_Types.IF_THEN_START:
					newBlock++;
					break;
				case Statement_Types.OIC:
					if (newBlock != 0) {
						newBlock--;
						break;
					}
					return;
				case Statement_Types.IF_THEN_FALSE:
				case Statement_Types.ELSE_IF:
					if (newBlock != 0) {
						continue;
					}
					return;
				}
			}
		}

		void ifTrueBlock(int location) {
			Console.WriteLine ("Inside if true");

			int savePosition = currentPosition;
			goToNextIfCondition ();
			Console.WriteLine ("ifTrueBlock: Should be next condition: " + actionList [currentPosition].type);
			int lastPosition = currentPosition;
			currentPosition = savePosition;

			while (currentPosition + 1 < lastPosition) {
				currentPosition++;

				Console.WriteLine ("ifTrueBlock Executing: " + actionList [currentPosition].type + ", " + actionList [currentPosition].location );
				actionMap [actionList [currentPosition].type] (actionList [currentPosition].location);
			}
			Console.WriteLine ("ifTrueBlock next statement: " + actionList [currentPosition + 1].type + ", " + actionList [currentPosition + 1].location);
		}

		void ifFalseBlock(int location) {
			Console.WriteLine ("Inside false");

			int savePosition = currentPosition;
			goToNextIfCondition ();
			int lastPosition = currentPosition;
			currentPosition = savePosition;

			while (currentPosition < lastPosition) {
				currentPosition++;
				actionMap [actionList [currentPosition].type] (actionList [currentPosition].location);
			}
		}

		void oic(int location) {
		}

		void goToNextSwitchCondition() {
			int newBlock = 0;

			while (currentPosition < actionList.Count - 1) {
				currentPosition++;
				switch (actionList [currentPosition].type) {
				case Statement_Types.SWITCH:
				case Statement_Types.IF_THEN_START:
					newBlock++;
					break;
				case Statement_Types.OIC:
					if (newBlock != 0) {
						newBlock--;
						break;
					}
					return;
				case Statement_Types.CASE:
				case Statement_Types.DEFAULT_CASE:
					if (newBlock != 0) {
						continue;
					}
					return;
				}
			}
		}

		void switchBlock(int location) {
			lolValue testCase = lolIt.getCopy();
			currentPosition++;

			while (currentPosition < actionList.Count && actionList [currentPosition].type != Statement_Types.OIC) {
				if (actionList [currentPosition].type == Statement_Types.CASE) {
					currentPosition++;
					actionMap [actionList [currentPosition].type] (actionList [currentPosition].location);
					if (testCase.getValue () == lolIt.getValue ()) {
						executeCase ();
						goToOIC ();
						break;
					}
				} else {
					executeCase ();
					goToOIC ();
					break;
				}
				goToNextSwitchCondition ();
			}
			currentPosition++;
		}

		void executeCase() {
			int savePosition = currentPosition;
			goToNextSwitchCondition ();
			int lastPosition = currentPosition;
			currentPosition = savePosition;
			while (currentPosition < lastPosition) {
				currentPosition++;

				while (currentPosition < actionList.Count - 2 && actionList [currentPosition].type == Statement_Types.CASE) {
					currentPosition += 2;
				}
				if (actionList [currentPosition].type == Statement_Types.DEFAULT_CASE) {
					currentPosition++;
				}
				actionMap [actionList [currentPosition].type] (actionList [currentPosition].location);
			}
		}

		void caseBlock(int location) {
		}

		void defaultBlock(int location) {
		}

		void gtfo(int location) {
			goToOIC ();
		}
		#endregion

		#region Helper functions

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
		#endregion
	}
}

