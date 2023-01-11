﻿using System.Collections.Generic;

namespace Maths_solver.Maths
{
	public class Operation : EquationItem
	{
		public enum OperationEnum
		{
			Addition,
			Subtraction,
			Multiplication,
			Division,
			OpenBracket,
			ClosedBracket,
			NONE
		}

		public static Dictionary<OperationEnum, string> operationToString =
			new Dictionary<OperationEnum, string>()
			{
				{OperationEnum.Addition, " + " },
				{OperationEnum.Subtraction, " - " },
				{OperationEnum.Multiplication, string.Empty },
				{OperationEnum.Division, " / " },
				{OperationEnum.OpenBracket, "(" },
				{OperationEnum.ClosedBracket, ")" },
			};

		public static Dictionary<char, OperationEnum> stringToOperation =
			new Dictionary<char, OperationEnum>()
			{
				{'+', OperationEnum.Addition},
				{'-', OperationEnum.Subtraction},
				{'*', OperationEnum.Multiplication},
				{'/', OperationEnum.Division},
				{'(', OperationEnum.OpenBracket},
				{')', OperationEnum.ClosedBracket}
			};

		public OperationEnum operation { get; }

		public Operation(OperationEnum operation)
		{
			this.operation = operation;
		}
	}
}
