using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace App26.CustomControls
{
    public class ValidTextBox : TextBox, INotifyPropertyChanged
    {
        public ValidTextBox()
        {
            ValidateText();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        protected bool SetField<T>(ref T field, T value, string propertyName)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        public static readonly DependencyProperty patternProperty =
            DependencyProperty.Register("pattern", typeof(string), typeof(ValidTextBox), new PropertyMetadata(string.Empty));

        public string RegexPattern
        {
            get
            {
                return (string)GetValue(patternProperty);
            }
            set
            {
                SetValue(patternProperty, value);
            }
        }

        internal void ValidateText()
        {
            this.TextChanged += ValidTextBox_TextChanged;
        }

        private void ValidTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.RegexPattern))
                return;

            SetStyle(CheckInDefaultValues());
        }


        private void SetStyle(bool value)
        {
            if (value)
            {
                if (Regex.IsMatch(this.Text, GetDefaultRegexValue()))
                {
                    this.BorderBrush = new SolidColorBrush(Colors.Purple);
                }
                else
                {
                    this.BorderBrush = new SolidColorBrush(Colors.Red);
                }
            }
            else
            {
                if (Regex.IsMatch(this.Text, this.RegexPattern))
                {
                    this.BorderBrush = new SolidColorBrush(Colors.Purple);
                }
                else
                {
                    this.BorderBrush = new SolidColorBrush(Colors.Red);
                }
            }
        }

        private bool CheckInDefaultValues()
        {
            if (!string.IsNullOrEmpty(DefaultRegexList.Where(x => x.Key == this.RegexPattern).Select(z => z.Value).FirstOrDefault()))
                return true;
            return false;
        }

        private string GetDefaultRegexValue()
        {
            return DefaultRegexList.Where(y => y.Key == this.RegexPattern).Select(s => s.Value).FirstOrDefault();
        }

        private static IDictionary<string, string> DefaultRegexList
        {
            get
            {
                return new Dictionary<string, string>()
                {
                    { "EMail", @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$" },
                    { "PhoneNumber", @"\(?\d{3}\)?-? *\d{3}-? *-?\d{4}" },
                    { "Address", @"([\d|A-Z]*) ([A-Z|a-z| ]*) ([A-Z|a-z]*)" },
                    { "Url", @"^http(s)?://([\w-]+.)+[\w-]+(/[\w- ./?%&=])?$" },
                    { "Hex", @"/^#?([a-f0-9]{6}|[a-f0-9]{3})$/" },
                    { "IPAddress", @"/^(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$/" },
                    { "HTML", @"/^<([a-z]+)([^<]+)*(?:>(.*)<\/\1>|\s+\/>)$/" }
                };
            }
        }
    }
}
