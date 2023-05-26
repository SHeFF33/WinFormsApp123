using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinFormsApp123;

namespace WinFormsApp123
{
    internal class LL
    {
        List<Token> token;
        public bool Succes = false;
        int i;
        public LL(List<Token> tokens)
        {
            this.token = tokens;
        }
        public void Start()
        {
            i = 0;
            try
            {
                Programm();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: #{ex.Message}");
                MessageBox.Show($"Errror! #{ex.Message}");
            }
        }
        public void Programm()
        {
            Succes = false;
            if (token[i].Type != Token.TokenType.VAR)
                throw new Exception($"Error!\nSTRING: {i + 1} - Ожидалось: var, а получено: {token[i].Qwerty}");
            Next();
            SpisOpis();
            if (token[i].Type != Token.TokenType.BEGIN)
                throw new Exception($"Error!\nSTRING: {i + 1} - Ожидалось: begin, а получено: {token[i].Qwerty}");
            Next();
            SpisOper();
            if (token[i].Type != Token.TokenType.END)
                throw new Exception($"Error!\nSTRING: {i + 1} - Ожидалось: end, а получено: {token[i].Qwerty}");
            Next();

            if (token[i].Type != Token.TokenType.POINT)
                throw new Exception($"Error!\nSTRING: {i + 1} - Ожидалось: точка, а получено: {token[i].Qwerty}");
            Succes = true;
        }

        public void SpisOpis()
        {
            if (token[i].Type != Token.TokenType.VARIABLE)
                throw new Exception($"Error!\nSTRING: {i + 1} - Ожидалось: переменная, а получено: {token[i].Qwerty}");
            Opis();
            Alt();
        }
        public void Opis()
        {
            SpisPerem();
            if (token[i].Type != Token.TokenType.COLON)
                throw new Exception($"Error!\nSTRING: {i + 1} - Ожидалось: двоеточие, а получено: {token[i].Qwerty}");
            Next();
            Type();
            if (token[i].Type != Token.TokenType.SEMICOLON)
                throw new Exception($"Error!\nSTRING: {i + 1} - Ожидалось: точка с запятой, а получено: {token[i].Qwerty}");
            Next();
        }

        public void Alt()
        {
            switch (token[i].Type)
            {
                case Token.TokenType.BEGIN:
                    break;

                case Token.TokenType.VARIABLE:
                    SpisOpis();
                    break;

                default: throw new Exception($"Error!\nSTRING: {i + 1} - Ожидалось: переменная, а получено: {token[i].Qwerty}");
            }
        }

        public void SpisOper()
        {
            if (token[i].Type != Token.TokenType.VARIABLE && token[i].Type != Token.TokenType.FOR)
                throw new Exception($"Error!\nSTRING: {i + 1} - Ожидалось: for или переменная, а получено: {token[i].Qwerty}");
            Oper();
            DopOper();
        }

        public void SpisPerem()
        {
            if (token[i].Type != Token.TokenType.VARIABLE)
                throw new Exception($"Error!\nSTRING: {i + 1} - Ожидалось: переменная, а получено: {token[i].Qwerty}");
            Next();

            X();
        }

        public void X()
        {
            switch (token[i].Type)
            {
                case Token.TokenType.COLON:
                    break;

                case Token.TokenType.COMMA:
                    Alt2();
                    break;

                default: throw new Exception($"Error!\nSTRING: {i + 1} - Ожидалось: двоеточие или запятая а получено: {token[i].Qwerty}");
            }
        }

        public void Alt2()
        {
            if (token[i].Type != Token.TokenType.COMMA)
                throw new Exception($"Error!\nSTRING: {i + 1} - Ожидалось: запятая, а получено: {token[i].Qwerty}");
            Next();

            if (token[i].Type != Token.TokenType.VARIABLE)
                throw new Exception($"Error!\nSTRING: {i + 1} - Ожидалось: переменная, а получено: {token[i].Qwerty}");
            Next();

            X();
        }

        public void Oper()
        {
            switch (token[i].Type)
            {
                case Token.TokenType.FOR:
                    For();
                    break;

                case Token.TokenType.VARIABLE:
                    Prisv();
                    break;

                case Token.TokenType.BEGIN:
                    Next();
                    break;

                default: throw new Exception($"Error!\nSTRING: {i + 1} - Ожидалось: for или переменная, а получено: {token[i].Qwerty}");
            }
        }

        public void DopOper()
        {
            if (token[i].Type == Token.TokenType.SEMICOLON)
                Next();
            switch (token[i].Type)
            {
                case Token.TokenType.END:
                    break;
                case Token.TokenType.SEMICOLON: //
                    break;
                case Token.TokenType.VARIABLE:
                    DopOper2();
                    break;
                case Token.TokenType.FOR:
                    DopOper2();
                    break;
                case Token.TokenType.BEGIN:
                    Next();
                    break;
                default: throw new Exception($"Error!\nSTRING: {i + 1} - Ожидалось: for, end или переменная, а получено: {token[i].Qwerty}");
            }
        }
        public void For()
        {
            if (token[i].Type != Token.TokenType.FOR)
                throw new Exception($"Error!\nSTRING: {i + 1} - Ожидалось: for, а получено: {token[i].Qwerty}");
            Next();
            Expr1();
            if (token[i].Type != Token.TokenType.TO)
                throw new Exception($"Error!\nSTRING: {i + 1} - Ожидалось: to, а получено: {token[i].Qwerty}");
            Next();
            Operand();
            if (token[i].Type != Token.TokenType.DO)
                throw new Exception($"Error!\nSTRING: {i + 1} - Ожидалось: do, а получено: {token[i].Qwerty}");
            Next();
            BlockOper();
        }

        public void BlockOper()
        {

            if (token[i].Type == Token.TokenType.VARIABLE || token[i].Type == Token.TokenType.FOR)
                Oper();

            if (token[i].Type == Token.TokenType.BEGIN)
            {

                Next();
                SpisOper();

                if (token[i].Type != Token.TokenType.END)
                    throw new Exception($"Error!\nSTRING: {i + 1} - Ожидалось: end, а получено: {token[i].Qwerty}");
                Next();
                if (token[i].Type != Token.TokenType.SEMICOLON)
                    throw new Exception($"Error!\nSTRING: {i + 1} - Ожидалось: точка с запятой, а получено: {token[i].Qwerty}");
                Next();

            }
        }

        public void Prisv()
        {
            if (token[i].Type != Token.TokenType.VARIABLE)
                throw new Exception($"Error!\nSTRING: {i + 1} - Ожидалось: переменная, а получено: {token[i].Qwerty}");
            Next();
            if (token[i].Type != Token.TokenType.ASSIGNMENT)
                throw new Exception($"Error!\nSTRING: {i + 1} - Ожидалось: знак присвоения, а получено: {token[i].Qwerty}");
            Next();
            Expr2();
            if (token[i].Type != Token.TokenType.SEMICOLON)
                throw new Exception($"Error!\nSTRING: {i + 1} - Ожидалось: точка с запятой , а получено: {token[i].Qwerty}");
            Next();
        }

        public void Expr1()
        {
            while (token[i].Type != Token.TokenType.TO)
            {
                Next();
            }
        }
        public void Expr2()
        {
            while (token[i].Type != Token.TokenType.SEMICOLON)
            {
                Next();
            }
        }

        public void DopOper2()
        {
            if (token[i].Type != Token.TokenType.VARIABLE && token[i].Type != Token.TokenType.FOR)
                throw new Exception($"Error!\nSTRING: {i + 1} - Ожидалось: переменная или for, а получено: {token[i].Qwerty}");
            SpisOper();
        }

        public void Type()
        {
            if (token[i].Type != Token.TokenType.INTEGER
                && token[i].Type != Token.TokenType.REAL
                && token[i].Type != Token.TokenType.DOUBLE)
                throw new Exception($"Error!\nSTRING: {i + 1} - Ожидалось: integer, real или double, а получено: {token[i].Qwerty}");
            Next();
        }
        public void Operand()
        {
            if (token[i].Type != Token.TokenType.VARIABLE && token[i].Type != Token.TokenType.NUMBER)
                throw new Exception($"Error!\nSTRING: {i + 1} - Ожидалось: перемнная или число, а получено: {token[i].Qwerty}");
            Next();
        }
        public void Next()
        {
            if (i < token.Count - 1)
            {
                i++;
            }
        }
    }
}
