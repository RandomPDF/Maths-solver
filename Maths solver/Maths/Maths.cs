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
		public static Dictionary<List<EquationItem>, List<EquationItem>> Differentials = new Dictionary<List<EquationItem>, List<EquationItem>>()
		{
			{new List<EquationItem>(){new Term(1, Function.sin, new List<EquationItem> {new Term(1, Function.a)}) },
				new List<EquationItem>(){new Term(1, Function.cos, new List<EquationItem> { new Term(1, Function.a) } ) } },

			{new List<EquationItem>(){new Term(1, Function.cos, new List<EquationItem> { new Term(1, Function.a) }) },
				new List<EquationItem>(){new Term(-1, Function.sin, new List<EquationItem> { new Term(1, Function.a) }) } },

			{new List<EquationItem>(){new Term(1, Function.tan, new List<EquationItem> { new Term(1, Function.a) }) },
				new List<EquationItem>(){new Term(1, Function.sec, new List<EquationItem> { new Term(2, Function.a) }) } },

			{new List<EquationItem>(){new Term(1, Function.cosec, new List<EquationItem> { new Term(1, Function.a) }) },
			new List<EquationItem>(){new Term(-1, Function.cosec, new List<EquationItem>{new Term(1, Function.a)}),
				new Operation(OperationEnum.Multiplication), new Term(1, Function.cot, new List<EquationItem>{new Term(1, Function.a)})}}
		};

		static List<EquationItem> test = new List<EquationItem>()
		{
			new Term(4, Function.cos, new List<EquationItem>{new Term(1, Function.a)}),
			new Operation(OperationEnum.Subtraction),
			new Term(2, Function.sin, new List<EquationItem>{new Term(1, Function.a)}),
			new Operation(OperationEnum.Addition),
			new Term(3, Function.tan, new List<EquationItem>{new Term(1, Function.a)}),
			new Operation(OperationEnum.Subtraction),
			new Term(10, Function.cosec, new List<EquationItem>{new Term(1, Function.a)})
		};

		//3x^3 + 5sin(x^2)^2
		static List<EquationItem> hard = new List<EquationItem>()
		{
			//3x^3
			new Term(3, Function.x, new List<EquationItem>{new Term(3, Function.a)}),

			new Operation(OperationEnum.Addition),

			//5sin(x^2)^2
			new Term(5, Function.sin, new Term(1, Function.x, new List<EquationItem>{new Term(2, Function.a)}) , new List<EquationItem>{new Term(2, Function.a)})
		};

		private static List<EquationItem> Differentiate(List<EquationItem> equation)
		{
			List<EquationItem> newEquation = new List<EquationItem>();

			//find term or operation in equation
			foreach (EquationItem Object in equation)
			{
				if(Object.GetType() == typeof(Term))
				{
					Term term = (Term)Object;

					List<EquationItem> differential = new List<EquationItem>();

					//finds correct key value pair
					foreach (List<EquationItem> key in Differentials.Keys.ToArray())
					{
						//get each term in differential
						foreach (EquationItem keyObject in key)
						{
							if (keyObject.GetType() == typeof(Term))
							{
								Term keyTerm = (Term)keyObject;

								//check if correct key
								//add function to test 2 terms and see if they match
								if (TermsEqual(term, keyTerm, false))
								{
									//return correct value
									differential = Differentials[key];
								}
							}

							//should be no operations in key
						}
					}

					//for each term in the correct differential
					foreach (EquationItem differentialObject in differential)
					{
						if (differentialObject.GetType() == typeof(Term))
						{
							Term differentialTerm = (Term)differentialObject;
							Term newTerm;

							//if first term, add coefficient (change later)
							if (differentialObject == differential[0])
							{
								newTerm = new Term(term.GetCoeficient() * differentialTerm.GetCoeficient(), differentialTerm.GetFunction(), differentialTerm.GetExponent());
							}
							else
							{
								newTerm = new Term(1, differentialTerm.GetFunction(), differentialTerm.GetExponent());
							}

							newEquation.Add(newTerm);

							//Checks if term is negative, and more than 2 items in the equation
							if(newTerm.GetCoeficient() < 0 && newEquation.Count >= 2)
							{
								if(newEquation[newEquation.Count - 2].GetType() == typeof(Operation))
								{
									Operation previousOperation = (Operation)newEquation[newEquation.Count - 2];

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

						if(differentialObject.GetType() == typeof(Operation))
						{
							newEquation.Add(differentialObject);
						}
					}
				}

				if(Object.GetType() == typeof(Operation))
				{
					newEquation.Add(Object);
				}
			}

			return newEquation;
		}

		private static bool EquationsEqual(List<EquationItem> equation1, List<EquationItem> equation2)
        {
            for (int i = 0; i < equation1.Count; i++)
            {
                for (int j = 0; j < equation2.Count; j++)
                {
					if(equation1[i].GetType() == typeof(Term) && equation2[j].GetType() == typeof(Term))
                    {
						Term term1 = (Term)equation1[i];
						Term term2 = (Term)equation2[j];

						if (!TermsEqual(term1, term2, false))
						{
							return false;
						}
					}
                }
            }

			return true;
        }

		public static bool TermsEqual(Term term1, Term term2, bool areExponents)
        {
			//check if functions match
			if(!areExponents && term1.GetFunction() == term2.GetFunction())
            {
				//check if exponents match
				if(term1.GetExponent() != null && term2.GetExponent() != null &&
					term1.GetExponent().Count == 1 && term1.GetExponent().Count == 1 &&
					term1.GetExponent()[0].GetType() == typeof(Term) &&
					term1.GetExponent()[0].GetType() == typeof(Term))
				{
					Term term1Exponent = (Term)term1.GetExponent()[0];
					Term term2Exponent = (Term)term2.GetExponent()[0];

					if (TermsEqual(term1Exponent, term2Exponent, true))
                    {
						return true;
                    }
                }

				if(term1.GetExponent() == null && term2.GetExponent() == null)
                {
					return true;
                }
            }

			if(areExponents && term1.GetFunction() == term2.GetFunction() &&
				term1.GetCoeficient() == term2.GetCoeficient())
            {
				return true;
            }

			return false;
        }

		private static void DisplayEquation(List<EquationItem> equation)
		{
			foreach (EquationItem item in equation)
			{
				if (item.GetType() == typeof(Term))
				{
					Term term = (Term)item;
					Console.Write(FormatTerm(term));
				}

				if (item.GetType() == typeof(Operation))
				{
					Operation operation = (Operation)item;
					Console.Write(FormatTerm(operation));
				}
			}
		}

		private static string FormatTerm(Term term)
		{
			string formatTerm = String.Empty;

			if (Math.Abs(term.GetCoeficient()) != 1) formatTerm += term.GetCoeficient();
			else if (term.GetCoeficient() == -1) formatTerm += "-";

			if (functions[term.GetFunction()]) formatTerm += $"{term.GetFunction()}({FormatTerm(term.GetInput())})";
			else formatTerm += term.GetFunction();

			if (term.GetExponent().Count == 1 && term.GetExponent()[0].GetType() == typeof(Term))
			{
				Term termExponent = (Term)term.GetExponent()[0];

				if (termExponent.GetCoeficient() != 1)
				{
					formatTerm += $"^{termExponent.GetCoeficient()}";
				}
			}

			return formatTerm;
		}

		private static string FormatTerm(Operation operation)
		{
			string formatTerm = String.Empty;

			switch(operation.GetOperation())
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

		public static void Run()
		{
			Console.WriteLine("Origional equation: ");
			DisplayEquation(test);

			Console.WriteLine("\n\nNew equation:");
            DisplayEquation(Differentiate(test));
		}
	}
}
