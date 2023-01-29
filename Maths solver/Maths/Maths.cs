using Maths_solver.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using static Maths_solver.Maths.Operation;

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
			{new Term(-1, Function.cosech, new List<EquationItem> { new Term(2) }) } },
		};

		private static object thisSender;
		public static event EventHandler<Step> ShowSteps;

		#region Differentiate
		private static List<EquationItem> DifferentiateEquation(List<EquationItem> equation)
		{
			List<EquationItem> newEquation = new List<EquationItem>();

			//find term or operation in equation
			foreach (EquationItem equationItem in equation)
			{
				if (equationItem.GetType() == typeof(Term))
				{
					Term term = (Term)equationItem;
					List<EquationItem> baseDifferential = new List<EquationItem>();

					//find differential by function
					if (Differentials.ContainsKey(term.function))
						baseDifferential = Differentials[term.function];

					DifferentiateTerm(baseDifferential, term, ref newEquation);
				}

				if (equationItem.GetType() == typeof(Operation)) newEquation.Add(equationItem);
			}

			FormatEquation(ref newEquation);

			ShowSteps?.Invoke(thisSender, new Step(Rule.None, Phase.End, equation, newEquation));

			return newEquation;
		}

		private static void ChainInput(Term term, out bool shouldChainInput,
			ref List<EquationItem> newEquation)
		{
			shouldChainInput = false;

			if (!EquationsEqual(term.input, new List<EquationItem> { new Term(Function.x) }))
			{
				shouldChainInput = true;

				ShowSteps?.Invoke(thisSender, new Step(Rule.Input, Phase.Start, term.input));

				//chain rule
				List<EquationItem> inputDifferential = DifferentiateEquation(term.input);

				//checks if equation is one term all multiplied
				bool isOneTerm = true;
				for (int i = 0; i < inputDifferential.Count; i++)
				{
					if (inputDifferential[i].GetType() == typeof(Operation) &&
						((Operation)inputDifferential[i]).operation != OperationEnum.Multiplication)
					{
						isOneTerm = false;
						break;
					}
				}

				//get the first term in input differential
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

				//if is constant with coefficient of 1 or 0 ignore adding
				if (!(first.function == Function.constant &&
					(first.coeficient == 0 || first.coeficient == 1)))
				{
					if (!isOneTerm) newEquation.Add(new Operation(OperationEnum.OpenBracket));

					for (int i = 0; i < inputDifferential.Count; i++)
						newEquation.Add(inputDifferential[i]);

					if (!isOneTerm) newEquation.Add(new Operation(OperationEnum.ClosedBracket));

					newEquation.Add(new Operation(OperationEnum.Multiplication));
				}
			}
		}

		private static void DifferentiateTerm(List<EquationItem> differential, Term term,
			ref List<EquationItem> newEquation)
		{
			Term newTerm = default;

			if (Differentials.ContainsKey(term.function))
			{
				ShowSteps?.Invoke(thisSender, new Step(Rule.Standard, Phase.Start,
					new List<EquationItem> { term }));

				ChainInput(term, out bool shouldChainInput, ref newEquation);

				#region differentiate function
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

				#endregion

				#region show steps
				//keep coefficients when showing steps
				List<EquationItem> shownDifferential = new List<EquationItem>();
				Term firstTerm = (Term)differential[0];
				for (int i = 0; i < differential.Count; i++)
				{
					if (i == 0) shownDifferential.Add(new Term(term.coeficient * firstTerm.coeficient, firstTerm.function, firstTerm.input, firstTerm.exponent));

					else shownDifferential.Add(differential[i]);
				}

				ShowSteps?.Invoke(thisSender, new Step(Rule.Standard, Phase.End,
					new List<EquationItem> { new Term(term.coeficient, term.function) },
					shownDifferential));

				if (shouldChainInput) ShowSteps?.Invoke(thisSender, new Step(Rule.Input, Phase.End));
				#endregion
			}
			else
			{
				#region differentiate others
				switch (term.function)
				{
					case Function.x:
						DifferentiateX(term, ref newTerm, ref newEquation);
						break;

					case Function.ln:
						DifferntiateLn(term, ref newEquation);
						break;

					default:
						DifferentiateConstant(term, ref newEquation);
						break;
				}
				#endregion
			}
		}

		private static void DifferntiateLn(Term term, ref List<EquationItem> newEquation)
		{
			ChainInput(term, out bool shouldChainInput, ref newEquation);

			if (term.coeficient != 1)
			{
				newEquation.Add(new Term(term.coeficient));
				newEquation.Add(new Operation(OperationEnum.Multiplication));
			}

			newEquation.Add(new Operation(OperationEnum.OpenBracket));

			for (int i = 0; i < term.input.Count; i++) newEquation.Add(term.input[i]);

			newEquation.Add(new Operation(OperationEnum.ClosedBracket));

			newEquation.Add(new Operation(OperationEnum.Power));
			newEquation.Add(new Term(-1));
		}

		private static void DifferentiateX(Term term, ref Term newTerm, ref List<EquationItem> newEquation)
		{
			//if term is not only ax^n
			if (term.exponent[0].GetType() != typeof(Term) || term.exponent.Count != 1) return;

			Term exponent = (Term)term.exponent[0];
			if (exponent.function != Function.constant) return;

			ShowSteps?.Invoke(thisSender, new Step(Rule.x, Phase.Start, new List<EquationItem> { term }));

			if (exponent.coeficient != 0)
			{
				//ax^n => anx^(n-1)
				newTerm = new Term(term.coeficient * exponent.coeficient, Function.x,
						new List<EquationItem> { new Term(exponent.coeficient - 1) });

				AddTerm(newTerm, ref newEquation);

				ShowSteps?.Invoke(thisSender, new Step(Rule.x, Phase.End,
					new List<EquationItem> { term }, new List<EquationItem> { newTerm }));
			}
			else
			{
				if (newEquation.Count > 1)
				{
					newEquation.RemoveAt(newEquation.Count - 1);
				}

				ShowSteps?.Invoke(thisSender, new Step(Rule.Constant, Phase.Start,
					new List<EquationItem> { term }));
			}
		}

		private static void DifferentiateConstant(Term term, ref List<EquationItem> newEquation)
		{
			ShowSteps?.Invoke(thisSender, new Step(Rule.Standard, Phase.Start, new List<EquationItem> { term }));

			List<EquationItem> newTerm = new List<EquationItem>();

			#region chain exponent
			bool chainExponent = false;

			List<EquationItem> termExponent = term.exponent;
			Term firstExponentTerm = (Term)termExponent[0];

			//if exponent is not constant, chain exponent
			if (firstExponentTerm.function != Function.constant)
			{
				chainExponent = true;

				ShowSteps?.Invoke(thisSender, new Step(Rule.Exponent, Phase.Start, term.exponent));

				//chain rule
				List<EquationItem> exponentDifferential = DifferentiateEquation(term.exponent);

				//checks if equation is one term all multiplied
				bool oneTerm = true;
				for (int i = 0; i < exponentDifferential.Count; i++)
				{
					if (exponentDifferential[i].GetType() != typeof(Operation)) continue;

					Operation exponentDifferentialOperation = (Operation)exponentDifferential[i];
					if (exponentDifferentialOperation.operation != OperationEnum.Multiplication)
					{
						oneTerm = false;
						break;
					}
				}

				//get the first term in exponent differential
				int firstIndex = 0;
				for (int i = 0; i < exponentDifferential.Count; i++)
				{
					if (exponentDifferential[i].GetType() == typeof(Term))
					{
						firstIndex = i;
						break;
					}
				}

				Term first = (Term)exponentDifferential[firstIndex];

				//if is constant with coefficient of 1 or 0 ignore adding
				if (!(first.function == Function.constant &&
					(first.coeficient == 0 || first.coeficient == 1)))
				{
					if (!oneTerm) newTerm.Add(new Operation(OperationEnum.OpenBracket));

					for (int i = 0; i < exponentDifferential.Count; i++)
						newTerm.Add(exponentDifferential[i]);

					if (!oneTerm) newTerm.Add(new Operation(OperationEnum.ClosedBracket));

					newTerm.Add(new Operation(OperationEnum.Multiplication));
				}

				ShowSteps?.Invoke(thisSender, new Step(Rule.Exponent, Phase.End, term.exponent));
				ShowSteps?.Invoke(thisSender, new Step(Phase.Start));
			}
			#endregion

			if (term.function == Function.constant && chainExponent)
			{
				ShowSteps?.Invoke(thisSender, new Step(Rule.ln, Phase.Start, new List<EquationItem> { new Term(term.coeficient) }));

				newTerm.Add(new Term(1, Function.ln,
					new List<EquationItem> { new Term(term.coeficient) }, new List<EquationItem> { new Term() }));

				newTerm.Add(new Operation(OperationEnum.Multiplication));

				ShowSteps?.Invoke(thisSender, new Step(Phase.End));
			}
			else if (term.function == Function.constant && !chainExponent)
			{
				ShowSteps?.Invoke(thisSender, new Step(Rule.Constant, Phase.Start,
					new List<EquationItem> { term }));

				ShowSteps?.Invoke(thisSender, new Step(Phase.End));
			}

			if (chainExponent) newTerm.Add(term);

			for (int i = 0; i < newTerm.Count; i++)
			{
				if (newTerm.GetType() == typeof(Term)) AddTerm((Term)newTerm[i], ref newEquation);
				else newEquation.Add(newTerm[i]);
			}

			ShowSteps?.Invoke(thisSender, new Step(Rule.None, Phase.End,
				new List<EquationItem> { term }, newTerm));
		}

		private static bool EquationsEqual(List<EquationItem> equation1, List<EquationItem> equation2)
		{
			if (equation1.Count != equation2.Count) return false;

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
				equation2[i].GetType() == typeof(Operation))
				{
					Operation operation1 = (Operation)equation1[i];
					Operation operation2 = (Operation)equation2[i];

					if (operation1.operation != operation2.operation) return false;
				}
				//check if same type
				if (equation1[i].GetType() != equation2[i].GetType()) return false;
			}
			return true;
		}
		private static bool TermsEqual(Term term1, Term term2, bool areExponents)
		{
			if (!areExponents)
			{
				//check if coefficients match
				if (term1.coeficient != term2.coeficient) return false;

				//check if functions match
				if (term1.function == term2.function)
				{
					if (term1.exponent != null && term2.exponent != null &&
						EquationsEqual(term1.exponent, term2.exponent)) return true;

					if (term1.exponent == null && term2.exponent == null) return true;
				}
			}
			if (areExponents && term1.exponent == term2.exponent &&
				term1.exponent == term2.exponent) return true;
			return false;
		}

		private static void AddTerm(Term newTerm, ref List<EquationItem> newEquation)
		{
			if (newTerm.coeficient != 0)
			{
				newEquation.Add(newTerm);

				FormatTerm(newTerm, ref newEquation);
			}
		}

		public static List<EquationItem> Start(List<EquationItem> newEquation)
		{
			ShowSteps?.Invoke(thisSender, new Step(Rule.None, Phase.Reset, newEquation, null));
			return DifferentiateEquation(newEquation);
		}

		#endregion

		#region Format
		private static void FormatEquation(ref List<EquationItem> equation)
		{
			if (equation.Count == 0) return;

			#region format operations

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
			#endregion

			#region format terms
			float newCoefficient = 1;
			int startTerm = -1;

			//terms
			for (int i = 0; i < equation.Count; i++)
			{
				if (equation[i].GetType() != typeof(Term)) continue;

				Term equationTerm = (Term)equation[i];
				List<EquationItem> equationTermExponent = equationTerm.exponent;
				Term equationFirstTermExponent = (Term)equationTermExponent.First();

				//convert x^0 to a constant
				if (equationFirstTermExponent.coeficient == 0 &&
					equationTerm.function == Function.x)
				{
					equation[i] = new Term(equationTerm.coeficient);
				}
			}

			#endregion

			for (int i = 0; i < equation.Count - 1; i++)
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

						if (formatted) FormatEquation(ref equation);
					}

					#endregion

					#region format coefficients


					//sort out coefficients for multiple multiplied terms
					if (first.operation == OperationEnum.Multiplication &&
						equation[i - 1].GetType() == typeof(Term) &&
						equation[i + 1].GetType() == typeof(Term))
					{
						Term firstTerm = (Term)equation[i - 1];
						Term secondTerm = (Term)equation[i + 1];

						if (secondTerm.coeficient == 1) continue;

						//checks is not case ln(2)2^x where the constant would otherwise come out to front (broken rn)
						if (!EquationsEqual(firstTerm.exponent, new List<EquationItem> { new Term(Function.x) }) &&
							!EquationsEqual(secondTerm.exponent, new List<EquationItem> { new Term(Function.x) }))
						{
							equation[i - 1] = new Term(firstTerm.coeficient * secondTerm.coeficient,
								firstTerm.function, firstTerm.input, firstTerm.exponent);

							equation[i + 1] = new Term(1, secondTerm.function, secondTerm.input, secondTerm.exponent);

							FormatEquation(ref equation);
						}
					}

					#endregion
				}

				#region format coefficients
				//struggles with brackets
				//format coefficients with multiple multiplied terms
				if (equation[i + 1].GetType() == typeof(Operation) && equation[i].GetType() == typeof(Term))
				{
					Term currentTerm = (Term)equation[i];
					Term startingTerm = new Term();

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

				#endregion
			}
		}

		private static void FormatTerm(Term newTerm, ref List<EquationItem> newEquation)
		{
			//Checks if term is negative, and more than 2 items in the equation
			if (newTerm.coeficient < 0 && newEquation.Count >= 2 &&
				newEquation[newEquation.Count - 2].GetType() == typeof(Operation))
			{
				Operation newEquationOperation = (Operation)newEquation[newEquation.Count - 2];
				bool negate = false;

				//Negate operation
				switch (newEquationOperation.operation)
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