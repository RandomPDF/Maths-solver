using System.Collections.Generic;

namespace Maths_solver
{
	public class Functions
	{
		public enum Function
		{
			sin,
			cos,
			tan,
			sinh,
			cosh,
			tanh,
			sech,
			cosech,
			coth,
			cosec,
			sec,
			cot,
			ln,
			a,
			x,
			arcsin,
			arccos,
			arctan,
			NONE
		}

		//Dictionary of function names, and if they require an input
		public static Dictionary<Function, bool> functions = new Dictionary<Function, bool>()
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
			{Function.arcsin, true},
			{Function.arccos, true},
			{Function.arctan, true},
			{Function.ln, true},

			{Function.a, false},
			{Function.x, false}
		};
	}
}