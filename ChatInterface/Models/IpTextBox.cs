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
        //регулярка для проверки формата айпи
        static Regex ip = new Regex(@"^[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}$");

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
                //если есть совпадения правильности формата , то все ок. Реальность существаовния айпи пока нет нужды проверять
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