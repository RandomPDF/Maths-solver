using System.Collections.Generic;

namespace Maths_solver.Maths
{
	public class Functions
	{
		public enum Function
		{
			sin, cos, tan, sinh, cosh, tanh, sech, cosech, coth, cosec, sec, cot, ln, constant, x, NONE
		}

		//Dictionary of function names, and if they require an input
		public static Dictionary<Function, bool> requiresInput = new Dictionary<Function, bool>()
		{
			{Function.sin, true},
			{Function.cos, true},
			{Function.tan, true},
			{Function.cosec, true},
			{Function.sec, true},
			{Function.cot, true},
			{Function.sinh, true},
			{Function.cosh, true},
			{Function.tanh, true},
			{Function.cosech, true},
			{Function.sech, true},
			{Function.coth, true},
			{Function.ln, true},

			{Function.constant, false},
			{Function.x, false},
		};

		//Dictionary of function names, and if they require a constant exponent
		public static bool constantExponent(Function function)
		{
			return requiresInput[function];
		}
	}
}