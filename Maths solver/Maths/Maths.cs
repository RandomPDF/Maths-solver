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
		public static Dictionary<Function, List<EquationItem>> Differentials = 
			new Dictionary<Function, List<EquationItem>>()
		{
			{Function.sin, new List<EquationItem>()
			{new Term(1, Function.cos, new List<EquationItem> { new Term(1, Function.constant) } ) } },


			{Function.cos, new List<EquationItem>()
			{new Term(-1, Function.sin, new List<EquationItem> { new Term(1, Function.constant) }) } },


			{Function.tan, new List<EquationItem>()
			{new Term(1, Function.sec, new List<EquationItem> { new Term(2, Function.constant) }) } },


			{Function.cosec, new List<EquationItem>()
			{new Term(-1, Function.cosec, new List<EquationItem>{new Term(1, Function.constant)}),
			new Operation(OperationEnum.Multiplication),
			new Term(1, Function.cot, new List<EquationItem>{new Term(1, Function.constant)})}},


			{Function.sec, new List<EquationItem>()
			{new Term(1, Function.sec, new List<EquationItem>{new Term(1, Function.constant)}),
			new Operation(OperationEnum.Multiplication),
			new Term(1, Function.tan, new List<EquationItem>{new Term(1, Function.constant)})}},


			{Function.cot, new List<EquationItem>()
			{new Term(-1, Function.cosec, new List<EquationItem> { new Term(2, Function.constant) }) } },


			{Function.sinh, new List<EquationItem>()
			{new Term(1, Function.cosh, new List<EquationItem> { new Term(1, Function.constant) } ) } },


			{Function.cosh, new List<EquationItem>()
			{new Term(1, Function.sinh, new List<EquationItem> { new Term(1, Function.constant) } ) } },


			{Function.tanh, new List<EquationItem>()
			{new Term(1, Function.sech, new List<EquationItem> { new Term(2, Function.constant) } ) } },


			{Function.cosech, new List<EquationItem>()
			{new Term(-1, Function.cosech, new List<EquationItem>{new Term(1, Function.constant)}),
			new Operation(OperationEnum.Multiplication),
			new Term(1, Function.coth, new List<EquationItem>{new Term(1, Function.constant)})}},


			{Function.sech, new List<EquationItem>()
			{new Term(-1, Function.sech, new List<EquationItem>{new Term(1, Function.constant)}),
			new Operation(OperationEnum.Multiplication),
			new Term(1, Function.tanh, new List<EquationItem>{new Term(1, Function.constant)})}},


			{Function.coth, new List<EquationItem>()
			{new Term(-1, Function.cosech, new List<EquationItem> { new Term(2, Function.constant) }) } },


			{Function.ln, new List<EquationItem>()
			{new Term(1, Function.x, new List<EquationItem> { new Term(-1, Function.constant) }) } },
		};

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
				//for each term in the correct differential
				foreach (EquationItem differentialObject in differential)
				{
					if (differentialObject.GetType() == typeof(Term))
					{
						Term differentialTerm = (Term)differentialObject;
						newTerm = default;

						//if first term, add coefficient (all operatins in value are Multiplication
						if (differentialObject == differential[0])
						{
							newTerm = new Term(term.coeficient * differentialTerm.coeficient,
								differentialTerm.function, differentialTerm.exponent);
						}
						else
						{
							newTerm = new Term(1, differentialTerm.function,
								differentialTerm.exponent);
						}

						AddTerm(newTerm, ref newEquation);

					}

					if (differentialObject.GetType() == typeof(Operation))
					{
						newEquation.Add(differentialObject);
					}
				}
			}
			else
			{
				switch(term.function)
				{
					case Function.x:
						DifferentiateX(term, ref newTerm, ref newEquation);
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

				if (exponent.coeficient != 0)
				{
					//ax^n => anx^(n-1)
					newTerm = new Term(term.coeficient * exponent.coeficient, Function.x,
							new List<EquationItem> 
							{ new Term(exponent.coeficient - 1, Function.constant) });

					AddTerm(newTerm, ref newEquation);
				}
				else
                {
					if(newEquation.Count > 1)
                    {
						newEquation.RemoveAt(newEquation.Count - 1);
					}
                }
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

		/*private static bool EquationsEqual(List<EquationItem> equation1, List<EquationItem> equation2)
        {
            for (int i = 0; i < equation1.Count; i++)
            {
				//check if terms equal
				if (equation1[i].GetType() == typeof(Term) && equation2[i].GetType() == typeof(Term))
				{
					Term term1 = (Term)equation1[i];
					Term term2 = (Term)equation2[i];

					if (!TermsEqual(term1, term2, false)) return false;
				}

				//check if operations equal
				if (equation1[i].GetType() == typeof(Operation) &&
				equation2[i].GetType() == typeof(Operation) &&
				(Operation)equation1[i] != (Operation)equation2[i])
				{
					Operation term1 = (Operation)equation1[i];
					Operation term2 = (Operation)equation2[i];

					if (term1.GetOperation() != term2.GetOperation()) return false;
				}

				//check if same type
				if (equation1[i].GetType() != equation2[i].GetType()) return false;
			}

			return true;
        }

		private static bool TermsEqual(Term term1, Term term2, bool areExponents)
        {
			//check if functions match
			if(!areExponents && term1.GetFunction() == term2.GetFunction())
            {
				if (term1.GetExponent() != null && term2.GetExponent() != null &&
					EquationsEqual(term1.GetExponent(), term2.GetExponent())) return true;

				if (term1.GetExponent() == null && term2.GetExponent() == null) return true;
            }

			if(areExponents && term1.GetFunction() == term2.GetFunction() &&
				term1.GetCoeficient() == term2.GetCoeficient()) return true;

			return false;
        }*/


		//failure case x³ - x⁰ + x
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

            for (int i = 0; i < equation.Count - 1; i++)
            {
				if(equation[i].GetType() == typeof(Operation) && 
					equation[i+1].GetType() == typeof(Operation))
                {
					//if equal and both subtaction
					if (((Operation)(equation[i])).operation == 
						((Operation)(equation[i+1])).operation &&
						((Operation)(equation[i])).operation == OperationEnum.Subtraction)
					{
						//change to one addition
						equation[i] = new Operation(OperationEnum.Addition);
					}
					//operations different. (one must be addition, the other subtraction)
                    else
                    {
						//change to one subraction
						equation[i] = new Operation(OperationEnum.Subtraction);
					}

					equation.RemoveAt(i + 1);

					FormatEquation(ref equation);
				}
            }
		}

		private static void FormatTerm(Term newTerm, ref List<EquationItem> newEquation)
		{
			//Checks if term is negative, and more than 2 items in the equation
			if (newTerm.coeficient < 0 && newEquation.Count >= 2 && 
				newEquation[newEquation.Count - 2].GetType() == typeof(Operation))
			{
				//Negate operation
				switch(((Operation)newEquation[newEquation.Count - 2]).operation)
				{
					case OperationEnum.Addition:
						newEquation[newEquation.Count - 2] = new Operation(OperationEnum.Subtraction);
						break;

					case OperationEnum.Subtraction:
						newEquation[newEquation.Count - 2] = new Operation(OperationEnum.Addition);
						break;
				}

				//negate coefficient
				newEquation[newEquation.Count - 1] =
						new Term(-newTerm.coeficient, newTerm.function, newTerm.exponent);
			}
		}
	}
}
