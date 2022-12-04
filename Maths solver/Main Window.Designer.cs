namespace Maths_solver
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
			this.SuspendLayout();
			// 
			// InputBox
			// 
			this.InputBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.InputBox.Location = new System.Drawing.Point(79, 74);
			this.InputBox.Margin = new System.Windows.Forms.Padding(2);
			this.InputBox.Name = "InputBox";
			this.InputBox.Size = new System.Drawing.Size(627, 112);
			this.InputBox.TabIndex = 2;
			this.InputBox.Text = "";
			// 
			// OutputBox
			// 
			this.OutputBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.OutputBox.Location = new System.Drawing.Point(79, 271);
			this.OutputBox.Margin = new System.Windows.Forms.Padding(2);
			this.OutputBox.Name = "OutputBox";
			this.OutputBox.ReadOnly = true;
			this.OutputBox.Size = new System.Drawing.Size(627, 123);
			this.OutputBox.TabIndex = 3;
			this.OutputBox.Text = "";
			// 
			// InputLabel
			// 
			this.InputLabel.AutoSize = true;
			this.InputLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.InputLabel.Location = new System.Drawing.Point(74, 47);
			this.InputLabel.Name = "InputLabel";
			this.InputLabel.Size = new System.Drawing.Size(55, 25);
			this.InputLabel.TabIndex = 4;
			this.InputLabel.Text = "Input";
			// 
			// OutputLabel
			// 
			this.OutputLabel.AutoSize = true;
			this.OutputLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.OutputLabel.Location = new System.Drawing.Point(74, 244);
			this.OutputLabel.Name = "OutputLabel";
			this.OutputLabel.Size = new System.Drawing.Size(71, 25);
			this.OutputLabel.TabIndex = 5;
			this.OutputLabel.Text = "Output";
			// 
			// Main
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Controls.Add(this.OutputLabel);
			this.Controls.Add(this.InputLabel);
			this.Controls.Add(this.OutputBox);
			this.Controls.Add(this.InputBox);
			this.Name = "Main";
			this.Text = "Form1";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.RichTextBox InputBox;
		private System.Windows.Forms.RichTextBox OutputBox;
		private System.Windows.Forms.Label InputLabel;
		private System.Windows.Forms.Label OutputLabel;
	}
}

