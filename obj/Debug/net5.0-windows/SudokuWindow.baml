   M S B A M L   `   `   `  ����A  =Sudoku, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:SudokuSudoku  5   	       Sudoku.SudokuWindow   5   	   \ XPresentationFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   &localclr-namespace:Sudoku     5   	   -   5          � Title$  Sudoku Solver��5   	   $	��450��6   $	��750��6,   .��5
      � -   5        $��RootGrid��5
       � ColumnDefinitions 5   
   �� 5      $��10 �6   �� 5      $	��150 �6   �� 5      $	��150 �6   �� 5      $��10 �6   �� 5      $��* �6   �� 5      $��10 �6    � RowDefinitions 5   
   �� 5      $��10 �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   �� 5      $��10 �6   .+�5   
   �� $��1��6   $��1��6!   $��2��61   $��3,0,0,3q�6E   $��
Algorithm:��6V   �� 5   
   -   5        $��AlgorithmCBox��5      $��1��6)   $��2��69   $��2��6F   $��0,0,0,3q�6Z   $��Center=�6k   .@�5      �� $��Brute force��6   �� 5       $��Optimised brute force��6   �� 5!      $��Pessimistic subdivision��6    �� 
IsSelected$
 True��6=   5"      �� 5#   
   -   5        $��LoadBtn��5#      $��	Load File��6    $��1��64   $��3��6D   $��0,3,0,3q�6g   �� 5$   
   -   5        $��SolveBtn��5$      $��Solve��6!   $��2��61   $��3��6A   $��0,3,0,3q�6e   �� 5%   
   -   5        $��PlaybackBtn��5%      $��Start Playback��6$   $��1��6=   $��4��6M   $��0,3,0,6q�6t   �� 5&   
   -   5        $��StopBtn��5&      $��Stop Playback��6    $��2��68   $��4��6H   $��0,3,0,6q�6U   6f   �� 5'   
   
���   �$��1q�6%   $��1��69   $��5��6I   $��2��6V   ._�5(      �� -   5        $��ConsoleScroll��5(      $
��Auto=�60   $
��Auto=�6S   $��Stretch=�6x   
���   �.��5)      �� -	   5        $��ConsoleText��5)      $��Consolas)�6`   $��10(�6v   
�������$	��Top=�6�   ��6/   ��  �� Path$  (local:Controller.LogText)�6�   5*      5+      �� 5,   
   -
   5        $��ClearBtn��5,      $��Clear��6!   $��1��61   $��6��6A   6N   �� 5-   
   -   5        $��SaveBtn��5-      $
��Save��6    $��2��6/   $��6��6?   6L   �� 5.   
   $��4��6   $��0��6'   $��8��64   $��2��6E   $��Visible=�6Y   $��Visible=�6   .��5/      � -   5        $��GameGrid��5/      $��10,10,10,10q�6#   6:   50      52      53      