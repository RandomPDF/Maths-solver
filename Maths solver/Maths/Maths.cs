using System;
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
			{new List<object>(){new Term(1, Function.cos, 1) }, new List<object>(){new Term(-1, Function.sin, 1) } }
		};

		static List<object> test = new List<object>()
		{
			new Term(1, Function.cos, 1)
		};

		static List<object> easy = new List<object>()
		{
			new Term(3, Function.sin, 1),
			Operation.Addition,
			new Term(5, Function.cos, 1)
		};

		//3x^3 + 5sin(x^2)^2
		static List<object> hard = new List<object>()
		{
			//3x^3
			new Term(3, Function.x, 3),

			Operation.Addition,

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

							newEquation.Add(new Term(term.GetCoeficient() * differentialTerm.GetCoeficient(), differentialTerm.GetFunction(), term.GetExponent()));
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

			switch(operation)
			{
				case Operation.Addition:
					formatTerm = " + ";
					break;

				case Operation.Subtraction:
					formatTerm = " - ";
					break;

				case Operation.Multiplication:
					formatTerm = " * ";
					break;

				case Operation.Division:
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
