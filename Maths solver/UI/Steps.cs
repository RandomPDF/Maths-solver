using Maths_solver.Maths;
using Maths_solver.UI;
using System;
using System.Windows.Forms;

namespace Maths_solver
{
	public partial class Steps : Form
	{
		public string StepsText
		{
			get { return StepsBox.Text; }
			set { StepsBox.Text = value; }
		}

		private int tabCount = 0;

		public Steps()
		{
			InitializeComponent();
			Maths.Maths.ShowSteps += ShowSteps;
		}

		#region Steps
		private void ShowSteps(object sender, Step step)
		{
			string input = Equation.AsString(step.input, false, false);
			string output = Equation.AsString(step.output, false, false);

			if (step.input == null || step.input.Count == 0) input = "0";
			if (step.output == null || step.output.Count == 0) output = "0";


			if (tabCount < 0) tabCount = 0;

			if (!(step.rule == Rule.None &&
				step.input == null && step.output == null))
			{
				//add tabs
				for (int i = 0; i < tabCount; i++) StepsBox.Text += "\t";
			}

			switch (step.phase)
			{
				case Phase.End:
					switch (step.rule)
					{
						case Rule.None:
							if (step.input != null && step.output != null)
							{
								StepsBox.Text += $"So {input} → {output}\n\n";
							}
							break;

						case Rule.Input:
							StepsBox.Text += $"Multiply the input differential ( {input} )" +
								$" by the differential ( {output} )" +
								$" using the chain rule keeping the input the same.\n";
							break;

						case Rule.Exponent:
							StepsBox.Text += $"Multiply the exponent differential " +
								$"( {input} ) by the term " +
								$"( {output} ) using the chain rule" +
								$"keeping the exponent the same.\n";
							break;

						case Rule.ln:
							StepsBox.Text += $"Multiply the differential ( {input} ) " +
								$"by standard result " +
								$"( {output} ) using the chain rule.\n";
							break;

						case Rule.xRule:
							StepsBox.Text += $"Multiply the differential ( {input} ) " +
								$"by x rule result " +
								$"( {output} ) using the chain rule for exponents.\n";
							break;

						case Rule.PowerRule:
							StepsBox.Text += $"Multiply the function ( {input} ) " +
								$"by the differential of " +
								$"( {output} ) using the power rule.\n";
							break;

						case Rule.Product:
							StepsBox.Text += $"Multiply ( {input} ) " +
								$"by the differential of ( {output} ) and add that to the " +
								$"product of the ( {output} ) and the differential of ( {input} )\n";
							break;

						default:
							StepsBox.Text += $"Using the {step.rule.ToString()} rule: " +
								$"{input} → " +
								$"{output}\n\n";
							break;
					}

					tabCount--;
					break;

				case Phase.Start:
					switch (step.rule)
					{
						case Rule.Constant:
							StepsBox.Text += $"Using the constant rule: {input} → 0\n\n";
							break;

						case Rule.Input:
							StepsBox.Text += $"Differentiate input " +
								$"{input}:\n";
							break;

						case Rule.Exponent:
							StepsBox.Text += $"Differentiate exponent " +
								$"{input}:\n";
							break;

						case Rule.ln:
							StepsBox.Text += $"Multiply by ln({input})\n";
							break;

						case Rule.Product:
							StepsBox.Text += $"Perform product rule on {input}:\n";
							break;

						case Rule.None:
							break;

						default:
							StepsBox.Text += $"Differentiate term {input}:\n";
							break;
					}

					tabCount++;
					break;

				case Phase.Reset:
					tabCount = 1;
					StepsBox.Text = $"Differentiate equation {input}:\n";
					break;
			}
		}

		#endregion

		private void ExitButton_Click(object sender, EventArgs e) { Hide(); }
	}
}
