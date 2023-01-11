using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Maths_solver.UI;
using static Maths_solver.UI.Main;

namespace Maths_solver
{
	public partial class Steps : Form
	{
		private int tabCount = 0;

		public Steps()
		{
			InitializeComponent();
			Maths.Maths.ShowSteps += ShowSteps;
		}

		#region Steps
		private void ShowSteps(object sender, Step step)
		{
			//add tabs
			for (int i = 0; i < tabCount; i++) StepsBox.Text += "\t";

			switch(step.phase)
			{
				case Phase.End:
					switch(step.rule)
					{
						case Rule.None: break;

						default:
							StepsBox.Text += $"Using the {step.rule.ToString()} rule:" +
								$"{EquationStr(step.input, false)} ->" +
								$"{EquationStr(step.output, false)}\n\n";
							break;
					}

					tabCount--;
					break;

				case Phase.Start:
					switch(step.rule)
					{
						case Rule.Constant:
							StepsBox.Text += $"Term {EquationStr(step.input, false)} -> 0\n\n";
							break;

						case Rule.Chain:
							StepsBox.Text += $"Differentiate input " +
								$"{EquationStr(step.input, false)}:\n\n";
							break;

						default:
							StepsBox.Text += $"Differentiate term {EquationStr(step.input, false)}:\n\n";
							break;
					}

					tabCount++;
					break;

				case Phase.Reset:
					tabCount = 0;
					StepsBox.Text = String.Empty;
					break;
			}
		}

		#endregion
	}
}
