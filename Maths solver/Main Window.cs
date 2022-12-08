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
using static Maths_solver.Functions;

namespace Maths_solver
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
				if (item.GetType() == typeof(Term))
				{
					Term term = (Term)item;
					equationStr += TermStr(term);
				}

				if (item.GetType() == typeof(Operation))
				{
					Operation operation = (Operation)item;
					equationStr += TermStr(operation);
				}
			}

			return equationStr;
		}

		private static string TermStr(Term term)
		{
			string formatTerm = String.Empty;

			//format coefficient
			if (Math.Abs(term.GetCoeficient()) != 1) formatTerm += term.GetCoeficient();
			else if (term.GetCoeficient() == -1) formatTerm += "-";

			//check if exponent 0
			if (term.GetExponent().Count == 1 && term.GetExponent()[0].GetType() == typeof(Term))
			{
				Term termExponent = (Term)term.GetExponent()[0];

				if (termExponent.GetCoeficient() == 0) return term.GetCoeficient().ToString();
			}

			//format function
			if (functions[term.GetFunction()]) formatTerm += term.GetFunction();

			else formatTerm += term.GetFunction();

			//format exponent
			if (term.GetExponent().Count == 1 && term.GetExponent()[0].GetType() == typeof(Term))
			{
				Term termExponent = (Term)term.GetExponent()[0];

				if (termExponent.GetCoeficient() != 1)
				{
					string exponentStr = termExponent.GetCoeficient().ToString();

					//turn float into multiple superscript characters
					for (int i = 0; i < exponentStr.Length; i++) formatTerm += Superscript[exponentStr[i]];
				}
			}

			//format input
			if (functions[term.GetFunction()]) formatTerm += $"({TermStr(term.GetInput())})";

			return formatTerm;
		}

		private static string TermStr(Operation operation)
		{
			string formatTerm = String.Empty;

			switch (operation.GetOperation())
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

		private static List<EquationItem> stringToEquation(string input)
		{
			List<EquationItem> equation = new List<EquationItem>();

			bool foundExponent = false;

			float coefficient = 1;
			Function function = Function.NONE;
			Term funcInput = null;
			float exponent = 1;

			string part = String.Empty;
			for (int i = 0; i < input.Length; i++)
			{
				if (input[i] == ' ') continue;

				FindCoefficient(ref part, input, i, ref coefficient);

				SeparateString(input, i, ref part, ref funcInput);

				//must be after separate string
				FindFunction(input, i, ref function, ref part);

				FindExponent(part, i , input, ref foundExponent, ref exponent);

				//if operation, new term
				if (Maths.operations.ContainsKey(input[i]))
				{
					coefficient = 1;
					function = Function.NONE;
					funcInput = null;
					exponent = 1;
					part = String.Empty;

					equation.Add(new Operation(Maths.operations[input[i]]));
				}

				CreateEquation(function, coefficient, funcInput, exponent, foundExponent, ref equation);
				
			}

			return equation;
		}

		private static void FindCoefficient(ref string part, string input, int i, ref float coefficient)
        {
			if (int.TryParse(part, out int _coefficient) &&
				!int.TryParse(input[i].ToString(), out int _))
			{
				coefficient = _coefficient;
				part = String.Empty;
			}
		}

		private static bool IsSuperscript(string input, int i, ref char character)
        {
			bool superscript = false;
			char[] superscriptValues = Superscript.Values.ToArray();

			for (int x = 0; x < superscriptValues.Length; x++)
			{
				if (input[i] == superscriptValues[x])
				{
					character = Superscript.Keys.ToArray()[x];
					superscript = true;
					break;
				}
			}

			return superscript;
		}

		private static void SeparateString(string input, int i, ref string part, ref Term funcInput)
        {
			switch (input[i])
			{
				//find input
				case ')':
					funcInput = stringToTerm(part, 1);
					part = String.Empty;
					break;

				default:
					part += input[i];
					break;
			}
		}

		private static void FindFunction(string input, int i, ref Function function, ref string part)
        {
			char _ = default;
			if (IsSuperscript(input, i, ref _))
			{
				if (function == Function.NONE &&
					Enum.TryParse(part.Substring(0, part.Length - 1), out Function f))
				{
					function = f;
					part = part[part.Length - 1].ToString();
				}
			}
			else if (input[i] == '(')
			{
				if (function == Function.NONE &&
					Enum.TryParse(part.Substring(0, part.Length - 1), out Function f))
				{
					function = f;
					part = String.Empty;
				}
			}
			else if (i + 1 == input.Length || (i + 1 < input.Length && input[i + 1] == ' ') ||
				(i + 1 < input.Length && Maths.operations.ContainsKey(input[i + 1])))
            {
				if (function == Function.NONE &&
					Enum.TryParse(input[i + 1].ToString(), out Function f))
				{
					function = f;
					part = String.Empty;
				}
			}
		}

		private static void FindExponent(string part, int x, string input, ref bool foundExponent, ref float exponent)
		{
			char _ = default;
			//if at end of string
			if (x + 1 >= input.Length ||
				(x + 1 < input.Length && !IsSuperscript(input, x + 1, ref _)))
			{
				string exponentStr = String.Empty;
				for (int i = 0; i < part.Length; i++)
				{
					char character = default;
					if (IsSuperscript(part, i, ref character))
					{
						exponentStr += character;
					}
				}

				//all characters exponents
				if (exponentStr.Length == part.Length && int.TryParse(exponentStr, out int _exponent))
				{
					exponent = _exponent;
					foundExponent = true;
				}
			}

			if (x == input.Length - 1) foundExponent = true;
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
					new List<EquationItem> { new Term(exponent, Function.a) }));
				}

				//if has no input but doesnt require input
				if (funcInput == null && !functions[function] && foundExponent)
				{
					equation.Add(new Term(coefficient, function,
					new List<EquationItem> { new Term(exponent, Function.a) }));
				}
			}
		}

		private static Term stringToTerm(string part, float coefficient)
		{
			if (!Enum.TryParse(part, out Function function)) return null;

			return new Term(coefficient, function, new List<EquationItem> { new Term(1, Function.a) });
		}

		private void InputBox_TextChanged(object sender, EventArgs e)
		{
			RichTextBox senderBox = sender as RichTextBox;

			string input = senderBox.Text;

			if (input.Length != 0 && input[input.Length - 1] == '^')
			{
				isSuperscript = !isSuperscript;

				UpdateBox(senderBox, input.Remove(input.Length - 1));
			}
			else if (isSuperscript)
			{
				if(Superscript.ContainsKey(input[input.Length - 1]))
				{
					UpdateBox(senderBox, input.Substring(0,
					input.Length - 1) + Superscript[input[input.Length - 1]]);
				}
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
			OutputBox.Text = 
				EquationStr(Maths.DifferentiateEquation((stringToEquation(InputBox.Text))));
		}
	}
}
