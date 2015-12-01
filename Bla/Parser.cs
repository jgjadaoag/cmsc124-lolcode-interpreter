using System;
using System.Collections.Generic;

namespace Bla
{
	public class Parser
	{
		List<Token> tokenList;
		int currentPosition;
		bool parseCheck;
		List<lolStatement> actionOrder;
		List<lolStatement> tempActionOrder;

		public Parser (string value)
		{
			parseCheck = true;
			tokenList = new List<Token> ();
			TokenStream ts = new TokenStream (value);
			Token t;
			currentPosition = 0;

			while(!ts.end()){
				t = ts.get ();
				if (t.getType() != TokenType.UNKNOWN && (t.getType() != TokenType.BTW || t.getType() != TokenType.OBTW)) {
					tokenList.Add (t);
				}
			}

			actionOrder = new List<lolStatement> ();
			tempActionOrder = new List<lolStatement> ();
		}

		public Parser (List<Token> tokenListP) {
			tokenList = new List<Token> ();
			currentPosition = 0;

			foreach (Token t in tokenListP) {
				this.tokenList.Add (t);

			}
			actionOrder = new List<lolStatement> ();
			tempActionOrder = new List<lolStatement> ();
		}

		public List<lolStatement> getActionOrder() {
			return actionOrder;
		}

		public bool parse(){
			return program2 ();
		}

		bool program(){
			return term(TokenType.HAI) && term(TokenType.STATEMENT_DELIMETER) && codeBlock() && term(TokenType.STATEMENT_DELIMETER) && term(TokenType.KTHXBYE);
		}

		bool program2(){
			return term(TokenType.HAI) && term(TokenType.STATEMENT_DELIMETER) && codeBlock(TokenType.KTHXBYE, actionOrder) && term(TokenType.STATEMENT_DELIMETER) && term(TokenType.KTHXBYE);
		}

		bool term(TokenType tokType){
			if (currentPosition == tokenList.Count)
				return false;
			//Console.WriteLine ("Current Token: "+tokenList[currentPosition].Item1.getType());
			//Console.WriteLine ("Ask token: "+tokType);
			return tokenList[currentPosition++].getType() == tokType;
		}

		bool codeBlock(){
			int save = currentPosition;
			tempActionOrder.Clear ();
			int lastInstruction = actionOrder.Count;
			if ((currentPosition = save) == save & statement () && term (TokenType.STATEMENT_DELIMETER) && codeBlock ()) {
				return true;
			} else if ((currentPosition = save) == save & statement ()) {
				//this is always called when an extra statement is called
				if (lastInstruction < actionOrder.Count - 1) {
					int repeatedInstruction;
					for ( repeatedInstruction = lastInstruction + 1; repeatedInstruction < actionOrder.Count; repeatedInstruction++) {
						if (actionOrder [repeatedInstruction].location == actionOrder [lastInstruction].location) {
							break;
						}
							
					}
					actionOrder.RemoveRange (repeatedInstruction, repeatedInstruction - lastInstruction);
				}
				return true;
			}
			return false;
		}

		bool codeBlock(TokenType endsWith, List<lolStatement> masterActionOrder) {
			List<TokenType> tl = new List<TokenType> ();
			tl.Add (endsWith);
			return codeBlock (tl, masterActionOrder);
		}

		bool codeBlock(List<TokenType> endsWith, List<lolStatement> masterActionOrder) {
			bool isStatement;
			do {
				if(tokenList.Count - 1 == currentPosition)
					return false;
				tempActionOrder.Clear();
				isStatement = statement2();
				masterActionOrder.AddRange(tempActionOrder);

			} while(isStatement && term(TokenType.STATEMENT_DELIMETER) && !endsWith.Contains (tokenList[currentPosition].getType()));

			currentPosition--;
			return isStatement;
		}

		bool statement2() {
			int save = currentPosition;

			if (term (TokenType.STATEMENT_DELIMETER)) {
				currentPosition = save;
				return true;
			}
			if ((currentPosition = save) == save & vardec ()) {
			} else if ((currentPosition = save) == save & variableAssignment ()) {
			} else if ((currentPosition = save) == save & ifThen ()) {
			} else if ((currentPosition = save) == save & switchBlock ()) {
			} else if ((currentPosition = save) == save & input ()) {
			} else if ((currentPosition = save) == save & output ()) {
			} else if ((currentPosition = save) == save & concatenation ()) {
			} else if ((currentPosition = save) == save & term(Bla.TokenType.GTFO)){ 
			} else if ((currentPosition = save) == save & expression ()) {
			} else {
				return false;
			}
			return true;
		}
		bool statement(){
			int save = currentPosition;

			if (term (TokenType.STATEMENT_DELIMETER)) {
				currentPosition = save;
				return true;
			}
			if ((currentPosition = save) == save & vardec ()) {
			} else if ((currentPosition = save) == save & variableAssignment ()) {
			} else if ((currentPosition = save) == save & ifThen ()) {
			} else if ((currentPosition = save) == save & switchBlock ()) {
			} else if ((currentPosition = save) == save & input ()) {
			} else if ((currentPosition = save) == save & output ()) {
			} else if ((currentPosition = save) == save & concatenation ()) {
			} else if ((currentPosition = save) == save & term(Bla.TokenType.GTFO)){ 
			} else if ((currentPosition = save) == save & expression ()) {
			} else {
				return false;
			}
			actionOrder.AddRange (tempActionOrder);
			tempActionOrder.Clear ();
			return true;
		}

		bool variableAssignment(){
			int save = currentPosition;
			if (term (TokenType.VARIABLE_IDENTIFIER) && term (TokenType.R) && expression ()) {
				tempActionOrder.Add (new lolStatement (Statement_Types.VARIABLE_ASSIGNMENT, save));
				currentPosition = save + 2;
				expression ();
			} else {
				return false;
			}
			return true;
		}

		bool input(){
			int save = currentPosition;
			if (term (TokenType.GIMMEH) && term (TokenType.VARIABLE_IDENTIFIER)) {
				tempActionOrder.Add (new lolStatement (Statement_Types.INPUT, save));
				return true;
			} else {
				return false;
			}
		}

		bool output(){
			tempActionOrder.Clear ();
			int save = currentPosition;
			if ((currentPosition = save) == save & term (TokenType.VISIBLE) && expression ()) {
				tempActionOrder.Clear ();
				tempActionOrder.Add (new lolStatement (Statement_Types.OUTPUT, save));
				currentPosition = save + 1;
				expression ();
			} else {
				return false;
			}

			return true;
		}

		bool concatenation(){
			int save = currentPosition;
			return(((currentPosition = save) == save & term(TokenType.SMOOSH) && stringList() && term(TokenType.MKAY)) ||
					((currentPosition = save) == save & term(TokenType.SMOOSH) && stringList())
				);
		}

		bool stringList(){
			int save = currentPosition;
			return (((currentPosition = save) == save & term(TokenType.YARN_LITERAL) && term(TokenType.AN) && stringList()) ||
				((currentPosition = save) == save & term(TokenType.YARN_LITERAL) && stringList()) ||
				((currentPosition = save) == save & term(TokenType.YARN_LITERAL))
					);
		}

		bool expression(){
			int save = currentPosition;

			if ((currentPosition = save) == save & term (TokenType.VARIABLE_IDENTIFIER)) {
				if (tokenList [save - 1].getType () == TokenType.STATEMENT_DELIMETER) {
					Console.WriteLine ("VARIABLE statement");
					//tempActionOrder.Clear ();
				}
				tempActionOrder.Add (new lolStatement (Statement_Types.VARIABLE_IDENTIFIER, save));
			} else if ((currentPosition = save) == save & mathOperator ()) {
			} else if ((currentPosition = save) == save & booleanOperation ()) {
			} else if ((currentPosition = save) == save & compareOperator ()) {
			} else if ((currentPosition = save) == save & literal ()) {
				if (tokenList [save - 1].getType () == TokenType.STATEMENT_DELIMETER) {
					Console.WriteLine ("Literal statement");
					//tempActionOrder.Clear ();
				}
				tempActionOrder.Add (new lolStatement (Statement_Types.LITERAL, save));
			} else {
				return false;
			}
			return true;
		}

		bool ifThen(){
			int save = currentPosition;
			return (((currentPosition = save) == save & ifThenStart() && term(TokenType.STATEMENT_DELIMETER) && ifTrueBlock() && term(TokenType.STATEMENT_DELIMETER) && ifFalseBlock() && term(TokenType.STATEMENT_DELIMETER) && term(TokenType.OIC)) ||
				((currentPosition = save) == save & ifThenStart() && term(TokenType.STATEMENT_DELIMETER) && ifTrueBlock() && term(TokenType.STATEMENT_DELIMETER) && term(TokenType.OIC))
				);
		}

		bool ifThenStart(){
			int actionSave = tempActionOrder.Count;

			if (expression () && term (TokenType.STATEMENT_DELIMETER) && term (TokenType.O_RLY)) {
				tempActionOrder.Add (new lolStatement (Statement_Types.IF_THEN_START, currentPosition - 1));
				MainClass.win.displayTextToConsole ("IN IF THEN START");
				return true;
			}

			tempActionOrder.RemoveRange(actionSave, tempActionOrder.Count - actionSave);
			return false;
		}

		bool ifTrueBlock(){
			int actionSave = tempActionOrder.Count;
			tempActionOrder.Add (new lolStatement (Statement_Types.IF_THEN_TRUE, currentPosition));

			List<TokenType> blockDelimeter = new List<TokenType> ();
			blockDelimeter.Add (TokenType.NO_WAI);
			blockDelimeter.Add (TokenType.OIC);

			List<lolStatement> oldTempActionOrder = tempActionOrder;
			tempActionOrder = new List<lolStatement> ();

			if (term (TokenType.YA_RLY) && term (TokenType.STATEMENT_DELIMETER) && codeBlock(blockDelimeter, oldTempActionOrder)) {
				oldTempActionOrder.AddRange (tempActionOrder);
				tempActionOrder = oldTempActionOrder;
				return true;
			}

			tempActionOrder = oldTempActionOrder;
			tempActionOrder.RemoveRange(actionSave, tempActionOrder.Count - actionSave);
			return false;
		}

		bool ifFalseBlock(){
			return term (TokenType.NO_WAI) && term(TokenType.STATEMENT_DELIMETER) && codeBlock();
		}
		/*
		bool ifCodeBlock() {
			List<lolStatement> oldTempActionOrder = tempActionOrder;
			tempActionOrder = new List<lolStatement>();
			bool isStatement;
			do {
				int save = currentPosition;
				int lastInstruction = actionOrder.Count;
				if ((currentPosition = save) == save & statement () && term (TokenType.STATEMENT_DELIMETER)) {
					isStatement = true;
				} else if ((currentPosition = save) == save & statement ()) {
					//this is always called when an extra statement is called
					if (lastInstruction < actionOrder.Count - 1) {
						int repeatedInstruction;
						for ( repeatedInstruction = lastInstruction + 1; repeatedInstruction < actionOrder.Count; repeatedInstruction++) {
							if (actionOrder [repeatedInstruction].location == actionOrder [lastInstruction].location) {
								break;
							}

						}
						actionOrder.RemoveRange (repeatedInstruction, repeatedInstruction - lastInstruction);
					}
					return true;
				}
				return false;
			} while(isStatement &&
				(tempActionOrder.Count == 0 || !isConditional (tempActionOrder [tempActionOrder.Count - 1])) && 
				tokenList[currentPosition].getType() != TokenType.OIC &&
				tokenList[currentPosition].getType() != TokenType.KTHXBYE);
			
			if (tokenList [currentPosition].getType () == TokenType.KTHXBYE || isStatement) {
				tempActionOrder = oldTempActionOrder;
				return false;
			}

			oldTempActionOrder.AddRange (tempActionOrder);
			tempActionOrder = oldTempActionOrder;
			return true;
		}

		bool isConditional(lolStatement ls) {
			switch (ls.type) {
			case Statement_Types.IF_THEN_TRUE:
			case Statement_Types.IF_THEN_FALSE:
			case Statement_Types.ELSE_IF:
				return true;
			}
			return false;
		}
*/
		bool switchBlock(){
			return expression() && term(TokenType.STATEMENT_DELIMETER) && term(TokenType.WTF) && term(TokenType.STATEMENT_DELIMETER) && caseBlock() && term(TokenType.OIC);
		}

		bool caseBlock(){
			int save = currentPosition;
			return (((currentPosition = save) == save & caseStatement() && caseBlock()) ||
			        ((currentPosition = save) == save & caseStatement())
			       );
		}

		bool caseStatement(){	
			int save = currentPosition;
			return (((currentPosition = save) == save & caseCondition() && codeBlock() && term(TokenType.STATEMENT_DELIMETER)) ||
			        ((currentPosition = save) == save & caseCondition() && term (TokenType.STATEMENT_DELIMETER)) ||
					((currentPosition = save) == save & defaultCase() && codeBlock() && term(TokenType.STATEMENT_DELIMETER))
					);
		}

		bool caseCondition(){
			int save = currentPosition;
			return (((currentPosition = save) == save & term(TokenType.OMG) && literal() && term (TokenType.STATEMENT_DELIMETER) && caseCondition()) ||
					((currentPosition = save) == save & term(TokenType.OMG) && literal())
			       );
		}	

		bool defaultCase(){
			return term (TokenType.OMGWTF) && term (TokenType.STATEMENT_DELIMETER);
		}

		bool mathOperator(){
			int save = currentPosition;
			if ((currentPosition = save) == save & addition ()) {
			} else if ((currentPosition = save) == save & subtraction ()) {
			} else if ((currentPosition = save) == save & multiplication ()) {
			} else if ((currentPosition = save) == save & division ()) {
			} else if ((currentPosition = save) == save & modulo ()) {
			} else if ((currentPosition = save) == save & maximum ()) {
			} else if ((currentPosition = save) == save & minimum ()) {
			} else {
				return false;
			}
			return true;
		}

		bool addition(){
			int actionSave = tempActionOrder.Count;
			tempActionOrder.Add (new lolStatement (Statement_Types.ADDITION, currentPosition));

			if (term (TokenType.SUM_OF) && expression () && term (TokenType.AN) && expression ()) {
				return true;
			}

			tempActionOrder.RemoveRange(actionSave, tempActionOrder.Count - actionSave);
			return false;
		}

		bool subtraction(){
			int actionSave = tempActionOrder.Count;
			tempActionOrder.Add (new lolStatement (Statement_Types.SUBTRACTION, currentPosition));

			if (term (TokenType.DIFF_OF) && expression () && term (TokenType.AN) && expression ()) {
				return true;
			}

			tempActionOrder.RemoveRange(actionSave, tempActionOrder.Count - actionSave);
			return false;
		}

		bool multiplication(){
			int actionSave = tempActionOrder.Count;
			tempActionOrder.Add (new lolStatement (Statement_Types.MULTIPLICATION, currentPosition));

			if (term (TokenType.PRODUKT_OF) && expression () && term (TokenType.AN) && expression ()) {
				return true;
			}

			tempActionOrder.RemoveRange(actionSave, tempActionOrder.Count - actionSave);
			return false;
		}

		bool division(){
			int actionSave = tempActionOrder.Count;
			tempActionOrder.Add (new lolStatement (Statement_Types.DIVISION, currentPosition));

			if (term (TokenType.QUOSHUNT_OF) && expression () && term (TokenType.AN) && expression ()) {
				return true;
			}

			tempActionOrder.RemoveRange(actionSave, tempActionOrder.Count - actionSave);
			return false;
		}

		bool modulo(){
			int actionSave = tempActionOrder.Count;
			tempActionOrder.Add (new lolStatement (Statement_Types.MODULO, currentPosition));

			if (term (TokenType.MOD_OF) && expression () && term (TokenType.AN) && expression ()) {
				return true;
			}

			tempActionOrder.RemoveRange(actionSave, tempActionOrder.Count - actionSave);
			return false;
		}

		bool maximum(){
			int actionSave = tempActionOrder.Count;
			tempActionOrder.Add (new lolStatement (Statement_Types.MODULO, currentPosition));

			if (term (TokenType.BIGGR_OF) && expression () && term (TokenType.AN) && expression ()) {
				return true;
			}

			tempActionOrder.RemoveRange(actionSave, tempActionOrder.Count - actionSave);
			return false;
		}

		bool minimum(){
			return term (TokenType.SMALLR_OF) && expression() && term(TokenType.AN) && expression();
		}

		bool compareOperator(){
			int save = currentPosition;
			return (((currentPosition = save) == save & equality()) ||
				((currentPosition = save) == save & inequality()) ||
				((currentPosition = save) == save & greaterThan()) ||
				((currentPosition = save) == save & lessThan ()) ||
				((currentPosition = save) == save & greaterThanOrEqual ()) ||
				((currentPosition = save) == save & lessThanOrEqual ())
				);
		}

		bool equality(){
			int save = currentPosition;
			if (term (TokenType.BOTH_SAEM) && expression () && term (TokenType.AN) && expression ()) {
				tempActionOrder.Clear ();
				tempActionOrder.Add (new lolStatement (Statement_Types.EQUALITY, save));
				currentPosition = save + 1;
				expression ();
				currentPosition++;
				expression ();
			} else {
				return false;
			}
			return true;
		}

		bool inequality(){
			int save = currentPosition;
			if (term (TokenType.DIFFRINT) && expression () && term (TokenType.AN) && expression ()) {
				tempActionOrder.Clear ();
				tempActionOrder.Add (new lolStatement (Statement_Types.INEQUALITY, save));
				currentPosition = save + 1;
				expression ();
				currentPosition++;
				expression ();
			} else {
				return false;
			}
			return true;
		}

		bool greaterThan(){
			return term (TokenType.DIFFRINT) && expression () && term (TokenType.AN) && minimum ();
		}

		bool lessThan(){
			return term (TokenType.DIFFRINT) && expression () && term (TokenType.AN) && maximum ();
		}

		bool greaterThanOrEqual(){
			return term (TokenType.BOTH_SAEM) && expression () && term (TokenType.AN) && maximum ();
		}

		bool lessThanOrEqual(){
			return term (TokenType.BOTH_SAEM) && expression () && term (TokenType.AN) && minimum ();
		}

		bool infiniteArityAnd(){
			return term (TokenType.ALL_OF) && expression () && term (TokenType.AN) && infiniteExpression () && term (TokenType.MKAY);
		}

		bool infiniteArityOr(){
			return term (TokenType.ANY_OF) && expression () && term (TokenType.AN) && infiniteExpression () && term (TokenType.MKAY);
		}

		bool infiniteExpression(){
			int save = currentPosition;
			return (((currentPosition = save) == save & expression() && term(TokenType.AN) && infiniteExpression()) ||
			        ((currentPosition = save) == save & expression()));
		}

		bool booleanOperation(){
			int save = currentPosition;
			return (((currentPosition = save) == save & andOperator()) ||
					((currentPosition = save) == save & orOperator ()) ||
					((currentPosition = save) == save & xorOperator ()) ||
			        ((currentPosition = save) == save & unaryNegation ()) ||
					((currentPosition = save) == save & infiniteArityAnd ()) ||
					((currentPosition = save) == save & infiniteArityOr ())
					);
		}

		bool andOperator(){
			int save = currentPosition;
			if (term (TokenType.BOTH_OF) && expression () && term (TokenType.AN) && expression ()) {
				tempActionOrder.Clear ();
				tempActionOrder.Add (new lolStatement (Statement_Types.AND, save));
				currentPosition = save + 1;
				expression ();
				currentPosition++;
				expression ();
			} else {
				return false;
			}
			return true;
		}

		bool orOperator(){
			int save = currentPosition;
			if (term (TokenType.EITHER_OF) && expression () && term (TokenType.AN) && expression ()) {
				tempActionOrder.Clear ();
				tempActionOrder.Add (new lolStatement (Statement_Types.OR, save));
				currentPosition = save + 1;
				expression ();
				currentPosition++;
				expression ();
			} else {
				return false;
			}
			return true;
		}

		bool xorOperator(){
			int save = currentPosition;
			if (term (TokenType.WON_OF) && expression () && term (TokenType.AN) && expression ()) {
				tempActionOrder.Clear ();
				tempActionOrder.Add (new lolStatement (Statement_Types.XOR, save));
				currentPosition = save + 1;
				expression ();
				currentPosition++;
				expression ();
			} else {
				return false;
			}
			return true;
		}

		bool unaryNegation(){
			int save = currentPosition;
			if (term (TokenType.NOT) && expression ()) {
				tempActionOrder.Clear ();
				tempActionOrder.Add (new lolStatement (Statement_Types.NOT, save));
				currentPosition = save + 1;
				expression ();
			} else {
				return false;
			}
			return true;
		}

		bool vardec(){
			int save = currentPosition;
			if ((currentPosition = save) == save & variableDeclarationItz()) {
			} else if ((currentPosition = save) == save & term (TokenType.I_HAS_A) && term (TokenType.VARIABLE_IDENTIFIER)) {
				tempActionOrder.Clear();
				tempActionOrder.Add (new lolStatement(Statement_Types.VARIABLE_DECLARATION, save));
			} else {
				return false;
			}
			return true;
		}

		bool variableDeclarationItz() {
			int save = currentPosition;
			tempActionOrder.Clear ();
			int actionSave = tempActionOrder.Count;
			tempActionOrder.Add (new lolStatement(Statement_Types.VARIABLE_DECLARATION_ITZ, save));

			if (term (TokenType.I_HAS_A) && term (TokenType.VARIABLE_IDENTIFIER) && term (TokenType.ITZ) && expression ()) {
				return true;
			}

			tempActionOrder.RemoveRange (actionSave, tempActionOrder.Count - actionSave);

			return false;
		}

		bool literal(){
			int save = currentPosition;
			if ((currentPosition = save) == save & term (TokenType.NUMBR_LITERAL)) {
			} else if ((currentPosition = save) == save & term (TokenType.NUMBAR_LITERAL)) {
			} else if ((currentPosition = save) == save & term(TokenType.STRING_DELIMETER) && term (TokenType.YARN_LITERAL) && term(TokenType.STRING_DELIMETER)) {
			} else if ((currentPosition = save) == save & term (TokenType.TROOF_LITERAL)) {
			} else {
				return false;
			}

			return true;
		}

		private Tuple<LOLType, string> createValue(LOLType type, string value) {
			return new Tuple<LOLType, string> (type, value);
		}
	}
}
	
