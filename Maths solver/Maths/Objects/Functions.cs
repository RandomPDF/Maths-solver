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
			{Function.arcsin, true},
			{Function.arccos, true},
			{Function.arctan, true},
			{Function.ln, true},

			{Function.a, false},
			{Function.x, false}
		};
	}
}