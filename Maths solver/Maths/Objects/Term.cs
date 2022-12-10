using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maths_solver
{
	public class Term : Functions, EquationItem
	{
		public Function function { get; }
		public float coeficient { get; }
		public Term input { get; }
		public List<EquationItem> exponent { get; }

		public Term(float coeficient, Function function, Term input, List<EquationItem> exponent)
		{
			if (functions[function])
			{
				this.coeficient = coeficient;
				this.function = function;
				this.input = input;
				this.exponent = exponent;
			}
            else
            {
				throw new Exception($"The function {function.ToString()} can't have an input.");
            }
		}

		public Term(float coeficient, Function function, List<EquationItem> exponent)
		{
			this.coeficient = coeficient;
			this.function = function;
			this.exponent = exponent;

			if (!functions[function])
			{
				//if function doesn't require an input
				input = null;
			}
			else
			{
				//if function requires an input
				input = new Term(1, Function.x, new List<EquationItem> { new Term(1, Function.constant)});
            }
		}

		public Term(float coeficient, Function function)
        {
			this.coeficient = coeficient;
			this.function = function;

			if (function == Function.constant)
			{
				this.exponent = null;
			}
            else
            {
				throw new Exception($"The function {function.ToString()} must have an exponent and input.");
			}
        }
	}
}
