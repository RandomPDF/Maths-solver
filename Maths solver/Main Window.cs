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
using static Maths_solver.Maths.Operation;

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

		private Steps StepsForm;

		public Main()
		{
			InitializeComponent();
		}

		#region Equation to string
		public static string EquationStr(List<EquationItem> equation, bool superscript)
		{
			if (equation == null) return String.Empty;

			string equationStr = String.Empty;

			foreach (EquationItem item in equation)
			{
				if (item.GetType() == typeof(Term)) equationStr += TermStr((Term)item, superscript);
				else if (item.GetType() == typeof(Operation)) equationStr += TermStr((Operation)item);
			}

			return equationStr;
		}

		private static string TermStr(Term term, bool superscript)
		{
			string formatTerm = String.Empty;

			//format coefficient
			if (((term.function == Function.constant || Math.Abs(term.coeficient) != 1) && !superscript)
				|| term.coeficient != 1 && superscript)
			{
				formatTerm += PartStr(term.coeficient.ToString(), superscript);
			}

			else if (term.coeficient == -1 && !superscript) formatTerm += PartStr("-", superscript);


			if(term.function != Function.constant) 
				formatTerm += PartStr(term.function.ToString(), superscript);

			//format exponent
			if (term.exponent != null)
			{
				string exponent = EquationStr(term.exponent, true);

				//if exponent 0 return coefficient only
				if (exponent == Superscript['0'].ToString()) return term.coeficient.ToString();
				else formatTerm += exponent;
			}

			//format input
			if (requiresInput[term.function]) formatTerm += $"({EquationStr(term.input, false)})";

			return formatTerm;
		}

		private static string TermStr(Operation operation)
		{
			return operationToString[operation.operation];
		}
		private static string PartStr(string part, bool superscript)
		{
			string displayed = string.Empty;
			if (!superscript) displayed = part;
			else
			{
				for (int i = 0; i < part.Length; i++)
				{
					displayed += Superscript[part[i]];
				}
			}

			return displayed;
		}
		#endregion

		#region String to equation
		private static List<EquationItem> stringToEquation(string inputSpaces)
		{
			List<EquationItem> equation = new List<EquationItem>();

			bool foundExponent = false;

			float coefficient = 1;
			Function function = Function.NONE;
			List<EquationItem> funcInput = null;
			List<EquationItem> exponent = new List<EquationItem> { new Term() };

			//remove spaces
			string input = String.Empty;
			for (int i = 0; i < inputSpaces.Length; i++)
			{
				if (inputSpaces[i] == ' ') continue;
				input += inputSpaces[i];
			}

			Stack<char> brackets = new Stack<char>();
			string part = String.Empty;
			for (int i = 0; i < input.Length; i++)
			{
				//ensure brackets are balanced, and can find input
				if (input[i] == '(') brackets.Push(input[i]);
				if (input[i] == ')') brackets.Pop();

				//finding input
				if (part.Length > 0 && part[0] == '(')
				{
					SeparateString(input[i], brackets, ref part, out funcInput);

					if(funcInput != null) CreateEquation(function, coefficient, funcInput,
						exponent, foundExponent, ref equation);

					continue;
				}

				FindCoefficient(ref part, input[i], ref coefficient);

				FindFunction(input, i, ref function, ref part);

				FindExponent(input, i, part, function, ref exponent, ref foundExponent);

				SeparateString(input[i], brackets, ref part, out funcInput);

				CreateEquation(function, coefficient, funcInput, exponent, foundExponent, ref equation);

				CheckOperation(input, i, ref coefficient, ref function, ref funcInput, ref exponent,
					ref part, ref equation, ref foundExponent);

			}

			return equation;
		}

		private static void FindCoefficient(ref string part, char next, ref float coefficient)
        {
			float _coefficient;
			//current part is int, but next part isn't, must be whole coefficient
			if (float.TryParse(part, out _coefficient) &&
				!float.TryParse(next.ToString(), out float _) && next != '.')
			{
				coefficient = _coefficient;
				part = String.Empty;
			}
			else if(float.TryParse(part + next.ToString(), out _coefficient)) coefficient = _coefficient;
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

		private static void SeparateString(char next, Stack<char> brackets, ref string part, out List<EquationItem> funcInput)
        {
			switch (next)
			{
				//find input
				case ')':
					if (brackets.Count == 0)
					{
						funcInput = stringToEquation(part.Substring(1));
						part = String.Empty;
					}
					else
					{
						funcInput = null;
						part += next;
					}
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
			if (stringToOperation.ContainsKey(input[i])) nextOperation = stringToOperation[input[i]];
			Function f = Function.NONE;

			//if next character is superscript or next char is input
			//or next is at end of string
			if (((IsSuperscript(input[i].ToString(), out string _) || input[i] == '(') &&
				Enum.TryParse(part, out f)) ||

				(i >= input.Length - 1 && Enum.TryParse(input[i].ToString(), out f) &&
				f.ToString() == input[i].ToString()) 
				|| nextOperation != OperationEnum.NONE && Enum.TryParse(part.ToString(), out f))
			{
				function = f;
				part = String.Empty;
			}
			else if(nextOperation != OperationEnum.NONE || i == input.Length - 1)
			{
				function = Function.constant;
				part = String.Empty;
			}
		}

		private static void FindExponent(string input, int i, string part, Function function,
			ref List<EquationItem> exponent, ref bool foundExponent)
		{
			if (function == Function.constant) foundExponent = true;
			else if (!IsSuperscript(input[i].ToString(), out string _))
			{
				if (IsSuperscript(part, out string exponentLong))
				{
					exponent = stringToEquation(exponentLong);
					foundExponent = true;
				}

				if (function == Function.x) foundExponent = true;
			}
			else if (IsSuperscript(input[i].ToString(), out string _) && i == input.Length - 1)
			{
				if (IsSuperscript(part + input[i], out string exponentShort) &&
					float.TryParse(exponentShort, out float exponentVal))
				{
					exponent = new List<EquationItem> { new Term(exponentVal) };
					foundExponent = true;
				}
			}
		}

		private static void CheckOperation(string input, int i,
			ref float coefficient, ref Function function, ref List<EquationItem> funcInput, 
			ref List<EquationItem> exponent, ref string part, ref List<EquationItem> equation, ref bool foundExponent)
		{
			char operation = input[i];

			//if operation, new term
			OperationEnum operationEnum = OperationEnum.NONE;
			if (stringToOperation.ContainsKey(operation)) operationEnum = stringToOperation[operation];

			//if end of current term
			if ((operationEnum != OperationEnum.NONE && operationEnum != OperationEnum.OpenBracket)
				|| i >= input.Length - 1)
			{
				//if nothing can be found, assume constant
				if(function == Function.NONE)
				{
					function = Function.constant;
					foundExponent = true;
					CreateEquation(function, coefficient, funcInput, exponent, foundExponent,
						ref equation);
				}

				coefficient = 1;
				function = Function.NONE;
				funcInput = null;
				exponent = new List<EquationItem> { new Term() };
				part = String.Empty;
				foundExponent = false;

				if(operationEnum != OperationEnum.NONE) equation.Add(new Operation(operationEnum));
			}
		}

		private static void CreateEquation(Function function, float coefficient, List<EquationItem> funcInput, List<EquationItem> exponent, bool foundExponent, ref List<EquationItem> equation)
		{
			if (function != Function.NONE)
			{
				//if has input and requires input
				if (funcInput != null && requiresInput[function])
				{
					equation.Add(new Term(coefficient, function, funcInput, exponent));
				}

				//if has no input but doesnt require input
				if (funcInput == null && !requiresInput[function] && foundExponent)
				{
					equation.Add(new Term(coefficient, function, exponent));
				}
			}
		}
		#endregion

		#region UI
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

		private void StepsButton_Click(object sender, EventArgs e)
		{
			if(StepsForm == null) StepsForm = new Steps();
			StepsForm.Show();
		}

		private void DifferentaiteButton_Click(object sender, EventArgs e)
		{
			OutputBox.Text =
				EquationStr(Maths.Maths.Start(stringToEquation(InputBox.Text)), false);
		}
		#endregion
	}
}
