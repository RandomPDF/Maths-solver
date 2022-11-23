using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maths_solver
{
	public class Term : Functions, EquationItem
	{
		private Function function;
		private float coeficient;
		private Term input;
		private Term exponent;

		public Term(float coeficient, Function function, Term input, Term exponent)
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

		public Term(float coeficient, Function function, Term exponent)
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
				input = new Term(1, Function.x, new Term(1, Function.a));
            }
		}

		public Term(float coeficient, Function function)
        {
			this.coeficient = coeficient;
			this.function = function;

			if (function == Function.a)
			{
				this.exponent = null;
			}
            else
            {
				throw new Exception($"The function {function.ToString()} must have an exponent and input.");
			}
        }

		public Function GetFunction() { return function; }
		public float GetCoeficient() { return coeficient; }
		public Term GetInput() { return input; }
		public Term GetExponent() { return exponent; }
	}
}
