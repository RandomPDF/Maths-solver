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
            this.ExitButton = new System.Windows.Forms.Button();
            this.piButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // InputBox
            // 
            this.InputBox.Font = new System.Drawing.Font("Cambria", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InputBox.Location = new System.Drawing.Point(20, 45);
            this.InputBox.Margin = new System.Windows.Forms.Padding(2);
            this.InputBox.Name = "InputBox";
            this.InputBox.Size = new System.Drawing.Size(1008, 304);
            this.InputBox.TabIndex = 2;
            this.InputBox.Text = "";
            this.InputBox.TextChanged += new System.EventHandler(this.InputBox_TextChanged);
            // 
            // OutputBox
            // 
            this.OutputBox.Font = new System.Drawing.Font("Cambria", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OutputBox.Location = new System.Drawing.Point(20, 441);
            this.OutputBox.Margin = new System.Windows.Forms.Padding(2);
            this.OutputBox.Name = "OutputBox";
            this.OutputBox.ReadOnly = true;
            this.OutputBox.Size = new System.Drawing.Size(1399, 369);
            this.OutputBox.TabIndex = 3;
            this.OutputBox.Text = "";
            // 
            // InputLabel
            // 
            this.InputLabel.AutoSize = true;
            this.InputLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InputLabel.Location = new System.Drawing.Point(15, 18);
            this.InputLabel.Name = "InputLabel";
            this.InputLabel.Size = new System.Drawing.Size(55, 25);
            this.InputLabel.TabIndex = 4;
            this.InputLabel.Text = "Input";
            // 
            // OutputLabel
            // 
            this.OutputLabel.AutoSize = true;
            this.OutputLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OutputLabel.Location = new System.Drawing.Point(16, 416);
            this.OutputLabel.Name = "OutputLabel";
            this.OutputLabel.Size = new System.Drawing.Size(71, 25);
            this.OutputLabel.TabIndex = 5;
            this.OutputLabel.Text = "Output";
            // 
            // DifferentiateButton
            // 
            this.DifferentiateButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DifferentiateButton.Location = new System.Drawing.Point(460, 376);
            this.DifferentiateButton.Margin = new System.Windows.Forms.Padding(2);
            this.DifferentiateButton.Name = "DifferentiateButton";
            this.DifferentiateButton.Size = new System.Drawing.Size(190, 30);
            this.DifferentiateButton.TabIndex = 6;
            this.DifferentiateButton.Text = "DIFFERENTIATE";
            this.DifferentiateButton.UseVisualStyleBackColor = true;
            this.DifferentiateButton.Click += new System.EventHandler(this.DifferentaiteButton_Click);
            // 
            // StepsButton
            // 
            this.StepsButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StepsButton.Location = new System.Drawing.Point(672, 376);
            this.StepsButton.Margin = new System.Windows.Forms.Padding(2);
            this.StepsButton.Name = "StepsButton";
            this.StepsButton.Size = new System.Drawing.Size(190, 30);
            this.StepsButton.TabIndex = 7;
            this.StepsButton.Text = "SHOW STEPS";
            this.StepsButton.UseVisualStyleBackColor = true;
            this.StepsButton.Click += new System.EventHandler(this.StepsButton_Click);
            // 
            // ExitButton
            // 
            this.ExitButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ExitButton.Location = new System.Drawing.Point(20, 814);
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(106, 45);
            this.ExitButton.TabIndex = 8;
            this.ExitButton.Text = "EXIT";
            this.ExitButton.UseVisualStyleBackColor = true;
            this.ExitButton.Click += new System.EventHandler(this.ExitButton_Click);
            // 
            // piButton
            // 
            this.piButton.Font = new System.Drawing.Font("Cambria", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.piButton.Location = new System.Drawing.Point(1033, 45);
            this.piButton.Name = "piButton";
            this.piButton.Size = new System.Drawing.Size(39, 30);
            this.piButton.TabIndex = 10;
            this.piButton.Text = "π";
            this.piButton.UseVisualStyleBackColor = true;
            this.piButton.Click += new System.EventHandler(this.piButton_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1444, 881);
            this.Controls.Add(this.piButton);
            this.Controls.Add(this.ExitButton);
            this.Controls.Add(this.StepsButton);
            this.Controls.Add(this.DifferentiateButton);
            this.Controls.Add(this.OutputLabel);
            this.Controls.Add(this.InputLabel);
            this.Controls.Add(this.OutputBox);
            this.Controls.Add(this.InputBox);
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
        private System.Windows.Forms.Button ExitButton;
        private System.Windows.Forms.Button piButton;
    }
}

