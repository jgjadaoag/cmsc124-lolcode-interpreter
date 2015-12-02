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
		List<TokenType> ifBlockDelimeter;
		List<TokenType> caseBlockDelimeter;

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

			initializeParser();
		}

		public Parser (List<Token> tokenListP) {
			tokenList = new List<Token> ();
			currentPosition = 0;

			foreach (Token t in tokenListP) {
				this.tokenList.Add (t);

			}
			initializeParser();
		}

		private void initializeParser () {
			actionOrder = new List<lolStatement> ();
			tempActionOrder = new List<lolStatement> ();

			ifBlockDelimeter = new List<TokenType> ();
			ifBlockDelimeter.Add (TokenType.NO_WAI);
			ifBlockDelimeter.Add (TokenType.OIC);

			caseBlockDelimeter = new List<TokenType> ();
			caseBlockDelimeter.Add (TokenType.OMG);
			caseBlockDelimeter.Add (TokenType.OMGWTF);
			caseBlockDelimeter.Add (TokenType.OIC);
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
			} else if ((currentPosition = save) == save & gtfo ()) {
			} else if ((currentPosition = save) == save & ifThen ()) {
			} else if ((currentPosition = save) == save & switchBlock ()) {
			} else if ((currentPosition = save) == save & input ()) {
			} else if ((currentPosition = save) == save & output ()) {
			} else if ((currentPosition = save) == save & concatenation ()) {
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
			} else if ((currentPosition = save) == save & gtfo ()) {
			} else if ((currentPosition = save) == save & ifThen ()) {
			} else if ((currentPosition = save) == save & switchBlock ()) {
			} else if ((currentPosition = save) == save & input ()) {
			} else if ((currentPosition = save) == save & output ()) {
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

		bool gtfo () {
			int save = currentPosition;
			if (term (TokenType.GTFO)) {
				tempActionOrder.Add (new lolStatement (Statement_Types.GTFO, save));
				return true;
			} else {
				return false;
			}
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
			int actionSave = tempActionOrder.Count;
			tempActionOrder.Add (new lolStatement (Statement_Types.CONCAT, currentPosition));

			if (term (TokenType.SMOOSH) && stringList() && term(TokenType.MKAY)) {
				return true;
			}

			tempActionOrder.RemoveRange(actionSave, tempActionOrder.Count - actionSave);
			return false;
		}

		bool stringList(){
			int save = currentPosition;

			if ((currentPosition = save) == save & stringList1()  && stringList() ) {
			} else if ((currentPosition = save) == save & stringList2() && stringList()) {
			} else if ((currentPosition = save) == save & stringList3()) {
			} else {
				return false;
			}
			return true;
		}

		bool stringList1(){
			int actionSave = tempActionOrder.Count;
			if (expression() && term(TokenType.AN)) {
				return true;
			}

			tempActionOrder.RemoveRange(actionSave, tempActionOrder.Count - actionSave);
			return false;
		}

		bool stringList2(){
			int actionSave = tempActionOrder.Count;

			if (expression()) {
				return true;
			}

			tempActionOrder.RemoveRange(actionSave, tempActionOrder.Count - actionSave);
			return false;
		}

		bool stringList3(){
			int actionSave = tempActionOrder.Count;

			if (expression()) {
				return true;
			}

			tempActionOrder.RemoveRange(actionSave, tempActionOrder.Count - actionSave);
			return false;
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
			} else if ((currentPosition = save) == save & concatenation ()) {
			} else if ((currentPosition = save) == save & literal ()) {
				if (tokenList [save - 1].getType () == TokenType.STATEMENT_DELIMETER) {
					Console.WriteLine ("Literal statement");
					//tempActionOrder.Clear ();
				}
			} else {
				return false;
			}
			return true;
		}

		#region Conditional

		bool ifThen(){
			int actionSave = tempActionOrder.Count;

			bool toContinue = true;
			if (ifThenStart () && term (TokenType.STATEMENT_DELIMETER) && ifTrueBlock () && term (TokenType.STATEMENT_DELIMETER)) {
				while (currentPosition < tokenList.Count && toContinue) {
					switch (tokenList [currentPosition].getType ()) {
					case TokenType.NO_WAI:
						if (ifFalseBlock () && term (TokenType.STATEMENT_DELIMETER) && term (TokenType.OIC)) {
							tempActionOrder.Add (new lolStatement (Statement_Types.OIC, currentPosition - 1));
							return true;
						}
						toContinue = false;
						continue;
					case TokenType.OIC:
						tempActionOrder.Add (new lolStatement (Statement_Types.OIC, currentPosition++));
						break;
					}
					return true;
				}

			}

			tempActionOrder.RemoveRange(actionSave, tempActionOrder.Count - actionSave);
			return false;
		}

		bool ifThenStart(){
			int actionSave = tempActionOrder.Count;

			if (expression () && term (TokenType.STATEMENT_DELIMETER) && term (TokenType.O_RLY)) {
				tempActionOrder.Add (new lolStatement (Statement_Types.IF_THEN_START, currentPosition - 1));
				return true;
			}

			tempActionOrder.RemoveRange(actionSave, tempActionOrder.Count - actionSave);
			return false;
		}

		bool ifTrueBlock(){
			int actionSave = tempActionOrder.Count;
			tempActionOrder.Add (new lolStatement (Statement_Types.IF_THEN_TRUE, currentPosition));


			List<lolStatement> oldTempActionOrder = tempActionOrder;
			tempActionOrder = new List<lolStatement> ();

			if (term (TokenType.YA_RLY) && term (TokenType.STATEMENT_DELIMETER) && codeBlock(ifBlockDelimeter, oldTempActionOrder)) {
				tempActionOrder = oldTempActionOrder;
				return true;
			}

			tempActionOrder = oldTempActionOrder;
			tempActionOrder.RemoveRange(actionSave, tempActionOrder.Count - actionSave);
			return false;
		}

		bool ifFalseBlock(){
			int actionSave = tempActionOrder.Count;
			tempActionOrder.Add (new lolStatement (Statement_Types.IF_THEN_FALSE, currentPosition));


			List<lolStatement> oldTempActionOrder = tempActionOrder;
			tempActionOrder = new List<lolStatement> ();

			if (term (TokenType.NO_WAI) && term (TokenType.STATEMENT_DELIMETER) && codeBlock(ifBlockDelimeter, oldTempActionOrder)) {
				tempActionOrder = oldTempActionOrder;
				return true;
			}

			tempActionOrder = oldTempActionOrder;
			tempActionOrder.RemoveRange(actionSave, tempActionOrder.Count - actionSave);
			return false;
		}

		bool switchBlock() {
			return switchStart() && term(TokenType.STATEMENT_DELIMETER) && caseBlock() && term(TokenType.OIC);
		}

		bool switchStart(){
			int actionSave = tempActionOrder.Count;

			if (expression () && term (TokenType.STATEMENT_DELIMETER) && term (TokenType.WTF)) {
				tempActionOrder.Add (new lolStatement (Statement_Types.SWITCH, currentPosition - 1));
				return true;
			}

			tempActionOrder.RemoveRange(actionSave, tempActionOrder.Count - actionSave);
			return false;
		}

		bool caseBlock(){
			int save = currentPosition;
			return (((currentPosition = save) == save & caseStatement() && caseBlock()) ||
				((currentPosition = save) == save & caseCondition() && defaultCaseStatement()) ||
				((currentPosition = save) == save & defaultCaseStatement()) ||
			    ((currentPosition = save) == save & caseStatement())
			       );
		}

		bool defaultCaseStatement(){	
			int actionSave = tempActionOrder.Count;

			List<lolStatement> oldTempActionOrder = tempActionOrder;
			tempActionOrder = new List<lolStatement> ();


			if (defaultCase()) {
				oldTempActionOrder.AddRange (tempActionOrder);
				if (codeBlock(caseBlockDelimeter, oldTempActionOrder) && term(TokenType.STATEMENT_DELIMETER)) {
					tempActionOrder = oldTempActionOrder;
					return true;
				}
			}

			tempActionOrder = oldTempActionOrder;
			tempActionOrder.RemoveRange(actionSave, tempActionOrder.Count - actionSave);
			return false;
		}

		bool caseStatement(){	
			int actionSave = tempActionOrder.Count;

			List<lolStatement> oldTempActionOrder = tempActionOrder;
			tempActionOrder = new List<lolStatement> ();


			if (caseCondition()) {
				oldTempActionOrder.AddRange (tempActionOrder);
				if (codeBlock(caseBlockDelimeter, oldTempActionOrder) && term(TokenType.STATEMENT_DELIMETER)) {
					tempActionOrder = oldTempActionOrder;
					return true;
				}
			}

			tempActionOrder = oldTempActionOrder;
			tempActionOrder.RemoveRange(actionSave, tempActionOrder.Count - actionSave);
			return false;
		}

		bool caseCondition(){
			int actionSave;
			bool entered = false;
			while (term(TokenType.OMG) ) {
				entered = true;
				actionSave = tempActionOrder.Count;
				tempActionOrder.Add (new lolStatement (Statement_Types.CASE, currentPosition - 1));
				if(literal() && term (TokenType.STATEMENT_DELIMETER)) {
					continue;
				}

				tempActionOrder.RemoveRange(actionSave, tempActionOrder.Count - actionSave);
				return false;
			}

			currentPosition--;
			return entered;
		}	

		bool defaultCase(){
			int actionSave = tempActionOrder.Count;
			tempActionOrder.Add (new lolStatement (Statement_Types.DEFAULT_CASE, currentPosition));

			if (term (TokenType.OMGWTF) && term (TokenType.STATEMENT_DELIMETER)) {
				return true;
			}

			tempActionOrder.RemoveRange(actionSave, tempActionOrder.Count - actionSave);
			return false;
		}

		#endregion

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
			tempActionOrder.Add (new lolStatement (Statement_Types.MAXIMUM, currentPosition));

			if (term (TokenType.BIGGR_OF) && expression () && term (TokenType.AN) && expression ()) {
				return true;
			}

			tempActionOrder.RemoveRange(actionSave, tempActionOrder.Count - actionSave);
			return false;
		}

		bool minimum(){
			int actionSave = tempActionOrder.Count;
			tempActionOrder.Add (new lolStatement (Statement_Types.MINIMUM, currentPosition));

			if (term (TokenType.SMALLR_OF) && expression () && term (TokenType.AN) && expression ()) {
				return true;
			}

			tempActionOrder.RemoveRange(actionSave, tempActionOrder.Count - actionSave);
			return false;
		}

		bool compareOperator(){
			int save = currentPosition;
			return (((currentPosition = save) == save & equality()) ||
				((currentPosition = save) == save & inequality())
				);
		}

		bool equality(){
			int actionSave = tempActionOrder.Count;
			tempActionOrder.Add (new lolStatement (Statement_Types.EQUALITY, currentPosition));

			if (term (TokenType.BOTH_SAEM) && expression () && term (TokenType.AN) && expression ()) {
				return true;
			}

			tempActionOrder.RemoveRange(actionSave, tempActionOrder.Count - actionSave);
			return false;
		}

		bool inequality(){
			int actionSave = tempActionOrder.Count;
			tempActionOrder.Add (new lolStatement (Statement_Types.INEQUALITY, currentPosition));

			if (term (TokenType.DIFFRINT) && expression () && term (TokenType.AN) && expression ()) {
				return true;
			}

			tempActionOrder.RemoveRange(actionSave, tempActionOrder.Count - actionSave);
			return false;
		}

		bool infiniteArityAnd(){
			int actionSave = tempActionOrder.Count;
			tempActionOrder.Add (new lolStatement (Statement_Types.ARITY_AND, currentPosition));

			if (term (TokenType.ALL_OF) && infiniteExpression () && term (TokenType.MKAY)) {
				return true;
			}

			tempActionOrder.RemoveRange(actionSave, tempActionOrder.Count - actionSave);
			return false;
		}

		bool infiniteArityOr(){
			int actionSave = tempActionOrder.Count;
			tempActionOrder.Add (new lolStatement (Statement_Types.ARITY_OR, currentPosition));

			if (term (TokenType.ANY_OF) && infiniteExpression () && term (TokenType.MKAY)) {
				return true;
			}

			tempActionOrder.RemoveRange(actionSave, tempActionOrder.Count - actionSave);
			return false;
		}

		bool infiniteExpression(){
			int save = currentPosition;

			if ((currentPosition = save) == save & infiniteExpression1() && infiniteExpression()) {
			} else if ((currentPosition = save) == save & infiniteExpression2()){
			} else {
				return false;
			}
			return true;
		}

		bool infiniteExpression1(){
			int actionSave = tempActionOrder.Count;
			if (expression() && term(TokenType.AN)) {
				return true;
			}

			tempActionOrder.RemoveRange(actionSave, tempActionOrder.Count - actionSave);
			return false;
		}

		bool infiniteExpression2(){
			int actionSave = tempActionOrder.Count;

			if (expression()) {
				return true;
			}

			tempActionOrder.RemoveRange(actionSave, tempActionOrder.Count - actionSave);
			return false;
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
			int actionSave = tempActionOrder.Count;
			tempActionOrder.Add (new lolStatement (Statement_Types.AND, currentPosition));

			if (term (TokenType.BOTH_OF) && expression () && term (TokenType.AN) && expression ()) {
				return true;
			}

			tempActionOrder.RemoveRange(actionSave, tempActionOrder.Count - actionSave);
			return false;
		}

		bool orOperator(){
			int actionSave = tempActionOrder.Count;
			tempActionOrder.Add (new lolStatement (Statement_Types.OR, currentPosition));

			if (term (TokenType.EITHER_OF) && expression () && term (TokenType.AN) && expression ()) {
				return true;
			}

			tempActionOrder.RemoveRange(actionSave, tempActionOrder.Count - actionSave);
			return false;
		}

		bool xorOperator(){
			int actionSave = tempActionOrder.Count;
			tempActionOrder.Add (new lolStatement (Statement_Types.XOR, currentPosition));

			if (term (TokenType.WON_OF) && expression () && term (TokenType.AN) && expression ()) {
				return true;
			}

			tempActionOrder.RemoveRange(actionSave, tempActionOrder.Count - actionSave);
			return false;
		}

		bool unaryNegation(){
			int actionSave = tempActionOrder.Count;
			tempActionOrder.Add (new lolStatement (Statement_Types.NOT, currentPosition));

			if (term (TokenType.NOT) && expression ()) {
				return true;
			}

			tempActionOrder.RemoveRange(actionSave, tempActionOrder.Count - actionSave);
			return false;
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

			tempActionOrder.Add (new lolStatement (Statement_Types.LITERAL, save));

			return true;
		}

		private Tuple<LOLType, string> createValue(LOLType type, string value) {
			return new Tuple<LOLType, string> (type, value);
		}
	}
}
	
