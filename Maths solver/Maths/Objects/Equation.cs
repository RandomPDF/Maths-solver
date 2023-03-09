using System;
using System.Collections.Generic;
using System.Linq;
using static Maths_solver.Maths.Functions;
using static Maths_solver.Maths.Operation;
using static Maths_solver.UI.Main;

namespace Maths_solver.Maths
{
	public class Equation : List<EquationItem>
	{
		#region Equation to string
		public static string AsString(Equation equation, bool inputted, bool useSuperscript)
		{
			bool exponentSearching = false;

			if (equation == null || equation.Count == 0) return String.Empty;

			string equationString = String.Empty;

			for (int i = 0; i < equation.Count; i++)
			{
				if (equation[i] == null) continue;

				if (equation[i].GetType() == typeof(Term)) equationString += TermStr(equation, (Term)equation[i],
					ref i, equation.Count, inputted, ref useSuperscript, ref exponentSearching);

				else if (equation[i].GetType() == typeof(Operation) && !exponentSearching)
					equationString += TermStr((Operation)equation[i], inputted, ref useSuperscript, ref exponentSearching);
			}

			return equationString;
		}

		private static string TermStr(Equation equation, Term term, ref int index, int equationLength, bool inputted, ref bool useSuperscript,
			ref bool exponentSearching)
		{
			string formatTerm = String.Empty;

			if (term.coeficient == 0 && equationLength == 1) return "0";

			if (!exponentSearching)
			{
				#region coefficient
				//format coefficient
				if (((term.function == Function.constant || System.Math.Abs(term.coeficient) != 1) && !useSuperscript)
					|| (term.coeficient != 1 || (equationLength != 1 && term.function == Function.constant)) && useSuperscript)
				{
					if (int.TryParse((term.coeficient / (float)Math.E).ToString(), out int multiplier) && term.coeficient != 0)
					{
						if (multiplier != 1) formatTerm += PartStr(multiplier.ToString(), useSuperscript);
						formatTerm += PartStr("e", useSuperscript);
					}
					else if (term.coeficient != -1 || term.function == Function.constant)
					{
						formatTerm += PartStr(term.coeficient.ToString(), useSuperscript);
					}
					else
					{
						formatTerm += PartStr("-", useSuperscript);
					}
				}

				else if (term.coeficient == -1 && !useSuperscript) formatTerm += PartStr("-", useSuperscript);
				#endregion

				if (term.function != Function.constant)
					formatTerm += PartStr(term.function.ToString(), useSuperscript);

				//exponent if not inputted into input box
				if (!inputted &&
					formatExponent(term, equationLength, inputted, ref formatTerm) == term.coeficient.ToString())
				{
					return term.coeficient.ToString();
				}

				//format input
				if (requiresInput[term.function])
				{
					if (!useSuperscript) formatTerm += $"({AsString(term.input, inputted, false)})";
					else formatTerm += ToSuperscript("(") + AsString(term.input, inputted, true) + ToSuperscript(")");
				}

				if (inputted &&
					formatExponent(term, equationLength, inputted, ref formatTerm) == term.coeficient.ToString())
				{
					return term.coeficient.ToString();
				}
			}

			if (exponentSearching)
			{
				Equation exponent = new Equation();
				Stack<OperationEnum> brackets = new Stack<OperationEnum>();

				int start = index;
				if (equation[index - 1].GetType() == typeof(Operation) &&
					((Operation)equation[index - 1]).operation == OperationEnum.OpenBracket)
				{
					start = index - 1;
				}

				for (int i = start; i < equation.Count; i++)
				{
					if (equation[i].GetType() == typeof(Operation))
					{
						OperationEnum operation = ((Operation)equation[i]).operation;

						if (operation == OperationEnum.OpenBracket) brackets.Push(OperationEnum.OpenBracket);

						if (operation == OperationEnum.ClosedBracket && brackets.Count > 0) brackets.Pop();
					}

					exponent.Add(equation[i]);

					if (brackets.Count == 0) break;

					index++;
				}

				if (exponent.Count > 1)
				{
					//remove brackets from exponent used to find exponent
					exponent.RemoveAt(0);
					exponent.RemoveAt(exponent.Count - 1);
				}

				formatTerm += AsString(exponent, false, true);
				exponentSearching = false;
			}

			return formatTerm;
		}

		private static string TermStr(Operation operation, bool inputted, ref bool useSuperscript,
			ref bool exponentSearching)
		{
			if (useSuperscript && operationToString.ContainsKey(operation.operation))
			{
				string operationString = operationToString[operation.operation];

				if (operationString == String.Empty)
				{
					if (inputted) return "*";
					return "";
				}

				return $" {ToSuperscript(operationString.Trim()[0].ToString())} ";
			}
			if (operationToString.ContainsKey(operation.operation))
			{
				if (operation.operation == OperationEnum.Multiplication && inputted) return ((char)0X00D7).ToString();
				return operationToString[operation.operation];
			}
			//must have been power operation
			else
			{
				exponentSearching = true;
				return String.Empty;
			}
		}
		private static string formatExponent(Term term, int equationLength, bool inputted, ref string formatTerm)
		{
			//format exponent if not null or 1
			if (term.exponent != null && !term.exponent.EquationsEqual(new Equation { new Term(1) }))
			{
				string exponent = AsString(term.exponent, inputted, true);

				//if exponent 0 return coefficient only
				if (exponent == ToSuperscript("0").ToString()) return term.coeficient.ToString();
				else formatTerm += exponent;
			}

			return String.Empty;
		}

		private static string PartStr(string part, bool superscript)
		{
			if (!superscript) return part;

			return ToSuperscript(part);
		}
		#endregion

		#region Equal
		public bool EquationsEqual(Equation equation)
		{
			//if length not equal, equations not equal
			if (Count != equation.Count) return false;

			for (int i = 0; i < Count; i++)
			{
				//check if terms equal
				if (base[i].GetType() == typeof(Term) && equation[i].GetType() == typeof(Term))
				{
					Term term1 = (Term)base[i];
					Term term2 = (Term)equation[i];
					if (!TermsEqual(term1, term2, false)) return false;
				}

				//check if operations equal
				if (base[i].GetType() == typeof(Operation) &&
				equation[i].GetType() == typeof(Operation))
				{
					Operation operation1 = (Operation)base[i];
					Operation operation2 = (Operation)equation[i];

					if (operation1.operation != operation2.operation) return false;
				}
				//check if same type
				if (base[i].GetType() != equation[i].GetType()) return false;
			}
			return true;
		}

		private bool TermsEqual(Term term1, Term term2, bool areExponents)
		{
			if (!areExponents)
			{
				//check if coefficients match
				if (term1.coeficient != term2.coeficient) return false;

				//check if functions match
				if (term1.function == term2.function)
				{
					if (term1.exponent != null && term2.exponent != null &&
						term1.exponent.EquationsEqual(term2.exponent)) return true;

					if (term1.exponent == null && term2.exponent == null) return true;

					if ((term1.exponent == null && term2.exponent.EquationsEqual(new Equation { new Term() }))
						|| (term1.exponent.EquationsEqual(new Equation { new Term() })) && term2.exponent == null)
						return true;
				}
			}
			if (areExponents && term1.exponent == term2.exponent &&
				term1.exponent == term2.exponent) return true;
			return false;
		}

		#endregion

		#region Add
		public void Add(Equation equation)
		{
			if (equation == null) return;
			for (int i = 0; i < equation.Count; i++) Add(equation[i]);
		}

		public new void Add(EquationItem item)
		{
			if (item == null) return;

			base.Add(item);

			if (item.GetType() != typeof(Term)) return;

			Term newTerm = (Term)item;
			if (newTerm.coeficient != 0 && newTerm != null) FormatTerm(newTerm);
		}

		private void FormatTerm(Term newTerm)
		{
			//Checks if term is negative, and more than 2 items in the equation
			if (newTerm.coeficient < 0 && Count >= 2 &&
				base[Count - 2].GetType() == typeof(Operation))
			{
				Operation newEquationOperation = (Operation)base[Count - 2];
				bool negate = false;

				//Negate operation
				switch (newEquationOperation.operation)
				{
					case OperationEnum.Addition:
						base[Count - 2] = new Operation(OperationEnum.Subtraction);
						negate = true;
						break;

					case OperationEnum.Subtraction:
						base[Count - 2] = new Operation(OperationEnum.Addition);
						negate = true;
						break;
				}

				if (negate)
				{
					//negate coefficient
					base[Count - 1] =
						new Term(-newTerm.coeficient, newTerm.function, newTerm.input, newTerm.exponent);
				}
			}

			//convert x^0 to constant
			Equation equationTermExponent = newTerm.exponent;

			if (equationTermExponent == null || equationTermExponent.Count <= 0 ||
				equationTermExponent[0].GetType() != typeof(Term)) return;

			Term equationFirstTermExponent = (Term)equationTermExponent[0];

			if (equationFirstTermExponent.coeficient == 0 && equationTermExponent.Count == 1)
			{
				base[Count - 1] = new Term(newTerm.coeficient);
			}
		}
		#endregion

		#region Format

		public static void Format(ref Equation equation)
		{
			if (equation == null || equation.Count == 0) return;

			#region format operations

			//if first term is operation and not open bracket
			if (equation.Count > 0 && equation[0].GetType() == typeof(Operation) &&
				((Operation)equation[0]).operation != OperationEnum.OpenBracket)
			{
				equation.RemoveAt(0);
			}

			//if last term is operation and not closed bracket
			if (equation.Count > 0 && equation[equation.Count - 1].GetType() == typeof(Operation) &&
				((Operation)equation[equation.Count - 1]).operation != OperationEnum.ClosedBracket)
			{
				equation.RemoveAt(equation.Count - 1);
			}
			#endregion

			float newCoefficient = 1;
			int startTerm = -1;

			for (int i = 0; i < equation.Count - 1; i++)
			{
				if (FormatOperations(i, ref equation)) continue;
				if (i < equation.Count - 1) FormatCoefficients(i, startTerm, ref newCoefficient, ref equation);
			}

			for (int i = 0; i < equation.Count; i++)
			{
				FormatConstants(i, ref equation);
			}
		}

		private static bool FormatOperations(int i, ref Equation equation)
		{
			//format operations
			if (equation[i].GetType() == typeof(Operation))
			{
				#region format all operations
				Operation first = (Operation)(equation[i]);

				if (equation[i + 1].GetType() == typeof(Operation))
				{
					bool formatted = false;
					Operation second = (Operation)(equation[i + 1]);

					//if equal and both subtaction or both addition
					if (first.operation == second.operation &&
						(first.operation == OperationEnum.Subtraction || first.operation == OperationEnum.Addition))
					{
						//change to one addition
						equation[i] = new Operation(OperationEnum.Addition);
						equation.RemoveAt(i + 1);
						formatted = true;
					}
					//one operation addition and the other subtraction
					else if ((first.operation == OperationEnum.Addition &&
						second.operation == OperationEnum.Subtraction) ||
						(first.operation == OperationEnum.Subtraction &&
						second.operation == OperationEnum.Addition))

					{
						//change to one subraction
						equation[i] = new Operation(OperationEnum.Subtraction);
						equation.RemoveAt(i + 1);
						formatted = true;
					}

					if (formatted) Format(ref equation);
				}

				#endregion

				#region format coefficients


				//sort out coefficients for multiple multiplied terms
				if (i >= 1 && first.operation == OperationEnum.Multiplication &&
					equation[i - 1].GetType() == typeof(Term) &&
					equation[i + 1].GetType() == typeof(Term))
				{
					Term firstTerm = (Term)equation[i - 1];
					Term secondTerm = (Term)equation[i + 1];

					if (secondTerm.coeficient == 1) return true;

					//checks is not case ln(2)2^x where the constant would otherwise come out to front (broken)
					if (!(!firstTerm.exponent.EquationsEqual(new Equation { new Term(1) }) &&
						firstTerm.function == Function.constant) &&

						!(!secondTerm.exponent.EquationsEqual(new Equation { new Term(1) }) &&
						secondTerm.function == Function.constant))
					{
						equation[i - 1] = new Term(firstTerm.coeficient * secondTerm.coeficient,
							firstTerm.function, firstTerm.input, firstTerm.exponent);

						equation[i + 1] = new Term(1, secondTerm.function, secondTerm.input, secondTerm.exponent);

						Format(ref equation);
					}
				}

				#endregion
			}

			return false;
		}

		private static void FormatCoefficients(int i, int startTerm, ref float newCoefficient,
			ref Equation equation)
		{
			//format coefficients with multiple multiplied terms
			if (equation[i + 1].GetType() == typeof(Operation) && equation[i].GetType() == typeof(Term))
			{
				Term currentTerm = (Term)equation[i];
				Term startingTerm = new Term(1);

				if (startTerm != -1) startingTerm = (Term)equation[startTerm];

				Operation nextOperation = (Operation)equation[i + 1];
				if (nextOperation.operation == OperationEnum.Multiplication)
				{
					if (startTerm == -1) startTerm = i;
					newCoefficient *= currentTerm.coeficient;
				}
				else if (startTerm != -1)
				{
					equation[startTerm] = new Term(newCoefficient, startingTerm.function, startingTerm.input, startingTerm.exponent);

					newCoefficient = 1;
				}
			}
		}

		private static void FormatConstants(int i, ref Equation equation)
		{
			if (equation[i].GetType() == typeof(Term) && ((Term)equation[i]).function == Function.constant &&
				((Term)equation[i]).exponent.EquationsEqual(new Equation { new Term(1) }))
			{
				//if left term is a multiplication
				if (i - 1 >= 0 && equation[i - 1].GetType() == typeof(Operation) &&
					((Operation)equation[i - 1]).operation == OperationEnum.Multiplication)
				{
					Term term = (Term)equation[i];

					bool format = false;

					//current term is just a 1
					if (term.coeficient == 1)
					{
						equation.RemoveAt(i);
						equation.RemoveAt(i - 1);
						format = true;
					}

					if (term.coeficient == 0 && equation[i - 2].GetType() == typeof(Term))
					{
						equation[i - 2] = new Term(0f);
						format = true;
					}

					if (format) Format(ref equation);
				}

				//if right term is a multiplication
				if (i + 1 < equation.Count && equation[i + 1].GetType() == typeof(Operation) &&
					((Operation)equation[i + 1]).operation == OperationEnum.Multiplication &&

					equation[i].GetType() == typeof(Term) && ((Term)equation[i]).function == Function.constant)
				{
					Term term = (Term)equation[i];

					//current term is just a 1
					if (term.coeficient == 1)
					{
						equation.RemoveAt(i);
						equation.RemoveAt(i + 1);
						Format(ref equation);
					}

					if (term.coeficient == 0 && equation[i + 2].GetType() == typeof(Term))
					{
						equation[i + 2] = new Term(0f);
						Format(ref equation);
					}
				}
			}

			//turn all 0 coefficients to a constant of 0
			if (i < equation.Count && equation[i].GetType() == typeof(Term))
			{
				Term term = (Term)equation[i];

				if (term.coeficient == 0) equation[i] = new Term(0f);
			}
		}

		#endregion

		#region Useful
		public bool requiresBrackets()
		{
			return Count > 1 &&
				!(this.Last().GetType() == typeof(Operation) &&
				((Operation)this.Last()).operation == OperationEnum.ClosedBracket &&
				this.First().GetType() == typeof(Operation) &&
				((Operation)this.First()).operation == OperationEnum.OpenBracket ||

				(this.First().GetType() == typeof(Operation) &&
				((Operation)this.First()).operation == OperationEnum.OpenBracket &&
				this[Count - 2].GetType() == typeof(Operation) &&
				((Operation)this[Count - 2]).operation == OperationEnum.Power)) &&

				!IsOne();
		}

		private bool IsOne()
		{
			//checks if equation is one term all multiplied
			for (int i = 0; i < Count; i++)
			{
				if (this[i].GetType() == typeof(Operation))
				{
					OperationEnum operation = ((Operation)this[i]).operation;

					if (operation != OperationEnum.Multiplication && operation != OperationEnum.Power &&
					operation != OperationEnum.OpenBracket && operation != OperationEnum.ClosedBracket)
					{
						return false;
					}
				}
			}

			return true;
		}

		public bool IsConstant()
		{
			if (this.Count > 0 && this[0].GetType() != typeof(Term)) return false;

			//if exponent just a constant
			return Count == 1 && ((Term)this[0]).function == Function.constant;
		}

		#endregion
	}
}
