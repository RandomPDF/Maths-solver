namespace Maths_solver
{
	public enum OperationEnum
	{ 
		Addition, 
		Subtraction, 
		Multiplication, 
		Division
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
