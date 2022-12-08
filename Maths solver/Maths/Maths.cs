using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Maths_solver
{
	internal class Maths : Functions
	{
		public static Dictionary<List<EquationItem>, List<EquationItem>> Differentials = 
			new Dictionary<List<EquationItem>, List<EquationItem>>()
		{
			{new List<EquationItem>()
			{new Term(1, Function.sin, new List<EquationItem> {new Term(1, Function.a)}) },

			new List<EquationItem>()
			{new Term(1, Function.cos, new List<EquationItem> { new Term(1, Function.a) } ) } },


			{new List<EquationItem>()
			{new Term(1, Function.cos, new List<EquationItem> { new Term(1, Function.a) }) },

			new List<EquationItem>()
			{new Term(-1, Function.sin, new List<EquationItem> { new Term(1, Function.a) }) } },


			{new List<EquationItem>()
			{new Term(1, Function.tan, new List<EquationItem> { new Term(1, Function.a) }) },

			new List<EquationItem>()
			{new Term(1, Function.sec, new List<EquationItem> { new Term(2, Function.a) }) } },


			{new List<EquationItem>()
			{new Term(1, Function.cosec, new List<EquationItem> { new Term(1, Function.a) }) },

			new List<EquationItem>()
			{new Term(-1, Function.cosec, new List<EquationItem>{new Term(1, Function.a)}),
			new Operation(OperationEnum.Multiplication),
			new Term(1, Function.cot, new List<EquationItem>{new Term(1, Function.a)})}},


			{new List<EquationItem>()
			{new Term(1, Function.sec, new List<EquationItem> { new Term(1, Function.a) }) },

			new List<EquationItem>()
			{new Term(1, Function.sec, new List<EquationItem>{new Term(1, Function.a)}),
			new Operation(OperationEnum.Multiplication),
			new Term(1, Function.tan, new List<EquationItem>{new Term(1, Function.a)})}},


			{new List<EquationItem>()
			{new Term(1, Function.cot, new List<EquationItem> { new Term(1, Function.a) }) },

			new List<EquationItem>()
			{new Term(-1, Function.cosec, new List<EquationItem> { new Term(2, Function.a) }) } },


			{new List<EquationItem>()
			{new Term(1, Function.ln, new List<EquationItem> { new Term(1, Function.a) }) },

			new List<EquationItem>()
			{new Term(1, Function.x, new List<EquationItem> { new Term(-1, Function.a) }) } },
		};

		public static Dictionary<char, OperationEnum> operations = 
			new Dictionary<char, OperationEnum>
		{   {'+', OperationEnum.Addition},
			{'-', OperationEnum.Subtraction},
			{'/', OperationEnum.Division} };

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

					if (term.GetFunction() != Function.x)
					{
						//finds correct key value pair
						foreach (List<EquationItem> key in Differentials.Keys.ToArray())
						{
							//get each term in differential
							foreach (EquationItem keyObject in key)
							{
								//should be no operations in key
								if (keyObject.GetType() != typeof(Term)) continue;

								Term keyTerm = (Term)keyObject;

								//check if correct key and return correct value
								if (TermsEqual(term, keyTerm, false)) differential = Differentials[key];
							}
						}
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

			//if differentiating x
			if (term.GetFunction() == Function.x) DifferentiateX(term, ref newTerm, ref newEquation);
			else
			{
				//for each term in the correct differential
				foreach (EquationItem differentialObject in differential)
				{
					if (differentialObject.GetType() == typeof(Term))
					{
						Term differentialTerm = (Term)differentialObject;
						newTerm = default;

						//if first term, add coefficient (change later)
						if (differentialObject == differential[0])
						{
							newTerm = new Term(term.GetCoeficient() * differentialTerm.GetCoeficient(),
								differentialTerm.GetFunction(), differentialTerm.GetExponent());
						}
						else
						{
							newTerm = new Term(1, differentialTerm.GetFunction(),
								differentialTerm.GetExponent());
						}

						AddTerm(newTerm, ref newEquation);

					}

					if (differentialObject.GetType() == typeof(Operation))
					{
						newEquation.Add(differentialObject);
					}
				}
			}
		}

		private static void DifferentiateX(Term term, ref Term newTerm, ref List<EquationItem> newEquation)
		{
			//check if the exponent isn't a long equation
			if (term.GetExponent()[0].GetType() == typeof(Term) && term.GetExponent().Count == 1)
			{
				Term exponent = (Term)term.GetExponent()[0];

				//if exponent isn't constant
				if (exponent.GetFunction() != Function.a) return;

				//ax^n => anx^(n-1)
				newTerm = new Term(term.GetCoeficient() * exponent.GetCoeficient(), Function.x,
						new List<EquationItem> { new Term(exponent.GetCoeficient() - 1, Function.a) });

				AddTerm(newTerm, ref newEquation);
			}
		}

		private static void AddTerm(Term newTerm, ref List<EquationItem> newEquation)
        {
			if (newTerm.GetCoeficient() != 0)
			{
				newEquation.Add(newTerm);

				FormatTerm(newTerm, ref newEquation);
			}
		}

		private static bool EquationsEqual(List<EquationItem> equation1, List<EquationItem> equation2)
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
        }

		private static void FormatEquation(ref List<EquationItem> equation)
        {
			//if first term is addition
			if (equation.Count > 0 && equation[0].GetType() == typeof(Operation) &&
				((Operation)equation[0]).GetOperation() == OperationEnum.Addition)
			{
				equation.RemoveAt(0);
			}
		}

		private static void FormatTerm(Term newTerm, ref List<EquationItem> newEquation)
		{
			//Checks if term is negative, and more than 2 items in the equation
			if (newTerm.GetCoeficient() < 0 && newEquation.Count >= 2 && 
				newEquation[newEquation.Count - 2].GetType() == typeof(Operation))
			{
				Operation previousOperation =
							(Operation)newEquation[newEquation.Count - 2];

				//change coeficient to positive, and operation to subtraction
				if (previousOperation.GetOperation() == OperationEnum.Addition)
				{
					newEquation.RemoveRange(newEquation.Count - 2, 2);
					newEquation.Add(new Operation(OperationEnum.Subtraction));

					newEquation.Add(new Term(-newTerm.GetCoeficient(),
						newTerm.GetFunction(), newTerm.GetExponent()));
				}

				//change coeficient to positive, and operation to addition
				if (previousOperation.GetOperation() == OperationEnum.Subtraction)
				{
					newEquation.RemoveRange(newEquation.Count - 2, 2);
					newEquation.Add(new Operation(OperationEnum.Addition));

					newEquation.Add(new Term(-newTerm.GetCoeficient(),
						newTerm.GetFunction(), newTerm.GetExponent()));
				}
			}
		}
	}
}
