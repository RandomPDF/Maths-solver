namespace Maths_solver
{
	public partial class OperationObject : IEquation
	{
		public Operation operation = default;

		public OperationObject(Operation operation) { this.operation = operation; }
	}
}
