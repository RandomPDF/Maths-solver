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

		//3x^3 + 5sin(x^2)^2
		List<object> equation = new List<object>()
		{
			//3x^3
			new Term(3, Function.x, 3),

			Operation.Addition,

			//5sin(x^2)^2
			new Term(5, Function.sin, new Term(1, Function.x, 2) , 2)
		};
	}
}
