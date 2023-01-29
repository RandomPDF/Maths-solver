using System;
using System.Windows.Forms;

namespace Maths_solver
{
	public partial class Instructions : Form
	{
		public Instructions()
		{
			InitializeComponent();
		}

		private void Instructions_Load(object sender, EventArgs e)
		{
			InstructionsBox.Text = "Just don't be an idiot OK?";
		}

		private void ExitButton_Click(object sender, EventArgs e) { Hide(); }
	}
}
