using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Windows;

namespace InfColorConvert
{
	class IntTextBox : TextBox
	{
		private Regex numRegEx = new Regex(@"[^0-9]+");
		private int indecValue = 1;

		public int Value
		{
			get
			{
				int result;
				string s = Regex.Replace(this.Text, @"[^0-9]+", "", RegexOptions.Compiled);
				if (int.TryParse(s, out result))
					return result;
				else
					return 0;
			}
			set { this.Text = value.ToString(); }
		}

		static IntTextBox()
		{

		}

		protected override void OnTextInput(TextCompositionEventArgs e)
		{
			if (numRegEx.IsMatch(e.Text))
				e.Handled = true;
			base.OnTextInput(e);
		}

		protected override void OnGotFocus(RoutedEventArgs e)
		{
			base.OnGotFocus(e);
		}

		protected override void OnLostFocus(RoutedEventArgs e)
		{
			int result;
			string s = Regex.Replace(this.Text, @"[^0-9]+", "", RegexOptions.Compiled);
			if (int.TryParse(s, out result))
			{
				result = Math.Min(result, 100);
				result = Math.Max(result, 1);
				this.Text = result.ToString();
			}
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
			int result;
			string s = Regex.Replace(this.Text, @"[^0-9]+", "", RegexOptions.Compiled);
			int.TryParse(s, out result);
			//result = Math.Round(result);
			result = Math.Min(result + indecValue, 100);
			this.Text = result.ToString();
		}

		public void decrease()
		{
			int result;
			string s = Regex.Replace(this.Text, @"[^0-9]+", "", RegexOptions.Compiled);
			int.TryParse(s, out result);
			//result = Math.Round(result);
			result = Math.Max(result - indecValue, 1);
			this.Text = result.ToString();
		}
	}
}
