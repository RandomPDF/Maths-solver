using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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

			List<EquationItem> test = new List<EquationItem>()
			{
				new Term(2.6f, Function.cos, new List<EquationItem>{new Term(1, Function.a)}),
				new Operation(OperationEnum.Subtraction),
				new Term(3.14f, Function.sin, new List<EquationItem>{new Term(1, Function.a)}),
				new Operation(OperationEnum.Addition),
				new Term(3, Function.tan, new List<EquationItem>{new Term(1, Function.a)}),
				new Operation(OperationEnum.Subtraction),
				new Term(19, Function.cosec, new List<EquationItem>{new Term(1, Function.a)}),
				new Operation(OperationEnum.Subtraction),
				new Term(0.2f, Function.x, new List<EquationItem>{new Term(-1.75f, Function.a)}),
				new Operation(OperationEnum.Addition),
				new Term(6.9f, Function.sec, new List<EquationItem>{new Term(1, Function.a)}),
				new Operation(OperationEnum.Subtraction),
				new Term(4.2f, Function.cot, new List<EquationItem>{new Term(1, Function.a)}),
				new Operation(OperationEnum.Addition),
				new Term(5.7f, Function.ln, new List<EquationItem>{new Term(1, Function.a)})
			};

			InputBox.Text = EquationStr(test);
			OutputBox.Text = EquationStr(Maths.DifferentiateEquation(test));
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
	}
}
