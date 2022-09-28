using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maths_solver
{
	public class Term
	{
		private int coeficient;
		private Function function;
		private Term input;
		private int exponent;

		public Term(int coeficient, Function function, Term input, int exponent)
		{ 
			this.coeficient = coeficient;
			this.function = function;
			this.input = input;
			this.exponent = exponent;
		}

		public Term(int coeficient, Function function, int exponent)
		{
			//only for if function doesn't require an input
			this.coeficient = coeficient;
			this.function = function;
			this.exponent = exponent;
			input = null;
		}
	}
}
