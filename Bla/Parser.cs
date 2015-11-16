using System;
using System.Collections.Generic;

namespace Bla
{
	public class Parser
	{
		List<Token> tokenList;
		int currentPosition;

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

			Console.WriteLine (tokenList);
		}

		public bool parse(){
			return program ();
		}

		bool program(){
			return term(TokenType.HAI) && statement() && term(TokenType.KTHXBYE);
		}

		bool term(TokenType tokType){
			return tokenList[currentPosition++].getType() == tokType;
		}

		bool codeBlock(){
			return false;
		}

		bool statement(){
			int save = currentPosition;
			return (((currentPosition = save) == save | vardec()) ||
					((currentPosition = save) == save | expression()) ||
					((currentPosition = save) == save | input()) ||
					((currentPosition = save) == save | output()) ||
					((currentPosition = save) == save | concatenation())
					);
		}

		bool variableAssignment(){
			return term (TokenType.VARIABLE_IDENTIFIER) && term (TokenType.R) && expression ();
		}

		bool input(){
			return term (TokenType.GIMMEH) && term (TokenType.VARIABLE_IDENTIFIER);
		}

		bool output(){
			return term (TokenType.VISIBLE) && expression();
		}

		bool concatenation(){
			int save = currentPosition;
			return(((currentPosition = save) == save | term(TokenType.SMOOSH) && stringList() && term(TokenType.MKAY)) ||
				((currentPosition = save) == save | term(TokenType.SMOOSH) && stringList())
				);
		}

		bool stringList(){
			int save = currentPosition;
			return (((currentPosition = save) == save | term(TokenType.YARN_LITERAL) && term(TokenType.AN) && stringList()) ||
					((currentPosition = save) == save | term(TokenType.YARN_LITERAL)) ||
					((currentPosition = save) == save | term(TokenType.YARN_LITERAL) && stringList())
					);
		}

		bool expression(){
			int save = currentPosition;
			return (((currentPosition = save) == save | term(TokenType.VARIABLE_IDENTIFIER)) ||
					((currentPosition = save) == save | mathOperator()) ||
					((currentPosition = save) == save | literal())
					);
		}

		bool mathOperator(){
			int save = currentPosition;
			return (((currentPosition = save) == save | addition()) ||
					((currentPosition = save) == save | subtraction()) ||
					((currentPosition = save) == save | multiplication()) ||
					((currentPosition = save) == save | division()) ||
					((currentPosition = save) == save | modulo()) ||
					((currentPosition = save) == save | maximum()) ||
					((currentPosition = save) == save | minimum())
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

		bool vardec(){
			int save = currentPosition;
			return (((currentPosition = save) == save | term(TokenType.I_HAS_A) && term(TokenType.VARIABLE_IDENTIFIER) && term(TokenType.ITZ) && assignRHS()) ||
				((currentPosition = save) == save | term(TokenType.I_HAS_A) && term(TokenType.VARIABLE_IDENTIFIER)) );
		}

		bool assignRHS(){
			int save = currentPosition;
			return (((currentPosition = save) == save | term (TokenType.VARIABLE_IDENTIFIER)) ||
					((currentPosition = save) == save | literal ()));
		}

		bool literal(){
			int save = currentPosition;
			return (((currentPosition = save) == save | term (TokenType.NUMBR_LITERAL)) || 
					((currentPosition = save) == save | term(TokenType.NUMBAR_LITERAL)) || 
					((currentPosition = save) == save | term(TokenType.YARN_LITERAL)) || 
					((currentPosition = save) == save | term(TokenType.TROOF_LITERAL)) );
		}
			
	}
}
	