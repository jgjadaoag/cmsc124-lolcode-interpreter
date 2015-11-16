﻿using System;
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
			return (((currentPosition = save) == save | vardec()));
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
	