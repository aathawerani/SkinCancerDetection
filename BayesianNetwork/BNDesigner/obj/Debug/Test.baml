   M S B A M L   `   `   `  ����A  =IBAyes, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:DiagramDesignerDiagramDesigner  5   	   C&clr-namespace:DiagramDesigner.ControlsDiagramDesigner.Controls  5   	       DiagramDesigner.Test   5   	   \ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      8x,http://schemas.microsoft.com/winfx/2006/xaml    5      )sclr-namespace:DiagramDesigner    5   	   2c&clr-namespace:DiagramDesigner.Controls    5   	   -   5          � Icon$  
ibayes.ico��5   	    � Title$ IBAyes��5   	    � WindowState$ 	Maximized=�6    � WindowStartupLocation$ CenterScreen=�60    � 	Resources 5
         mnuMainMenu m� IsCheckable � ToolTip��%�  &	        o� 5   
   $��30��6#   $��10, 10, 5, 5q�6.   $
��Left=�6D   
�������.@�5      m� $��_File��6   .@�5      m� $
��_New��6    . 6)   m� 5      $��_Open��6    . 6*   m� 5      $��_Close��6    . 6+   �� 5      m� 5      $��Menu with Submenu��6   .@�5      m� $��_submenuitem1��6    . 66   m� 5      $��_submenuitem2��6    . .@�5      m� $��_submenuitem2.1��6#    . 6<   5      5      �� 5      m� 5      $��_Menu item with ToolTip��6    5      _� 5      .��6"   ToolTip Information.5      5      5      5       5!      .��5#      �  � RowDefinitions 5$   
   �� 5%      $
��Auto �6   �� 5&      $��* �6   .+�5*   
   ��   	MyToolbar#��[ 6   � 5-   
   -   5        $��grdDesigner��5-      $��1��6    � ColumnDefinitions 5.      �� 5/      $��1* �6#   60   �� 50      $��8* �6#   60   .+�54      �� $��Explorer��6   $��1,0,0,1q�6)   .��55      �� -   5        $��lstNodes��55      $��1q�6W   
�������56      57      � 5:      $��1��6   $��1��6    .+�5B      �� $��Bayesian Network��6   $��2,0,0,0q�65   .��5E      �  5F      �� 5G      $��* �6,   69   �� 5H      $��30 �6,   6:   .+�5K      �� $
��Auto=�6'   $
��Auto=�5L      .��5M      $   DiagramDesigner.DesignerCanvas  -   5        	  Name$	 
MyDesigner��5M   @   |�.   WindowBackgroundBrush#��[ 5N   !   $��10q�5O   !   
 � FocusVisualStyle
 6-   Z�  � LayoutTransform 5Q   "   �� 5R   &    �� ScaleX 65   ��  �� Path$ Value� �� ElementName$ 
zoomSlider�� �� ScaleY 5S      �� $ Value�$ 
zoomSlider��5U      5W      �� 5X      $��1��6!   
�������$��1q�6A   $	��170��6U   $��Right=�6a   ._�5Y      ��  �� Orientation$ 
Horizontal=�6)   .+�5[      �� -   5         ��Name$ 
zoomSlider��5[   %   $	��160��69   $��Center=�6E    � Minimum$	 0.1Q�6b    � Maximum$ 2Q�6p    �� TickPlacement$
 Both=�6|    � Value$ 1Q�6�   $��30��6�   5\   #   5]      5^      5a      5b      5c      �� 5d   
   |�.  $��2��6)   
�������$��Stretch=�5e      $��Right=�61   5|      5~      