using System;
using System.Collections.Generic;

namespace Bla
{
	public class Parser
	{
		List<Token> tokenList;
		int currentPosition;
		List<lolStatement> actionOrder;
		List<lolStatement> tempActionOrder;

		public Parser (string value)
		{
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
			return program ();
		}

		bool program(){
			return term(TokenType.HAI) && term(TokenType.STATEMENT_DELIMETER) && codeBlock() && term(TokenType.STATEMENT_DELIMETER) && term(TokenType.KTHXBYE);
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
			if ((currentPosition = save) == save & statement () && term (TokenType.STATEMENT_DELIMETER) && codeBlock ()) {
				return true;
			}
			tempActionOrder.Clear ();
			if ((currentPosition = save) == save & statement ()) {
				return true;
			}
			return false;
		}

		bool statement(){
			int save = currentPosition;
			if (term (TokenType.STATEMENT_DELIMETER)) {
				currentPosition = save;
				return true;
			}
			if ((currentPosition = save) == save & vardec ()) {
			} else if ((currentPosition = save) == save & variableAssignment ()) {
			} else if ((currentPosition = save) == save & caseBlock ()) {
			} else if ((currentPosition = save) == save & expression ()) {
			} else if ((currentPosition = save) == save & input ()) {
			} else if ((currentPosition = save) == save & output ()) {
			} else if ((currentPosition = save) == save & ifThen ()) {
			} else if ((currentPosition = save) == save & concatenation ()) {
			} else {
				return false;
			}
			actionOrder.AddRange (tempActionOrder);
			tempActionOrder.Clear ();
			return true;
		}

		bool variableAssignment(){
			return term (TokenType.VARIABLE_IDENTIFIER) && term (TokenType.R) && expression ();
		}

		bool input(){
			if (term (TokenType.GIMMEH) && term (TokenType.VARIABLE_IDENTIFIER)) {
				new Dialog ( tokenList[currentPosition-1].getValue());
				MainClass.win.refreshSymbol (MainClass.st);
				return true;
			}
			return false;
		}

		bool output(){
			int save = currentPosition;
			if ((currentPosition = save) == save & term (TokenType.VISIBLE) && expression ()) {
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
					((currentPosition = save) == save & term(TokenType.YARN_LITERAL)) ||
					((currentPosition = save) == save & term(TokenType.YARN_LITERAL) && stringList())
					);
		}

		bool expression(){
			int save = currentPosition;
			return (((currentPosition = save) == save & term(TokenType.VARIABLE_IDENTIFIER)) ||
					((currentPosition = save) == save & mathOperator()) ||
			        ((currentPosition = save) == save & booleanOperation()) ||
			        ((currentPosition = save) == save & compareOperator()) ||
					((currentPosition = save) == save & literal())
					);
		}

		bool ifThen(){
			int save = currentPosition;
			return (((currentPosition = save) == save & ifThenStart() && term(TokenType.STATEMENT_DELIMETER) && ifBlock() && term(TokenType.STATEMENT_DELIMETER) && elseBlock() && term(TokenType.STATEMENT_DELIMETER) && term(TokenType.OIC)) ||
				((currentPosition = save) == save & ifThenStart() && term(TokenType.STATEMENT_DELIMETER) && ifBlock() && term(TokenType.STATEMENT_DELIMETER) && term(TokenType.OIC))
				);
		}

		bool ifThenStart(){
			return expression () && term (TokenType.STATEMENT_DELIMETER) && term (TokenType.O_RLY);
		}

		bool ifBlock(){
			return term (TokenType.YA_RLY) && term (TokenType.STATEMENT_DELIMETER) && codeBlock ();
		}

		bool elseBlock(){
			return term (TokenType.NO_WAI) && term(TokenType.STATEMENT_DELIMETER) && codeBlock();
		}

		bool switchBlock(){
			return expression() && term(TokenType.STATEMENT_DELIMETER) && term(TokenType.WTF) && term(TokenType.STATEMENT_DELIMETER) && caseStatement() && term(TokenType.OIC);
		}

		bool caseStatement(){	
			int save = currentPosition;
			return (((currentPosition = save) == save & caseCondition() && codeBlock() && caseStatement()) ||
					((currentPosition = save) == save & caseCondition() && codeBlock() && term(TokenType.STATEMENT_DELIMETER)) ||
			        ((currentPosition = save) == save & caseCondition() && codeBlock() && term(TokenType.GTFO) && term(TokenType.STATEMENT_DELIMETER)) ||
					((currentPosition = save) == save & caseCondition() && defaultCase() && codeBlock() && term(TokenType.STATEMENT_DELIMETER))
					);
		}

		bool caseCondition(){
			int save = currentPosition;
			return (((currentPosition = save) == save & caseCondition() && term(TokenType.OMG) && literal() && term(TokenType.STATEMENT_DELIMETER)) ||
					((currentPosition = save) == save & term(TokenType.OMG) && literal() && term(TokenType.STATEMENT_DELIMETER))
					);
		}

		bool defaultCase(){
			return term (TokenType.OMGWTF) && term (TokenType.STATEMENT_DELIMETER);
		}

		bool mathOperator(){
			int save = currentPosition;
			return (((currentPosition = save) == save & addition()) ||
					((currentPosition = save) == save & subtraction()) ||
					((currentPosition = save) == save & multiplication()) ||
					((currentPosition = save) == save & division()) ||
					((currentPosition = save) == save & modulo()) ||
					((currentPosition = save) == save & maximum()) ||
					((currentPosition = save) == save & minimum())
					);
		}

		bool addition(){
			return term (TokenType.SUM_OF) && expression() && term(TokenType.AN) && expression();
		}

		bool subtraction(){
			return term (TokenType.DIFF_OF) && expression() && term(TokenType.AN) && expression();
		}

		bool multiplication(){
			return term (TokenType.PRODUKT_OF) && expression() && term(TokenType.AN) && expression();
		}

		bool division(){
			return term (TokenType.QUOSHUNT_OF) && expression() && term(TokenType.AN) && expression();
		}

		bool modulo(){
			return term (TokenType.MOD_OF) && expression() && term(TokenType.AN) && expression();
		}

		bool maximum(){
			return term (TokenType.BIGGR_OF) && expression() && term(TokenType.AN) && expression();
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
			return term (TokenType.BOTH_SAEM) && expression () && term (TokenType.AN) && expression ();
		}

		bool inequality(){
			return term (TokenType.DIFFRINT) && expression () && term (TokenType.AN) && expression ();
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
			return term (TokenType.BOTH_OF) && expression () && term (TokenType.AN) && expression ();
		}

		bool orOperator(){
			return term (TokenType.EITHER_OF) && expression () && term (TokenType.AN) && expression ();
		}

		bool xorOperator(){
			return term (TokenType.WON_OF) && expression () && term (TokenType.AN) && expression ();
		}

		bool unaryNegation(){
			return term (TokenType.NOT) && expression ();
		}

		bool vardec(){
			int save = currentPosition;
			if ((currentPosition = save) == save & term (TokenType.I_HAS_A) && term (TokenType.VARIABLE_IDENTIFIER) && term (TokenType.ITZ) && assignRHS ()) {
				tempActionOrder.Add (new lolStatement(Statement_Types.VARIABLE_DECLARATION_ITZ, save));
			} else if ((currentPosition = save) == save & term (TokenType.I_HAS_A) && term (TokenType.VARIABLE_IDENTIFIER)) {
				tempActionOrder.Add (new lolStatement(Statement_Types.VARIABLE_DECLARATION, save));
			} else {
				return false;
			}
			return true;
		}

		bool assignRHS(){
			int save = currentPosition;
			return (((currentPosition = save) == save & term (TokenType.VARIABLE_IDENTIFIER)) ||
					((currentPosition = save) == save & literal ()));
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
	
