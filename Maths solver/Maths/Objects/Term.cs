using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maths_solver.Maths
{
	public class Term : Functions, EquationItem
	{
		public float coeficient { get; }
		public Function function { get; }
		public List<EquationItem> input { get; }
		public List<EquationItem> exponent { get; }

		//e.g. sin^2(x)
		public Term(float coeficient, Function function, List<EquationItem> input, List<EquationItem> exponent)
		{
			this.coeficient = coeficient;
			this.function = function;
			this.input = input;
			this.exponent = exponent;
		}

		//e.g. x^3
		public Term(float coeficient, Function function, List<EquationItem> exponent)
		{
			this.coeficient = coeficient;
			this.function = function;

			if (requiresInput[function]) this.input = new List<EquationItem>() { new Term(Function.x) };
			else this.input = null;

			this.exponent = exponent;
		}

		//e.g. 5x
		public Term(float coeficient, Function function)
        {
			this.coeficient = coeficient;
			this.function = function;

			if (requiresInput[function]) this.input = new List<EquationItem>() { new Term(Function.x) };
			else this.input = null;

			this.exponent = new List<EquationItem> { new Term() };
		}

		public Term(Function function, List<EquationItem> exponent)
        {
			this.coeficient = 1;
			this.function = function;

			if (requiresInput[function]) this.input = new List<EquationItem>() { new Term(Function.x) };
			else this.input = null;

			this.exponent = exponent;
		}

		//e.g. x
		public Term(Function function)
        {
			this.coeficient = 1;
			this.function = function;

			if (requiresInput[function]) this.input = new List<EquationItem>() { new Term(Function.x) };
			else this.input = null;

			this.exponent = null;
		}

		//constants
		public Term(float coeficient)
        {
			this.coeficient = coeficient;
			this.function = Function.constant;
			this.input = null;
			this.exponent = null;
		}

		//1
		public Term()
		{
			this.coeficient = 1;
			this.function = Function.constant;
			this.input = null;
			this.exponent = null;
		}

	}
}
