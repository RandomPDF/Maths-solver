using System.Collections.Generic;
using static Maths_solver.Maths.Functions;
using static Maths_solver.Maths.Operation;

namespace Maths_solver.Maths
{
	public class Equation : List<EquationItem>
	{
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

			if (equationFirstTermExponent.coeficient == 0 &&
				newTerm.function == Function.x)
			{
				base[Count - 1] = new Term(newTerm.coeficient);
			}
		}
	}
}
