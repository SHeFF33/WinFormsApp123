using System.Data;
using System.IO;
using System.Linq.Expressions;
using System.Reflection.Metadata;
using System.Text.RegularExpressions;
using static System.Windows.Forms.LinkLabel;

namespace WinFormsApp123
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            form = this;
        }
        public List<string> forToken = new List<string>();
        public List<string> listBuf = new List<string>();
        public List<char> forChar = new List<char>();
        List<Token> tokens = new List<Token>();
        List<string> lexemes = new List<string>();
        string str;
        string type;
        char ch;
        Token token;
        private void button1_Click(object sender, EventArgs e)
        {
            string text = "";
            using (StreamReader fs = new StreamReader(textBox1.Text))
            {
                while (true)
                {
                    string temp = fs.ReadLine();
                    if (temp == null) break;
                    text += temp;
                    text += " \n  ";
                }
                richTextBox1.Text = text;
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            lexemes = new List<string>();
            string subText = "";

            bool IsFinishLexemAnalis = true;

            foreach (char s in richTextBox1.Text)
            {
                    if (Lexems.IsIDVariable(subText) && (s == '.' || s == ' ' || s == ',' || s == '<' || s == ':' || s == '>' || s == ';' || s == '+' || s == '-' || s == '*' || s == '/' || s =='\n'))
                    {
                        lexemes.Add(subText + " - Идентификатор;");
                        subText = "";
                    }
                    else if (Lexems.IsLiteral(subText) && (s == ' ' || s == ';' || s == ')' || s == '\n'))
                    {
                        lexemes.Add(subText + " - Литератор;");
                        subText = "";
                    }
                    else if (Lexems.IsSeparator(subText) && (s == ' ' || s == ')' || s == '\n' || char.IsDigit(s) || char.IsLetter(s)))
                    {
                        if (subText != "\n")
                        {
                            lexemes.Add(subText + " - Разделитель;");
                        }
                        subText = "";
                    }
                else if (subText == Environment.NewLine || subText == " " || s == '\n')
                {
                    subText = "";
                }
                if (subText.Length > 8)
                {
                    IsFinishLexemAnalis = false;
                }
                subText += s;
            }
            if (IsFinishLexemAnalis == true)
                MessageBox.Show("Лексический анализ завершен успешно!");

            if (IsFinishLexemAnalis == false)
                MessageBox.Show("ERROR!\nЛексический анализ приостановлен!\nОбнаружена ошибка!");
            richTextBox2.Clear();

            int i = 0;
            foreach (string lexem in lexemes)
            {
                i++;
                richTextBox2.Text += i + " " + lexem + Environment.NewLine;
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            bool IsFinishLexemAnalis = true;
            richTextBox3.Clear();
            listBuf = new List<string>();
            forToken = new List<string>();
            forChar = new List<char>();
            int i = 0;
            string subText = " ";
            foreach (char s in richTextBox1.Text)
            {
                    if (Lexems.IsOperator(subText) && (s == '.' || s == ':' || s == ',' || s == ' ' || s == '(' || s == ' ' || s == '<' || s == '>' || s == ';' || s == '\n'))
                    {
                        i++;
                        listBuf.Add(subText + " ");
                        forToken.Add("I");
                        forChar.Add(' ');

                        subText = "";
                    }
                    else if (Lexems.IsLiteral(subText) && (s == ' ' || s == ';' || s == ')' || s == '\n'))
                    {
                        i++;
                        listBuf.Add(subText + " ");
                        forToken.Add("D");
                        forChar.Add(' ');

                        subText = "";
                    }
                    else if (Lexems.IsSeparator(subText) && (s == ' ' || s == ')' || s == '\n' || char.IsDigit(s) || char.IsLetter(s)))
                    {
                        i++;
                        if (subText != "\n")
                        {
                            listBuf.Add(subText + " ");
                            forToken.Add("R");
                            forChar.Add(s);
                        }
                        subText = "";
                    }

                    else if (Lexems.IsIDVariable(subText) && !Lexems.IsOperator(subText) && (s == '.' || s == ' ' || s == ',' || s == '<' || s == ':' || s == '>' || s == ';' || s == '+' || s == '-' || s == '*' || s == '/' || s == '\n'))
                    {
                        i++;
                        listBuf.Add(subText + " ");
                        forToken.Add("P");
                        forChar.Add(' ');
                        subText = "";
                    }

                    else if (subText == Environment.NewLine || subText == " " || s == '\n')
                    {
                        subText = "";
                    }
                if (subText.Length > 8)
                {
                    IsFinishLexemAnalis = false;
                }
                subText += s;
            }
            if (IsFinishLexemAnalis == true)
                MessageBox.Show("Классификация лексем завершена успешно!");

            if (IsFinishLexemAnalis == false)
                MessageBox.Show("ERROR!\nКлассификация лексем не завершена!");
            tokens.Clear();
            for (i = 0; i < listBuf.Count; i++)
            {
                str = listBuf[i].Split(' ')[0];
                type = forToken[i];

                if (type == "I")
                {
                    try
                    {
                        if (Token.IsSpecialWord(str))
                        {
                            Token token = new Token(Token.SpecialWords[str]);
                            token.Qwerty = str;
                            tokens.Add(token);
                        }
                    }
                    catch (Exception)
                    {
                        richTextBox3.Text += $"Непредвиденная ошибка в поиске специального слова\n";
                    }
                }
                else if (type == "D")
                {
                    try
                    {
                        token = new Token(Token.TokenType.NUMBER);
                        token.Value = str;
                        token.Qwerty = str;
                        tokens.Add(token);
                        continue;
                    }
                    catch (Exception)
                    {
                        richTextBox3.Text += $"Непредвиденная ошибка в поиске литерала";
                    }
                }
                else if (type == "P")
                {
                    try
                    {
                        token = new Token(Token.TokenType.VARIABLE);
                        token.Value = str;
                        token.Qwerty = str;
                        tokens.Add(token);
                        continue;
                    }
                    catch (Exception)
                    {
                        richTextBox3.Text += $"Непредвиденная ошибка в поиске переменной";
                    }
                }
                else if (type == "R")
                {
                    try
                    {
                        if (Token.IsSpecialSymbol(str))
                        {
                            token = new Token(Token.SpecialSymbols[str]);
                            token.Qwerty = str;
                            tokens.Add(token);
                            continue;
                        }
                    }
                    catch (Exception)
                    {
                        richTextBox3.Text += $"Непредвиденная ошибка в поиске разделителей";
                    }
                }
            }
            Token.PrintTokens(richTextBox3, tokens);
        }
        private void button5_Click(object sender, EventArgs e)
        {
            richTextBox4.Clear();
            LR recognizer = new LR(tokens);
            recognizer.Start();
            lexemes.Clear();
            tokens.Clear();
        }
        public void richTextBox4_TextChanged(object sender, EventArgs e)
        {

        }

        public static Form1 form;

        public void Conclusion(string text)
        {
            richTextBox4.Text += text + Environment.NewLine;
        }
    }
}

