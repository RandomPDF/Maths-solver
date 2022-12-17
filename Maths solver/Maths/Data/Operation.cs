namespace Maths_solver.Maths
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

	public class Operation : EquationItem
	{
		public OperationEnum operation { get; }

		public Operation(OperationEnum operation)
		{
			this.operation = operation;
		}
	}
}
