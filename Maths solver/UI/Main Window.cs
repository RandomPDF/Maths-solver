﻿using Maths_solver.Maths;
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

		private bool baseIsSuperscript = false;

		private bool isSuperscript
		{
			get { return baseIsSuperscript; }
			set
			{
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
		public static string EquationStr(List<EquationItem> equation, bool useSuperscript)
		{
			bool returnSuperscript = false;

			if (equation == null || equation.Count == 0) return "0";

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
			if (((term.function == Function.constant || Math.Abs(term.coeficient) != 1) && !useSuperscript)
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
		private static List<EquationItem> stringToEquation(string rawInput)
		{
			List<EquationItem> equation = new List<EquationItem>();

			bool foundExponent = false;

			float coefficient = 1;
			Function function = Function.NONE;
			List<EquationItem> functionInput = null;
			List<EquationItem> exponent = new List<EquationItem> { new Term() };

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
				//ensure brackets are balanced, and can find input
				if (finalInput[nextIndex] == '(') brackets.Push(finalInput[nextIndex]);
				if (finalInput[nextIndex] == ')') brackets.Pop();

				//finding input and exponent
				if ((currentPart.Length > 0 && currentPart[0] == '(') || functionInput != null)
				{
					SeparateString(finalInput[nextIndex], brackets, ref currentPart, ref functionInput);

					FindExponent(finalInput, nextIndex, currentPart, function, functionInput, ref exponent, ref foundExponent);

					CreateEquation(function, coefficient, functionInput, exponent, foundExponent, ref equation);

					continue;
				}

				FindCoefficient(ref currentPart, finalInput[nextIndex], ref coefficient);

				FindFunction(finalInput, nextIndex, ref function, ref currentPart);

				FindExponent(finalInput, nextIndex, currentPart, function, functionInput, ref exponent, ref foundExponent);

				SeparateString(finalInput[nextIndex], brackets, ref currentPart, ref functionInput);

				CreateEquation(function, coefficient, functionInput, exponent, foundExponent, ref equation);

				CheckOperation(finalInput, nextIndex, ref coefficient, ref function, ref functionInput, ref exponent,
					ref currentPart, ref equation, ref foundExponent);

			}

			return equation;
		}

		private static void FindCoefficient(ref string currentPart, char nextCharacter, ref float coefficient)
		{
			float newCoefficient;
			//current part is int, but next part isn't, must be whole coefficient
			if (float.TryParse(currentPart, out newCoefficient) &&
				!float.TryParse(nextCharacter.ToString(), out float _) && nextCharacter != '.')
			{
				coefficient = newCoefficient;
				currentPart = String.Empty;
			}
			else if (float.TryParse(currentPart + nextCharacter.ToString(), out newCoefficient)) coefficient = newCoefficient;
		}

		private static bool IsSuperscript(string input, out string inputToSuperscript)
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

		private static void SeparateString(char nextCharacter, Stack<char> brackets, ref string currentPart, ref List<EquationItem> functionInput)
		{
			//find input
			if (nextCharacter == ')')
			{
				if (brackets.Count == 0)
				{
					functionInput = stringToEquation(currentPart.Substring(1));
					currentPart = String.Empty;
				}
				else
				{
					functionInput = null;
					currentPart += nextCharacter;
				}
			}
			//move onto next char
			else
			{
				currentPart += nextCharacter;
			}
		}

		private static void FindFunction(string input, int nextIndex, ref Function function, ref string currentPart)
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
			else if (nextOperation != OperationEnum.NONE || nextIndex == input.Length - 1)
			{
				function = Function.constant;
			}
		}

		private static void FindExponent(string input, int nextIndex, string currentPart, Function function, List<EquationItem> functionInput, ref List<EquationItem> exponent, ref bool foundExponent)
		{
			if (foundExponent) return;

			//function with exponent of 1
			if (functionInput != null && ((nextIndex >= input.Length - 1 && input[nextIndex] == ')') ||
				(input[nextIndex] != ')' && !IsSuperscript(input[nextIndex].ToString(), out string _))))
			{
				foundExponent = true;
				return;
			}

			string exponentLong = String.Empty;

			if (!IsSuperscript(input[nextIndex].ToString(), out string _))
			{
				if (IsSuperscript(currentPart, out exponentLong))
				{
					exponent = stringToEquation(exponentLong);
					foundExponent = true;
				}

				if (function == Function.x || function == Function.constant ||
					function == Function.e) foundExponent = true;
			}
			else if (IsSuperscript(input[nextIndex].ToString(), out string _) && nextIndex >= input.Length - 1 &&
				((functionInput == null && IsSuperscript(currentPart + input[nextIndex], out exponentLong)) ||
				(functionInput != null && IsSuperscript(currentPart, out exponentLong))))
			{
				exponent = stringToEquation(exponentLong);
				foundExponent = true;
			}
		}


		private static void CheckOperation(string input, int nextIndex,
			ref float coefficient, ref Function function, ref List<EquationItem> functionInput,
			ref List<EquationItem> exponent, ref string currentPart, ref List<EquationItem> equation, ref bool foundExponent)
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

				coefficient = 1;
				function = Function.NONE;
				functionInput = null;
				exponent = new List<EquationItem> { new Term() };
				currentPart = String.Empty;
				foundExponent = false;

				if (operationEnum != OperationEnum.NONE) equation.Add(new Operation(operationEnum));
			}
		}

		private static void CreateEquation(Function function, float coefficient, List<EquationItem> funcInput, List<EquationItem> exponent, bool foundExponent, ref List<EquationItem> equation)
		{
			if (function == Function.NONE || !foundExponent) return;

			//if has input and requires input
			if (funcInput != null && requiresInput[function])
			{
				equation.Add(new Term(coefficient, function, funcInput, exponent));
			}

			//if has no input but doesnt require input
			if (funcInput == null && !requiresInput[function])
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

		private void UpdateBox(RichTextBox box, string text, int newCursorPosition)
		{
			box.Text = text;

			if (newCursorPosition >= 0) box.SelectionStart = newCursorPosition;
			else box.SelectionStart = text.Length;

			box.SelectionLength = 0;
		}

		private void InputBox_KeyUp(object sender, KeyEventArgs e)
		{
			UpdateCursor(sender);
		}

		private void UpdateCursor(object sender)
		{
			RichTextBox senderBox = sender as RichTextBox;
			currentCursorPosition = senderBox.SelectionStart;
		}

		private void StepsButton_Click(object sender, EventArgs e) { StepsForm.Show(); }

		private void InstructionsButton_Click(object sender, EventArgs e) { InstructionsForm.Show(); }

		private void DifferentaiteButton_Click(object sender, EventArgs e)
		{
			//OutputBox.Text = EquationStr(Maths.Maths.Start(stringToEquation(InputBox.Text)), false);

			//debugging
			OutputBox.Text = EquationStr(stringToEquation(InputBox.Text), false);
		}

		private void ExitButton_Click(object sender, EventArgs e) { Application.Exit(); }

		private void ButtonClick(object sender, bool isFunction)
		{
			Button button = sender as Button;

			int insertPosition = InputBox.Text.Length - 1;

			if (currentCursorPosition >= 0) insertPosition = currentCursorPosition;

			string superscriptText = String.Empty;

			if (isFunction) superscriptText = button.Text + "()";
			else superscriptText = button.Text;

			if (isSuperscript) superscriptText = ToSuperscript(superscriptText);

			if (currentCursorPosition >= 0 && currentCursorPosition < InputBox.Text.Length)
			{
				InputBox.Text = InputBox.Text.Insert(insertPosition, superscriptText);
				InputBox.SelectionStart = insertPosition + superscriptText.Length;
			}

			else
			{
				//superscript changes here?
				InputBox.Text += superscriptText;
				InputBox.SelectionStart = InputBox.Text.Length;
			}

			//correct superscript change
			isSuperscript = IsSuperscript(InputBox.Text[InputBox.Text.Length - 1].ToString(), out _);

			UpdateCursor(InputBox);
			InputBox.Focus();
		}

		private void ConstantClick(object sender, EventArgs e)
		{
			ButtonClick(sender, false);
		}

		private void FunctionClick(object sender, EventArgs e)
		{
			ButtonClick(sender, true);
			InputBox.SelectionStart--;
		}

		private void SuperscriptButton(object sender, EventArgs e)
		{
			isSuperscript = !isSuperscript;
			InputBox.Focus();
		}

		#endregion
	}
}