using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace Bla
{
	public enum TokenType 
	{
		VARIABLE_IDENTIFIER,
		NUMBR_LITERAL,
		NUMBAR_LITERAL,
		YARN_LITERAL,
		TROOF_LITERAL,
		TYPE_LITERAL,
		HAI,
		KTHXBYE,
		BTW,
		OBTW,
		TLDR,
		I_HAS_A,
		ITZ,
		R,
		SUM_OF,
		DIFF_OF,
		PRODUKT_OF,
		QUOSHUNT_OF,
		MOD_OF,
		BIGGR_OF,
		SMALLR_OF,
		BOTH_OF,
		EITHER_OF,
		WON_OF,
		NOT,
		ALL_OF,
		ANY_OF,
		BOTH_SAEM,
		DIFFRINT,
		SMOOSH,
		MAEK,
		A,
		IS_NOW_A,
		VISIBLE,
		GIMMEH,
		O_RLY,
		YA_RLY,
		MEBBE,
		NO_WAI,
		OIC,
		WTF,
		OMG, 
		OMGWTF,
		STRING_DELIMETER,
		COMMENT,
		UNKNOWN
	}
	public class Token
	{
		string value;
		TokenType type;
		public Token (string value, TokenType type)
		{
			this.value = value;
			this.type = type;
		}
		public string getValue() {
			return value;
		}
		public TokenType getType() {
			return type;
		}
	}
	public class TokenStream {
		readonly string input;
		long currentPosition;
		Dictionary<TokenType, Regex> tokenDetails;
		public TokenStream (string input) {
			this.input = input;
			currentPosition = 0;

			//Filling up token details
			tokenDetails = new Dictionary<TokenType, string> ();
			tokenDetails.Add(TokenType.VARIABLE_IDENTIFIER, new Regex(@"^[a-zA-Z](\w |_)*$"));
			tokenDetails.Add(TokenType.NUMBR_LITERAL, new Regex(@"[-+]?\d+"));
			tokenDetails.Add(TokenType.NUMBAR_LITERAL, new Regex(@"[-+]?\d*\.\d+"));
			/*
			tokenDetails.add(TokenType.YARN_LITERAL,
			tokenDetails.add(TokenType.TROOF_LITERAL,
			tokenDetails.add(TokenType.TYPE_LITERAL,
				tokenDetails.add(TokenType.HAI,
				tokenDetails.add(TokenType.KTHXBYE,
				tokenDetails.add(TokenType.BTW,
				tokenDetails.add(TokenType.OBTW,
				tokenDetails.add(TokenType.TLDR,
				tokenDetails.add(TokenType.I_HAS_A,
				tokenDetails.add(TokenType.ITZ,
				tokenDetails.add(TokenType.R,
				tokenDetails.add(TokenType.SUM_OF,
				tokenDetails.add(TokenType.DIFF_OF,
				tokenDetails.add(TokenType.PRODUKT_OF,
				tokenDetails.add(TokenType.QUOSHUNT_OF,
				tokenDetails.add(TokenType.MOD_OF,
				tokenDetails.add(TokenType.BIGGR_OF,
				tokenDetails.add(TokenType.SMALLR_OF,
				tokenDetails.add(TokenType.BOTH_OF,
				tokenDetails.add(TokenType.EITHER_OF,
				tokenDetails.add(TokenType.WON_OF,
				tokenDetails.add(TokenType.NOT,
				tokenDetails.add(TokenType.ALL_OF,
				tokenDetails.add(TokenType.ANY_OF,
				tokenDetails.add(TokenType.BOTH_SAEM,
				tokenDetails.add(TokenType.DIFFRINT,
				tokenDetails.add(TokenType.SMOOSH,
				tokenDetails.add(TokenType.MAEK,
				tokenDetails.add(TokenType.A,
				tokenDetails.add(TokenType.IS_NOW_A,
				tokenDetails.add(TokenType.VISIBLE,
				tokenDetails.add(TokenType.GIMMEH,
				tokenDetails.add(TokenType.O_RLY,
				tokenDetails.add(TokenType.YA_RLY,
				tokenDetails.add(TokenType.MEBBE,
				tokenDetails.add(TokenType.NO_WAI,
				tokenDetails.add(TokenType.OIC,
				tokenDetails.add(TokenType.WTF,
				tokenDetails.add(TokenType.OMG, 
				tokenDetails.add(TokenType.OMGWTF,
				tokenDetails.add(TokenType.STRING_DELIMETER,
				tokenDetails.add(TokenType.COMMENT,
*/
		}
		public Token get() {
			long startPosition = currentPosition;
			long endPosition = currentPosition;
			string scannedString = "";
			TokenType scannedType;
			for (; endPosition < input.Length; endPosition++) {
				scannedString += this.input [endPosition];
				scannedType = identifyToken (scannedString);
				if (scannedType != TokenType.UNKNOWN) {
					if (Char.IsWhiteSpace (this.input [endPosition + 1])) {
						break;
					}
				}
			}
			return new Token ("", scannedType);
		}

		private TokenType identifyToken(string str) {
			foreach (KeyValuePair<TokenType, Regex> kvp in tokenDetails) {
				if(kvp.Value.IsMatch (str))
					return kvp.Key;
			}
			return TokenType.UNKNOWN;
		}
	}
}

