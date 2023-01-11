using Maths_solver.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Maths_solver.Maths;

namespace Maths_solver.UI
{
    public enum Rule
    {
        Standard, x, Constant, Chain, None
    }

    public enum Phase
    {
		Start, End, Reset
    }

    public struct Step
    {
        public Rule rule;
        public Phase phase;
        public List<EquationItem> input;
        public List<EquationItem> output;

		public Step(Rule rule, Phase phase, List<EquationItem> input, List<EquationItem> output)
        {
            this.rule = rule;
			this.output = output;
            this.input = input;
			this.phase = phase;
        }

		public Step(Rule rule, Phase phase, List<EquationItem> input)
        {
            this.rule = rule;
            this.input = input;
			this.phase = phase;

            output = null;
            phase = Phase.Start;
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
