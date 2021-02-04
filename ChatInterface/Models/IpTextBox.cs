using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace ChatInterface.Models
{
    public class IpTextBox : FrameworkElement
    {

        static Regex ip = new Regex(@"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b");

        public static  DependencyProperty TextProperty;

        static IpTextBox()
        {
            TextProperty = DependencyProperty.Register(
                        "TextCheck",
                        typeof(string),
                        typeof(IpTextBox),
                        new FrameworkPropertyMetadata());
        }
        public string TextCheck
        {
            get {
                string str = (string)GetValue(TextProperty);
                MatchCollection result = ip.Matches(str);
                if (result.Count != 0)
                {
                    ClientConnection.registered = "registered";
                    return "Succes login";
                }
                else
                {
                    return "Ip has wrong format.";
                }
            }
            set {
                    SetValue(TextProperty, value); 
            }
        }

    }


}