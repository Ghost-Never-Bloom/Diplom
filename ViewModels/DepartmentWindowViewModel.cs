using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using TaskTracking3.Entities;
using TaskTracking3.Commands;
using TaskTracking3.Views;
using Microsoft.EntityFrameworkCore;

namespace TaskTracking3.ViewModels
{
    public class DepartmentWindowViewModel : ViewModelBase
    {
        List<Department>? _departments = null;
        private int _selectedIndex = -1;
        private int _departmentId;
        private string _title;
        private string _description;
        private string _number;

        public List<Department>? Departments
        {
            get => _departments;
            set => Set(ref _departments, value, nameof(Departments));
        }
        public int SelectedIndex
        {
            get => _selectedIndex;
            set => Set(ref _selectedIndex, value, nameof(SelectedIndex));
        }
        public string Title
        {
            get => _title;
            set => Set(ref _title, value, nameof(Title));
        }
        public string Description
        {
            get => _description;
            set => Set(ref _description, value, nameof(Description));
        }
        public string Number
        {
            get => _number;
            set => Set(ref _number, value, nameof(Number));
        }

        public ICommand AddDepartmentCommand => new Command(a =>
        {
            AddDepartment();
            LoadDepartments();
        });
        public ICommand SaveDepartmentCommand => new Command(a =>
        {
            SaveDepartment();
            LoadDepartments();
        });
        public ICommand DeleteDepartmentCommand => new Command(a =>
        {
            DeleteDepartment();
            LoadDepartments();
        });
        public ICommand EditDepartmentCommand => new Command(e =>
        {
            SetDepartment();
        });
        public ICommand BackCommand => new Command(a =>
        {
            OpenWindow(new MainWindow());
        });

        public DepartmentWindowViewModel()
        {
            LoadDepartments();
        }

        private void LoadDepartments()
        {
            using (var db = new ApplicationDbContext())
            {
                Departments = db.Departments.ToList();
            }
        }

        private void SetDepartment()
        {
            if (_selectedIndex < 0)
                return;

            _departmentId = _departments[SelectedIndex].Id;

            using (var db = new ApplicationDbContext())
            {
                var department = db.Departments.Where(t => t.Id == _departmentId).SingleOrDefault();

                if (department == null)
                    return;

                Title = department.Title;
                Description = department.Description == null ? "" : department.Description;
                Number = Convert.ToString(department.PersonalNumber);

                db.SaveChanges();
            }
        }

        private void AddDepartment()
        {
            if (Title == "" || Description == "" || Number == "")
            {
                MessageBox.Show("Please type info.");
                return;
            }

            Department department = new Department();
            department.Title = Title;
            department.Description = Description;
            department.PersonalNumber = Convert.ToInt32(Number);

            using (var db = new ApplicationDbContext())
            {
                db.Departments.Add(department);
                db.SaveChanges();
            }
        }

        private void SaveDepartment()
        {
            if (_departmentId < 0)
                return;

            using (var db = new ApplicationDbContext())
            {
                var department = db.Departments.Where(t => t.Id == _departmentId).SingleOrDefault();

                if (department == null)
                    return;

                department.Title = Title;
                department.Description = Description;
                department.PersonalNumber = Convert.ToInt32(Number);

                db.SaveChanges();
            }

            Title = "";
            Description = "";
            Number = "";
            _departmentId = -1;
        }

        private void DeleteDepartment()
        {
            if (_selectedIndex < 0)
                return;

            int tagId = _departments[SelectedIndex].Id;

            using (var db = new ApplicationDbContext())
            {
                var department = db.Departments.Include(d => d.Tasks).Include(d => d.Employees).Where(d => d.Id == _departmentId).SingleOrDefault();

                if (department == null || department.Tasks.Count > 0 || department.Employees.Count > 0)
                    return;

                db.Departments.Remove(department);
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
