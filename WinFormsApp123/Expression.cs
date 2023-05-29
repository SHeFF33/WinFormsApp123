using System.Text.RegularExpressions;
using WinFormsApp123;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace WinFormsApp123
{
    public class Expression
    {
        List<Token> ExprStack = new List<Token>();
        Stack<string> Operations = new Stack<string>();
        Stack<int> Prioritis = new Stack<int>();
        int index = 0;
        int countLpar = 0;
        int countRpar = 0;
        string output = null;
        Dictionary<string, int> priority = new Dictionary<string, int>()
        {
            {"(", 0},
            {")", 1},
            {"+", 2}, {"-", 2},
            {"*", 3}, {"/", 3}
        };
        public void TakeToken(Token token)
        {
            ExprStack.Add(token);
        }
        public void Start()
        {
            Decstra();
            ReversePolishNotation();
        }
        private void HighPriority(string operation)
        {
            int count = Operations.Count();
            Stack<string> temp = new Stack<string>();
            Stack<int> priorityTemp = new Stack<int>();
            for (int i = 0; i < count; i++)
            {
                if (Prioritis.Peek() >= priority[operation])
                {
                    output += Operations.Pop();
                    Prioritis.Pop();
                }
                else
                {
                    temp.Push(Operations.Pop());
                    priorityTemp.Push(Prioritis.Pop());
                }
            }
            temp.Reverse();
            priorityTemp.Reverse();
            int countTemp = temp.Count();
            for (int i = 0; i < countTemp; i++)
            {
                Operations.Push(temp.Pop());
                Prioritis.Push(priorityTemp.Pop());
            }
            Operations.Push(ExprStack[index].Qwerty);
            Prioritis.Push(priority[operation]);
        }

        private void Decstra()
        {
            if (ExprStack[index].Type == Token.TokenType.LPAR || ExprStack[index].Type == Token.TokenType.NUMBER || ExprStack[index].Type == Token.TokenType.VARIABLE)
            {
                Prioritis.Push(0);
                while (index != ExprStack.Count())
                {
                    if (ExprStack[index].Type == Token.TokenType.NUMBER || ExprStack[index].Type == Token.TokenType.VARIABLE)
                    {
                        output += ExprStack[index].Qwerty + " ";
                        index++;
                    }
                    else if (ExprStack[index].Type == Token.TokenType.PLUS)
                    {
                        string operation = "+";

                        if ((priority[operation] > Prioritis.Peek()) || Operations.Count() == 0)
                        {
                            Operations.Push(ExprStack[index].Qwerty);
                            Prioritis.Push(priority[operation]);
                        }
                        else
                        {
                            HighPriority(operation);
                        }
                        index++;
                    }

                    else if (ExprStack[index].Type == Token.TokenType.MINUS)
                    {
                        string operation = "-";

                        if ((priority[operation] > Prioritis.Peek()) || Operations.Count() == 0)
                        {
                            Operations.Push(ExprStack[index].Qwerty);
                            Prioritis.Push(priority[operation]);
                        }
                        else
                        {
                            HighPriority(operation);
                        }
                        index++;
                    }

                    else if (ExprStack[index].Type == Token.TokenType.MULTIPLY)
                    {
                        string operation = "*";

                        if ((priority[operation] > Prioritis.Peek()) || Operations.Count() == 0)
                        {
                            Operations.Push(ExprStack[index].Qwerty);
                            Prioritis.Push(priority[operation]);
                        }
                        else
                        {
                            HighPriority(operation);
                        }
                        index++;
                    }

                    else if (ExprStack[index].Type == Token.TokenType.DIVISION)
                    {
                        string operation = "/";

                        if ((priority[operation] > Prioritis.Peek()) || Operations.Count() == 0)
                        {
                            Operations.Push(ExprStack[index].Qwerty);
                            Prioritis.Push(priority[operation]);
                        }
                        else
                        {
                            HighPriority(operation);
                        }
                        index++;
                    }

                    else if (ExprStack[index].Type == Token.TokenType.LPAR)
                    {
                        string operation = "(";
                        countLpar++;

                        if ((priority[operation] > Prioritis.Peek()) || Operations.Count() == 0)
                        {
                            Operations.Push(ExprStack[index].Qwerty);
                            Prioritis.Push(priority[operation]);
                        }
                        else
                        {
                            Operations.Push(operation);
                            Prioritis.Push(priority[operation]);
                        }
                        index++;
                    }

                    else if (ExprStack[index].Type == Token.TokenType.RPAR)
                    {
                        string operation = ")";
                        countLpar--;
                        countRpar++;

                        if ((priority[operation] > Prioritis.Peek() || Operations.Count() == 0) && countLpar == countRpar )
                        {
                            Operations.Push(ExprStack[index].Qwerty);
                            Prioritis.Push(priority[operation]);
                        }
                        else
                        {
                            HighPriority(operation);
                            Operations.Pop();
                            Operations.Pop();
                            Prioritis.Pop();
                            Prioritis.Pop();
                        }
                        index++;
                    }
                }
                int countOperations = Operations.Count();
                for (int i = 0; i < countOperations; i++)
                {
                    output += Operations.Pop();
                }
            }
        }
        public void ReversePolishNotation()
        {
            Dictionary<int, string> M = new Dictionary<int, string>();
            Stack<string> stackOperand = new Stack<string>();
            int key = 1;
            for (int i = 0; i < output.Count(); i++)
            {
                char currentChar = output[i];
                switch (currentChar)
                {

                    case ('+'):
                        {
                            M.Add(key, "+" + " " + stackOperand.Pop() + " " + stackOperand.Pop());
                            stackOperand.Push("M" + key.ToString());
                            key++;
                            break;
                        }

                    case ('-'):
                        {
                            M.Add(key, "-" + " " + stackOperand.Pop() + " " + stackOperand.Pop());
                            stackOperand.Push("M" + key.ToString());
                            key++;
                            break;
                        }

                    case ('*'):
                        {
                            M.Add(key, "*" + " " + stackOperand.Pop() + " " + stackOperand.Pop());
                            stackOperand.Push("M" + key.ToString());
                            key++;
                            break;
                        }

                    case ('/'):
                        {
                            M.Add(key, "/" + " " + stackOperand.Pop() + " " + stackOperand.Pop());
                            stackOperand.Push("M" + key.ToString());
                            key++;
                            break;
                        }

                    default:
                        {
                            if (Regex.IsMatch(currentChar.ToString(), "^[a-zA-Z]+$") || Regex.IsMatch(currentChar.ToString(), "^[0-9]+$"))
                            {
                                string temp = null;
                                while (output[i] != ' ')
                                {
                                    temp += output[i].ToString();
                                    i++;
                                }
                                stackOperand.Push(temp);
                            }
                            else if (currentChar == ' ')
                            {
                            }
                            else
                            {
                                throw new System.Exception();
                            }
                            break;
                        }
                }
            }
            Form1.form.Conclusion($"Выражение:");
            Form1.form.Conclusion("Обратная польская нотация:");
            Form1.form.Conclusion(output);
            Form1.form.Conclusion("Матричный вид:");
            int countOutput = stackOperand.Count;
            for (int i = 0; i < countOutput; i++)
            {
                Form1.form.Conclusion(stackOperand.Pop());
            }
            int countM = M.Count;
            for (int i = 1; i < countM + 1; i++)
            {
                Form1.form.Conclusion("M" + i + ":" + M[i]);
            }
            Form1.form.Conclusion("-------------------------");
        }
    }
}