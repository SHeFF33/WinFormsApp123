using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp123
{
    public class Token
    {
        public TokenType Type;
        public string Value;
        public string Qwerty;
        public Token(TokenType type)
        {
            Type = type;
        }
        public override string ToString()
        {
            return string.Format("{0} - {1}", Value, Type);
        }
        public enum TokenType
        {
            FOR, VAR, INTEGER, REAL, DOUBLE, DO, NUMBER, BEGIN, END, NETERM, EXPR,
            TO, PLUS,
            MINUS, EQUAL, SEMICOLON, MULTIPLY, COMMA, DIVISION, POINT, COLON, VARIABLE, COLONEQUAL, ASSIGNMENT, LPAR, RPAR
        }
        public static TokenType[] Delimiters = new TokenType[]
            { 
               TokenType.PLUS, TokenType.MINUS,
                TokenType.EQUAL,TokenType.SEMICOLON, TokenType.MULTIPLY,
                TokenType.COMMA,TokenType.DIVISION, TokenType.POINT, TokenType.COLON, TokenType.ASSIGNMENT
            };
        public static TokenType[] Words = new TokenType[]
           {
               TokenType.FOR, TokenType.VAR,
                TokenType.INTEGER, TokenType.DO, TokenType.BEGIN,
                TokenType.END, TokenType.TO
           };
        public static Dictionary<string, TokenType> SpecialWords = new Dictionary<string, TokenType>() 
        {
            { "integer", TokenType.INTEGER },
            { "real", TokenType.REAL },
            { "double", TokenType.DOUBLE },
            { "for", TokenType.FOR },
            { "var", TokenType.VAR },
            { "do", TokenType.DO },
            { "to", TokenType.TO },
            { "begin", TokenType.BEGIN },
            { "end", TokenType.END }
        };
        public static bool IsSpecialWord(string word)
        {
            if (string.IsNullOrEmpty(word))
            {
                return false;
            }
            return (SpecialWords.ContainsKey(word));
        }
        public static Dictionary<string, TokenType> SpecialSymbols = new Dictionary<string, TokenType>()
        {
            { "+", TokenType.PLUS },
            { "-", TokenType.MINUS },
            { "=", TokenType.EQUAL },
            { "*", TokenType.MULTIPLY },
            { "(", TokenType.LPAR },
            { ")", TokenType.RPAR },
            { "/", TokenType.DIVISION },
            { ":=", TokenType.ASSIGNMENT},
            { ",", TokenType.COMMA },
            { ":", TokenType.COLON },
            { ".", TokenType.POINT },
            { ";", TokenType.SEMICOLON },
        };
        public static bool IsSpecialSymbol(string str)
        {
            return SpecialSymbols.ContainsKey(str);
        }
        public static void PrintTokens(System.Windows.Forms.RichTextBox richtextbox, List<Token> list)
        {
            int i = 0;
            richtextbox.Text = "";
            foreach (var t in list)
            {
                i++;
                richtextbox.Text += $"{i} {t}";
                richtextbox.Text += Environment.NewLine;
            }
        }
    }
}
