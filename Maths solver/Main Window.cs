using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Maths_solver
{
	public partial class Main : Form
	{
		public Main()
		{
			InitializeComponent();
		}

		//3x^3 + 5sin(x)^2
		List<IEquation> equation = new List<IEquation>()
		{
			//3x^3
			new Term(3, Function.x, 3),

			new OperationObject(Operation.Addition),

			//5sin(x)^2
			new Term(5, Function.sin , 2)
		};
	}
}
