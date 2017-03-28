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
            this.Text = this.Text.Replace(unitsStr, string.Empty);
            base.OnGotFocus(e);
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            double result;
            if (double.TryParse(this.Text, out result))
                this.Text = result.ToString() + unitsStr;
            else
                this.Undo();
            base.OnLostFocus(e);
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            e.Handled = (e.Key == Key.Space);

            if (e.Key == Key.Up)
            {
                double result;
                double.TryParse(this.Text, out result);
                result = Math.Round(result);
                result++;
                Text = result.ToString();
            }

            if (e.Key == Key.Down)
            {
                double result;
                double.TryParse(this.Text, out result);
                result = Math.Round(result);
                if (result > 0)
                    result--;
                Text = result.ToString();
            }
            base.OnPreviewKeyDown(e);
        }

    }
}
