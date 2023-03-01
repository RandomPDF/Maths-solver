using Maths_solver.Maths;

namespace Maths_solver.UI
{
	public enum Rule
	{
		Standard, x, xRule, PowerRule, Product, Constant, Input, Exponent, ln, None
	}

	public enum Phase
	{
		Start, End, Reset, None
	}

	public struct Step
	{
		public Rule rule;
		public Phase phase;
		public Equation input;
		public Equation output;

		public Step(Rule rule, Phase phase, Equation input, Equation output)
		{
			this.rule = rule;
			this.output = output;
			this.input = input;
			this.phase = phase;
		}

		public Step(Rule rule, Phase phase, Equation input)
		{
			this.rule = rule;
			this.input = input;
			this.phase = phase;

			output = null;
		}

		public Step(Rule rule, Phase phase)
		{
			this.rule = rule;
			this.phase = phase;

			input = null;
			output = null;
		}

		public Step(Phase phase)
		{
			this.phase = phase;

			input = null;
			output = null;
			rule = Rule.None;
		}
	}
}