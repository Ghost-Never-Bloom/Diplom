using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using TaskTracking3.Entities;
using Microsoft.EntityFrameworkCore;
using TaskTracking3.Commands;
using TaskTracking3.Views;

namespace TaskTracking3.ViewModels
{
    public class EmployeeWindowViewModel : ViewModelBase
    {
        List<Employee>? _employees = null;
        private int _selectedIndex = -1;
        private int _employeeId;
        private string _firstName;
        private string _lastName;
        private string _age;
        private string _photoPath;
        private string _departmentId;

        public List<Employee> Employees
        {
            get => _employees;
            set => Set(ref _employees, value, nameof(Employees));
        }
        public int SelectedIndex
        {
            get => _selectedIndex;
            set => Set(ref _selectedIndex, value, nameof(SelectedIndex));
        }
        public string FirstName
        {
            get => _firstName;
            set => Set(ref _firstName, value, nameof(FirstName));
        }
        public string LastName
        {
            get => _lastName;
            set => Set(ref _lastName, value, nameof(LastName));
        }
        public string Age
        {
            get => _age;
            set => Set(ref _age, value, nameof(Age));
        }
        public string PhotoPath
        {
            get => _photoPath;
            set => Set(ref _photoPath, value, nameof(PhotoPath));
        }
        public string DepartmentId
        {
            get => _departmentId;
            set => Set(ref _departmentId, value, nameof(DepartmentId));
        }

        public ICommand AddEmployeeCommand => new Command(a =>
        {
            AddEmployee();
            LoadEmployees();
        });
        public ICommand SaveEmployeeCommand => new Command(a =>
        {
            SaveEmployee();
            LoadEmployees();
        });
        public ICommand DeleteEmployeeCommand => new Command(a =>
        {
            DeleteEmployee();
            LoadEmployees();
        });
        public ICommand EditEmployeeCommand => new Command(e =>
        {
            SetEmployee();
        });
        public ICommand BackCommand => new Command(a =>
        {
            OpenWindow(new MainWindow());
        });

        public EmployeeWindowViewModel()
        {
            LoadEmployees();
        }

        private void LoadEmployees()
        {
            using (var db = new ApplicationDbContext())
            {
                Employees = db.Employees.ToList();
            }
        }

        private void SetEmployee()
        {
            if (_selectedIndex < 0)
                return;

            _employeeId = _employees[SelectedIndex].Id;

            using (var db = new ApplicationDbContext())
            {
                var employee = db.Employees.Where(e => e.Id == _employeeId).SingleOrDefault();

                if (employee == null)
                    return;

                FirstName = employee.FirstName;
                LastName = employee.LastName;
                Age = Convert.ToString(employee.Age);
                PhotoPath = employee.PhotoPath;
                DepartmentId = Convert.ToString(employee.DepartmentId);

                db.SaveChanges();
            }
        }

        private void AddEmployee()
        {
            if (FirstName == "" || LastName == "" || Age == "" || PhotoPath == "" || DepartmentId == "")
            {
                MessageBox.Show("Please type info.");
                return;
            }

            Employee employee = new Employee();

            employee.FirstName = FirstName;
            employee.LastName = LastName;
            employee.PhotoPath = PhotoPath;
            employee.Age = Convert.ToInt32(Age);
            employee.DepartmentId = Convert.ToInt32(DepartmentId);

            using (var db = new ApplicationDbContext())
            {
                db.Employees.Add(employee);
                db.SaveChanges();
            }
        }

        private void SaveEmployee()
        {
            if (_employeeId < 0)
                return;

            using (var db = new ApplicationDbContext())
            {
                var employee = db.Employees.Where(t => t.Id == _employeeId).SingleOrDefault();

                if (employee == null)
                    return;

                employee.FirstName = FirstName;
                employee.LastName = LastName;
                employee.PhotoPath = PhotoPath;
                employee.Age = Convert.ToInt32(Age);
                employee.DepartmentId = Convert.ToInt32(DepartmentId);

                db.SaveChanges();
            }

            FirstName = "";
            LastName = "";
            PhotoPath = "";
            Age = "";
            DepartmentId = "";
            _employeeId = -1;
        }
        
        private void DeleteEmployee()
        {
            if (_selectedIndex < 0)
                return;

            int employeeId = _employees[SelectedIndex].Id;

            using (var db = new ApplicationDbContext())
            {
                var employee = db.Employees.Include(t => t.Tasks).Where(t => t.Id == employeeId).SingleOrDefault();

                if (employee == null || employee.Tasks.Count > 0)
                    return;

                db.Employees.Remove(employee);
                db.SaveChanges();
            }
        }

        private void OpenWindow(Window window)
        {
            Window oldWindow = Application.Current.MainWindow;

            window.Show();
            Application.Current.MainWindow = window;
            oldWindow.Close();
        }
    }
}
