   M S B A M L   `   `   `  ����I  EIQD_UI_Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullG4clr-namespace:IQD_UI_Library;assembly=IQD_UI_LibraryIQD_UI_Library  5   	    	Interface   IQDHackathon.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   Flocal4clr-namespace:IQD_UI_Library;assembly=IQD_UI_Library     5   	   -   5          �Name$
  home��5       � Title$ 
MainWindow��5   	   $	��720��6   $
��1100��6)    � WindowStartupLocation$ CenterScreen=�5	   	    � WindowStyle$
 None=�6.   
������  � AllowsTransparency$
 True��5
   "    � 
ResizeMode$ NoResize=�5   	    � WindowState$ Normal=�6   $��Poppins)�64      AppFlowDirection#���   5   	   .��5      ��  �� CornerRadius$ 10��6     PrimaryBackgroundColor#���  6   ._�5   
   �  � RowDefinitions 5      �� 5      $
��auto �6    60   �� 5      $��* �6    6-   .+�5      � $��0��6   $��0��6#   .+�5      �� $��Right=�6   $	��Top=�69   $��0,5,10,0q�6Q   $��30��6c   	 �� Orientation$	 
Horizontal=�6o     SPAppFlowDirection#���  6�   .+�5      �� -   5        
 ��Name$
 btnClose��5        IconButtonsStyle#���  6/    	 close#���  6Z   
 � Tag$ IsCloseButton��6�   �� 5      -   5        $
 
btnRestore��5      $	��5 0q�61   ��5      �� 5       �� 
TargetType" ��6$    �� BasedOn# [ 6A   .�5   "   ��  �� Property � ��  maximize �� Value# �  6<    �� Triggers 5   "   �� 5   &    �� Binding 62   ��  �� ElementName$
 home�� �� Path$ WindowState� �� Value 	Maximized6i   .a�5   *   ��  � ��  restore# [ 6D   5    '   5"      5$      �� 5%      -   5        $
 btnMinimize��5%      #��[ 62     minimize#���  6\   6�   5&      5(      5.      5/      50      