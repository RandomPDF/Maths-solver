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
			{new List<EquationItem>(){new Term(1, Function.sin, new Term(1, Function.a)) },
				new List<EquationItem>(){new Term(1, Function.cos, new Term(1, Function.a)) } },

			{new List<EquationItem>(){new Term(1, Function.cos, new Term(1, Function.a)) },
				new List<EquationItem>(){new Term(-1, Function.sin, new Term(1, Function.a)) } },

			{new List<EquationItem>(){new Term(1, Function.tan, new Term(1, Function.a)) },
				new List<EquationItem>(){new Term(1, Function.sec, new Term(2, Function.a)) } },

			{new List<EquationItem>(){new Term(1, Function.cosec, new Term(1, Function.a)) },
			new List<EquationItem>(){new Term(-1, Function.cosec, new Term(1, Function.a)),
				new Operation(OperationEnum.Multiplication), new Term(1, Function.cot, new Term(1, Function.a))}}
		};

		static List<EquationItem> test = new List<EquationItem>()
		{
			new Term(4, Function.cos, new Term(1, Function.a)),
			new Operation(OperationEnum.Subtraction),
			new Term(2, Function.sin, new Term(1, Function.a)),
			new Operation(OperationEnum.Addition),
			new Term(3, Function.tan, new Term(1, Function.a)),
			new Operation(OperationEnum.Subtraction),
			new Term(10, Function.cosec, new Term(1, Function.a))
		};

		//3x^3 + 5sin(x^2)^2
		static List<EquationItem> hard = new List<EquationItem>()
		{
			//3x^3
			new Term(3, Function.x, new Term(3, Function.a)),

			new Operation(OperationEnum.Addition),

			//5sin(x^2)^2
			new Term(5, Function.sin, new Term(1, Function.x, new Term(2, Function.a)) ,new Term(2, Function.a))
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
								if (term.GetFunction() == keyTerm.GetFunction() &&
									term.GetExponent() == keyTerm.GetExponent())
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

			if (term.GetExponent() != 1) formatTerm += $"^{term.GetExponent()}";
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
