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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.button11 = new System.Windows.Forms.Button();
            this.button12 = new System.Windows.Forms.Button();
            this.button13 = new System.Windows.Forms.Button();
            this.button14 = new System.Windows.Forms.Button();
            this.button15 = new System.Windows.Forms.Button();
            this.button17 = new System.Windows.Forms.Button();
            this.button18 = new System.Windows.Forms.Button();
            this.SuperscriptCheckbox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // InputBox
            // 
            this.InputBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.InputBox.DetectUrls = false;
            this.InputBox.Font = new System.Drawing.Font("Cambria", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InputBox.Location = new System.Drawing.Point(20, 45);
            this.InputBox.Margin = new System.Windows.Forms.Padding(2);
            this.InputBox.Name = "InputBox";
            this.InputBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Horizontal;
            this.InputBox.Size = new System.Drawing.Size(1253, 45);
            this.InputBox.TabIndex = 2;
            this.InputBox.Text = "";
            this.InputBox.WordWrap = false;
            this.InputBox.TextChanged += new System.EventHandler(this.InputBox_TextChanged);
            this.InputBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.InputBox_KeyUp);
            // 
            // OutputBox
            // 
            this.OutputBox.Cursor = System.Windows.Forms.Cursors.No;
            this.OutputBox.DetectUrls = false;
            this.OutputBox.Font = new System.Drawing.Font("Cambria", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OutputBox.Location = new System.Drawing.Point(21, 223);
            this.OutputBox.Margin = new System.Windows.Forms.Padding(2);
            this.OutputBox.Name = "OutputBox";
            this.OutputBox.ReadOnly = true;
            this.OutputBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Horizontal;
            this.OutputBox.Size = new System.Drawing.Size(1252, 45);
            this.OutputBox.TabIndex = 3;
            this.OutputBox.Text = "";
            this.OutputBox.WordWrap = false;
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
            this.OutputLabel.Location = new System.Drawing.Point(16, 196);
            this.OutputLabel.Name = "OutputLabel";
            this.OutputLabel.Size = new System.Drawing.Size(71, 25);
            this.OutputLabel.TabIndex = 5;
            this.OutputLabel.Text = "Output";
            // 
            // DifferentiateButton
            // 
            this.DifferentiateButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DifferentiateButton.Location = new System.Drawing.Point(369, 136);
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
            this.StepsButton.Location = new System.Drawing.Point(687, 136);
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
            this.piButton.Location = new System.Drawing.Point(20, 330);
            this.piButton.Name = "piButton";
            this.piButton.Size = new System.Drawing.Size(39, 30);
            this.piButton.TabIndex = 10;
            this.piButton.Text = "π";
            this.piButton.UseVisualStyleBackColor = true;
            this.piButton.Click += new System.EventHandler(this.ConstantClick);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Cambria", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(66, 330);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(39, 30);
            this.button1.TabIndex = 12;
            this.button1.Text = "e";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.ConstantClick);
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Cambria", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(21, 363);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(84, 30);
            this.button2.TabIndex = 13;
            this.button2.Text = "sin";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.FunctionClick);
            // 
            // button3
            // 
            this.button3.Font = new System.Drawing.Font("Cambria", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.Location = new System.Drawing.Point(111, 363);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(84, 30);
            this.button3.TabIndex = 14;
            this.button3.Text = "cos";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.FunctionClick);
            // 
            // button4
            // 
            this.button4.Font = new System.Drawing.Font("Cambria", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button4.Location = new System.Drawing.Point(201, 363);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(84, 30);
            this.button4.TabIndex = 15;
            this.button4.Text = "tan";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.FunctionClick);
            // 
            // button5
            // 
            this.button5.Font = new System.Drawing.Font("Cambria", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button5.Location = new System.Drawing.Point(21, 400);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(84, 30);
            this.button5.TabIndex = 16;
            this.button5.Text = "cosec";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.FunctionClick);
            // 
            // button6
            // 
            this.button6.Font = new System.Drawing.Font("Cambria", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button6.Location = new System.Drawing.Point(111, 400);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(84, 30);
            this.button6.TabIndex = 17;
            this.button6.Text = "sec";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.FunctionClick);
            // 
            // button7
            // 
            this.button7.Font = new System.Drawing.Font("Cambria", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button7.Location = new System.Drawing.Point(201, 400);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(84, 30);
            this.button7.TabIndex = 18;
            this.button7.Text = "cot";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.FunctionClick);
            // 
            // button8
            // 
            this.button8.Font = new System.Drawing.Font("Cambria", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button8.Location = new System.Drawing.Point(111, 330);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(39, 30);
            this.button8.TabIndex = 19;
            this.button8.Text = "x";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.ConstantClick);
            // 
            // button9
            // 
            this.button9.Font = new System.Drawing.Font("Cambria", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button9.Location = new System.Drawing.Point(295, 331);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(39, 30);
            this.button9.TabIndex = 20;
            this.button9.Text = "ln";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.FunctionClick);
            // 
            // button10
            // 
            this.button10.Font = new System.Drawing.Font("Cambria", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button10.Location = new System.Drawing.Point(475, 400);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(84, 30);
            this.button10.TabIndex = 26;
            this.button10.Text = "coth";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.FunctionClick);
            // 
            // button11
            // 
            this.button11.Font = new System.Drawing.Font("Cambria", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button11.Location = new System.Drawing.Point(385, 400);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(84, 30);
            this.button11.TabIndex = 25;
            this.button11.Text = "sech";
            this.button11.UseVisualStyleBackColor = true;
            this.button11.Click += new System.EventHandler(this.FunctionClick);
            // 
            // button12
            // 
            this.button12.Font = new System.Drawing.Font("Cambria", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button12.Location = new System.Drawing.Point(295, 400);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(84, 30);
            this.button12.TabIndex = 24;
            this.button12.Text = "cosech";
            this.button12.UseVisualStyleBackColor = true;
            this.button12.Click += new System.EventHandler(this.FunctionClick);
            // 
            // button13
            // 
            this.button13.Font = new System.Drawing.Font("Cambria", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button13.Location = new System.Drawing.Point(475, 363);
            this.button13.Name = "button13";
            this.button13.Size = new System.Drawing.Size(84, 30);
            this.button13.TabIndex = 23;
            this.button13.Text = "tanh";
            this.button13.UseVisualStyleBackColor = true;
            this.button13.Click += new System.EventHandler(this.FunctionClick);
            // 
            // button14
            // 
            this.button14.Font = new System.Drawing.Font("Cambria", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button14.Location = new System.Drawing.Point(385, 363);
            this.button14.Name = "button14";
            this.button14.Size = new System.Drawing.Size(84, 30);
            this.button14.TabIndex = 22;
            this.button14.Text = "cosh";
            this.button14.UseVisualStyleBackColor = true;
            this.button14.Click += new System.EventHandler(this.FunctionClick);
            // 
            // button15
            // 
            this.button15.Font = new System.Drawing.Font("Cambria", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button15.Location = new System.Drawing.Point(295, 363);
            this.button15.Name = "button15";
            this.button15.Size = new System.Drawing.Size(84, 30);
            this.button15.TabIndex = 21;
            this.button15.Text = "sinh";
            this.button15.UseVisualStyleBackColor = true;
            this.button15.Click += new System.EventHandler(this.FunctionClick);
            // 
            // button17
            // 
            this.button17.Font = new System.Drawing.Font("Cambria", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button17.Location = new System.Drawing.Point(385, 330);
            this.button17.Name = "button17";
            this.button17.Size = new System.Drawing.Size(39, 30);
            this.button17.TabIndex = 28;
            this.button17.Text = "+";
            this.button17.UseVisualStyleBackColor = true;
            this.button17.Click += new System.EventHandler(this.ConstantClick);
            // 
            // button18
            // 
            this.button18.Font = new System.Drawing.Font("Cambria", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button18.Location = new System.Drawing.Point(432, 330);
            this.button18.Name = "button18";
            this.button18.Size = new System.Drawing.Size(39, 30);
            this.button18.TabIndex = 29;
            this.button18.Text = "-";
            this.button18.UseVisualStyleBackColor = true;
            this.button18.Click += new System.EventHandler(this.ConstantClick);
            // 
            // SuperscriptCheckbox
            // 
            this.SuperscriptCheckbox.Appearance = System.Windows.Forms.Appearance.Button;
            this.SuperscriptCheckbox.BackColor = System.Drawing.SystemColors.ControlLight;
            this.SuperscriptCheckbox.FlatAppearance.BorderColor = System.Drawing.SystemColors.ActiveBorder;
            this.SuperscriptCheckbox.FlatAppearance.CheckedBackColor = System.Drawing.Color.DimGray;
            this.SuperscriptCheckbox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SuperscriptCheckbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F);
            this.SuperscriptCheckbox.Location = new System.Drawing.Point(201, 332);
            this.SuperscriptCheckbox.Margin = new System.Windows.Forms.Padding(2);
            this.SuperscriptCheckbox.Name = "SuperscriptCheckbox";
            this.SuperscriptCheckbox.Size = new System.Drawing.Size(39, 28);
            this.SuperscriptCheckbox.TabIndex = 30;
            this.SuperscriptCheckbox.Text = "^";
            this.SuperscriptCheckbox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.SuperscriptCheckbox.UseVisualStyleBackColor = false;
            this.SuperscriptCheckbox.Click += new System.EventHandler(this.SuperscriptButton);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1284, 881);
            this.Controls.Add(this.SuperscriptCheckbox);
            this.Controls.Add(this.button18);
            this.Controls.Add(this.button17);
            this.Controls.Add(this.button10);
            this.Controls.Add(this.button11);
            this.Controls.Add(this.button12);
            this.Controls.Add(this.button13);
            this.Controls.Add(this.button14);
            this.Controls.Add(this.button15);
            this.Controls.Add(this.button9);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
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
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.Button button4;
		private System.Windows.Forms.Button button5;
		private System.Windows.Forms.Button button6;
		private System.Windows.Forms.Button button7;
		private System.Windows.Forms.Button button8;
		private System.Windows.Forms.Button button9;
		private System.Windows.Forms.Button button10;
		private System.Windows.Forms.Button button11;
		private System.Windows.Forms.Button button12;
		private System.Windows.Forms.Button button13;
		private System.Windows.Forms.Button button14;
		private System.Windows.Forms.Button button15;
		private System.Windows.Forms.Button button17;
		private System.Windows.Forms.Button button18;
		private System.Windows.Forms.CheckBox SuperscriptCheckbox;
    }
}

