using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maths_solver
{
	internal class Maths : Functions
	{
		public static Dictionary<List<object>, List<object>> Differentials = new Dictionary<List<object>, List<object>>()
		{
			{new List<object>(){new Term(1, Function.sin, 1) }, new List<object>(){new Term(1, Function.cos, 1) } },
			{new List<object>(){new Term(1, Function.cos, 1) }, new List<object>(){new Term(-1, Function.sin, 1) } },
			{new List<object>(){new Term(1, Function.tan, 1)}, new List<object>(){new Term(1, Function.sec, 2)} }
		};

		static List<object> test = new List<object>()
		{
			new Term(4, Function.cos, 1),
			new Operation(OperationEnum.Subtraction),
			new Term(2, Function.sin, 1),
			new Operation(OperationEnum.Addition),
			new Term(3, Function.tan, 1)
		};

		//3x^3 + 5sin(x^2)^2
		static List<object> hard = new List<object>()
		{
			//3x^3
			new Term(3, Function.x, 3),

			new Operation(OperationEnum.Addition),

			//5sin(x^2)^2
			new Term(5, Function.sin, new Term(1, Function.x, 2) ,2)
		};

		private static void Differentiate(List<object> equation)
		{
			List<object> newEquation = new List<object>();

			//find term or operation in equation
			foreach (object Object in equation)
			{
				if(Object.GetType() == typeof(Term))
				{
					Term term = (Term)Object;

					List<object> differential = new List<object>();

					//finds correct key value pair
					foreach (List<object> key in Differentials.Keys.ToArray())
					{
						if (key.GetType() == typeof(List<object>))
						{
							//get each term in differential
							foreach (object keyObject in key)
							{
								if (keyObject.GetType() == typeof(Term))
								{
									Term keyTerm = (Term)keyObject;

									//check if correct key
									if (term.GetFunction() == keyTerm.GetFunction() &&
										term.GetExponent() == keyTerm.GetExponent())
									{
										//return correct value
										differential = Differentials[key];
									}
								}
							}
						}
					}

					//for each term in the correct differential
					foreach (object differentialObject in differential)
					{
						if (Object.GetType() == typeof(Term))
						{
							Term differentialTerm = (Term)differentialObject;

							Term newTerm = new Term(term.GetCoeficient() * differentialTerm.GetCoeficient(), differentialTerm.GetFunction(), differentialTerm.GetExponent());

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
					}
				}

				if(Object.GetType() == typeof(Operation))
				{
					newEquation.Add(Object);
				}
			}

			Console.WriteLine("\n\nNew equation:");
			DisplayEquation(newEquation);
		}

		private static void DisplayEquation(List<object> equation)
		{
			foreach (object item in equation)
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
					formatTerm = " * ";
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

			Differentiate(test);
		}
	}
}
