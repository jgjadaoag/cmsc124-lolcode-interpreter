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
			tokenDetails.add(Tuple.Create(TokenType.YARN_LITERAL, "[a-zA-Z](\w | _)*"));
			tokenDetails.add(Tuple.Create(TokenType.TROOF_LITERAL, "(WIN|FAIL)"));
			tokenDetails.add(Tuple.Create(TokenType.TYPE_LITERAL, "(YARN|NUMBR|NUMBAR|TROOF|NOOB)"));
				tokenDetails.add(Tuple.Create(TokenType.HAI, "HAI"));
				tokenDetails.add(Tuple.Create(TokenType.KTHXBYE, 
				tokenDetails.add(Tuple.Create(TokenType.BTW,
				tokenDetails.add(Tuple.Create(TokenType.OBTW,
				tokenDetails.add(Tuple.Create(TokenType.TLDR,
				tokenDetails.add(Tuple.Create(TokenType.I_HAS_A,
				tokenDetails.add(Tuple.Create(TokenType.ITZ,
				tokenDetails.add(Tuple.Create(TokenType.R,
				tokenDetails.add(Tuple.Create(TokenType.SUM_OF,
				tokenDetails.add(Tuple.Create(TokenType.DIFF_OF,
				tokenDetails.add(Tuple.Create(TokenType.PRODUKT_OF,
				tokenDetails.add(Tuple.Create(TokenType.QUOSHUNT_OF,
				tokenDetails.add(Tuple.Create(TokenType.MOD_OF,
				tokenDetails.add(Tuple.Create(TokenType.BIGGR_OF,
				tokenDetails.add(Tuple.Create(TokenType.SMALLR_OF,
				tokenDetails.add(Tuple.Create(TokenType.BOTH_OF,
				tokenDetails.add(Tuple.Create(TokenType.EITHER_OF,
				tokenDetails.add(Tuple.Create(TokenType.WON_OF,
				tokenDetails.add(Tuple.Create(TokenType.NOT,
				tokenDetails.add(Tuple.Create(TokenType.ALL_OF,
				tokenDetails.add(Tuple.Create(TokenType.ANY_OF,
				tokenDetails.add(Tuple.Create(TokenType.BOTH_SAEM,
				tokenDetails.add(Tuple.Create(TokenType.DIFFRINT,
				tokenDetails.add(Tuple.Create(TokenType.SMOOSH,
				tokenDetails.add(Tuple.Create(TokenType.MAEK,
				tokenDetails.add(Tuple.Create(TokenType.A,
				tokenDetails.add(Tuple.Create(TokenType.IS_NOW_A,
				tokenDetails.add(Tuple.Create(TokenType.VISIBLE,
				tokenDetails.add(Tuple.Create(TokenType.GIMMEH,
				tokenDetails.add(Tuple.Create(TokenType.O_RLY,
				tokenDetails.add(Tuple.Create(TokenType.YA_RLY,
				tokenDetails.add(Tuple.Create(TokenType.MEBBE,
				tokenDetails.add(Tuple.Create(TokenType.NO_WAI,
				tokenDetails.add(Tuple.Create(TokenType.OIC,
				tokenDetails.add(Tuple.Create(TokenType.WTF,
				tokenDetails.add(Tuple.Create(TokenType.OMG, 
				tokenDetails.add(Tuple.Create(TokenType.OMGWTF,
				tokenDetails.add(Tuple.Create(TokenType.STRING_DELIMETER,
				tokenDetails.add(Tuple.Create(TokenType.COMMENT,
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

