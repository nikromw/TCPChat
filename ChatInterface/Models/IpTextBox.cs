using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace ChatInterface.Models
{
    public class IpTextBox : TextBox
    {

        static Regex ip = new Regex(@"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b");

        public static readonly DependencyProperty TextProperty;

        static IpTextBox()
        {
            TextProperty = DependencyProperty.Register(
                        "TextCheck",
                        typeof(string),
                        typeof(IpTextBox),
                        new FrameworkPropertyMetadata(),
                        CheckText);
        }
        public string TextCheck
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }


        private static bool CheckText(object value)
        {
            string str = (string)value;
            MatchCollection result = ip.Matches(str);
            if (result.Count != 0)
            {
                ClientConnection.registered = "registered";
                return true;
            }
            else
            {
                ClientConnection.registered = "unregistered";
                return false;
            }

        }
    }


}