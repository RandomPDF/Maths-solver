using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maths_solver
{
	public class Term
	{
		//Dictionary of function names, and if they require an input
		public Dictionary<Function, bool> Functions = new Dictionary<Function, bool>()
		{
			{Function.sin, true},
			{Function.cos, true},
			{Function.tan, true},
			{Function.cosec, true},
			{Function.sec, true},
			{Function.cot, true},
			{Function.arcsin, true},
			{Function.arccos, true},
			{Function.arctan, true},
			{Function.ln, true},

			{Function.a, false},
			{Function.x, false}
		};

		private Function function;
		private int coeficient;
		private Term input;
		private int exponent;

		public Term(int coeficient, Function function, Term input, int exponent)
		{
			if (Functions[function])
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

		public Term(int coeficient, Function function, int exponent)
		{
			if (!Functions[function])
			{
				//only for if function doesn't require an input
				this.coeficient = coeficient;
				this.function = function;
				this.exponent = exponent;
				input = null;
			}
            else
            {
				throw new Exception($"The function {function.ToString()} requires an input parameter");
            }
		}
	}
}
