using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChatInterface.Interface
{
    /// <summary>
    /// Interaction logic for IpControl.xaml
    /// </summary>
    public partial class IpControl : UserControl
    {
        public IpControl()
        {
            InitializeComponent();
        }
        static Regex ip = new Regex(@"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b");

        public static readonly DependencyProperty TextProperty;

        static IpControl()
        {
            TextProperty = DependencyProperty.Register(
                        "TextCheck",
                        typeof(string),
                        typeof(IpControl),
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
