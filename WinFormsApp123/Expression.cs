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
        LR lr;
        List<Token> ExpressionStack = new List<Token>();
        Stack<string> Operations = new Stack<string>();
        Stack<int> Prioritis = new Stack<int>();
        int index = 0;
        string output = null;
        Dictionary<string,int> priority = new Dictionary<string, int>()
        {
            {"+", 0}, {"-", 0},
            {"*", 1}, {"/", 1}
        };
        public void TakeToken(Token token)
        {

            ExpressionStack.Add(token);
        }
        public void Start()
        {
                Decstra();
                PolishNotation();

        }
        private void HighPriority(string operation)  
        {
            int count = Operations.Count();
            Stack<string> temp = new Stack<string>();
            Stack<int> priorityTemp = new Stack<int>();
            for (int i = 0; i < count; i++)
            {
                if(Prioritis.Peek() >= priority[operation])
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
            for(int i = 0; i < countTemp; i++)
            {
                Operations.Push(temp.Pop());
                Prioritis.Push(priorityTemp.Pop());
            }
            Operations.Push(ExpressionStack[index].Qwerty);
            Prioritis.Push(priority[operation]);  
        }

        private void Decstra()
        {
            if (ExpressionStack[index].Type == Token.TokenType.VARIABLE || ExpressionStack[index].Type == Token.TokenType.NUMBER)
            {
                Prioritis.Push(0);

                while (index != ExpressionStack.Count())
                {
                    if (ExpressionStack[index].Type == Token.TokenType.NUMBER || ExpressionStack[index].Type == Token.TokenType.VARIABLE)
                    {
                        output += ExpressionStack[index].Qwerty + " ";
                        index++;
                    }
                    else if (ExpressionStack[index].Type == Token.TokenType.PLUS)
                    {
                        string operation = "+";

                        if ((priority[operation] > Prioritis.Peek()) || Operations.Count() == 0)
                        {
                            Operations.Push(ExpressionStack[index].Qwerty);
                            Prioritis.Push(priority[operation]);
                        }
                        else
                        {
                            HighPriority(operation);
                        }
                        index++;
                    }
                    else if (ExpressionStack[index].Type == Token.TokenType.MINUS)
                    {
                        string operation = "-";
                        if ((priority[operation] > Prioritis.Peek()) || Operations.Count() == 0)
                        {
                            Operations.Push(ExpressionStack[index].Qwerty);
                            Prioritis.Push(priority[operation]);
                        }
                        else
                        {
                            HighPriority(operation);
                        }
                        index++;
                    }
                    else if (ExpressionStack[index].Type == Token.TokenType.MULTIPLY)
                    {
                        string operation = "*";
                        if ((priority[operation] > Prioritis.Peek()) || Operations.Count() == 0)
                        {
                            Operations.Push(ExpressionStack[index].Qwerty);
                            Prioritis.Push(priority[operation]);
                        }
                        else
                        {
                            HighPriority(operation);
                        }
                        index++;
                    }
                    else if (ExpressionStack[index].Type == Token.TokenType.DIVISION)
                    {
                        string operation = "/";
                        if ((priority[operation] > Prioritis.Peek()) || Operations.Count() == 0)

                        {
                            Operations.Push(ExpressionStack[index].Qwerty);
                            Prioritis.Push(priority[operation]);
                        }
                        else
                        {
                            HighPriority(operation);
                        }
                        index++;
                    }
                    else if (ExpressionStack[index].Type == Token.TokenType.VARIABLE || ExpressionStack[index].Type == Token.TokenType.NUMBER)
                    {
                        break;
                    }
                }
                int countOperations = Operations.Count();
                for (int i = 0; i < countOperations; i++)
                {
                    output += Operations.Pop();
                }
            }
        }
        public void PolishNotation()
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
                            M.Add(key, stackOperand.Pop() + " " + stackOperand.Pop() + " " + "+");
                            stackOperand.Push("M" + key.ToString());
                            key++;
                            break;
                        }
                    case ('-'):
                        {
                            M.Add(key, stackOperand.Pop() + " " + stackOperand.Pop() + " " + "-");
                            stackOperand.Push("M" + key.ToString());
                            key++;
                            break;
                        }
                    case ('*'):
                        {
                            M.Add(key, stackOperand.Pop() + " " + stackOperand.Pop() + " " + "*");
                            stackOperand.Push("M" + key.ToString());
                            key++;
                            break;
                        }
                    case ('/'):
                        {
                            M.Add(key, stackOperand.Pop() + " " + stackOperand.Pop() + " " + "/");
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