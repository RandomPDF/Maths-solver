namespace Maths_solver.Maths
{
	public class Term : Functions, EquationItem
	{
		public float coeficient { get; }
		public Function function { get; }
		public Equation input { get; }
		public Equation exponent { get; }

		//e.g. sin^2(x)
		public Term(float coeficient, Function function, Equation input, Equation exponent)
		{
			this.coeficient = coeficient;
			this.function = function;
			this.input = input;
			this.exponent = exponent;
		}

		//e.g. x^3
		public Term(float coeficient, Function function, Equation exponent)
		{
			this.coeficient = coeficient;
			this.function = function;

			if (requiresInput[function]) this.input = new Equation() { new Term(Function.x) };
			else this.input = null;

			this.exponent = exponent;
		}

		//e.g. 5x
		public Term(float coeficient, Function function)
		{
			this.coeficient = coeficient;
			this.function = function;

			if (requiresInput[function]) this.input = new Equation() { new Term(Function.x) };
			else this.input = null;

			this.exponent = new Equation { new Term() };
		}

		public Term(Function function, Equation exponent)
		{
			this.coeficient = 1;
			this.function = function;

			if (requiresInput[function]) this.input = new Equation() { new Term(Function.x) };
			else this.input = null;

			this.exponent = exponent;
		}

		//e.g. x
		public Term(Function function)
		{
			this.coeficient = 1;
			this.function = function;

			if (requiresInput[function]) this.input = new Equation() { new Term(Function.x) };
			else this.input = null;

			this.exponent = new Equation { new Term() };
		}

		//constants
		public Term(float coeficient)
		{
			this.coeficient = coeficient;
			this.function = Function.constant;
			this.input = null;
			this.exponent = new Equation { new Term() };
		}

		//base term case
		public Term()
		{
			this.coeficient = 1;
			this.function = Function.constant;
			this.input = null;
			this.exponent = null;
		}

	}
}
