   M S B A M L   `   `   `  ����H  DChatInterface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:ChatInterfaceChatInterface  5      ;"clr-namespace:ChatInterface.ModelsChatInterface.Models  5          ChatInterface.MainControl   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      -localclr-namespace:ChatInterface     5      5custom"clr-namespace:ChatInterface.Models     5      .��5
      �   � ColumnDefinitions  5   
   �� 5      6!   �� 5      6!   �� 5      6!   �� 5      6!   �� 5      6!    � RowDefinitions 5   
   �� 5      6   �� 5      6   �� 5      6   �� 5      6   �� 5      6   �� 5      6   �� 5      6   .+�5   
   �� -   5        $��Enter��5      $��2��6!   $��Center=�61   $��0,16,0,0q�6N   $��4��6`   $	��Top=�6m   $��26��6�   $��78��6�   6�   �� 5   
   -   5         ��Name$ 
LoginInput��5      $��2��6&   $��Center=�66   $��0,10,0,0q�6S   $��1��6e    �� TextWrapping$
 Wrap=�6s   $	��Top=�6�   $	��120��6�   $��25��6�   $��Center=�6�    �� TextAlignment$ Center=�6�   �� 5   
   -   5        $ 	PassInput��5      $��2��6%   $��Center=�65   $��0,10,0,0q�6R   $��2��6d   $
 Wrap=�6q   $	��Top=�6�   $	��120��6�   $��25��6�   $��Center=�6�   $ Center=�6�   �� 5   
   $��1��6   $
��Left=�6$   $��	80,15,0,0q�6?   $��1��6R   $��Login��6_   $
��Wrap=�6l   $	��Top=�6�   �� 5   
   $��1��6   $
��Left=�6$   $��	80,15,0,0q�6?   $��2��6R   $��Password��6_   $
��Wrap=�6o   $	��Top=�6�   �� 5    
   -   5        $��Registration��5       $��3��6(   $��Center=�68   $��4��6U   $��Center=�6b   $��26��6}   $��78��6�   6�   �� 5!   
   -   5        $ IpBox��5!      $��2��6"   $��Center=�63   $��0,10,0,0q�6P   $��3��6b   $
 Wrap=�6p   $	��Top=�6�   $	��120��6�   $��22��6�   $   ChatInterface.Models.IpTextBox  5"   
   -   5          Name$ 
CheckIpBox��5"         	TextCheck 60   ��  �� ElementName$ IpBox�� �� Path$
 Text�5#      �� 5$      $��1��6   $
��Left=�6(   $��	80,10,0,0q�6C   $��3��6V   $��	Server Ip��6c   $
��Wrap=�6t   $	��Top=�6�   $��22��6�   $��50��6�   5&      5'      