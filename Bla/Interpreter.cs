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
		public Interpreter (string input)
		{
			actionMap = new Dictionary<Statement_Types, lolAction> ();
			tokenList = new List<Token> ();
			TokenStream ts = new TokenStream (input);
			Token t;
			currentPosition = 0;
			variableTable = new SymbolTable ();

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
			foreach (lolStatement ls in actionList) {
				actionMap [ls.type] (ls.location);
				Console.WriteLine (ls.type);
			}
		}

		void addActionDefinitions() {
			actionMap.Add (Statement_Types.VARIABLE_DECLARATION_ITZ, variableDeclarationItz);
			actionMap.Add (Statement_Types.VARIABLE_DECLARATION, variableDeclaration);
			actionMap.Add (Statement_Types.VARIABLE_ASSIGNMENT, variableAssignment);
		}
		void variableDeclarationItz(int location) {

			if (!variableTable.hasVariable (tokenList [location + 1].getValue ())) {
				Console.WriteLine("MEW? "+tokenList[location + 3]);
				variableTable.createVar(tokenList[location + 1].getValue(), 
					tokToLolType[tokenList[location + 3].getType()],
					tokenList[location + 3].getValue());
			}
			MainClass.win.refreshSymbol (variableTable);
		}
		void variableDeclaration(int location) {
			if (!variableTable.hasVariable (tokenList [location + 1].getValue ())) {
				variableTable.createVar(tokenList[location + 1].getValue(), 
					LOLType.NOOB,
					"");
			}
			MainClass.win.refreshSymbol (variableTable);
		}

		void variableAssignment(int location){
			if (variableTable.hasVariable (tokenList [location].getValue ())) {
				variableTable.setVar (tokenList[location].getValue(), 
					tokToLolType[tokenList[location + 2].getType()],
					tokenList[location + 2].getValue());
			}
			MainClass.win.refreshSymbol (variableTable);
		}

		void term(TokenType t) {
			if (t != tokenList [currentPosition++].getType ()) {
				throw new ApplicationException ("Error in token");
			}
		}
	}
}

