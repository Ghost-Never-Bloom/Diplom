using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using TaskTracking3.Commands;
using TaskTracking3.Entities;
using TaskTracking3.Views;

namespace TaskTracking3.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private List<Task> _tasks = null!;
        private Task _selectedItem;

        public List<Task> Tasks
        {
            get => _tasks;
            set => Set(ref _tasks, value, nameof(Tasks));
        }
        public Task SelectedItem
        {
            get => _selectedItem;
            set => Set(ref _selectedItem, value, nameof(SelectedItem));
        }

        public ICommand ExitCommand => new Command(e => Application.Current.MainWindow.Close());
        public ICommand AddTaskCommand => new Command(a =>
        {
            if (new EditTaskWindow("add", null).ShowDialog() == true)
                LoadTasks();
        });
        public ICommand EditTaskCommand => new Command(a =>
        {
            if (new EditTaskWindow("edit", _selectedItem).ShowDialog() == true)
                LoadTasks();
        });
        public ICommand DeleteTaskCommand => new Command(a =>
        {
            if (DeleteTask())
                LoadTasks();
        });

        public ICommand EditTagsCommand => new Command(e =>
        {
            OpenWindow(new TagsWindow());
        });
        public ICommand EditEmployeesCommand => new Command(e =>
        {
            OpenWindow(new EmployeeWindow());
        });
        public ICommand EditDepartmentsCommand => new Command(e =>
        {
            OpenWindow(new DepartmentWindow());
        });

        public MainWindowViewModel()
        {
            LoadTasks();
        }

        private void LoadTasks()
        {
            using (var db = new ApplicationDbContext())
            {
                Tasks = db.Tasks
                    .Include(c => c.TaskCategory)
                    .Include(d => d.Department)
                    .Include(e => e.Employees)
                    .Include(t => t.Tags)
                    .ToList();
            }
        }

        private bool DeleteTask()
        {
            using (var db = new ApplicationDbContext())
            {
                var task = db.Tasks.Include(t => t.Tags).Include(t => t.Employees).Where(t => t.Id == _selectedItem.Id).SingleOrDefault();

                if (task == null)
                    return false;

                if (task.Tags.Count > 0)
                {
                    var tags = db.Tags.Include(s => s.Tasks).Where(t => t.Tasks.Contains(task)).ToList();

                    if (tags == null || tags.Count == 0)
                        goto skipOne;

                    foreach (var tag in tags)
                        tag.Tasks.Remove(task);
                    task.Tags.Clear();
                }

                skipOne:
                if(task.Employees.Count > 0)
                {
                    var employees = db.Employees.Include(s => s.Tasks).Where(t => t.Tasks.Contains(task)).ToList();

                    if (employees == null || employees.Count == 0)
                        goto skipTwo;

                    foreach (var employee in employees)
                        employee.Tasks.Remove(task);
                    task.Tags.Clear();
                }

                skipTwo:
                db.Tasks.Remove(task);
                db.SaveChanges();
            }

            return true;
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
