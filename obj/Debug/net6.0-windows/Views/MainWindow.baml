   M S B A M L   `   `   `  ����A  =Diplom, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:TaskTracking3.ViewsTaskTracking3.Views  5   	   C&clr-namespace:TaskTracking3.ViewModelsTaskTracking3.ViewModels  5   	   $    TaskTracking3.Views.MainWindow   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   3local!clr-namespace:TaskTracking3.Views     5   	   5vm&clr-namespace:TaskTracking3.ViewModels     5   	     � Title$  Главное окно��5	   	   $	��450��6   $	��800��6+    � DataContext 5
      2   ,TaskTracking3.ViewModels.MainWindowViewModel  5   
   .��5      �  � RowDefinitions 5   
   �� 5      $��0.17* �6   �� 5      .+�5   
   �� $��0��5       �� Orientation$ 
Horizontal=�5      $��0, 10, 0, 10q�5      $��Center=�5      .+�5      �� $	��120��5      $-��'Редактировать отделы��5      $��0, 0, 10, 0q�5      ��5      �� *EditDepartmentsCommand+�� 5      $	��120��5      $��Сотрудники��5      $��0, 0, 10, 0q�5       ��5!      �� *EditEmployeesCommand+�� 5"      $	��120��5#      $��Тэги��5$      $��0, 0, 10, 0q�5%      ��5&      �� *EditTagsCommand+�� 5'      $	��120��5(      $#��Добавить задачу��5)      $��0, 0, 10, 0q�5*      ��5+      �� *AddTaskCommand+�� 5,      $	��120��5-      $!��Удалить задачу��5.      $��0, 0, 10, 0q�5/      ��50      �� *DeleteTaskCommand+�� 51      $	��120��52      $��
0, 0, 0, 0q�53      $��
Выйти��54      ��55      �� *ExitCommand+56      �� 58   
   $��1��59      $��Stretch=�5:      $��Hidden=�5;      ��5<      �� *Tasks+ �� UpdateSourceTrigger$ PropertyChanged=� �� SelectedItem 5=      �� *SelectedItem+$ PropertyChanged=� � ContextMenu 5>      �� 5?      .@�5@      m� -   5        $��EditTask��5A      $��	Edit Task��5B       m� Command 5C      �� *EditTaskCommand+m� 5D      -   5        $��
DeleteTask��5E      $��Delete Task��5F       5G      �� *DeleteTaskCommand+5H      ��5J      �� 5K      .R�5L      �� 
���   �$��1q�5N      ._�5O      � $��80��5P   !    5Q      �� 5R   "   �� 5S   "   �� 5T   "   �� 5U   "   .+�5X      �� $��0��5Y   %   $ 
Horizontal=�5Z   %   .+�5[   "   �� $��
5, 0, 0, 0q�5\   )   $��Название:��5]   )   �� 5^   "   $��5, 0, 10, 0q�5_   )   ��5`   )   �� *Title+�� 5b   "   $��
5, 0, 0, 0q�5c   )   $��Отделы:��5d   )   �� 5e   "   $��
5, 0, 0, 0q�5f   )   ��5g   )   �� *Department.Title+5h      �� 5j      $��1��5k   %   $ 
Horizontal=�5l   %   .+�5m   "   �� $��
5, 0, 0, 0q�5n   )   $'��!Дата начала работ:��5o   )   �� 5p   "   $��5, 0, 10, 0q�5q   )   ��5r   )   �� *	StartDate+�� 5t   "   $��
5, 0, 0, 0q�5u   )   $-��'Дата окончания работ:��5v   )   �� 5w   "   $��5, 0, 10, 0q�5x   )   ��5y   )   �� *	EndDate+5z      �� 5|      $��2��5}   )   $ 
Horizontal=�5~   )   .+�5   "   �� $��
5, 0, 0, 0q�5�   )   $��Сотрудники:��5�   )   �� 5�   "   $��
5, 0, 0, 0q�5�   )   ��5�   )   �� *EmployeesInTask+5�      �� 5�      $��3��5�   %   $ 
Horizontal=�5�   %   .+�5�   "   �� $��
5, 0, 0, 0q�5�   )   $��	Тэги:��5�   )   �� 5�   "   $��
5, 0, 0, 0q�5�   )   ��5�   )   �� *
TaskTags+5�      5�      5�      5�      5�      5�      5�      