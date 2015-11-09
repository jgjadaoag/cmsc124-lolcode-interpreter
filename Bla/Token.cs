using System;
using System.Text.RegularExpressions;

namespace Bla
{
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
		STRING_DELIMETER
	}
	public class TokenStream {
		string input;
		long currentPosition;
		public TokenStream (string input) {
			this.input = input;
			currentPosition = 0;
		}
		public Token get() {
			for (; currentPosition < input.Length; currentPosition++) {
			}
		}
	}
}

