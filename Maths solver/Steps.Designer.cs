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
			this.label1 = new System.Windows.Forms.Label();
			this.StepsBox = new System.Windows.Forms.RichTextBox();
			this.ExitButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(78, 29);
			this.label1.TabIndex = 0;
			this.label1.Text = "Steps";
			// 
			// StepsBox
			// 
			this.StepsBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.StepsBox.Location = new System.Drawing.Point(12, 41);
			this.StepsBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.StepsBox.Name = "StepsBox";
			this.StepsBox.ReadOnly = true;
			this.StepsBox.Size = new System.Drawing.Size(1879, 929);
			this.StepsBox.TabIndex = 1;
			this.StepsBox.Text = "";
			// 
			// ExitButton
			// 
			this.ExitButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ExitButton.Location = new System.Drawing.Point(12, 976);
			this.ExitButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.ExitButton.Name = "ExitButton";
			this.ExitButton.Size = new System.Drawing.Size(141, 55);
			this.ExitButton.TabIndex = 2;
			this.ExitButton.Text = "EXIT";
			this.ExitButton.UseVisualStyleBackColor = true;
			this.ExitButton.Click += new System.EventHandler(this.ExitButton_Click);
			// 
			// Steps
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1925, 1084);
			this.Controls.Add(this.ExitButton);
			this.Controls.Add(this.StepsBox);
			this.Controls.Add(this.label1);
			this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.Name = "Steps";
			this.Text = "Steps";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.RichTextBox StepsBox;
        private System.Windows.Forms.Button ExitButton;
    }
}