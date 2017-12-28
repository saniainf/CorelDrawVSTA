using System;
using System.Windows;
using System.Windows.Controls;
using System.Text.RegularExpressions;

namespace InfTrimMarks
{
	/// <summary>
	/// Логика взаимодействия для NumericUpDown.xaml
	/// </summary>
	public partial class NumericUpDown : UserControl
	{
		private const string unitsStr = " mm";

		public NumericUpDown()
		{
			InitializeComponent();
		}

		public double Value
		{
			get
			{
				double result;
				string s = Regex.Replace(numericTextBox.Text, @"[^0-9]+", "", RegexOptions.Compiled);
				if (double.TryParse(s, out result))
					return result;
				else
					return 0f;
			}
			set { numericTextBox.Text = value.ToString() + unitsStr; }
		}

		protected virtual void OnValueChanged(RoutedPropertyChangedEventArgs<decimal> args)
		{
			RaiseEvent(args);
		}

		private void upButton_Click(object sender, EventArgs e)
		{
			numericTextBox.increase();
		}

		private void downButton_Click(object sender, EventArgs e)
		{
			numericTextBox.decrease();
		}
	}
}
