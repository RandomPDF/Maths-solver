using Maths_solver.UI;
using System;
using System.Collections.Generic;
using static Maths_solver.Maths.Operation;

namespace Maths_solver.Maths
{
	internal class Maths : Functions
	{
		private Dictionary<Function, Equation> Differentials =
			new Dictionary<Function, Equation>()
		{
			{Function.sin, new Equation() {new Term(Function.cos) } },
			{Function.cos, new Equation() {new Term(-1, Function.sin)} },

			{Function.tan, new Equation()
			{new Term(Function.sec, new Equation { new Term(2) }) } },

			{Function.cosec, new Equation()
			{new Term(-1, Function.cosec),
			new Operation(OperationEnum.Multiplication),
			new Term(Function.cot)}},

			{Function.sec, new Equation()
			{new Term(Function.sec),
			new Operation(OperationEnum.Multiplication),
			new Term(Function.tan)}},

			{Function.cot, new Equation()
			{new Term(-1, Function.cosec, new Equation { new Term(2) }) } },

			{Function.sinh, new Equation() {new Term(Function.cosh) } },
			{Function.cosh, new Equation() {new Term(Function.sinh) } },

			{Function.tanh, new Equation()
			{new Term(Function.sech, new Equation { new Term(2) } ) } },

			{Function.cosech, new Equation()
			{new Term(-1, Function.cosech),
			new Operation(OperationEnum.Multiplication),
			new Term(Function.coth)}},

			{Function.sech, new Equation()
			{new Term(-1, Function.sech),
			new Operation(OperationEnum.Multiplication),
			new Term(Function.tanh)}},

			{Function.coth, new Equation()
			{new Term(-1, Function.cosech, new Equation { new Term(2) }) } },
		};

		private object thisSender;
		public static event EventHandler<Step> ShowSteps;

		#region Differentiate

		private bool ValidEquation(Equation equation, ref Equation newEquation)
		{
			if (equation == null) return false;

			//if equation just constant, say its 0
			if (equation.Count == 1 && equation[0].GetType() == typeof(Term) &&
				((Term)equation[0]).function == Function.constant && (((Term)equation[0]).exponent == null
				|| ((Term)equation[0]).exponent.EquationsEqual(new Equation { new Term(1) })))
			{
				newEquation.Add(new Term(0f));
				return false;
			}

			return true;
		}

		private void Differentiate(Equation equation, ref Equation newEquation, ref Stack<OperationEnum> brackets)
		{
			Equation input = new Equation();

			//find term or operation in equation
			for (int index = 0; index < equation.Count; index++)
			{
				FindBrackets(equation, ref index, ref brackets, ref input, ref newEquation, ref equation);

				if (brackets.Count > 0 || index >= equation.Count) continue;

				if (equation[index].GetType() == typeof(Term))
				{
					Term term = (Term)equation[index];
					Equation baseDifferential = new Equation();

					//find differential by function
					if (Differentials.ContainsKey(term.function))
						baseDifferential = Differentials[term.function];

					DifferentiateTerm(baseDifferential, term, ref newEquation);
				}

				if (equation[index].GetType() == typeof(Operation)) newEquation.Add(equation[index]);
			}
		}

		private void FindBrackets(Equation equation, ref int index, ref Stack<OperationEnum> brackets,
			ref Equation input, ref Equation newEquation, ref Equation origionalEquation)
		{
			if (equation[index].GetType() == typeof(Operation))
			{
				OperationEnum operation = ((Operation)equation[index]).operation;

				if (operation == OperationEnum.OpenBracket)
				{
					brackets.Push(operation);

					if (brackets.Count == 1) return;
				}

				if (operation == OperationEnum.ClosedBracket && brackets.Count > 0) brackets.Pop();
			}

			if (brackets.Count > 0) input.Add(equation[index]);

			if (brackets.Count == 0 && input.Count > 0)
			{
				DifferentiateInput(input, ref index, ref newEquation, ref origionalEquation);
			}
		}

		private void DifferentiateInput(Equation input, ref int index, ref Equation newEquation,
			ref Equation origionalEquation)
		{
			float exponent = 1;

			//if bracket has exponent
			if (index + 1 < origionalEquation.Count && origionalEquation[index + 1].GetType() == typeof(Operation)
				&& ((Operation)origionalEquation[index + 1]).operation == OperationEnum.Power)
			{
				//assume next term is exponent which is just a constant
				exponent = ((Term)origionalEquation[index + 2]).coeficient;

				index += 2;
			}

			//if exponent 0, is just a constant so differentiates to 0
			if (exponent == 0) return;

			//if bracket is to a power perform x^n -> nx^n-1
			if (exponent != 1)
			{
				//add n to front
				newEquation.Add(new Term(exponent));
				newEquation.Add(new Operation(OperationEnum.Multiplication));

				newEquation.Add(new Operation(OperationEnum.OpenBracket));

				//add x
				newEquation.Add(input);

				newEquation.Add(new Operation(OperationEnum.ClosedBracket));

				//add n-1
				if (exponent != 2)
				{
					newEquation.Add(new Operation(OperationEnum.Power));
					newEquation.Add(new Term(exponent - 1));
				}

				newEquation.Add(new Operation(OperationEnum.Multiplication));
			}

			//multiply by differential
			Equation inputEquation = DifferentiateEquation(input);

			//if differential term isn't just 1
			if (!inputEquation.EquationsEqual(new Equation { new Term(1) }) || exponent == 1)
			{
				bool addBrackets = inputEquation.Count > 1;

				if (addBrackets) newEquation.Add(new Operation(OperationEnum.OpenBracket));

				newEquation.Add(inputEquation);

				if (addBrackets) newEquation.Add(new Operation(OperationEnum.ClosedBracket));
			}

			index++;
		}

		private Equation DifferentiateEquation(Equation equation)
		{
			Equation newEquation = new Equation();

			if (!ValidEquation(equation, ref newEquation)) return newEquation;

			Stack<OperationEnum> brackets = new Stack<OperationEnum>();
			Differentiate(equation, ref newEquation, ref brackets);

			FormatEquation(ref newEquation);

			ShowSteps?.Invoke(thisSender, new Step(Rule.None, Phase.End, equation, newEquation));

			return newEquation;
		}

		private Equation ChainInput(Term term, out bool shouldChainInput)
		{
			Equation equation = new Equation();

			shouldChainInput = false;

			if (!term.input.EquationsEqual(new Equation { new Term(Function.x) }))
			{
				shouldChainInput = true;

				ShowSteps?.Invoke(thisSender, new Step(Rule.Input, Phase.Start, term.input));

				//chain rule
				Equation inputDifferential = DifferentiateEquation(term.input);

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

				Term first = null;
				if (inputDifferential.Count > 0 && inputDifferential[firstIndex].GetType() == typeof(Term))
					first = (Term)inputDifferential[firstIndex];

				//if is 1 or 0 ignore adding
				if (!inputDifferential.EquationsEqual(new Equation { new Term(0f) }) &&
					!inputDifferential.EquationsEqual(new Equation { new Term(1) }))
				{
					if (!isOneTerm) equation.Add(new Operation(OperationEnum.OpenBracket));

					equation.Add(inputDifferential);

					if (!isOneTerm) equation.Add(new Operation(OperationEnum.ClosedBracket));

					equation.Add(new Operation(OperationEnum.Multiplication));
				}
			}

			return equation;
		}

		private void DifferentiateTerm(Equation differential, Term term,
			ref Equation newEquation)
		{
			ShowSteps?.Invoke(thisSender, new Step(Rule.Standard, Phase.Start, new Equation { term }));

			Term newTerm = default;

			if (Differentials.ContainsKey(term.function))
			{
				Equation newTermEquation = new Equation();

				ChainFunction(differential, term, ref newTerm, ref newTermEquation);

				Equation inputDifferential = ChainInput(term, out bool shouldChainInput);
				newTermEquation.Add(inputDifferential);

				//Shouldn't apply if exponent is 1
				if (!term.exponent.EquationsEqual(new Equation { new Term(1) }))
					newTermEquation.Add(ChainXRule(term));

				#region show steps
				//keep coefficients when showing steps
				Equation shownDifferential = new Equation();
				Term firstTerm = (Term)differential[0];

				for (int i = 0; i < differential.Count; i++)
				{
					if (i == 0) shownDifferential.Add(new Term(term.coeficient * firstTerm.coeficient, firstTerm.function, firstTerm.input, firstTerm.exponent));

					else shownDifferential.Add(differential[i]);
				}

				ShowSteps?.Invoke(thisSender, new Step(Rule.Standard, Phase.End,
					new Equation { new Term(term.coeficient, term.function) },
					shownDifferential));


				for (int i = 0; i < shownDifferential.Count; i++)
				{
					if (shownDifferential[i].GetType() == typeof(Term))
					{
						Term _term = (Term)shownDifferential[i];
						shownDifferential[i] = new Term(_term.coeficient, _term.function, term.input, _term.exponent);
					}
				}

				if (shouldChainInput)
				{
					ShowSteps?.Invoke(thisSender, new Step(Phase.Start));

					ShowSteps?.Invoke(thisSender,
						new Step(Rule.Input, Phase.End, inputDifferential, shownDifferential));
				}

				ShowSteps?.Invoke(thisSender, new Step(Phase.Start));

				FormatEquation(ref newTermEquation);

				ShowSteps?.Invoke(thisSender, new Step(Rule.None, Phase.End, new Equation { term },
					newTermEquation));

				newEquation.Add(newTermEquation);

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

		private void ChainFunction(Equation differential, Term term, ref Term newTerm,
			ref Equation newEquation)
		{
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

					newEquation.Add(newTerm);

				}

				if (differentialObject.GetType() == typeof(Operation))
				{
					newEquation.Add(differentialObject);
				}
			}

			newEquation.Add(new Operation(OperationEnum.Multiplication));
		}

		private void DifferntiateLn(Term term, ref Equation newEquation)
		{
			Equation differential = ChainInput(term, out bool shouldChainInput);

			Equation xRule = ChainXRule(term);

			ShowSteps?.Invoke(thisSender, new Step(Rule.x, Phase.End, new Equation { term }, xRule));

			ShowSteps?.Invoke(thisSender, new Step(Rule.Input, Phase.End, differential, xRule));

			differential.Add(xRule);
			FormatEquation(ref differential);

			ShowSteps?.Invoke(thisSender, new Step(Phase.Start));
			ShowSteps?.Invoke(thisSender, new Step(Rule.None, Phase.End, new Equation { term }, differential));

			differential.Add(new Operation(OperationEnum.Multiplication));

			//if the input isn't nothing. Add brackets if the count is greater than 1 and isn't just an x

			Equation standardResult = new Equation();
			bool addBrackets = term.input.Count > 0 && term.input != null &&
				!term.input.EquationsEqual(new Equation { new Term(Function.x) });

			if (addBrackets) standardResult.Add(new Operation(OperationEnum.OpenBracket));

			standardResult.Add(term.input);

			if (addBrackets) standardResult.Add(new Operation(OperationEnum.ClosedBracket));

			standardResult.Add(new Operation(OperationEnum.Power));
			standardResult.Add(new Term(-1));

			ShowSteps?.Invoke(thisSender, new Step(Phase.Start));
			ShowSteps?.Invoke(thisSender, new Step(Rule.Standard, Phase.End,
				new Equation { new Term(1, term.function, term.input, new Equation { new Term(1) }) },
				standardResult));

			ShowSteps?.Invoke(thisSender, new Step(Phase.Start));
			ShowSteps?.Invoke(thisSender, new Step(Rule.ln, Phase.End, differential, standardResult));

			differential.Add(standardResult);

			FormatEquation(ref differential);

			newEquation.Add(differential);

			ShowSteps?.Invoke(thisSender, new Step(Phase.Start));
			ShowSteps?.Invoke(thisSender, new Step(Rule.None, Phase.End, new Equation { term }, differential));
		}

		private void DifferentiateX(Term term, ref Term newTerm, ref Equation newEquation)
		{
			//if term is not only ax^n
			if (term.exponent[0].GetType() != typeof(Term) || term.exponent.Count != 1) return;

			Term exponent = (Term)term.exponent[0];
			if (exponent.function != Function.constant) return;

			if (exponent.coeficient != 0)
			{
				newTerm = ApplyXRule(term);
				newEquation.Add(newTerm);

				ShowSteps?.Invoke(thisSender, new Step(Rule.x, Phase.End,
					new Equation { term }, new Equation { newTerm }));
			}
			else
			{
				if (newEquation.Count > 1)
				{
					newEquation.RemoveAt(newEquation.Count - 1);
				}

				ShowSteps?.Invoke(thisSender, new Step(Rule.Constant, Phase.Start,
					new Equation { term }));
			}
		}

		private Equation ChainXRule(Term term)
		{
			Equation equation = new Equation();
			Term xRule = ApplyXRule(term);

			if (xRule == null) return null;

			if (xRule.coeficient != 1 && xRule.function != Function.constant)
			{
				equation.Add(xRule);
				equation.Add(new Operation(OperationEnum.Multiplication));
			}

			return equation;
		}

		private Term ApplyXRule(Term term)
		{
			if (term.exponent == null || term.exponent.Count <= 0) return null;

			Term exponent = (Term)term.exponent[0];

			//ax^n => anx^(n-1)
			return new Term(term.coeficient * exponent.coeficient, term.function, term.input,
					new Equation { new Term(exponent.coeficient - 1) });
		}

		private void DifferentiateConstant(Term term, ref Equation newEquation)
		{
			Equation newTerm = new Equation();

			#region chain exponent

			bool chainExponent = term.exponent != null && term.exponent.Count > 0 &&
				term.exponent[0].GetType() == typeof(Term) &&
				((Term)term.exponent[0]).function != Function.constant;

			if (chainExponent) newTerm.Add(term);

			if (term.exponent != null && term.exponent.Count > 0 && term.exponent[0].GetType() == typeof(Term))
			{
				Equation termExponent = term.exponent;
				Term firstExponentTerm = (Term)termExponent[0];

				//if exponent is not constant, chain exponent
				if (firstExponentTerm.function != Function.constant)
				{
					chainExponent = true;

					ShowSteps?.Invoke(thisSender, new Step(Rule.Exponent, Phase.Start, term.exponent));

					//chain rule
					Equation exponentDifferential = DifferentiateEquation(term.exponent);

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

					//if 1 or 0 ignore adding
					if (!exponentDifferential.EquationsEqual(new Equation { new Term(1) }) &&
						!exponentDifferential.EquationsEqual(new Equation { new Term(0f) }))
					{
						newTerm.Add(new Operation(OperationEnum.Multiplication));

						if (!oneTerm) newTerm.Add(new Operation(OperationEnum.OpenBracket));

						newTerm.Add(exponentDifferential);

						if (!oneTerm) newTerm.Add(new Operation(OperationEnum.ClosedBracket));
					}

					ShowSteps?.Invoke(thisSender, new Step(Rule.Exponent, Phase.End, exponentDifferential,
						new Equation { term }));

					ShowSteps?.Invoke(thisSender, new Step(Phase.Start));
				}
			}
			#endregion

			if (term.function == Function.constant && chainExponent &&
				!int.TryParse((term.coeficient / (float)Math.E).ToString(), out _))
			{
				ShowSteps?.Invoke(thisSender, new Step(Rule.ln, Phase.Start, new Equation { new Term(term.coeficient) }));

				newTerm.Add(new Term(1, Function.ln,
					new Equation { new Term(term.coeficient) }, new Equation { new Term(1) }));

				newTerm.Add(new Operation(OperationEnum.Multiplication));

				ShowSteps?.Invoke(thisSender, new Step(Phase.End));
			}
			else if (term.function == Function.constant && !chainExponent)
			{
				ShowSteps?.Invoke(thisSender, new Step(Rule.Constant, Phase.Start,
					new Equation { term }));

				ShowSteps?.Invoke(thisSender, new Step(Phase.End));
			}

			newEquation.Add(newTerm);

			if (chainExponent) ShowSteps?.Invoke(thisSender, new Step(Rule.None, Phase.End,
				new Equation { term }, newTerm));

			else ShowSteps?.Invoke(thisSender, new Step(Phase.End));
		}

		public Equation Start(Equation newEquation)
		{
			FormatEquation(ref newEquation);

			ShowSteps?.Invoke(thisSender, new Step(Rule.None, Phase.Reset, newEquation, null));
			return DifferentiateEquation(newEquation);
		}

		#endregion

		#region Format
		private void FormatEquation(ref Equation equation)
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
			if (equation.Count > 0 && equation[equation.Count - 1].GetType() == typeof(Operation) &&
				((Operation)equation[equation.Count - 1]).operation != OperationEnum.ClosedBracket)
			{
				equation.RemoveAt(equation.Count - 1);
			}
			#endregion

			float newCoefficient = 1;
			int startTerm = -1;

			for (int i = 0; i < equation.Count - 1; i++)
			{
				if (FormatOperations(i, ref equation)) continue;
				if (i < equation.Count - 1) FormatCoefficients(i, startTerm, ref newCoefficient, ref equation);
			}

			for (int i = 0; i < equation.Count; i++)
			{
				FormatConstants(i, ref equation);
			}
		}

		private bool FormatOperations(int i, ref Equation equation)
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
				if (i >= 1 && first.operation == OperationEnum.Multiplication &&
					equation[i - 1].GetType() == typeof(Term) &&
					equation[i + 1].GetType() == typeof(Term))
				{
					Term firstTerm = (Term)equation[i - 1];
					Term secondTerm = (Term)equation[i + 1];

					if (secondTerm.coeficient == 1) return true;

					//checks is not case ln(2)2^x where the constant would otherwise come out to front (broken rn)
					if (!firstTerm.exponent.EquationsEqual(new Equation { new Term(Function.x) }) &&
						!secondTerm.exponent.EquationsEqual(new Equation { new Term(Function.x) }))
					{
						equation[i - 1] = new Term(firstTerm.coeficient * secondTerm.coeficient,
							firstTerm.function, firstTerm.input, firstTerm.exponent);

						equation[i + 1] = new Term(1, secondTerm.function, secondTerm.input, secondTerm.exponent);

						FormatEquation(ref equation);
					}
				}

				#endregion
			}

			return false;
		}

		private void FormatCoefficients(int i, int startTerm, ref float newCoefficient,
			ref Equation equation)
		{
			//format coefficients with multiple multiplied terms
			if (equation[i + 1].GetType() == typeof(Operation) && equation[i].GetType() == typeof(Term))
			{
				Term currentTerm = (Term)equation[i];
				Term startingTerm = new Term(1);

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
		}

		private void FormatConstants(int i, ref Equation equation)
		{
			//if left or right term is a multiplication and the current term is just a 1
			if (((i - 1 >= 0 && equation[i - 1].GetType() == typeof(Operation) &&
				((Operation)equation[i - 1]).operation == OperationEnum.Multiplication) ||

				(i + 1 < equation.Count && equation[i + 1].GetType() == typeof(Operation) &&
				((Operation)equation[i + 1]).operation == OperationEnum.Multiplication)) &&

				(equation[i].GetType() == typeof(Term) && ((Term)equation[i]).function == Function.constant &&
				((Term)equation[i]).coeficient == 1))
			{
				equation.RemoveAt(i);
				FormatEquation(ref equation);
			}
		}

		#endregion
	}
}