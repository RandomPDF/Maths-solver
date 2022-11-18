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
		private OperationEnum operation;

		public Operation(OperationEnum operation)
		{
			this.operation = operation;
		}

		public OperationEnum GetOperation() { return operation; }
	}
}
