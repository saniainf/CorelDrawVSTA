using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Windows;

namespace InfTrimMarks
{
	class DoubleTextBox : TextBox
	{
		private const string unitsStr = " mm";
		private string decSep = NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator;
		private Regex numRegEx = new Regex(@"[^0-9]+");
		private double indecValue = 1.0;

		static DoubleTextBox()
		{

		}

		protected override void OnTextInput(TextCompositionEventArgs e)
		{
			if (numRegEx.IsMatch(e.Text))
				e.Handled = true;
			if (e.Text == decSep && !this.Text.Contains(decSep))
				e.Handled = false;
			base.OnTextInput(e);
		}

		protected override void OnGotFocus(RoutedEventArgs e)
		{
			//this.Text = this.Text.Replace(unitsStr, string.Empty);
			base.OnGotFocus(e);
		}

		protected override void OnLostFocus(RoutedEventArgs e)
		{
			double result;
			string s = Regex.Replace(this.Text, @"[^0-9]+", "", RegexOptions.Compiled);
			if (double.TryParse(s, out result))
				this.Text = result.ToString() + unitsStr;
			else
				this.Undo();
			base.OnLostFocus(e);
		}

		protected override void OnPreviewKeyDown(KeyEventArgs e)
		{
			e.Handled = (e.Key == Key.Space);

			if (e.Key == Key.Up)
				increase();
			if (e.Key == Key.Down)
				decrease();

			base.OnPreviewKeyDown(e);
		}

		public void increase()
		{
			double result;
			string s = Regex.Replace(this.Text, @"[^0-9]+", "", RegexOptions.Compiled);
			double.TryParse(s, out result);
			//result = Math.Round(result);
			result = result + indecValue;
			this.Text = result.ToString() + unitsStr;
		}

		public void decrease()
		{
			double result;
			string s = Regex.Replace(this.Text, @"[^0-9]+", "", RegexOptions.Compiled);
			double.TryParse(s, out result);
			//result = Math.Round(result);
			if (result > indecValue + 0.1f)
				result = result - indecValue;
			this.Text = result.ToString() + unitsStr;
		}
	}
}
