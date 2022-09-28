using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maths_solver
{
	public class Term : IEquation
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
			//x^a doesn't have an input so input can be null
			if (function == Function.x)
			{
				this.coeficient = coeficient;
				this.function = function;
				this.exponent = exponent;
				input = null;
			}
			//for a function requiring an input where none is given, set it to be x
			else
			{
				input = new Term(1, Function.x, 1);
			}
		}
	}
}
