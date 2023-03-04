namespace Maths_solver
{
	partial class Steps
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
			this.stepsLabel = new System.Windows.Forms.Label();
			this.StepsBox = new System.Windows.Forms.RichTextBox();
			this.ExitButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// stepsLabel
			// 
			this.stepsLabel.AutoSize = true;
			this.stepsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.stepsLabel.Location = new System.Drawing.Point(9, 7);
			this.stepsLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.stepsLabel.Name = "stepsLabel";
			this.stepsLabel.Size = new System.Drawing.Size(63, 25);
			this.stepsLabel.TabIndex = 0;
			this.stepsLabel.Text = "Steps";
			// 
			// StepsBox
			// 
			this.StepsBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.StepsBox.Cursor = System.Windows.Forms.Cursors.Arrow;
			this.StepsBox.Font = new System.Drawing.Font("Cambria", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.StepsBox.Location = new System.Drawing.Point(9, 33);
			this.StepsBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.StepsBox.Name = "StepsBox";
			this.StepsBox.ReadOnly = true;
			this.StepsBox.Size = new System.Drawing.Size(1410, 756);
			this.StepsBox.TabIndex = 1;
			this.StepsBox.Text = "";
			this.StepsBox.WordWrap = false;
			// 
			// ExitButton
			// 
			this.ExitButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ExitButton.Location = new System.Drawing.Point(9, 793);
			this.ExitButton.Name = "ExitButton";
			this.ExitButton.Size = new System.Drawing.Size(106, 45);
			this.ExitButton.TabIndex = 2;
			this.ExitButton.Text = "EXIT";
			this.ExitButton.UseVisualStyleBackColor = true;
			this.ExitButton.Click += new System.EventHandler(this.ExitButton_Click);
			// 
			// Steps
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoScroll = true;
			this.ClientSize = new System.Drawing.Size(963, 817);
			this.Controls.Add(this.ExitButton);
			this.Controls.Add(this.StepsBox);
			this.Controls.Add(this.stepsLabel);
			this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.Name = "Steps";
			this.Text = "Steps";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label stepsLabel;
		private System.Windows.Forms.RichTextBox StepsBox;
		private System.Windows.Forms.Button ExitButton;
	}
}