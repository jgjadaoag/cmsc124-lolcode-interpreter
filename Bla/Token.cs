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
		int currentPosition;
		Dictionary<TokenType, Regex> tokenDetails;
		public TokenStream (string input) {
			this.input = input;
			Console.WriteLine ("Input Length: " + input.Length.ToString ());
			currentPosition = 0;

			//Filling up token details
			tokenDetails = new Dictionary<TokenType, Regex> ();
			tokenDetails.Add (TokenType.HAI, new Regex (@"^HAI"));
			tokenDetails.Add (TokenType.KTHXBYE, new Regex (@"^KTHXBYE"));
			tokenDetails.Add(TokenType.BTW,new Regex(@"^BTW$"));
			tokenDetails.Add(TokenType.OBTW,new Regex(@"^OBTW$"));
			tokenDetails.Add(TokenType.TLDR,new Regex(@"^TLDR$"));
			tokenDetails.Add(TokenType.I_HAS_A,new Regex(@"^I HAS A$"));
			tokenDetails.Add(TokenType.ITZ,new Regex(@"ITZ^$"));
			tokenDetails.Add(TokenType.R,new Regex(@"^R$"));
			tokenDetails.Add(TokenType.SUM_OF,new Regex(@"^SUM OF$"));
			tokenDetails.Add(TokenType.DIFF_OF,new Regex(@"^DIFF OF$"));
			tokenDetails.Add(TokenType.PRODUKT_OF,new Regex(@"^PRODUKT OF$"));
			tokenDetails.Add(TokenType.QUOSHUNT_OF,new Regex(@"^QUOSHUNT OF$"));
			tokenDetails.Add(TokenType.MOD_OF, new Regex(@"^MOD OF$"));
			tokenDetails.Add(TokenType.BIGGR_OF, new Regex(@"^BIGGR OF$"));
			tokenDetails.Add(TokenType.SMALLR_OF, new Regex(@"^SMALLR OF$"));
			tokenDetails.Add(TokenType.BOTH_OF, new Regex(@"^BOTH OF$"));
			tokenDetails.Add(TokenType.EITHER_OF, new Regex(@"^EITHER OF$"));
			tokenDetails.Add(TokenType.WON_OF, new Regex(@"^WON OF$"));
			tokenDetails.Add(TokenType.NOT, new Regex(@"^NOT$"));
			tokenDetails.Add(TokenType.ALL_OF,new Regex(@"^ALL OF$"));
			tokenDetails.Add(TokenType.ANY_OF, new Regex(@"^ANY OF$"));
			tokenDetails.Add(TokenType.BOTH_SAEM, new Regex(@"^BOTH SAEM$"));
			tokenDetails.Add(TokenType.DIFFRINT, new Regex(@"^DIFFRINT$"));
			tokenDetails.Add(TokenType.SMOOSH, new Regex(@"^SMOOSH$"));
			tokenDetails.Add(TokenType.MAEK, new Regex(@"^MAEK$"));
			tokenDetails.Add(TokenType.A, new Regex(@"^A$"));
			tokenDetails.Add(TokenType.IS_NOW_A, new Regex(@"^IS NOW A$"));
			tokenDetails.Add(TokenType.VISIBLE, new Regex(@"^VISIBLE$"));
			tokenDetails.Add(TokenType.GIMMEH, new Regex(@"^GIMMEH$"));
			tokenDetails.Add(TokenType.O_RLY, new Regex(@"^O RLY$"));
			tokenDetails.Add(TokenType.YA_RLY, new Regex(@"^YA RLY$"));
			tokenDetails.Add(TokenType.MEBBE, new Regex(@"^MEBBE$"));
			tokenDetails.Add(TokenType.NO_WAI, new Regex(@"^NO WAI$"));
			tokenDetails.Add(TokenType.OIC, new Regex(@"^OIC$"));
			tokenDetails.Add(TokenType.WTF, new Regex(@"^WTF$"));
			tokenDetails.Add(TokenType.OMG, new Regex(@"^OMG$"));
			tokenDetails.Add(TokenType.OMGWTF, new Regex(@"^OMGWTF$"));
			tokenDetails.Add(TokenType.STRING_DELIMETER, new Regex(@"^\""$"));
		//	tokenDetails.Add (TokenType.COMMENT, new Regex (@"^"$"));
			tokenDetails.Add(TokenType.VARIABLE_IDENTIFIER, new Regex(@"^[a-zA-Z](\w|_)*$"));
			tokenDetails.Add(TokenType.NUMBR_LITERAL, new Regex(@"^[-+]?\d+$"));
			tokenDetails.Add(TokenType.NUMBAR_LITERAL, new Regex(@"^[-+]?\d*\.\d+$"));
			tokenDetails.Add(TokenType.YARN_LITERAL, new Regex(@"^[a-zA-Z](\w | _)*"));
			tokenDetails.Add (TokenType.TROOF_LITERAL, new Regex (@"^(WIN|FAIL)"));
			tokenDetails.Add (TokenType.TYPE_LITERAL, new Regex (@"^(YARN|NUMBR|NUMBAR|TROOF|NOOB)"));
			skipSpace ();
		}
		public Token get() {
			int endPosition = currentPosition;
			string scannedString = "";
			TokenType scannedType = TokenType.UNKNOWN;
			for (; endPosition < input.Length; endPosition++) {
				Console.WriteLine ("endPosition: " + endPosition.ToString ());
				scannedString += this.input [endPosition];
				scannedType = identifyToken (scannedString);
				if (scannedType != TokenType.UNKNOWN) {
					if ((endPosition == input.Length - 1) || 
						(Char.IsWhiteSpace (this.input [endPosition + 1]))) {
						Console.WriteLine ("aa endPosition: " + endPosition.ToString ());
						currentPosition = endPosition + 1;
						break;
					}
				}
			}

			skipSpace ();

			return new Token (scannedString, scannedType);
		}

		public bool end() {
			if (currentPosition == input.Length)
				return true;
			else
				return false;
		}

		private void skipSpace(){
			while (currentPosition < input.Length && Char.IsWhiteSpace (input [currentPosition])) {
				currentPosition++;
			}
		}
		private TokenType identifyToken(string str) {
			Console.WriteLine ("Matching " + str);
			foreach (KeyValuePair<TokenType, Regex> kvp in tokenDetails) {
				if (kvp.Value.IsMatch (str)) {
					Console.WriteLine ("Matched to " + kvp.Key.ToString());
					return kvp.Key;
				}
			}
			Console.WriteLine ("Matched to " + TokenType.UNKNOWN.ToString());
			return TokenType.UNKNOWN;
		}
	}
}

