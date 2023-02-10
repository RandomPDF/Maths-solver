﻿using Maths_solver.UI;
using System;
using System.Collections.Generic;
using static Maths_solver.Maths.Operation;

namespace Maths_solver.Maths
{
	internal class Math : Functions
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
		private Equation DifferentiateEquation(Equation equation)
		{
			if (equation == null) return null;

			Equation newEquation = new Equation();

			//if equation just constant, say its 0
			if (equation.Count == 1 && equation[0].GetType() == typeof(Term) &&
				((Term)equation[0]).function == Function.constant)
			{
				newEquation.Add(new Term(0f));
			}

			//find term or operation in equation
			foreach (EquationItem equationItem in equation)
			{
				if (equationItem.GetType() == typeof(Term))
				{
					Term term = (Term)equationItem;
					Equation baseDifferential = new Equation();

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

		private Equation ChainInput(Term term, out bool shouldChainInput)
		{
			Equation equation = new Equation();

			shouldChainInput = false;

			if (!EquationsEqual(term.input, new Equation { new Term(Function.x) }))
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
				if (inputDifferential.Count > 0) { first = (Term)inputDifferential[firstIndex]; }

				//if is constant with coefficient of 1 or 0 ignore adding
				if (first != null && !(first.function == Function.constant &&
					(first.coeficient == 0 || first.coeficient == 1)))
				{
					if (!isOneTerm) equation.Add(new Operation(OperationEnum.OpenBracket));

					for (int i = 0; i < inputDifferential.Count; i++)
						equation.Add(inputDifferential[i]);

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

				newTermEquation = ChainInput(term, out bool shouldChainInput);

				ChainFunction(differential, term, ref newTerm, ref newTermEquation);

				Equation xRule = ChainXRule(term);
				for (int i = 0; i < xRule.Count; i++) differential.Add(xRule[i]);

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

				if (shouldChainInput) ShowSteps?.Invoke(thisSender, new Step(Rule.Input, Phase.End));

				ShowSteps?.Invoke(thisSender, new Step(Phase.Start));

				FormatEquation(ref newTermEquation);

				ShowSteps?.Invoke(thisSender, new Step(Rule.None, Phase.End, new Equation { term },
					newTermEquation));

				for (int i = 0; i < newTermEquation.Count; i++) newEquation.Add(newTermEquation[i]);

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

			ShowSteps?.Invoke(thisSender, new Step(Rule.x, Phase.End,
					new Equation { term }, xRule));

			for (int i = 0; i < xRule.Count; i++) differential.Add(xRule[i]);

			//if the input isn't nothing. Add brackets if the count is greater than 1 or the first term isn't a constant nor has a coefficient of 1

			bool addBrackets = term.input.Count > 0 && term.input != null &&
				(term.input.Count > 1 || (term.input[0].GetType() == typeof(Term) &&
				((Term)term.input[0]).function != Function.constant && ((Term)term.input[0]).coeficient != 1));

			if (addBrackets) differential.Add(new Operation(OperationEnum.OpenBracket));

			for (int i = 0; i < term.input.Count; i++) differential.Add(term.input[i]);

			if (addBrackets) differential.Add(new Operation(OperationEnum.ClosedBracket));

			differential.Add(new Operation(OperationEnum.Power));
			differential.Add(new Term(-1));

			FormatEquation(ref differential);

			for (int i = 0; i < differential.Count; i++) newEquation.Add(differential[i]);

			ShowSteps?.Invoke(thisSender, new Step(Rule.ln, Phase.End,
				new Equation { term }, differential));
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

			if (xRule.coeficient != 1 && xRule.function != Function.constant)
			{
				equation.Add(xRule);
				equation.Add(new Operation(OperationEnum.Multiplication));
			}

			return equation;
		}

		private Term ApplyXRule(Term term)
		{
			Term exponent = (Term)term.exponent[0];

			//ax^n => anx^(n-1)
			return new Term(term.coeficient * exponent.coeficient, term.function, term.input,
					new Equation { new Term(exponent.coeficient - 1) });
		}

		private void DifferentiateConstant(Term term, ref Equation newEquation)
		{
			Equation newTerm = new Equation();

			#region chain exponent
			bool chainExponent = false;

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
			}
			#endregion

			if (term.function == Function.constant && chainExponent)
			{
				ShowSteps?.Invoke(thisSender, new Step(Rule.ln, Phase.Start, new Equation { new Term(term.coeficient) }));

				newTerm.Add(new Term(1, Function.ln,
					new Equation { new Term(term.coeficient) }, new Equation { new Term() }));

				newTerm.Add(new Operation(OperationEnum.Multiplication));

				ShowSteps?.Invoke(thisSender, new Step(Phase.End));
			}
			else if (term.function == Function.constant && !chainExponent)
			{
				ShowSteps?.Invoke(thisSender, new Step(Rule.Constant, Phase.Start,
					new Equation { term }));

				ShowSteps?.Invoke(thisSender, new Step(Phase.End));
			}

			if (chainExponent) newTerm.Add(term);

			for (int i = 0; i < newTerm.Count; i++)
			{
				if (newTerm.GetType() == typeof(Term)) newEquation.Add((Term)newTerm[i]);
				else newEquation.Add(newTerm[i]);
			}

			ShowSteps?.Invoke(thisSender, new Step(Rule.None, Phase.End,
				new Equation { term }, newTerm));
		}

		private bool EquationsEqual(Equation equation1, Equation equation2)
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
		private bool TermsEqual(Term term1, Term term2, bool areExponents)
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

		public Equation Start(Equation newEquation)
		{
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
			if (equation.Count > 0 && equation[equation.Count - 1].GetType() == typeof(Operation))
			{
				equation.RemoveAt(equation.Count - 1);
			}
			#endregion

			float newCoefficient = 1;
			int startTerm = -1;

			for (int i = 0; i < equation.Count - 1; i++)
			{
				if (FormatOperations(i, ref equation)) continue;
				FormatCoefficients(i, startTerm, ref newCoefficient, ref equation);
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
					if (!EquationsEqual(firstTerm.exponent, new Equation { new Term(Function.x) }) &&
						!EquationsEqual(secondTerm.exponent, new Equation { new Term(Function.x) }))
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
		}

		private void FormatConstants(int i, ref Equation equation)
		{
			//if left or right term is a multiplication and the current term is just a 1
			if (((i - 1 >= 0 && equation[i - 1].GetType() == typeof(Operation) &&
				((Operation)equation[i - 1]).operation == OperationEnum.Multiplication) ||

				(i + 1 >= 0 && equation[i + 1].GetType() == typeof(Operation) &&
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