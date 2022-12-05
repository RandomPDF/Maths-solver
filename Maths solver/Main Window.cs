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

			float coefficient = 1;
			Function function = Function.NONE;
			Term funcInput = null;

			string part = String.Empty;
			for (int i = 0; i < input.Length; i++)
			{
				if (input[i] == ' ') continue;

				//find coefficient
				if (int.TryParse(part, out int _coefficient) &&
					!int.TryParse(input[i].ToString(), out int _))
				{
					coefficient = _coefficient;
					part = String.Empty;
				}

				//seperate into seperate parts
				switch (input[i])
				{
					//find function
					case '(':
						if (Enum.TryParse(part, out Function f)) function = f;
						part = String.Empty;
						break;

					//find input
					case ')':
						funcInput = stringToTerm(part, 1);
						part = String.Empty;
						break;

					default:
						part += input[i];
						break;
				}

				

				//if operation, new term
				if (Maths.operations.ContainsKey(input[i]))
				{
					coefficient = 1;
					function = Function.NONE;
					funcInput = null;
					part = String.Empty;

					equation.Add(new Operation(Maths.operations[input[i]]));
				}

				if (function != Function.NONE && funcInput != null)
				{
					equation.Add(new Term(coefficient, function, funcInput,
						new List<EquationItem> { new Term(1, Function.a)}));
				}
			}

			return equation;
		}

		private static Term stringToTerm(string part, float coefficient)
		{
			if (!Enum.TryParse(part, out Function function)) return null;

			return new Term(coefficient, function, new List<EquationItem> { new Term(1, Function.a) });
		}

		private void InputBox_TextChanged(object sender, EventArgs e)
		{
			RichTextBox senderBox = sender as RichTextBox;
			//OutputBox.Text = senderBox.Text;
		}

		private void DifferentaiteButton_Click(object sender, EventArgs e)
		{
			OutputBox.Text = 
				EquationStr(Maths.DifferentiateEquation((stringToEquation(InputBox.Text))));
		}
	}
}
