using Maths_solver.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using static Maths_solver.Maths.Functions;
using static Maths_solver.Maths.Operation;

namespace Maths_solver.UI
{
	public partial class Main : Form
	{
		private Maths.Math math = new Maths.Math();

		private static Dictionary<char, char> CharacterToSuperscript = new Dictionary<char, char>()
		{
			{'0', (char)0X2070}, {'1', (char)0X00B9}, {'2', (char)0X00B2}, {'3', (char)0X00B3},
			{'4', (char)0X2074}, {'5', (char)0X2075}, {'6', (char)0X2076}, {'7', (char)0X2077},
			{'8', (char)0X2078}, {'9', (char)0X2079},

			{'+', (char)0X207A}, {'-', (char)0X207B}, {' ', ' '}, {'.', (char)0X0387},
			{'(', (char)0X207D}, {')', (char)0X207E},

			{'π', (char)0X2DEB},
			{'a', (char)0X1D43}, {'b', (char)0X1D47}, {'c', (char)0X1D9C}, {'d', (char)0X1D48},
			{'e', (char)0X1D49}, {'f', (char)0X1DA0}, {'g', (char)0X1D4D}, {'h', (char)0X02B0},
			{'i', (char)0X2071}, {'j', (char)0X02B2}, {'k', (char)0X1D4F}, {'l', (char)0X02E1},
			{'m', (char)0X1D50}, {'n', (char)0X207F}, {'o', (char)0X1D52}, {'p', (char)0X1D56},
			{'r', (char)0X02B3}, {'s', (char)0X02E2}, {'t', (char)0X1D57}, {'u', (char)0X1D58},
			{'v', (char)0X1D5B}, {'w', (char)0X02B7}, {'x', (char)0X02E3}, {'y', (char)0X02B8},
			{'z', (char)0X1DBB}
		};

		private bool isRealtime = false;

		private bool baseIsSuperscript = false;

		private bool isSuperscript
		{
			get { return baseIsSuperscript; }
			set
			{
				//change checked value when changed.
				if (baseIsSuperscript != value)
				{
					baseIsSuperscript = value;
					SuperscriptCheckbox.Checked = isSuperscript;
				}
			}
		}

		private string currentInput = String.Empty;
		private string previousInput = String.Empty;

		private int currentCursorPosition = -1;

		private Steps StepsForm = new Steps();
		private Instructions InstructionsForm = new Instructions();

		public Main()
		{
			InitializeComponent();

			FormBorderStyle = FormBorderStyle.None;
			WindowState = FormWindowState.Maximized;

			StepsForm.FormBorderStyle = FormBorderStyle.None;
			StepsForm.WindowState = FormWindowState.Maximized;
			StepsForm.Hide();

			InstructionsForm.FormBorderStyle = FormBorderStyle.None;
			InstructionsForm.WindowState = FormWindowState.Maximized;
			InstructionsForm.Hide();
		}

		#region Equation to string
		public static string EquationStr(Equation equation, bool useSuperscript)
		{
			bool returnSuperscript = false;

			if (equation == null || equation.Count == 0) return String.Empty;

			string equationString = String.Empty;

			foreach (EquationItem equationItem in equation)
			{
				if (equationItem == null) continue;

				if (equationItem.GetType() == typeof(Term)) equationString += TermStr((Term)equationItem, equation.Count, ref useSuperscript, ref returnSuperscript);

				else if (equationItem.GetType() == typeof(Operation)) equationString += TermStr((Operation)equationItem, ref useSuperscript, ref returnSuperscript);
			}

			return equationString;
		}

		private static string TermStr(Term term, int equationLength, ref bool useSuperscript,
			ref bool returnSuperscript)
		{
			string formatTerm = String.Empty;

			#region coefficient
			//format coefficient
			if (((term.function == Function.constant || System.Math.Abs(term.coeficient) != 1) && !useSuperscript)
				|| (term.coeficient != 1 || (equationLength != 1 && term.function == Function.constant)) && useSuperscript)
			{
				formatTerm += PartStr(term.coeficient.ToString(), useSuperscript);
			}

			else if (term.coeficient == -1 && !useSuperscript) formatTerm += PartStr("-", useSuperscript);
			#endregion

			if (term.function != Function.constant)
				formatTerm += PartStr(term.function.ToString(), useSuperscript);

			#region exponent
			//format exponent
			if (term.exponent != null)
			{
				string exponent = EquationStr(term.exponent, true);

				//if exponent 0 return coefficient only
				if (exponent == CharacterToSuperscript['0'].ToString()) return term.coeficient.ToString();
				else formatTerm += exponent;
			}
			#endregion

			//format input
			if (requiresInput[term.function])
			{
				if (!useSuperscript) formatTerm += $"({EquationStr(term.input, false)})";
				else formatTerm += CharacterToSuperscript['('] + EquationStr(term.input, true) + CharacterToSuperscript[')'];
			}

			if (returnSuperscript)
			{
				returnSuperscript = false;
				useSuperscript = false;
			}

			return formatTerm;
		}

		private static string TermStr(Operation operation, ref bool useSuperscript,
			ref bool returnSuperscript)
		{
			if (useSuperscript)
			{
				return $" {CharacterToSuperscript[(operationToString[operation.operation].Trim())[0]]} ";
			}
			else if (operationToString.ContainsKey(operation.operation))
			{
				return operationToString[operation.operation];
			}
			else
			{
				returnSuperscript = true;
				useSuperscript = true;
				return String.Empty;
			}
		}
		private static string PartStr(string part, bool superscript)
		{
			if (!superscript) return part;

			return ToSuperscript(part);
		}
		#endregion

		#region String to equation

		private bool CheckEquation(string input, ref Equation equation)
		{
			//if input is just a number
			if (float.TryParse(input, out float result))
			{
				equation.Add(new Term(result));
				return false;
			}

			if (input.Count(f => f == '(') != input.Count(f => f == ')'))
			{
				ErrorBox.Text += $"The equation '{input}' has a bracket imbalance. There must be an equal number of closed and open brackets. \n";

				return false;
			}

			return true;
		}

		private Equation StringToEquation(string rawInput)
		{
			Equation equation = new Equation();

			if (!CheckEquation(rawInput, ref equation)) return equation;

			bool foundExponent = false;

			float coefficient = 1;
			Function function = Function.NONE;
			Equation functionInput = null;
			Equation exponent = new Equation { new Term() };

			#region format input
			rawInput = rawInput.ToLower();

			//remove spaces
			string finalInput = String.Empty;
			for (int i = 0; i < rawInput.Length; i++)
			{
				if (rawInput[i] == ' ') continue;
				finalInput += rawInput[i];
			}
			#endregion

			Stack<char> brackets = new Stack<char>();
			string currentPart = String.Empty;
			for (int nextIndex = 0; nextIndex < finalInput.Length; nextIndex++)
			{
				//If extra bracket, move on.
				if (ExtraBrackets(finalInput, nextIndex, brackets, ref equation,
					ref currentPart, ref functionInput)) continue;

				//if open bracket, add to stack. If closed bracket, remove from stack
				FormatBracketsStack(finalInput, nextIndex, ref brackets);

				FindCoefficient(ref currentPart, finalInput[nextIndex], ref coefficient);

				if (brackets.Count <= 0 || (finalInput[nextIndex] == '(' && brackets.Count == 1))
					FindFunction(finalInput, nextIndex, ref function, ref currentPart);

				FindExponent(finalInput, nextIndex, currentPart, function, functionInput, ref exponent, ref foundExponent);

				currentPart += finalInput[nextIndex];

				//find input and then exponents after
				if (FindInputExponents(finalInput, nextIndex, brackets, function, ref currentPart, ref coefficient,
					ref exponent, ref functionInput, ref equation, ref foundExponent)) continue;

				CreateEquation(function, coefficient, functionInput, exponent, foundExponent, ref equation);

				CheckOperation(finalInput, nextIndex, ref coefficient, ref function, ref functionInput, ref exponent,
					ref currentPart, ref equation, ref foundExponent);
			}

			return equation;
		}

		private bool FindInputExponents(string finalInput, int nextIndex, Stack<char> brackets,
			Function function, ref string currentPart, ref float coefficient, ref Equation exponent,
			ref Equation functionInput, ref Equation equation, ref bool foundExponent)
		{
			if (brackets.Count <= 0 && function == Function.constant && IsSuperscript(currentPart, out _))
				FindBracketExponent(finalInput, nextIndex, ref currentPart, ref function,
				ref functionInput, ref coefficient, ref exponent, ref foundExponent, ref equation);

			//finding input and exponent
			if (brackets.Count <= 0 && (currentPart.Length <= 0 || finalInput[nextIndex] != ')')) return false;

			//finds bracket or function input
			FindInput(function, ref currentPart, ref functionInput, ref brackets, ref equation);

			//find exponent after input
			if (brackets.Count <= 0) FindFunctionInputExponent(function, finalInput, nextIndex, functionInput,
				ref coefficient, ref exponent, ref currentPart, ref equation, ref foundExponent);

			return true;
		}

		private void FindBracketExponent(string finalInput, int nextIndex, ref string currentPart,
			ref Function function, ref Equation functionInput, ref float coefficient, ref Equation exponent,
			ref bool foundExponent, ref Equation equation)
		{
			FindExponent(finalInput, nextIndex, currentPart, function, functionInput, ref exponent,
				ref foundExponent);

			//if exponent not found or exponent is just a 1
			if (!foundExponent || (exponent.Count <= 1 && exponent[0].GetType() == typeof(Term) &&
				((Term)exponent[0]).coeficient == 1 && ((Term)exponent[0]).function == Function.constant)) return;

			equation.Add(new Operation(OperationEnum.Power));

			for (int i = 0; i < exponent.Count; i++)
			{
				equation.Add(exponent[i]);
			}

			//restart a new term.
			coefficient = 1;
			function = Function.NONE;
			functionInput = null;
			exponent = new Equation { new Term() };
			currentPart = String.Empty;
			foundExponent = false;
		}

		private bool FindFunctionInputExponent(Function function, string finalInput, int nextIndex,
			Equation functionInput, ref float coefficient, ref Equation exponent,
			ref string currentPart, ref Equation equation, ref bool foundExponent)
		{
			//input of function
			if (requiresInput.ContainsKey(function) && requiresInput[function])
			{
				if (finalInput[nextIndex] != ')' && functionInput != null)
				{
					CheckOperation(finalInput, nextIndex, ref coefficient, ref function, ref functionInput,
						ref exponent, ref currentPart, ref equation, ref foundExponent);

					return true;
				}

				FindExponent(finalInput, nextIndex, currentPart, function, functionInput, ref exponent,
					ref foundExponent);

				//don't create term twice from bracket of input
				if (finalInput[nextIndex] != ')' || nextIndex >= finalInput.Length - 1)
					CreateEquation(function, coefficient, functionInput, exponent, foundExponent, ref equation);
			}

			return false;
		}

		private void FindInput(Function function, ref string currentPart,
			ref Equation functionInput, ref Stack<char> brackets, ref Equation equation)
		{
			if (brackets.Count != 0 || currentPart.Length < 2) return;

			Equation inputEquation = StringToEquation(currentPart.Substring(1, currentPart.Length - 2));

			//input is within brackets
			if (function == Function.NONE || function == Function.constant || function == Function.e)
			{
				for (int i = 0; i < inputEquation.Count; i++)
				{
					equation.Add(inputEquation[i]);
				}

				equation.Add(new Operation(OperationEnum.ClosedBracket));

				currentPart = String.Empty;
			}
			//input is within function input
			else
			{
				functionInput = inputEquation;
				currentPart = String.Empty;
			}
		}

		private bool ExtraBrackets(string finalInput, int nextIndex, Stack<char> brackets,
			ref Equation equation, ref string currentPart, ref Equation functionInput)
		{
			//if current is an open bracket, check if first character or if previous is an operation
			if ((finalInput[nextIndex] != '(' || (nextIndex != 0 &&
				!stringToOperation.ContainsKey(finalInput[nextIndex - 1]))) || brackets.Count != 0)
				return false;


			equation.Add(new Operation(OperationEnum.OpenBracket));

			currentPart += finalInput[nextIndex];
			FormatBracketsStack(finalInput, nextIndex, ref brackets);

			return true;
		}

		private void FormatBracketsStack(string finalInput, int nextIndex, ref Stack<char> brackets)
		{
			//ensure brackets are balanced, and can find input
			if (finalInput[nextIndex] == '(') brackets.Push(finalInput[nextIndex]);
			if (finalInput[nextIndex] == ')' && brackets.Count > 0) brackets.Pop();
		}

		private void FindCoefficient(ref string currentPart, char nextCharacter, ref float coefficient)
		{
			float newCoefficient;

			//current part is int, but next part isn't, must be whole coefficient
			if (float.TryParse(currentPart, out newCoefficient) &&
				!float.TryParse(nextCharacter.ToString(), out float _) && nextCharacter != '.')
			{
				coefficient = newCoefficient;
				currentPart = String.Empty;
			}

			else if (float.TryParse(currentPart + nextCharacter.ToString(), out newCoefficient))
				coefficient = newCoefficient;
		}

		private bool IsSuperscript(string input, out string inputToSuperscript)
		{
			inputToSuperscript = String.Empty;

			//if no text, not superscript
			if (input.Length == 0) return false;

			//loop through each character in text
			for (int i = 0; i < input.Length; i++)
			{
				bool isCharacterSuperscript = false;
				char[] superscriptValues = CharacterToSuperscript.Values.ToArray();

				//if character matches any superscript character, is superscript
				for (int x = 0; x < superscriptValues.Length; x++)
				{
					if (input[i] != superscriptValues[x]) continue;

					inputToSuperscript += CharacterToSuperscript.Keys.ToArray()[x];
					isCharacterSuperscript = true;
					break;
				}

				if (!isCharacterSuperscript) return false;
			}

			return true;
		}

		private static string ToSuperscript(string input)
		{
			string inputToSuperscript = String.Empty;
			for (int i = 0; i < input.Length; i++)
			{
				if (CharacterToSuperscript.ContainsKey(input[i]))
					inputToSuperscript += CharacterToSuperscript[input[i]];
			}

			return inputToSuperscript;
		}

		private void FindFunction(string input, int nextIndex, ref Function function, ref string currentPart)
		{
			if (function != Function.NONE) return;

			OperationEnum nextOperation = OperationEnum.NONE;
			if (stringToOperation.ContainsKey(input[nextIndex])) nextOperation = stringToOperation[input[nextIndex]];
			Function newFunction = Function.NONE;

			//if next character is superscript or next char is input
			//or next is at end of string
			if (((IsSuperscript(input[nextIndex].ToString(), out string _) || input[nextIndex] == '(') &&
				Enum.TryParse(currentPart, out newFunction)) ||

				(nextIndex >= input.Length - 1 && Enum.TryParse(input[nextIndex].ToString(), out newFunction) &&
				newFunction.ToString() == input[nextIndex].ToString())
				|| nextOperation != OperationEnum.NONE && Enum.TryParse(currentPart.ToString(), out newFunction))
			{
				function = newFunction;
				currentPart = String.Empty;
			}
			else if ((nextOperation != OperationEnum.NONE && nextOperation != OperationEnum.ClosedBracket)
				|| nextIndex == input.Length - 1)
			{
				function = Function.constant;
			}
		}

		private void FindExponent(string input, int nextIndex, string currentPart, Function function, Equation functionInput, ref Equation exponent, ref bool foundExponent)
		{
			if (foundExponent) return;

			string exponentLong = String.Empty;

			if (!IsSuperscript(input[nextIndex].ToString(), out string _))
			{
				if (IsSuperscript(currentPart, out exponentLong))
				{
					exponent = StringToEquation(exponentLong);
					foundExponent = true;
				}

				if (function == Function.x || function == Function.constant ||
					function == Function.e) foundExponent = true;
			}
			//end of string, but try to find exponent
			else if (IsSuperscript(input[nextIndex].ToString(), out string _) && nextIndex >= input.Length - 1 &&
				IsSuperscript(currentPart + input[nextIndex], out exponentLong))
			{
				exponent = StringToEquation(exponentLong);
				foundExponent = true;
			}


			//find where exponent 1
			if (functionInput != null &&
				((nextIndex >= input.Length - 1 && input[nextIndex] == ')') ||
				(input[nextIndex] != ')' && !IsSuperscript(input[nextIndex].ToString(), out string _))))
			{
				foundExponent = true;
				return;
			}
		}

		private void CheckOperation(string input, int nextIndex,
			ref float coefficient, ref Function function, ref Equation functionInput,
			ref Equation exponent, ref string currentPart, ref Equation equation,
			ref bool foundExponent)
		{
			char operation = input[nextIndex];

			//if operation, new term
			OperationEnum operationEnum = OperationEnum.NONE;
			if (stringToOperation.ContainsKey(operation)) operationEnum = stringToOperation[operation];

			//if end of current term
			if ((operationEnum != OperationEnum.NONE && operationEnum != OperationEnum.OpenBracket)
				|| nextIndex >= input.Length - 1)
			{
				//if nothing can be found, assume constant
				if (function == Function.NONE)
				{
					function = Function.constant;
					foundExponent = true;
					CreateEquation(function, coefficient, functionInput, exponent, foundExponent,
						ref equation);
				}

				//restart a new term.
				coefficient = 1;
				function = Function.NONE;
				functionInput = null;
				exponent = new Equation { new Term() };
				currentPart = String.Empty;
				foundExponent = false;

				if (operationEnum != OperationEnum.NONE) equation.Add(new Operation(operationEnum));
			}
		}

		private static void CreateEquation(Function function, float coefficient, Equation funcInput, Equation exponent, bool foundExponent, ref Equation equation)
		{
			if (function == Function.NONE || !foundExponent) return;

			//if has input and requires input
			if (funcInput != null && funcInput.Count != 0 && requiresInput[function])
			{
				equation.Add(new Term(coefficient, function, funcInput, exponent));
			}

			//if has no input but doesnt require input
			if ((funcInput == null || funcInput.Count == 0) && !requiresInput[function])
			{
				equation.Add(new Term(coefficient, function, exponent));
			}
		}
		#endregion

		#region UI
		private void InputBox_TextChanged(object sender, EventArgs e)
		{
			RichTextBox senderBox = sender as RichTextBox;
			currentInput = senderBox.Text.ToLower();

			//backspace must be pressed
			if (currentInput.Length < previousInput.Length)
			{
				previousInput = senderBox.Text.ToLower();
				return;
			}

			int newCharIndex = int.MinValue;
			char newChar = '\0';
			for (int i = 0; i < currentInput.Length; i++)
			{
				//added char must be at end
				if (previousInput.Length <= i && previousInput.Length != currentInput.Length)
				{
					newChar = currentInput[currentInput.Length - 1];
					newCharIndex = currentInput.Length - 1;
					break;
				}
				else if (currentInput[i] != previousInput[i])
				{
					newChar = currentInput[i];
					newCharIndex = i;
					break;
				}
			}

			//if input isn't nothing, and the last character was ^
			if (currentInput.Length > 0 && newChar == '^')
			{
				isSuperscript = !isSuperscript;

				//ignore ^ character
				UpdateBox(senderBox, currentInput.Remove(newCharIndex, 1), newCharIndex);
			}
			else if (isSuperscript)
			{
				//No longer superscript
				if (currentInput.Length <= newCharIndex || newCharIndex < 0 || !CharacterToSuperscript.ContainsKey(currentInput[newCharIndex]))
				{
					isSuperscript = false;
					return;
				}

				//replace newly added character with superscript variant
				string newString = currentInput.Remove(newCharIndex, 1);

				newString = newString.Insert(newCharIndex,
					CharacterToSuperscript[currentInput[newCharIndex]].ToString());

				UpdateBox(senderBox, newString, newCharIndex + 1);
			}
			else
			{
				UpdateBox(senderBox, currentInput, newCharIndex + 1);
			}

			previousInput = senderBox.Text.ToLower();
		}

		private void InputChanged()
		{
			if (isRealtime) DifferentaiteButton_Click(this, EventArgs.Empty);
		}

		private void UpdateBox(RichTextBox box, string text, int newCursorPosition)
		{
			box.Text = text;

			if (newCursorPosition >= 0) box.SelectionStart = newCursorPosition;
			else box.SelectionStart = text.Length;

			box.SelectionLength = 0;
		}

		private void InputBox_KeyDown(object sender, KeyEventArgs e)
		{
			//enter can't be pressed
			if (e.KeyCode == Keys.Enter) e.SuppressKeyPress = true;
		}

		private void InputBox_KeyUp(object sender, KeyEventArgs e)
		{
			RichTextBox senderBox = sender as RichTextBox;

			//update cursor position after each character inputted
			currentCursorPosition = senderBox.SelectionStart;

			InputChanged();
		}

		#region Buttons
		private void StepsButton_Click(object sender, EventArgs e) { StepsForm.Show(); }

		private void InstructionsButton_Click(object sender, EventArgs e) { InstructionsForm.Show(); }

		private void DifferentaiteButton_Click(object sender, EventArgs e)
		{
			ErrorBox.Text = String.Empty;

			//string output = EquationStr(math.Start(StringToEquation(InputBox.Text)), false);

			//debugging
			string output = EquationStr(StringToEquation(InputBox.Text), false);

			if (output != String.Empty || InputBox.Text == String.Empty) OutputBox.Text = output;
			else OutputBox.Text = "ERROR";
		}

		private void ExitButton_Click(object sender, EventArgs e) { Application.Exit(); }

		private void ButtonClick(object sender, bool isFunction)
		{
			Button button = sender as Button;

			int insertPosition = InputBox.Text.Length - 1;

			if (currentCursorPosition >= 0) insertPosition = currentCursorPosition;

			string text = String.Empty;

			if (isFunction) text = button.Text + "()";
			else text = button.Text;

			if (isSuperscript) text = ToSuperscript(text);

			bool prevSuperscript = isSuperscript;

			if (currentCursorPosition >= 0 && currentCursorPosition < InputBox.Text.Length)
			{
				InputBox.Text = InputBox.Text.Insert(insertPosition, text);

				if (!isFunction) InputBox.SelectionStart = insertPosition + text.Length;
				else InputBox.SelectionStart = insertPosition + text.Length - 1;
			}
			else
			{
				//superscript changes here?
				InputBox.Text += text;

				if (!isFunction) InputBox.SelectionStart = InputBox.Text.Length;
				else InputBox.SelectionStart = InputBox.Text.Length - 1;
			}

			//correct superscript change
			isSuperscript = prevSuperscript;

			currentCursorPosition = InputBox.SelectionStart;
			InputBox.Focus();

			InputChanged();
		}

		private void ConstantClick(object sender, EventArgs e)
		{
			ButtonClick(sender, false);
		}

		private void FunctionClick(object sender, EventArgs e)
		{
			ButtonClick(sender, true);
		}

		private void SuperscriptButton(object sender, EventArgs e)
		{
			isSuperscript = !isSuperscript;
			InputBox.Focus();
		}

		private void Realtime_CheckedChanged(object sender, EventArgs e)
		{
			CheckBox checkBox = sender as CheckBox;
			isRealtime = checkBox.Checked;
		}

		#endregion

		#endregion
	}
}
