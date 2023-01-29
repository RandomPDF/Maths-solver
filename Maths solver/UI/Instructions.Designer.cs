namespace Maths_solver
{
	partial class Instructions
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
			this.ExitButton = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.InstructionsBox = new System.Windows.Forms.RichTextBox();
			this.SuspendLayout();
			// 
			// ExitButton
			// 
			this.ExitButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ExitButton.Location = new System.Drawing.Point(20, 970);
			this.ExitButton.Name = "ExitButton";
			this.ExitButton.Size = new System.Drawing.Size(134, 51);
			this.ExitButton.TabIndex = 0;
			this.ExitButton.Text = "EXIT";
			this.ExitButton.UseVisualStyleBackColor = true;
			this.ExitButton.Click += new System.EventHandler(this.ExitButton_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(142, 29);
			this.label1.TabIndex = 1;
			this.label1.Text = "Instructions";
			// 
			// InstructionsBox
			// 
			this.InstructionsBox.Cursor = System.Windows.Forms.Cursors.Arrow;
			this.InstructionsBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.InstructionsBox.Location = new System.Drawing.Point(17, 41);
			this.InstructionsBox.Name = "InstructionsBox";
			this.InstructionsBox.ReadOnly = true;
			this.InstructionsBox.Size = new System.Drawing.Size(1873, 923);
			this.InstructionsBox.TabIndex = 2;
			this.InstructionsBox.Text = "";
			// 
			// Instructions
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1902, 1033);
			this.Controls.Add(this.InstructionsBox);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.ExitButton);
			this.Name = "Instructions";
			this.Text = "Instructions";
			this.Load += new System.EventHandler(this.Instructions_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button ExitButton;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.RichTextBox InstructionsBox;
	}
}