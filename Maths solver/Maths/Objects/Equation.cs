using System.Collections.Generic;
using static Maths_solver.Maths.Operation;

namespace Maths_solver.Maths
{
	public class Equation : List<EquationItem>
	{
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


		public void Add(Equation equation)
		{
			for (int i = 0; i < equation.Count; i++) Add(equation[i]);
		}

		public new void Add(EquationItem item)
		{
			if (item == null) return;

			base.Add(item);

			if (item.GetType() != typeof(Term)) return;

			Term newTerm = (Term)item;
			if (newTerm.coeficient != 0) FormatTerm(newTerm);
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
	}
}
