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

		public Equation outputEquation;

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

		private void Differentiate(List<List<Equation>> differentials, ref Equation newEquation, ref Stack<OperationEnum> brackets)
		{
			Equation input = new Equation();

			//find term or operation in equation
			for (int differentialIndex = 0; differentialIndex < differentials.Count; differentialIndex++)
			{
				//Differentiate singular terms
				if (differentials[differentialIndex].Count == 1)
				{
					Equation equation = differentials[differentialIndex][0];

					for (int termIndex = 0; termIndex < equation.Count; termIndex++)
					{
						FindBrackets(equation, ref termIndex, ref brackets, ref input, ref newEquation, ref equation);

						if (brackets.Count > 0 || termIndex >= equation.Count) continue;

						if (equation[termIndex].GetType() == typeof(Term))
						{
							Term term = (Term)equation[termIndex];
							Equation baseDifferential = new Equation();

							//find differential by function
							if (Differentials.ContainsKey(term.function))
								baseDifferential = Differentials[term.function];

							DifferentiateTerm(baseDifferential, term, ref newEquation);
						}

						if (equation[termIndex].GetType() == typeof(Operation)) newEquation.Add(equation[termIndex]);
					}
				}
				//Differentiate product of equations
				else
				{
					Equation equation1 = differentials[differentialIndex][0];

					Equation equation2 = new Equation();
					for (int equationIndex = 1; equationIndex < differentials[differentialIndex].Count;
					equationIndex++)
					{
						equation2.Add(differentials[differentialIndex][equationIndex]);
					}

					Equation.Format(ref equation1);
					Equation.Format(ref equation2);

					DifferentiateProduct(equation1, equation2, ref newEquation);
				}
			}
		}

		private void DifferentiateProduct(Equation equation1, Equation equation2, ref Equation newEquation)
		{
			ShowSteps?.Invoke(thisSender, new Step(Rule.Product, Phase.Start, equation1, equation2));

			if (equation1.requiresBrackets()) newEquation.Add(new Operation(OperationEnum.OpenBracket));
			newEquation.Add(equation1);
			if (equation1.requiresBrackets()) newEquation.Add(new Operation(OperationEnum.ClosedBracket));

			newEquation.Add(new Operation(OperationEnum.Multiplication));

			Equation differential2 = DifferentiateEquation(equation2);
			if (differential2.requiresBrackets()) newEquation.Add(new Operation(OperationEnum.OpenBracket));
			newEquation.Add(differential2);
			if (differential2.requiresBrackets()) newEquation.Add(new Operation(OperationEnum.ClosedBracket));


			newEquation.Add(new Operation(OperationEnum.Addition));


			if (equation2.requiresBrackets()) newEquation.Add(new Operation(OperationEnum.OpenBracket));
			newEquation.Add(equation2);
			if (equation2.requiresBrackets()) newEquation.Add(new Operation(OperationEnum.ClosedBracket));

			newEquation.Add(new Operation(OperationEnum.Multiplication));

			Equation differential1 = DifferentiateEquation(equation1);
			if (differential1.requiresBrackets()) newEquation.Add(new Operation(OperationEnum.OpenBracket));
			newEquation.Add(differential1);
			if (differential1.requiresBrackets()) newEquation.Add(new Operation(OperationEnum.ClosedBracket));

			ShowSteps?.Invoke(thisSender, new Step(Phase.End));
			ShowSteps?.Invoke(thisSender, new Step(Phase.End));
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
			Equation differential = new Equation();
			Equation exponent = new Equation();
			Equation inputEquation = new Equation();

			//if bracket has exponent
			if (index + 1 < origionalEquation.Count && origionalEquation[index + 1].GetType() == typeof(Operation)
			&& ((Operation)origionalEquation[index + 1]).operation == OperationEnum.Power)
			{
				Stack<OperationEnum> brackets = new Stack<OperationEnum>();
				int count = 1;

				for (int i = index + 2; i < origionalEquation.Count; i++)
				{
					count++;
					exponent.Add(origionalEquation[i]);

					if (origionalEquation[i].GetType() == typeof(Operation))
					{
						OperationEnum operation = ((Operation)origionalEquation[i]).operation;
						if (operation == OperationEnum.OpenBracket) brackets.Push(operation);
						else if (operation == OperationEnum.ClosedBracket && brackets.Count > 0) brackets.Pop();
					}
					//if term is exponent which is just a constant or exponent found
					if (i == index + 2 && origionalEquation[i].GetType() == typeof(Term) || brackets.Count == 0)
					{
						index += count;
						break;
					}

				}
			}

			//if exponent 0, is just a constant so differentiates to 0
			if (exponent.EquationsEqual(new Equation { new Term(0f) })) return;

			//if bracket is to the power of 1, differentiate input and add brackets if required
			if (exponent.Count == 0 || exponent.EquationsEqual(new Equation { new Term(1) }))
			{
				ShowSteps?.Invoke(thisSender, new Step(Rule.Input, Phase.Start, input));
				inputEquation = DifferentiateEquation(input);
				if (inputEquation.requiresBrackets()) differential.Add(new Operation(OperationEnum.OpenBracket));
				differential.Add(inputEquation);
				if (inputEquation.requiresBrackets()) differential.Add(new Operation(OperationEnum.ClosedBracket));
			}
			//if bracket is to a power perform x^n -> nx^n-1
			else if (exponent.IsConstant())
			{
				Equation xRule = new Equation
				{
					exponent, new Operation(OperationEnum.Multiplication), new Operation(OperationEnum.OpenBracket),
					input, new Operation(OperationEnum.ClosedBracket), new Operation(OperationEnum.Power),
					new Term(((Term)exponent[0]).coeficient - 1), new Operation(OperationEnum.Multiplication)
				};

				ShowSteps?.Invoke(thisSender, new Step(Rule.x, Phase.End, input, xRule));
				ShowSteps?.Invoke(thisSender, new Step(Phase.Start));
				differential.Add(xRule);

				ShowSteps?.Invoke(thisSender, new Step(Rule.Input, Phase.Start, input));
				//multiply by differential
				inputEquation = DifferentiateEquation(input);
				ShowSteps?.Invoke(thisSender, new Step(Phase.End));

				ShowSteps?.Invoke(thisSender, new Step(Rule.xRule, Phase.End, inputEquation, xRule));
				ShowSteps?.Invoke(thisSender, new Step(Phase.Start));

				//if differential term isn't just 1
				if (!inputEquation.EquationsEqual(new Equation { new Term(1) }) ||
				exponent.EquationsEqual(new Equation { new Term(1) }))
				{
					if (inputEquation.requiresBrackets()) differential.Add(new Operation(OperationEnum.OpenBracket));

					differential.Add(inputEquation);

					if (inputEquation.requiresBrackets()) differential.Add(new Operation(OperationEnum.ClosedBracket));
				}
			}
			else
			{
				//perform x^f(x) where x is the input of the brackets
				Equation equation = new Equation
				{
					new Operation(OperationEnum.OpenBracket),
					input,
					new Operation(OperationEnum.ClosedBracket),
					new Operation(OperationEnum.Power),
					exponent
				};

				differential.Add(PowerRule(equation, input, exponent));
			}

			ShowSteps?.Invoke(thisSender, new Step(Rule.None, Phase.End, input, differential));

			newEquation.Add(differential);
			index++;
		}

		private List<List<Equation>> FindDifferentials(Equation equation, ref Equation newEquation)
		{
			Stack<OperationEnum> brackets = new Stack<OperationEnum>();

			List<List<Equation>> Differentials = new List<List<Equation>>() { new List<Equation>() };

			Equation currentPart = new Equation();

			int differentialIndex = 0;

			for (int index = 0; index < equation.Count; index++)
			{
				if (equation[index].GetType() == typeof(Operation))
				{
					OperationEnum operation = ((Operation)equation[index]).operation;

					if (operation == OperationEnum.OpenBracket) brackets.Push(operation);
					if (operation == OperationEnum.ClosedBracket && brackets.Count > 0) brackets.Pop();

					//if not multiplied, differentiate term as normal
					if (operation != OperationEnum.Multiplication && operation != OperationEnum.Power
					&& operation != OperationEnum.ClosedBracket && brackets.Count == 0)
					{
						Differentials[differentialIndex].Add(currentPart);

						currentPart = new Equation();
						differentialIndex++;
						Differentials.Add(new List<Equation>());
					}

					//if multiplied, use product rule on list of Equations
					if (operation == OperationEnum.Multiplication && brackets.Count == 0)
					{
						Differentials[differentialIndex].Add(currentPart);

						currentPart = new Equation();
					}
				}

				currentPart.Add(equation[index]);
			}

			Differentials[differentialIndex].Add(currentPart);

			return Differentials;
		}

		private Equation DifferentiateEquation(Equation equation)
		{
			Equation newEquation = new Equation();

			if (!ValidEquation(equation, ref newEquation)) return newEquation;

			Stack<OperationEnum> brackets = new Stack<OperationEnum>();

			List<List<Equation>> Differentials = FindDifferentials(equation, ref newEquation);

			Differentiate(Differentials, ref newEquation, ref brackets);

			Equation.Format(ref newEquation);

			if (newEquation != null && newEquation.Count > 0)
			{
				ShowSteps?.Invoke(thisSender, new Step(Phase.End));
				ShowSteps?.Invoke(thisSender, new Step(Rule.None, Phase.End, equation, newEquation));
				ShowSteps?.Invoke(thisSender, new Step(Phase.Start));
			}

			outputEquation = newEquation;
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

				//if is 1 ignore adding
				if (!inputDifferential.EquationsEqual(new Equation { new Term(1) }))
				{
					if (inputDifferential.requiresBrackets()) equation.Add(new Operation(OperationEnum.OpenBracket));

					equation.Add(inputDifferential);

					if (inputDifferential.requiresBrackets())
						equation.Add(new Operation(OperationEnum.ClosedBracket));

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
			Equation newTermEquation = new Equation();

			if (Differentials.ContainsKey(term.function))
			{
				bool shouldChainInput = false;

				Equation inputDifferential = null;

				Equation shownDifferential = new Equation();

				//don't chain input or exponent if using power rule
				if (term.exponent.IsConstant())
				{
					ChainFunction(differential, term, ref newTerm, ref newTermEquation);

					//keep coefficients when showing steps for differential of basic function
					Term firstTerm = (Term)differential[0];

					for (int i = 0; i < differential.Count; i++)
					{
						if (i == 0) shownDifferential.Add(new Term(term.coeficient * firstTerm.coeficient, firstTerm.function, firstTerm.input, firstTerm.exponent));

						else shownDifferential.Add(differential[i]);
					}

					ShowSteps?.Invoke(thisSender, new Step(Rule.Standard, Phase.End,
					new Equation { new Term(term.coeficient, term.function) },
					shownDifferential));

					inputDifferential = ChainInput(term, out shouldChainInput);
					newTermEquation.Add(inputDifferential);
				}

				//Shouldn't apply if exponent is 1
				if (!term.exponent.EquationsEqual(new Equation { new Term(1) }))
				{
					if (term.exponent.IsConstant())
					{
						ShowSteps?.Invoke(thisSender, new Step(Phase.Start));
						Equation xRule = ChainXRule(new Term(1, term.function, term.input, term.exponent));

						if (newTermEquation.Count >= 0)
						{
							ShowSteps?.Invoke(thisSender, new Step(Phase.End));
							ShowSteps?.Invoke(thisSender, new Step(Rule.xRule, Phase.End, newTermEquation, xRule));
						}

						newTermEquation.Add(xRule);
					}
					//apply power rule
					else
					{
						newTermEquation.Add(PowerRule(term));
					}
				}



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
					ShowSteps?.Invoke(thisSender, new Step(Phase.End));

					ShowSteps?.Invoke(thisSender,
					new Step(Rule.Input, Phase.End, inputDifferential, shownDifferential));
				}

				Equation.Format(ref newTermEquation);
			}
			else
			{
				#region differentiate others
				switch (term.function)
				{
					case Function.x:
						DifferentiateX(term, ref newTerm, ref newTermEquation);
						break;

					case Function.ln:
						DifferntiateLn(term, ref newTermEquation);
						break;

					default:
						DifferentiateConstant(term, ref newTermEquation);
						break;
				}
				#endregion
			}

			ShowSteps?.Invoke(thisSender, new Step(Rule.None, Phase.End, new Equation { term }, newTermEquation));
			ShowSteps?.Invoke(thisSender, new Step(Phase.Start));

			newEquation.Add(newTermEquation);
		}

		private Equation PowerRule(Term term)
		{
			Term function = new Term(1, term.function, term.input, new Equation { new Term(1) });
			Equation differential = new Equation { term };

			Equation multiplier = new Equation { new Term(1, Function.ln, new Equation { function },
			new Equation { new Term(1)}), new Operation(OperationEnum.Multiplication)};

			if (term.exponent.requiresBrackets()) multiplier.Add(new Operation(OperationEnum.OpenBracket));
			multiplier.Add(term.exponent);
			if (term.exponent.requiresBrackets()) multiplier.Add(new Operation(OperationEnum.ClosedBracket));

			ShowSteps?.Invoke(thisSender, new Step(Rule.PowerRule, Phase.End, new Equation { function }, multiplier));
			ShowSteps?.Invoke(thisSender, new Step(Phase.Start));

			Equation multiplierDifferential = DifferentiateEquation(multiplier);
			ShowSteps?.Invoke(thisSender, new Step(Phase.End));
			ShowSteps?.Invoke(thisSender, new Step(Phase.End));

			differential.Add(new Operation(OperationEnum.Multiplication));

			if (multiplierDifferential.requiresBrackets()) differential.Add(new Operation(OperationEnum.OpenBracket));
			differential.Add(multiplierDifferential);

			if (multiplierDifferential.requiresBrackets())
				differential.Add(new Operation(OperationEnum.ClosedBracket));

			return differential;
		}

		private Equation PowerRule(Equation equation, Equation input, Equation exponent)
		{
			Equation differential = equation;

			Equation multiplier = new Equation { new Term(1, Function.ln, input,
			new Equation { new Term(1)}), new Operation(OperationEnum.Multiplication)};

			if (exponent.requiresBrackets()) multiplier.Add(new Operation(OperationEnum.OpenBracket));
			multiplier.Add(exponent);
			if (exponent.requiresBrackets()) multiplier.Add(new Operation(OperationEnum.ClosedBracket));

			ShowSteps?.Invoke(thisSender, new Step(Rule.PowerRule, Phase.End, differential, multiplier));
			ShowSteps?.Invoke(thisSender, new Step(Phase.Start));

			Equation multiplierDifferential = DifferentiateEquation(multiplier);
			ShowSteps?.Invoke(thisSender, new Step(Phase.End));
			ShowSteps?.Invoke(thisSender, new Step(Phase.End));

			differential.Add(new Operation(OperationEnum.Multiplication));

			if (multiplierDifferential.requiresBrackets()) differential.Add(new Operation(OperationEnum.OpenBracket));
			differential.Add(multiplierDifferential);

			if (multiplierDifferential.requiresBrackets())
				differential.Add(new Operation(OperationEnum.ClosedBracket));

			return differential;
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
			Equation differential = new Equation();
			Equation xRule = new Equation();
			Equation standardResult = new Equation();

			if (term.exponent.IsConstant())
			{
				differential = ChainInput(term, out bool shouldChainInput);
				xRule = ChainXRule(term);

				if (xRule.Count > 0 && differential.Count > 0)
				{
					ShowSteps?.Invoke(thisSender, new Step(Rule.Input, Phase.End, differential, xRule));
				}
				else ShowSteps?.Invoke(thisSender, new Step(Phase.End));

				differential.Add(xRule);

				//special case for adding brackets
				//if the input isn't nothing. Add brackets if the count is greater than 1 and isn't just an
				bool requiresBrackets = term.input.Count == 1 && term.input[0].GetType() == typeof(Term) &&
				(!((Term)term.input[0]).exponent.EquationsEqual(new Equation { new Term(1) }) ||
				((Term)term.input[0]).coeficient != 1) ||
				term.input.requiresBrackets();

				if (requiresBrackets) standardResult.Add(new Operation(OperationEnum.OpenBracket));

				standardResult.Add(term.input);

				if (requiresBrackets) standardResult.Add(new Operation(OperationEnum.ClosedBracket));

				standardResult.Add(new Operation(OperationEnum.Power));
				standardResult.Add(new Term(-1));

				ShowSteps?.Invoke(thisSender, new Step(Phase.Start));

				ShowSteps?.Invoke(thisSender, new Step(Rule.Standard, Phase.End,
				new Equation { new Term(1, term.function, term.input, new Equation { new Term(1) }) },
				standardResult));

				ShowSteps?.Invoke(thisSender, new Step(Phase.Start));

				if (differential.Count > 0 && standardResult.Count > 0)
					ShowSteps?.Invoke(thisSender, new Step(Rule.ln, Phase.End, differential, standardResult));
				else ShowSteps?.Invoke(thisSender, new Step(Phase.End));
			}
			else
			{
				differential = PowerRule(term);
			}

			Equation.Format(ref differential);

			differential.Add(new Operation(OperationEnum.Multiplication));

			differential.Add(standardResult);

			Equation.Format(ref differential);

			newEquation.Add(differential);
		}

		private void DifferentiateX(Term term, ref Term newTerm, ref Equation newEquation)
		{
			//if exponent not 0
			if (!term.exponent.EquationsEqual(new Equation { new Term(0f) }))
			{
				if (term.exponent.IsConstant())
				{
					newTerm = ApplyXRule(term);
					newEquation.Add(newTerm);

					ShowSteps?.Invoke(thisSender, new Step(Rule.x, Phase.End,
					new Equation { term }, new Equation { newTerm }));
				}
				else
				{
					newEquation.Add(PowerRule(term));
				}
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

			if (equation.Count > 0)
			{
				ShowSteps?.Invoke(thisSender, new Step(Rule.x, Phase.End,
				new Equation { term }, equation));

				ShowSteps?.Invoke(thisSender, new Step(Phase.Start));
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

						if (exponentDifferential.requiresBrackets())
							newTerm.Add(new Operation(OperationEnum.OpenBracket));

						newTerm.Add(exponentDifferential);

						if (exponentDifferential.requiresBrackets())
							newTerm.Add(new Operation(OperationEnum.ClosedBracket));
					}

					ShowSteps?.Invoke(thisSender, new Step(Phase.End));
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
			Equation.Format(ref newEquation);

			if (newEquation != null && newEquation.Count > 0)
				ShowSteps?.Invoke(thisSender, new Step(Rule.None, Phase.Reset, newEquation, null));

			return DifferentiateEquation(newEquation);
		}

		#endregion
	}
}
