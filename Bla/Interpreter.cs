using System;
using System.Media;
using System.Text.RegularExpressions;
using System.Globalization;
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
		FUNCTION_DEFINITION,
		FUNCTION_END,
		FUNCTION_CALL,
		FUNCTION_RETURN,
		CAST_MAEK,
		CAST_IS_NOW_A,
		LOOP_START,
		LOOP_END
	}

	public class lolStatement {
		public readonly Statement_Types type;
		public readonly int location;
		public lolStatement(Statement_Types st, int loc) {
			this.type = st;
			this.location = loc;
		}

		public void printDetails() {
			Console.WriteLine("Type: " + type + ", " + "Location: " + location);
		}
	}
	public class Interpreter
	{
		class LolIt : lolValue
		{
			SymbolTable variableTable;
			public LolIt(LOLType t, string v, SymbolTable st) : base(t, v) {
				variableTable = st;
			}

			public new void setValue(lolValue lv) {
				base.setValue (lv);
				variableTable.setVar ("IT", lv.getType (), lv.getValue ());
			}

			public new void setValue(LOLType t, string v) {
				base.setValue (t, v);
				variableTable.setVar ("IT", t, v);
			}
		}

		delegate void lolAction(int location);

		List <Token> tokenList;
		List <lolStatement> actionList;

		Dictionary <Statement_Types, lolAction> actionMap;
		Dictionary <TokenType, LOLType> tokToLolType;

		SymbolTable variableTable;
		FunctionTable functionTable;
		LolIt lolIt;

		int currentPosition;
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
			functionTable = new FunctionTable();
			lolIt = new LolIt (LOLType.NOOB, "", variableTable);
			variableTable.createVar ("IT", lolIt);

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
			actionMap.Add (Statement_Types.FUNCTION_DEFINITION, functionDefinition);
			actionMap.Add (Statement_Types.FUNCTION_CALL, functionCall);
			actionMap.Add (Statement_Types.FUNCTION_END, functionEnd);
			actionMap.Add (Statement_Types.FUNCTION_RETURN, functionReturn);
			actionMap.Add (Statement_Types.CAST_MAEK, castMaek);
			actionMap.Add (Statement_Types.CAST_IS_NOW_A, castIsNowA);
			actionMap.Add (Statement_Types.LOOP_START, loopBlock);
			actionMap.Add (Statement_Types.LOOP_END, loopEnd);
			actionMap.Add (Statement_Types.ELSE_IF, elseIf);
		}

		public void runProgram() {
			Console.WriteLine ("=============================================");
			Console.WriteLine ("=ACTION=LIST================================");
			Console.WriteLine ("=============================================");
			foreach (lolStatement ls in actionList) {
				Console.WriteLine (ls.type.ToString() + ", " + tokenList[ls.location].getValue());
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
			throw new ApplicationException (errorMessage);
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
				lolIt.setValue(num1);
			}
			else {
				lolIt.setValue(num2);
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
				lolIt.setValue(num2);
			}
			else {
				lolIt.setValue(num1);
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


			while(currentPosition < actionList.Count - 1 && actionList[currentPosition].location < locationEnd){
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
				Console.WriteLine("goToMkay: " + tokenList [location].getType ());
				if (stack == 0 && tokenList [location].getType () == TokenType.MKAY)
					break;
				if (tokenList [location].getType () == TokenType.ANY_OF || tokenList [location].getType () == TokenType.ALL_OF || 
						tokenList [location].getType () == TokenType.SMOOSH || tokenList [location].getType () == TokenType.I_IZ)
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
				Console.WriteLine((actionList [currentPosition].location));
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
			/*
			currentPosition++;
			actionMap [actionList[currentPosition].type] (actionList[currentPosition].location);
			*/
			concat(location);
			while(location < tokenList.Count - 1 && tokenList[location].getType() != TokenType.EXCLAMATION && tokenList[location].getType() != TokenType.STATEMENT_DELIMETER) {
				location++;
			}
			if(tokenList[location].getType() == TokenType.EXCLAMATION) {
				MainClass.win.displayTextToConsoleNoLine (parseString(lolIt.getValue()));
			} else {
				MainClass.win.displayTextToConsole (parseString(lolIt.getValue()));
			}
		}

		string parseString(string str){
			string holder = "";

			for (int count = 0; count < str.Length; count++) {
				if (str [count] == ':') {
					switch (str [count + 1]) {
						case ')':
							holder += "\n";
							count = count + 1;
							break;
						case '>':
							holder += "\t";
							count = count + 1;
							break;
						case 'o':
							SystemSounds.Beep.Play ();
							count = count + 1;
							break;
						case '"':
							holder += "\"";
							count = count + 1;
							break;
						case ':':
							holder += ":";
							count = count + 1;
							break;
						//default: holder += str[count];
					}
				} else {
					holder += str [count];
				}
			}
			return holder;
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
		
			location+=3;
			if (tokenList [location].getValue () == "\"") {
				location += 2;
			}

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
			if (implicitCast (lolIt, LOLType.TROOF).getValue () == "WIN") {
				currentPosition++;
				actionMap [actionList [currentPosition].type] (actionList [currentPosition].location);
			} else {
				currentPosition += 2;
				while(actionList [currentPosition].type != Statement_Types.OIC) {
					goToNextIfCondition ();
					actionMap [actionList [currentPosition].type] (actionList [currentPosition].location);
				}
				if (actionList [currentPosition].type == Statement_Types.OIC) {
					return;
				}
			}
			if(currentPosition + 1 < actionList.Count)
				currentPosition++;
			goToOIC ();
		}

		void elseIf(int location) {
			currentPosition++;
			actionMap [actionList [currentPosition].type] (actionList [currentPosition].location);
			if(implicitCast(lolIt, LOLType.TROOF).getValue() == "FAIL") {
				return;
			}
			
			int savePosition = currentPosition;
			goToNextIfCondition ();
			int lastPosition = currentPosition;
			currentPosition = savePosition;

			while (currentPosition + 1 < lastPosition) {
				currentPosition++;
				actionMap [actionList [currentPosition].type] (actionList [currentPosition].location);
			}
			goToOIC();
		}

		void goToOIC() {
			int newBlock = 0;

			while (currentPosition < actionList.Count - 1) {
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

		void goToEndBlock() {
			Stack<TokenType> blocks = new Stack<TokenType> ();

			while (currentPosition < actionList.Count - 1) {
				currentPosition++;
				switch (actionList [currentPosition].type) {
				case Statement_Types.SWITCH:
				case Statement_Types.IF_THEN_START:
					blocks.Push (TokenType.OIC);
					break;
				case Statement_Types.FUNCTION_DEFINITION:
					blocks.Push (TokenType.IF_U_SAY_SO);
					break;
				case Statement_Types.OIC:
					if (blocks.Count != 0 && blocks.Peek() == TokenType.OIC) {
						blocks.Pop();
						break;
					}
					return;
				case Statement_Types.FUNCTION_END:
					if (blocks.Count != 0 && blocks.Peek() == TokenType.IF_U_SAY_SO) {
						blocks.Pop();
						break;
					}
					return;
				}
			}
		}

		void goToNextIfCondition() {
			int newBlock = 0;

			while (currentPosition < actionList.Count) {
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

			int savePosition = currentPosition;
			goToNextIfCondition ();
			int lastPosition = currentPosition;
			currentPosition = savePosition;

			while (currentPosition + 1 < lastPosition) {
				currentPosition++;

				actionMap [actionList [currentPosition].type] (actionList [currentPosition].location);
			}
		}

		void ifFalseBlock(int location) {

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
			Console.Write("switchBlock: ");
			actionList [currentPosition].printDetails();
		}

		void executeCase() {
			int savePosition = currentPosition;
			goToOIC ();
			Console.WriteLine("executeCase goToOIC: " + actionList [currentPosition].type + ", " + actionList [currentPosition].location);
			int lastPosition = currentPosition;
			currentPosition = savePosition;
			Console.WriteLine("lastPosition: " + lastPosition);
			while (currentPosition < lastPosition) {
				Console.WriteLine("currentPosition: " + currentPosition);
				currentPosition++;

				while (currentPosition < actionList.Count - 2 && actionList [currentPosition].type == Statement_Types.CASE) {
					currentPosition += 2;
				}
				if (actionList [currentPosition].type == Statement_Types.DEFAULT_CASE) {
					currentPosition++;
				}
				Console.WriteLine("Switch is Executing: " + actionList [currentPosition].type + ", " + currentPosition);
				actionMap [actionList [currentPosition].type] (actionList [currentPosition].location);
			}
		}

		void caseBlock(int location) {
		}

		void defaultBlock(int location) {
		}

		void gtfo(int location) {
			lolIt.setValue (LOLType.NOOB, "");
			int newBlock = 0;

			//Assumes parser is correct and that every opening block is closed
			while (currentPosition < actionList.Count - 1) {
				Console.Write("gtfo: ");
				actionList [currentPosition].printDetails();

				switch (actionList [currentPosition].type) {
				case Statement_Types.SWITCH:
				case Statement_Types.IF_THEN_START:
				case Statement_Types.FUNCTION_DEFINITION:
				case Statement_Types.LOOP_START:
					newBlock++;
					break;
				case Statement_Types.OIC:
				case Statement_Types.LOOP_END:
				case Statement_Types.FUNCTION_END:
					if (newBlock != 0) {
						newBlock--;
						break;
					}
					return;
				}
				currentPosition++;
			}
		}
		#endregion

		#region Function
		int goToLineDelimiter(int location) {
			while(location < tokenList.Count - 1 && tokenList[location].getType() != TokenType.STATEMENT_DELIMETER) {
				location++;
			}
			return location;
		}


		void functionDefinition(int location) {
			int start = currentPosition;
			string name = tokenList[location + 1].getValue();
			Console.WriteLine ("functionDefinition: " + tokenList[location].getType());

			List<string> parameters = readFormalParameters (location);
			goToEndBlock ();

			if(!functionTable.addFunction(tokenList[location + 1].getValue(), new LolFunction(start, currentPosition, parameters))){
				setError("Function is already defined");
				return;
			}
		}

		//accepts location of HOW_IZ_I token
		List<string> readFormalParameters(int location) {
			List<string> parameterList = new List<string>();
			HashSet<string> parameterSet = new HashSet<string>();

			if(tokenList[location + 2].getType() == TokenType.STATEMENT_DELIMETER) {
				Console.WriteLine("Empty Parameter");
				return parameterList;
			}

			int endLocation = goToLineDelimiter(location);
			location += 3;
			while(location < endLocation) {
				if(parameterSet.Add(tokenList[location].getValue())) {
					parameterList.Add(tokenList[location].getValue());
				} else {
					setError("Parameters with same names");
					return null;
				}

				location += 3;
			}

			return parameterList;
			
		}
		void functionEnd(int location) {
		}

		void functionCall(int location) {
			LolFunction fun = functionTable.getFunction(tokenList[location + 1].getValue());
			if(fun == null) {
				setError("Unknown function");
				return;
			}

			int savePosition = currentPosition;
			SymbolTable saveVariable = variableTable;
			FunctionTable saveFunction = functionTable;

			List<lolValue> paramList = getActualParam(location);
			if(fun.parameters.Count != paramList.Count) {
				setError("Wrong number of parameters");
				return;
			}

			variableTable = new SymbolTable ();
			functionTable = new FunctionTable();
			for(int iii = 0; iii < paramList.Count; iii++) {
				variableTable.createVar(fun.parameters[iii], paramList[iii]) ;
			}

			currentPosition = fun.startLocation + 1;

			while(currentPosition < fun.endLocation) {
				actionMap [actionList[currentPosition].type] (actionList[currentPosition].location);
				currentPosition++;
			}

			currentPosition = savePosition;
			variableTable = saveVariable;
			functionTable = saveFunction;
			MainClass.win.refreshSymbol(variableTable);
		}

		List<lolValue> getActualParam(int location) {
			List<lolValue> paramList = new List<lolValue>();

			int locationEnd = goToMkay(location + 1);

			while(currentPosition < actionList.Count - 1 && actionList[currentPosition + 1].location < locationEnd){
				currentPosition++;
				actionMap [actionList [currentPosition].type] (actionList [currentPosition].location);

				paramList.Add(lolIt.getCopy());
			}

			Console.WriteLine("getActualParam: " + tokenList[locationEnd].getType());

			return paramList;
		}

		void functionReturn(int location) {
			currentPosition++;
			actionMap [actionList[currentPosition].type] (actionList[currentPosition].location);
			lolValue lv = lolIt.getCopy();
			gtfo(location);
			lolIt.setValue(lv);
		}

		#endregion
		#region Loop
		void goToLoopEnd() {
			int newBlock = 0;

			while(currentPosition < actionList.Count - 1) {
				switch (actionList [currentPosition].type) {
				case Statement_Types.LOOP_START:
					newBlock++;
					break;
				case Statement_Types.LOOP_END:
					if (newBlock != 0) {
						newBlock--;
						break;
					}
					return;
				}
				currentPosition++;
			}
		}
		void loopBlock(int location) {
			Console.WriteLine("Inside loopBlock");
			int savePosition = currentPosition;
			currentPosition++;
			gtfo(location);
			int lastPosition = currentPosition - 1;
			currentPosition = savePosition;
			Console.Write("loopBlock: ");
			actionList [savePosition + 1].printDetails();
			Console.Write("loopBlock: ");
			actionList [lastPosition].printDetails();
			Console.WriteLine("loopBlock: " + tokenList[location + 2].getValue());
			Console.WriteLine("loopBlock: " + tokenList[location + 4].getValue());
			Console.WriteLine("loopBlock: " + tokenList[location + 5].getValue());


			bool til = tokenList[location + 5].getType() == TokenType.TIL;
			bool broken = false;
			while(!broken && condition(savePosition, til)) {
				while(currentPosition < lastPosition ) {
					if(actionList[currentPosition].type == Statement_Types.GTFO) {
						broken = true;
						Console.WriteLine("BROKEN");
						break;
					}
					actionMap [actionList[currentPosition].type] (actionList[currentPosition].location);
					Console.Write("loopBlock loop: ");
					actionList [currentPosition].printDetails();
					currentPosition++;
				}
				update(location);
			}
			currentPosition = lastPosition;

		}

		bool condition(int savePosition, bool til) {
			currentPosition = savePosition + 1;
			actionMap[actionList[currentPosition].type] (actionList[currentPosition].location);
			if(til) {
				return implicitCast(lolIt, LOLType.TROOF).getValue() == "FAIL";
			} else {
				return implicitCast(lolIt, LOLType.TROOF).getValue() == "WIN";
			}
		}

		void update(int location) {
			lolValue ctr = variableTable.getVar(tokenList[location + 4].getValue());

			if (tokenList[location + 2].getType() == TokenType.NERFIN) {
				variableTable.setVar(tokenList[location + 4].getValue(), LOLType.NUMBR, int.Parse(implicitCast(ctr, LOLType.NUMBR).getValue()) - 1 + "");
			} else {
				variableTable.setVar(tokenList[location + 4].getValue(), LOLType.NUMBR, int.Parse(implicitCast(ctr, LOLType.NUMBR).getValue()) + 1 + "");
			}
		}

		void loopEnd(int location) {
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
							decimal dec;
							if(decimal.TryParse(lv.getValue().ToString(), out dec) == true)
								newValue = decimal.Parse(lv.getValue()).ToString();
							else setError("Unable to cast value");
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
							decimal dec;
							if (decimal.TryParse (lv.getValue ().ToString (), out dec) == true) {
								newValue = decimal.Parse (lv.getValue ()).ToString ();
						
								int num;
								if (int.TryParse (newValue, out num) == true)
									newValue = int.Parse (lv.getValue ()).ToString ();
							}
							else setError("Unable to cast value");
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

