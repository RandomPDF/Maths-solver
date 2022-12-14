using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Maths_solver.Maths;
using static Maths_solver.Maths.Functions;

namespace Maths_solver.UI
{
	public partial class Main : Form
	{
		private static Dictionary<char, char> Superscript = new Dictionary<char, char>()
		{
			{'.', (char)0X00B7}, {'-', (char)0X207B}, {'0', (char)0X2070}, {'1', (char)0X00B9},
			{'2', (char)0X00B2}, {'3', (char)0X00B3}, {'4', (char)0X2074}, {'5', (char)0X2075}, 
			{'6', (char)0X2076}, {'7', (char)0X2077}, {'8', (char)0X2078}, {'9', (char)0X2079}
		};

		private bool isSuperscript = false;

		public Main()
		{
			InitializeComponent();
		}

		private static string EquationStr(List<EquationItem> equation)
		{
			string equationStr = String.Empty;

			foreach (EquationItem item in equation)
			{
				if (item.GetType() == typeof(Term)) equationStr += TermStr((Term)item);

				else if (item.GetType() == typeof(Operation)) equationStr += TermStr((Operation)item);
			}

			return equationStr;
		}

		private static string TermStr(Term term)
		{
			string formatTerm = String.Empty;

			//format coefficient
			if (Math.Abs(term.coeficient) != 1) formatTerm += term.coeficient;
			else if (term.coeficient == -1) formatTerm += "-";

			//check if exponent 0
			if (term.exponent.Count == 1 && term.exponent[0].GetType() == typeof(Term))
			{
				Term termExponent = (Term)term.exponent[0];

				if (termExponent.coeficient == 0) return term.coeficient.ToString();
			}

			formatTerm += term.function;

			//format exponent
			if (term.exponent.Count == 1 && term.exponent[0].GetType() == typeof(Term))
			{
				Term termExponent = (Term)term.exponent[0];

				if (termExponent.coeficient != 1)
				{
					string exponentStr = termExponent.coeficient.ToString();

					//turn float into multiple superscript characters
					for (int i = 0; i < exponentStr.Length; i++) formatTerm += Superscript[exponentStr[i]];
				}
			}

			//format input
			if (functions[term.function]) formatTerm += $"({TermStr(term.input)})";

			return formatTerm;
		}

		private static string TermStr(Operation operation)
		{
			string formatTerm = String.Empty;

			switch (operation.operation)
			{
				case OperationEnum.Addition:
					formatTerm = " + ";
					break;

				case OperationEnum.Subtraction:
					formatTerm = " - ";
					break;

				case OperationEnum.Multiplication:
					formatTerm = String.Empty;
					break;

				case OperationEnum.Division:
					formatTerm = " / ";
					break;
			}

			return formatTerm;
		}

		private static List<EquationItem> stringToEquation(string inputSpaces)
		{
			List<EquationItem> equation = new List<EquationItem>();

			bool foundExponent = false;

			float coefficient = 1;
			Function function = Function.NONE;
			Term funcInput = null;
			float exponent = 1;

			//remove spaces
			string input = String.Empty;
			for (int i = 0; i < inputSpaces.Length; i++)
			{
				if (inputSpaces[i] == ' ') continue;
				input += inputSpaces[i];
			}
			
			string part = String.Empty;
			for (int i = 0; i < input.Length; i++)
			{
				FindCoefficient(ref part, input[i], ref coefficient);

				FindFunction(input, i, ref function, ref part);

				FindExponent(i ,input, function, ref part, ref foundExponent, out exponent);

				SeparateString(input[i], ref part, out funcInput);

				CreateEquation(function, coefficient, funcInput, exponent, foundExponent, ref equation);

				CheckOperation(input[i], ref coefficient, ref function, ref funcInput, ref exponent,
					ref part, ref equation, ref foundExponent);

			}

			return equation;
		}

		private static void FindCoefficient(ref string part, char next, ref float coefficient)
        {
			//current part is int, but next part isn't, must be whole coefficient
			if (int.TryParse(part, out int _coefficient) &&
				!int.TryParse(next.ToString(), out int _))
			{
				coefficient = _coefficient;
				part = String.Empty;
			}
		}

		private static bool IsSuperscript(string text, out string superscript)
        {
			superscript = String.Empty;

			//if no text, not superscript
			if (text.Length == 0) return false;

			//loop through each character in text
			for (int i = 0; i < text.Length; i++)
			{
				bool charSuperscript = false;
				char[] superscriptValues = Superscript.Values.ToArray();

				//if character matches any superscript character, is superscript
				for (int x = 0; x < superscriptValues.Length; x++)
				{
					if (text[i] == superscriptValues[x])
					{
						superscript += Superscript.Keys.ToArray()[x];
						charSuperscript = true;
						break;
					}
				}

				if (!charSuperscript) return false;
			}

			return true;
		}

		private static void SeparateString(char next, ref string part, out Term funcInput)
        {
			switch (next)
			{
				//find input
				case ')':
					funcInput = stringToTerm(part.Substring(1), 1);
					part = String.Empty;
					break;

				//move onto next char
				default:
					funcInput = null;
					part += next;
					break;
			}
		}

		private static void FindFunction(string input, int i, ref Function function, ref string part)
        {
			if (function != Function.NONE) return;

			OperationEnum nextOperation = OperationEnum.NONE;
			Function f = Function.NONE;

			switch(input[i])
            {
				case '+':
					nextOperation = OperationEnum.Addition;
					break;

				case '-':
					nextOperation = OperationEnum.Subtraction;
					break;

				case '/':
					nextOperation = OperationEnum.Division;
					break;
            }

			//if next character is superscript or next char is input
			//or next is at end of string
			if (((IsSuperscript(input[i].ToString(), out string _) || input[i] == '(') &&
				Enum.TryParse(part, out f)) ||

				(i >= input.Length - 1 && Enum.TryParse(input[i].ToString(), out f)) ||
				nextOperation != OperationEnum.NONE && Enum.TryParse(part.ToString(), out f))
			{
				function = f;
				part = String.Empty;
			}
		}

		private static void FindExponent(int i, string input, Function function, ref string part,
			ref bool foundExponent, out float exponent)
		{
			exponent = 1;
			string check = String.Empty;

			//if within string and no more exponents and function is x
			if (function == Function.x && i < input.Length - 1 && !IsSuperscript(input[i].ToString(), out string _))
			{
				check = part.ToString();
				part = String.Empty;
				foundExponent = true;
			}

			//if next is end of string
			if (i == input.Length - 1)
			{
				//if next is superscript
				if (IsSuperscript(input[i].ToString(), out string _))
				{
					//check previous exponents, and next exponents
					check = part + input[i].ToString();
					part = String.Empty;
				}
				else foundExponent = true;
			}

			//if at end of string, part must be exponent
			if (i > input.Length - 1)
			{
				check = part;
				foundExponent = true;
			}

			//string isn't exponent, continue
			if (!IsSuperscript(check.ToString(), out string exponentStr)) return;

			//set exponent as string
			if (int.TryParse(exponentStr, out int _exponent))
			{
				exponent = _exponent;
				foundExponent = true;
			}
		}

		private static void CheckOperation(char operation, 
			ref float coefficient, ref Function function, ref Term funcInput, ref float exponent,
			ref string part, ref List<EquationItem> equation, ref bool foundExponent)
		{
			//if operation, new term
			OperationEnum operationEnum = OperationEnum.NONE;

			switch (operation)
			{
				case '+':
					operationEnum = OperationEnum.Addition;
					break;

				case '-':
					operationEnum = OperationEnum.Subtraction;
					break;

				case '/':
					operationEnum = OperationEnum.Division;
					break;

					/*default:
						operationEnum = OperationEnum.Multiplication;
						break;*/
			}

			//if end of current term
			if (operationEnum != OperationEnum.NONE || operation == ')')
			{
				coefficient = 1;
				function = Function.NONE;
				funcInput = null;
				exponent = 1;
				part = String.Empty;
				foundExponent = false;

				if(operationEnum != OperationEnum.NONE) equation.Add(new Operation(operationEnum));
			}
		}

		private static void CreateEquation(Function function, float coefficient, Term funcInput, float exponent, bool foundExponent,
			ref List<EquationItem> equation)
		{
			if (function != Function.NONE)
			{
				//if has input and requires input
				if (funcInput != null && functions[function])
				{
					equation.Add(new Term(coefficient, function, funcInput,
					new List<EquationItem> { new Term(exponent, Function.constant) }));
				}

				//if has no input but doesnt require input
				if (funcInput == null && !functions[function] && foundExponent)
				{
					equation.Add(new Term(coefficient, function,
					new List<EquationItem> { new Term(exponent, Function.constant) }));
				}
			}
		}

		//turn into recursive function in future
		private static Term stringToTerm(string part, float coefficient)
		{
			if (!Enum.TryParse(part, out Function function)) return null;

			return new Term(coefficient, function, new List<EquationItem> { new Term(1, Function.constant) });
		}

		private void InputBox_TextChanged(object sender, EventArgs e)
		{
			RichTextBox senderBox = sender as RichTextBox;

			string input = senderBox.Text;

			//if input isn't nothing, and the last character was ^
			if (input.Length > 0 && input[input.Length - 1] == '^')
			{
				//change superscripts
				isSuperscript = !isSuperscript;

				//ignore ^ character
				UpdateBox(senderBox, input.Remove(input.Length - 1));
			}
			else if (isSuperscript)
			{
				//if the character can be superscript
				if(Superscript.ContainsKey(input[input.Length - 1]))
				{
					UpdateBox(senderBox, input.Substring(0,
					input.Length - 1) + Superscript[input[input.Length - 1]]);
				}
				//Otherwise, no longer superscript
				else isSuperscript = false;
			}
		}

		private void UpdateBox(RichTextBox box, string text)
        {
			box.Text = text;
			box.SelectionStart = text.Length;
			box.SelectionLength = 0;
		}

		private void DifferentaiteButton_Click(object sender, EventArgs e)
		{
			//OutputBox.Text = EquationStr(Maths.Maths.DifferentiateEquation((stringToEquation(InputBox.Text))));
			OutputBox.Text = EquationStr(stringToEquation(InputBox.Text));
		}
	}
}
