using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Net.Sockets;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;

namespace ChatInterface
{
    /// <summary>
    /// Interaction logic for RegControl.xaml
    /// </summary>
    public partial class RegControl : UserControl
    {
      
        public RegControl()
        {
            InitializeComponent();
        }

        private void RegClick(object sender, RoutedEventArgs e)
        {
            this.Content = new MainControl();
        }

       
    }
}
