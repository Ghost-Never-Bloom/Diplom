using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using TaskTracking3.Views;
using TaskTracking3.Commands;

namespace TaskTracking3.ViewModels
{
    public class LoginWindowViewModel : ViewModelBase
    {
        private string _username;
        private string _password;

        public string Username
        {
            get => _username;
            set => Set(ref _username, value, nameof(Username));
        }
        public string Password
        {
            get => _password;
            set => Set(ref _password, value, nameof(Password));
        }

        public ICommand LoginButton => new Command(l =>
        {
            if (IsAdmin() || IsManager())
                OpenMainWindow();
            else
                MessageBox.Show("Incorrect input. Try again.");
        });

        private bool IsAdmin() => (Username == "Admin" && Password == "Admin") ? true : false;
        private bool IsManager() => (Username == "Manager" && Password == "Manager") ? true : false;

        private void OpenMainWindow()
        {
            MainWindow main = new MainWindow();
            Window window = Application.Current.MainWindow;

            main.Show();
            Application.Current.MainWindow = main;
            window.Close();
        }
    }
}
