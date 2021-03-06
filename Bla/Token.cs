﻿using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace Bla
{
	public enum TokenType 
	{
		UNKNOWN,
		VARIABLE_IDENTIFIER,
		NUMBR_LITERAL,
		NUMBAR_LITERAL,
		YARN_LITERAL,
		TROOF_LITERAL,
		TYPE_LITERAL,
		HAI,
		KTHXBYE,
		EXCLAMATION,
		UPPIN,
		NERFIN,
		TLDR,
		ITZ,
		R,
		NOT,
		DIFFRINT,
		SMOOSH,
		MAEK,
		A,
		VISIBLE,
		GIMMEH,
		MEBBE,
		OIC,
		WTF,
		OMG, 
		OMGWTF,
		GTFO,
		YR,
		TIL,
		WILE,
		STRING_DELIMETER,
		STATEMENT_DELIMETER,
		AN,
		LINE_COMMENT,
		BLOCK_COMMENT,
		MKAY,
		BTW,
		OBTW,
		I_HAS_A,
		IM_IN_YR,
		IM_OUTTA_YR,
		HOW_IZ_I,
		IF_U_SAY_SO,
		I_IZ,
		FOUND_YR,
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
		ALL_OF,
		ANY_OF,
		IS_NOW_A,
		BOTH_SAEM,
		O_RLY,
		YA_RLY,
		NO_WAI
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
		public readonly Dictionary<TokenType, Regex> tokenDetails;
		bool stringFlag;
		bool stringRead;

		public TokenStream (string input) {
			this.input = input;
			Console.WriteLine ("Input Length: " + input.Length.ToString ());
			currentPosition = 0;
			stringFlag = false;
			stringRead = false;

			//Filling up token details
			tokenDetails = new Dictionary<TokenType, Regex> ();
			tokenDetails.Add(TokenType.HAI, new Regex (@"^HAI$"));
			tokenDetails.Add(TokenType.KTHXBYE, new Regex (@"^KTHXBYE$"));
			tokenDetails.Add(TokenType.BTW,new Regex(@"^BTW$"));
			tokenDetails.Add(TokenType.OBTW,new Regex(@"^OBTW$"));
			tokenDetails.Add(TokenType.TLDR,new Regex(@"^TLDR$"));
			tokenDetails.Add(TokenType.I_HAS_A,new Regex(@"^I HAS A"));
			tokenDetails.Add(TokenType.ITZ,new Regex(@"^ITZ$"));
			tokenDetails.Add(TokenType.R,new Regex(@"^R$"));
			tokenDetails.Add(TokenType.SUM_OF,new Regex(@"^SUM OF"));
			tokenDetails.Add(TokenType.DIFF_OF,new Regex(@"^DIFF OF"));
			tokenDetails.Add(TokenType.PRODUKT_OF,new Regex(@"^PRODUKT OF"));
			tokenDetails.Add(TokenType.QUOSHUNT_OF,new Regex(@"^QUOSHUNT OF"));
			tokenDetails.Add(TokenType.MOD_OF, new Regex(@"^MOD OF"));
			tokenDetails.Add(TokenType.BIGGR_OF, new Regex(@"^BIGGR OF"));
			tokenDetails.Add(TokenType.SMALLR_OF, new Regex(@"^SMALLR OF"));
			tokenDetails.Add(TokenType.BOTH_OF, new Regex(@"^BOTH OF"));
			tokenDetails.Add(TokenType.EITHER_OF, new Regex(@"^EITHER OF"));
			tokenDetails.Add(TokenType.WON_OF, new Regex(@"^WON OF"));
			tokenDetails.Add(TokenType.NOT, new Regex(@"^NOT$"));
			tokenDetails.Add(TokenType.UPPIN, new Regex(@"^UPPIN$"));
			tokenDetails.Add(TokenType.NERFIN, new Regex(@"^NERFIN$"));
			tokenDetails.Add(TokenType.ALL_OF,new Regex(@"^ALL OF"));
			tokenDetails.Add(TokenType.ANY_OF, new Regex(@"^ANY OF"));
			tokenDetails.Add(TokenType.BOTH_SAEM, new Regex(@"^BOTH SAEM"));
			tokenDetails.Add(TokenType.DIFFRINT, new Regex(@"^DIFFRINT$"));
			tokenDetails.Add(TokenType.SMOOSH, new Regex(@"^SMOOSH$"));
			tokenDetails.Add(TokenType.MAEK, new Regex(@"^MAEK$"));
			tokenDetails.Add(TokenType.A, new Regex(@"^A$"));
			tokenDetails.Add(TokenType.IS_NOW_A, new Regex(@"^IS NOW A"));
			tokenDetails.Add(TokenType.VISIBLE, new Regex(@"^VISIBLE$"));
			tokenDetails.Add(TokenType.GIMMEH, new Regex(@"^GIMMEH$"));
			tokenDetails.Add(TokenType.O_RLY, new Regex(@"^O RLY?"));
			tokenDetails.Add(TokenType.YA_RLY, new Regex(@"^YA RLY"));
			tokenDetails.Add(TokenType.MEBBE, new Regex(@"^MEBBE$"));
			tokenDetails.Add(TokenType.NO_WAI, new Regex(@"^NO WAI"));
			tokenDetails.Add(TokenType.OIC, new Regex(@"^OIC$"));
			tokenDetails.Add(TokenType.WTF, new Regex(@"^WTF\?$"));
			tokenDetails.Add(TokenType.OMG, new Regex(@"^OMG$"));
			tokenDetails.Add(TokenType.OMGWTF, new Regex(@"^OMGWTF$"));
			tokenDetails.Add(TokenType.GTFO, new Regex(@"^GTFO$"));
			tokenDetails.Add(TokenType.STRING_DELIMETER, new Regex(@"^\""$"));
			tokenDetails.Add (TokenType.LINE_COMMENT, new Regex (@"^BTW.*\n$"));
			tokenDetails.Add (TokenType.BLOCK_COMMENT, new Regex (@"^OBTW.*TLDR$"));
			tokenDetails.Add (TokenType.AN, new Regex (@"^AN$"));
			tokenDetails.Add (TokenType.MKAY, new Regex (@"^MKAY$"));
			tokenDetails.Add(TokenType.VARIABLE_IDENTIFIER, new Regex(@"^[a-zA-Z](\w|_)*$"));
			tokenDetails.Add(TokenType.NUMBR_LITERAL, new Regex(@"^[-+]?\d+$"));
			tokenDetails.Add(TokenType.NUMBAR_LITERAL, new Regex(@"^[-+]?\d*\.\d+$"));
			tokenDetails.Add(TokenType.YARN_LITERAL, new Regex(@"[\S 	]*\"""));
			tokenDetails.Add (TokenType.TROOF_LITERAL, new Regex (@"^(WIN|FAIL)$"));
			tokenDetails.Add (TokenType.TYPE_LITERAL, new Regex (@"^(YARN|NUMBR|NUMBAR|TROOF|NOOB)"));
			tokenDetails.Add (TokenType.STATEMENT_DELIMETER, new Regex (@"^(\n|,)$"));
			tokenDetails.Add (TokenType.HOW_IZ_I, new Regex (@"^HOW IZ I"));
			tokenDetails.Add (TokenType.I_IZ, new Regex (@"^I IZ"));
			tokenDetails.Add (TokenType.IF_U_SAY_SO, new Regex (@"^IF U SAY SO"));
			tokenDetails.Add (TokenType.FOUND_YR, new Regex (@"^FOUND YR"));
			tokenDetails.Add (TokenType.YR, new Regex (@"^YR$"));
			tokenDetails.Add (TokenType.IM_IN_YR, new Regex (@"^IM IN YR"));
			tokenDetails.Add (TokenType.IM_OUTTA_YR, new Regex (@"^IM OUTTA YR"));
			tokenDetails.Add (TokenType.TIL, new Regex (@"^TIL$"));
			tokenDetails.Add (TokenType.WILE, new Regex (@"^WILE$"));
			tokenDetails.Add (TokenType.EXCLAMATION, new Regex (@"^!$"));

			skipSpace ();
		}

		public Token get() {
			int endPosition = currentPosition;
			string scannedString = "";

			if (stringFlag ) {
				if (input [currentPosition] != '"' && !stringRead) {
					stringRead = true;
					return readString (input.Substring (currentPosition));
				} else if (!stringRead) {
					stringRead = true;
					return new Token ("", TokenType.YARN_LITERAL);
				} else {
					stringFlag = false;
					stringRead = false;
					if ((currentPosition == input.Length - 1) ||
						(Char.IsWhiteSpace (this.input [currentPosition + 1])) ||
						(input[currentPosition + 1] == ',')) {
						currentPosition++;
						skipSpace ();
						return new Token ("\"", TokenType.STRING_DELIMETER);
					}
					return new Token ("", TokenType.UNKNOWN);
				}
			}
			 
			TokenType scannedType = identifyToken (input.Substring (currentPosition));

			//If the scanned token has more than one word
			if (scannedType > TokenType.OBTW) {
				string rgx = tokenDetails [scannedType].ToString ();
				Console.WriteLine (scannedString);
				if (rgx [rgx.Length - 1] != '$') {
					//try{
					/*
						scannedString = input.Substring (currentPosition, tokenDetails [scannedType].ToString ().Length - 1);
						currentPosition += tokenDetails [scannedType].ToString ().Length - 1;
						skipSpace ();
						return new Token (scannedString, scannedType);
						*/
					scannedString = input.Substring (currentPosition, tokenDetails [scannedType].ToString ().Length - 1);
					int save = currentPosition;
					currentPosition += tokenDetails [scannedType].ToString ().Length - 2;

					if ((currentPosition == input.Length - 1) ||
						(Char.IsWhiteSpace (this.input [currentPosition + 1])) ||
						(input[currentPosition + 1] == ',')) {
						currentPosition++;
						skipSpace ();
						return new Token (scannedString, scannedType);
					}
					currentPosition = save;
					scannedString = "";
					//} catch (Exception a){}
				}
			}
			if (stringFlag == false) {
				for (; endPosition < input.Length; endPosition++) {
				
					Console.WriteLine ("endPosition: " + endPosition.ToString ());
					scannedString += this.input [endPosition];
					scannedType = identifyToken (scannedString);

					if ((endPosition == input.Length - 1) ||
					   (Char.IsWhiteSpace (this.input [endPosition + 1])) ||
					   (scannedType == TokenType.STRING_DELIMETER) ||
						(scannedType == TokenType.STATEMENT_DELIMETER) ||
						(input[endPosition + 1] == ',')) {
						currentPosition = endPosition + 1;
						break;
					}
				}
					
			}

			if(scannedType == TokenType.BTW){
				while (currentPosition < input.Length && input [currentPosition++] != '\n')
					;
				currentPosition--;
				
			} else if(scannedType == TokenType.OBTW){
				int tldrPosition = input.Substring (currentPosition).IndexOf ("TLDR");

				if (tldrPosition < 0) {
					currentPosition = input.Length;
				} else {
					currentPosition += 4 + tldrPosition;
				}
			}

			if(!stringFlag)
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
			while (currentPosition < input.Length && Char.IsWhiteSpace (input [currentPosition]) && input[currentPosition] != '\n') {
				currentPosition++;
			}
		}
			
		private Token readString(string str){
			string holder = "";

			char previous = '\0';
			foreach(char c in str){
				if(c == '"' && previous != ':'){
					currentPosition += holder.Length;
					return new Token(holder, TokenType.YARN_LITERAL);
				} else if(c == '\n'){
					MainClass.win.displayTextToConsole ("Error: Unterminated yarn."); 
				} else {
					holder += c;
				}
					
				Console.WriteLine (holder);
				previous = c;
			}

			return new Token ("", TokenType.UNKNOWN);
		}

		private TokenType identifyToken(string str) {
			Console.WriteLine ("Matching " + str);
			Console.WriteLine("string flag: " +  stringFlag);

			if (stringFlag == true) {
				stringFlag = false;
				return TokenType.STRING_DELIMETER;
			}

			bool variableFlag = false; 

			foreach (KeyValuePair<TokenType, Regex> kvp in tokenDetails) {
				if (kvp.Value.IsMatch (str)) {
					Console.WriteLine ("Matched to " + kvp.Key.ToString());
					if (kvp.Key == TokenType.STRING_DELIMETER) {
						stringFlag = true;
					}
					if (kvp.Key == TokenType.YARN_LITERAL)
						continue;
					if (kvp.Key == TokenType.VARIABLE_IDENTIFIER) {
						variableFlag = true;
						continue;
					}
					return kvp.Key;
				}
			}
			if (variableFlag)
				return TokenType.VARIABLE_IDENTIFIER;
			return TokenType.UNKNOWN;
		}
	}
}

