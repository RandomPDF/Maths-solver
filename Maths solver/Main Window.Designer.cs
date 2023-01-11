namespace Maths_solver.UI
{
	partial class Main
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.InputBox = new System.Windows.Forms.RichTextBox();
			this.OutputBox = new System.Windows.Forms.RichTextBox();
			this.InputLabel = new System.Windows.Forms.Label();
			this.OutputLabel = new System.Windows.Forms.Label();
			this.DifferentiateButton = new System.Windows.Forms.Button();
			this.StepsButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// InputBox
			// 
			this.InputBox.Font = new System.Drawing.Font("Cambria", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.InputBox.Location = new System.Drawing.Point(26, 55);
			this.InputBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.InputBox.Name = "InputBox";
			this.InputBox.Size = new System.Drawing.Size(1864, 373);
			this.InputBox.TabIndex = 2;
			this.InputBox.Text = "";
			this.InputBox.TextChanged += new System.EventHandler(this.InputBox_TextChanged);
			// 
			// OutputBox
			// 
			this.OutputBox.Font = new System.Drawing.Font("Cambria", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.OutputBox.Location = new System.Drawing.Point(26, 543);
			this.OutputBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.OutputBox.Name = "OutputBox";
			this.OutputBox.ReadOnly = true;
			this.OutputBox.Size = new System.Drawing.Size(1864, 479);
			this.OutputBox.TabIndex = 3;
			this.OutputBox.Text = "";
			// 
			// InputLabel
			// 
			this.InputLabel.AutoSize = true;
			this.InputLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.InputLabel.Location = new System.Drawing.Point(20, 22);
			this.InputLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.InputLabel.Name = "InputLabel";
			this.InputLabel.Size = new System.Drawing.Size(68, 29);
			this.InputLabel.TabIndex = 4;
			this.InputLabel.Text = "Input";
			// 
			// OutputLabel
			// 
			this.OutputLabel.AutoSize = true;
			this.OutputLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.OutputLabel.Location = new System.Drawing.Point(21, 512);
			this.OutputLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.OutputLabel.Name = "OutputLabel";
			this.OutputLabel.Size = new System.Drawing.Size(88, 29);
			this.OutputLabel.TabIndex = 5;
			this.OutputLabel.Text = "Output";
			// 
			// DifferentiateButton
			// 
			this.DifferentiateButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.DifferentiateButton.Location = new System.Drawing.Point(614, 463);
			this.DifferentiateButton.Name = "DifferentiateButton";
			this.DifferentiateButton.Size = new System.Drawing.Size(253, 37);
			this.DifferentiateButton.TabIndex = 6;
			this.DifferentiateButton.Text = "DIFFERENTIATE";
			this.DifferentiateButton.UseVisualStyleBackColor = true;
			this.DifferentiateButton.Click += new System.EventHandler(this.DifferentaiteButton_Click);
			// 
			// StepsButton
			// 
			this.StepsButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.StepsButton.Location = new System.Drawing.Point(896, 463);
			this.StepsButton.Name = "StepsButton";
			this.StepsButton.Size = new System.Drawing.Size(253, 37);
			this.StepsButton.TabIndex = 7;
			this.StepsButton.Text = "SHOW STEPS";
			this.StepsButton.UseVisualStyleBackColor = true;
			this.StepsButton.Click += new System.EventHandler(this.StepsButton_Click);
			// 
			// Main
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1902, 1033);
			this.Controls.Add(this.StepsButton);
			this.Controls.Add(this.DifferentiateButton);
			this.Controls.Add(this.OutputLabel);
			this.Controls.Add(this.InputLabel);
			this.Controls.Add(this.OutputBox);
			this.Controls.Add(this.InputBox);
			this.Margin = new System.Windows.Forms.Padding(4);
			this.Name = "Main";
			this.Text = "Calculator";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.RichTextBox InputBox;
		private System.Windows.Forms.RichTextBox OutputBox;
		private System.Windows.Forms.Label InputLabel;
		private System.Windows.Forms.Label OutputLabel;
		private System.Windows.Forms.Button DifferentiateButton;
		private System.Windows.Forms.Button StepsButton;
	}
}

