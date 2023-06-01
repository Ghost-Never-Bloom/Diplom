using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TaskTracking3.Entities;
using TaskTracking3.ViewModels;

namespace TaskTracking3.Views
{
    /// <summary>
    /// Interaction logic for EditTaskWindow.xaml
    /// </summary>
    public partial class EditTaskWindow : Window
    {
        private EditTaskWindowViewModel? _viewModel = null;

        public EditTaskWindow(string type, Task? task)
        {
            InitializeComponent();

            _viewModel = (EditTaskWindowViewModel)DataContext;

            Title = (type == "edit") ? "Edit Person" : "Add Person";
            AddButton.Visibility = (type == "edit") ? Visibility.Collapsed : Visibility.Visible;
            SaveButton.Visibility = (type == "edit") ? Visibility.Visible : Visibility.Collapsed;

            if (_viewModel != null)
                _viewModel.SetTask(task);
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = (_viewModel != null) ? _viewModel.IsChanged : true;
            this.Close();
        }
    }
}
