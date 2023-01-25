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
			if(tabCount < 0) tabCount = 0;

			if (!(step.rule == Rule.None &&
				step.input == null && step.output == null))
			{
				//add tabs
				for (int i = 0; i < tabCount; i++) StepsBox.Text += "\t";
			}

			switch(step.phase)
			{
				case Phase.End:
					switch(step.rule)
					{
						case Rule.None:
							if (step.input != null && step.output != null)
							{
								StepsBox.Text += $"So {EquationStr(step.input, false)} → " +
									$"{EquationStr(step.output, false)}\n\n";
							}
							break;

						case Rule.Input:
							StepsBox.Text += $"Multiply the input differential by the main differential using the chain rule keeping the input the same.\n";
							break;

						case Rule.Exponent:
							StepsBox.Text += $"Multiply the exponent differential by the main differential using the chain rule keeping the exponent the same.\n";
							break;

						default:
							StepsBox.Text += $"Using the {step.rule.ToString()} rule: " +
								$"{EquationStr(step.input, false)} → " +
								$"{EquationStr(step.output, false)}\n\n";
							break;
					}

					tabCount--;
					break;

				case Phase.Start:
					switch(step.rule)
					{
						case Rule.Constant:
							StepsBox.Text += $"Using the constant rule: {EquationStr(step.input, false)} → 0\n";
							break;

						case Rule.Input:
							StepsBox.Text += $"Differentiate input " +
								$"{EquationStr(step.input, false)}:\n";
							break;

						case Rule.Exponent:
							StepsBox.Text += $"Differentiate exponent " +
								$"{EquationStr(step.input, false)}:\n";
							break;

						case Rule.ln:
							StepsBox.Text += $"Multiply by ln({EquationStr(step.input, false)})\n";
							break;

						case Rule.None:
							break;

						default:
							StepsBox.Text += $"Differentiate term {EquationStr(step.input, false)}:\n";
							break;
					}

					tabCount++;
					break;

				case Phase.Reset:
					tabCount = 1;
					StepsBox.Text = $"Differentiate equation {EquationStr(step.input, false)}:\n";
					break;
			}
		}

		#endregion

		private void ExitButton_Click(object sender, EventArgs e) { Hide(); }
    }
}
