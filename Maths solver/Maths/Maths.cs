using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Maths_solver.Maths
{
	internal class Maths : Functions
	{
		private static Dictionary<Function, List<EquationItem>> Differentials = 
			new Dictionary<Function, List<EquationItem>>()
		{
			{Function.sin, new List<EquationItem>() {new Term(Function.cos) } },

			{Function.cos, new List<EquationItem>() {new Term(-1, Function.sin)} },


			{Function.tan, new List<EquationItem>()
			{new Term(Function.sec, new List<EquationItem> { new Term(2) }) } },


			{Function.cosec, new List<EquationItem>()
			{new Term(-1, Function.cosec),
			new Operation(OperationEnum.Multiplication),
			new Term(Function.cot)}},


			{Function.sec, new List<EquationItem>()
			{new Term(Function.sec),
			new Operation(OperationEnum.Multiplication),
			new Term(Function.tan)}},


			{Function.cot, new List<EquationItem>()
			{new Term(-1, Function.cosec, new List<EquationItem> { new Term(2) }) } },


			{Function.sinh, new List<EquationItem>() {new Term(Function.cosh) } },


			{Function.cosh, new List<EquationItem>() {new Term(Function.sinh) } },


			{Function.tanh, new List<EquationItem>()
			{new Term(Function.sech, new List<EquationItem> { new Term(2) } ) } },


			{Function.cosech, new List<EquationItem>()
			{new Term(-1, Function.cosech),
			new Operation(OperationEnum.Multiplication),
			new Term(Function.coth)}},


			{Function.sech, new List<EquationItem>()
			{new Term(-1, Function.sech),
			new Operation(OperationEnum.Multiplication),
			new Term(Function.tanh)}},


			{Function.coth, new List<EquationItem>()
			{new Term(-1, Function.cosech) } },


			{Function.ln, new List<EquationItem>()
			{new Term(Function.x, new List<EquationItem> { new Term(-1) }) } },
		};

		private static object sender;
		public static event EventHandler<Step> ShowSteps;

        #region Differentiate
        public static List<EquationItem> DifferentiateEquation(List<EquationItem> equation)
		{
			List<EquationItem> newEquation = new List<EquationItem>();

			//find term or operation in equation
			foreach (EquationItem Object in equation)
			{
				if(Object.GetType() == typeof(Term))
				{
					Term term = (Term)Object;
					List<EquationItem> differential = new List<EquationItem>();

					//find differential by function
					if (Differentials.ContainsKey(term.function))
					{
						differential = Differentials[term.function];
					}

					DifferentiateTerm(differential, term, ref newEquation);
				}

				if(Object.GetType() == typeof(Operation)) newEquation.Add(Object);
			}

			FormatEquation(ref newEquation);

			return newEquation;
		}

		private static void DifferentiateTerm(List<EquationItem> differential, Term term, 
			ref List<EquationItem> newEquation)
		{
			Term newTerm = default;

			if (Differentials.ContainsKey(term.function)) 
			{
				ShowSteps?.Invoke(sender, new Step(Rule.Standard, new List<EquationItem>{ term }));

				ShowSteps?.Invoke(sender, new Step(Rule.Chain, term.input));

				//chain rule
				List<EquationItem> inputDifferential = DifferentiateEquation(term.input);

				ShowSteps?.Invoke(sender, new Step(Phase.End));

				//checks if equation is one term all multiplied
				bool oneTerm = true;
				for (int i = 0; i < inputDifferential.Count; i++)
				{
					if (inputDifferential[i].GetType() == typeof(Operation) &&
						((Operation)inputDifferential[i]).operation != OperationEnum.Multiplication)
					{
						oneTerm = false;
						break;
					}
				}

				int firstIndex = 0;
				for (int i = 0; i < inputDifferential.Count; i++)
				{
					if (inputDifferential[i].GetType() == typeof(Term))
					{
						firstIndex = i;
						break;
					}
				}

				Term first = (Term)inputDifferential[firstIndex];

				//if is a constant with coefficient of 1 or 0 ignore adding
				if (!(first.function == Function.constant &&
					(first.coeficient == 0 || first.coeficient == 1)))
				{
					if (!oneTerm) newEquation.Add(new Operation(OperationEnum.OpenBracket));

					for (int i = 0; i < inputDifferential.Count; i++)
						newEquation.Add(inputDifferential[i]);

					if (!oneTerm) newEquation.Add(new Operation(OperationEnum.ClosedBracket));

					newEquation.Add(new Operation(OperationEnum.Multiplication));
				}

				//for each term in the correct differential
				foreach (EquationItem differentialObject in differential)
				{
					if (differentialObject.GetType() == typeof(Term))
					{
						Term differentialTerm = (Term)differentialObject;
						newTerm = default;

						//if first term, add coefficient (all operations in value are Multiplication)
						if (differentialObject == differential[0])
						{
							newTerm = new Term(term.coeficient * differentialTerm.coeficient,
								differentialTerm.function, term.input, differentialTerm.exponent);
						}
						else
						{
							newTerm = new Term(1, differentialTerm.function, term.input,
								differentialTerm.exponent);
						}

						AddTerm(newTerm, ref newEquation);

					}

					if (differentialObject.GetType() == typeof(Operation))
					{
						newEquation.Add(differentialObject);
					}
				}

				ShowSteps?.Invoke(sender, new Step(Rule.Standard,
					new List<EquationItem> { new Term(term.function) }, differential));
			}
			else
			{
				switch(term.function)
				{
					case Function.x:
						DifferentiateX(term, ref newTerm, ref newEquation);
						break;

					case Function.constant:
						ShowSteps?.Invoke(sender, new Step(Rule.Constant,
							new List<EquationItem> { new Term(term.coeficient) }));
						break;
				}
			}
		}

		private static void DifferentiateX(Term term, ref Term newTerm, ref List<EquationItem> newEquation)
		{
			//if term is ax^n only
			if (term.exponent[0].GetType() == typeof(Term) && term.exponent.Count == 1 &&
				((Term)term.exponent[0]).function == Function.constant)
			{
				Term exponent = (Term)term.exponent[0];

				ShowSteps?.Invoke(sender, new Step(Rule.x, new List<EquationItem> { term }));

				if (exponent.coeficient != 0)
				{
					//ax^n => anx^(n-1)
					newTerm = new Term(term.coeficient * exponent.coeficient, Function.x,
							new List<EquationItem> { new Term(exponent.coeficient - 1) });

					AddTerm(newTerm, ref newEquation);

					ShowSteps?.Invoke(sender, new Step(Rule.x, new List<EquationItem> { term },
						new List<EquationItem>{ newTerm}));

					ShowSteps?.Invoke(sender, new Step(Phase.End));
				}
				else
                {
					if(newEquation.Count > 1)
                    {
						newEquation.RemoveAt(newEquation.Count - 1);
					}

					ShowSteps?.Invoke(sender, new Step(Rule.Constant, new List<EquationItem> { term }));
				}

				ShowSteps?.Invoke(sender, new Step(Phase.End));
			}
		}

		private static void AddTerm(Term newTerm, ref List<EquationItem> newEquation)
        {
			if (newTerm.coeficient != 0)
			{
				newEquation.Add(newTerm);

				FormatTerm(newTerm, ref newEquation);
			}
		}

        #endregion
        #region Format
        private static void FormatEquation(ref List<EquationItem> equation)
        {
			//if first term is addition
			if (equation.Count > 0 && equation[0].GetType() == typeof(Operation) &&
				((Operation)equation[0]).operation == OperationEnum.Addition)
			{
				equation.RemoveAt(0);
			}

			//if last term is operation
			if (equation[equation.Count - 1].GetType() == typeof(Operation))
			{
				equation.RemoveAt(equation.Count - 1);
			}

			float newCoefficient = 1;
			int startTerm = -1;

			//terms
            for (int i = 0; i < equation.Count; i++)
            {
				//convert x^0 to a constant
				if (equation[i].GetType() == typeof(Term) &&
					((Term)((Term)equation[i]).exponent[0]).coeficient == 0 &&
					((Term)equation[i]).function == Function.x)
				{
					equation[i] = new Term(((Term)equation[i]).coeficient);
				}
			}

			//operations
            for (int i = 0; i < equation.Count - 1; i++)
            {
				//format operations
				if (equation[i].GetType() == typeof(Operation))
                {
					Operation first = (Operation)(equation[i]);

					if (equation[i + 1].GetType() == typeof(Operation))
					{
						bool formatted = false;
						Operation second = (Operation)(equation[i + 1]);

						//if equal and both subtaction
						if (first.operation == second.operation &&
							first.operation == OperationEnum.Subtraction)
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

						if(formatted) FormatEquation(ref equation);
					}

					if (first.operation == OperationEnum.Multiplication &&
						((Term)equation[i + 1]).coeficient != 1 && 
						equation[i-1].GetType() == typeof(Term) &&
						equation[i+1].GetType() == typeof(Term))
					{
						Term firstTerm = (Term)equation[i - 1];
						Term secondTerm = (Term)equation[i + 1];

						equation[i - 1] = new Term(firstTerm.coeficient * secondTerm.coeficient,
							firstTerm.function, firstTerm.input, firstTerm.exponent);

						equation[i + 1] = new Term(1, secondTerm.function, secondTerm.input, secondTerm.exponent);

						FormatEquation(ref equation);
					}
				}

				//struggles with brackets
				//format coefficients with multiple multiplied terms
				if (equation[i+1].GetType() == typeof(Operation) && equation[i].GetType() == typeof(Term))
				{
					Term currentTerm = (Term)equation[i];
					Term startingTerm = new Term();

					if (startTerm != -1) startingTerm = (Term)equation[startTerm];

					if (((Operation)equation[i+1]).operation == OperationEnum.Multiplication)
					{
						if (startTerm == -1) startTerm = i;
						newCoefficient *= currentTerm.coeficient;
					}
					else if(startTerm != -1)
					{
						equation[startTerm] = new Term(newCoefficient, startingTerm.function, startingTerm.input, startingTerm.exponent);

						newCoefficient = 1;
					}
				}
			}
		}

		private static void FormatTerm(Term newTerm, ref List<EquationItem> newEquation)
		{
			//Checks if term is negative, and more than 2 items in the equation
			if (newTerm.coeficient < 0 && newEquation.Count >= 2 && 
				newEquation[newEquation.Count - 2].GetType() == typeof(Operation))
			{
				bool negate = false;
				//Negate operation
				switch(((Operation)newEquation[newEquation.Count - 2]).operation)
				{
					case OperationEnum.Addition:
						newEquation[newEquation.Count - 2] = new Operation(OperationEnum.Subtraction);
						negate = true;
						break;

					case OperationEnum.Subtraction:
						newEquation[newEquation.Count - 2] = new Operation(OperationEnum.Addition);
						negate = true;
						break;
				}

				if (negate)
				{
					//negate coefficient
					newEquation[newEquation.Count - 1] =
							new Term(-newTerm.coeficient, newTerm.function, newTerm.input, newTerm.exponent);
				}
			}
		}

        #endregion
    }
}
