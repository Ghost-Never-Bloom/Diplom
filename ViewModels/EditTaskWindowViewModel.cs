using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TaskTracking3.Commands;
using TaskTracking3.Entities;
using TaskTracking3.Views;

namespace TaskTracking3.ViewModels
{
    public class EditTaskWindowViewModel : ViewModelBase
    {
        private int _taskId = -1;
        public bool IsChanged = false;
        private string _title;
        private string _description;
        private string _startWork;
        private string _endWork;

        private string _taskCategorySelectedItem;
        private string _departmentSelectedItem;

        private List<ListBoxItem> _tagList;
        private List<ListBoxItem> _employeeList;

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
        public string StartWork
        {
            get => _startWork;
            set => Set(ref _startWork, value, nameof(StartWork));
        }
        public string EndWork
        {
            get => _endWork;
            set => Set(ref _endWork, value, nameof(EndWork));
        }
        
        public string TaskCategorySelectedItem
        {
            get => _taskCategorySelectedItem;
            set => Set(ref _taskCategorySelectedItem, value, nameof(TaskCategorySelectedItem));
        }
        public string DepartmentSelectedItem
        {
            get => _departmentSelectedItem;
            set => Set(ref _departmentSelectedItem, value, nameof(DepartmentSelectedItem));
        }

        public List<string> TaskCategoryList { get; set; }
        public List<string> DepartmentList { get; set; }

        public List<ListBoxItem> TagList
        {
            get => _tagList;
            set => Set(ref _tagList, value, nameof(TagList));
        }
        public List<ListBoxItem> EmployeeList
        {
            get => _employeeList;
            set => Set(ref _employeeList, value, nameof(EmployeeList));
        }

        public ICommand AddCommand => new Command(a =>
        {
            AddTask(GetSelectedTagIndexes(), GetSelectedEmployeeIndexes());
            IsChanged = true;
        });
        public ICommand SaveCommand => new Command(s =>
        {
            SaveTask(GetSelectedTagIndexes(), GetSelectedEmployeeIndexes());
            IsChanged = true;
        });

        public EditTaskWindowViewModel()
        {
            TaskCategoryList = GetTaskCategoryList();
            DepartmentList = GetDepartmentList();
        }

        public void SetTask(Task? task)
        {
            if(task == null)
            {
                TagList = GetTagList();
                EmployeeList = GetEmployeeList();
                return;
            }

            Title = task.Title;
            Description = task.Description;
            StartWork = task.StartWork.Value.ToShortDateString();
            EndWork = task.EndWork.Value.ToShortDateString();
            TaskCategorySelectedItem = task.TaskCategory.Title;
            DepartmentSelectedItem = task.Department.Title;

            _taskId = task.Id;
            TagList = GetTagList(_taskId);
            EmployeeList = GetEmployeeList(_taskId);
        }

        private List<string> GetTaskCategoryList()
        {
            List<string> taskCategories;

            using(var db = new ApplicationDbContext())
            {
                taskCategories = db.TaskCategories.Select(t => t.Title).ToList();
            }

            return taskCategories;
        }
        private List<string> GetDepartmentList()
        {
            List<string> departments;

            using (var db = new ApplicationDbContext())
            {
                departments = db.Departments.Select(t => t.Title).ToList();
            }

            return departments;
        }

        private List<ListBoxItem> GetTagList()
        {
            List<ListBoxItem> list = new List<ListBoxItem>();

            using (var db = new ApplicationDbContext())
            {
                var tags = db.Tags
                    .Include(s => s.Tasks)
                    .ToList();

                if (tags == null)
                    return list;

                foreach (var tag in tags)
                {
                    list.Add(new ListBoxItem()
                    {
                        Content = $"{tag.Id} {tag.Title}"
                    });
                }
            }

            return list;
        }
        private List<ListBoxItem> GetTagList(int taskId)
        {
            List<ListBoxItem> list = new List<ListBoxItem>();

            using (var db = new ApplicationDbContext())
            {
                var task = db.Tasks
                    .Where(t => t.Id == taskId)
                    .SingleOrDefault();
                var tags = db.Tags
                    .Include(s => s.Tasks)
                    .ToList();

                if (task == null || tags == null)
                    return list;

                foreach (var tag in tags)
                {
                    
                    list.Add(new ListBoxItem()
                    {
                        Content = $"{tag.Id} {tag.Title}",
                        IsSelected = (tag.Tasks.Contains(task)) ? true : false
                    });
                }
            }

            return list;
        }

        private List<ListBoxItem> GetEmployeeList()
        {
            List<ListBoxItem> list = new List<ListBoxItem>();

            using (var db = new ApplicationDbContext())
            {
                var employees = db.Employees
                    .Include(s => s.Tasks)
                    .ToList();

                if (employees == null)
                    return list;

                foreach (var employee in employees)
                {
                    list.Add(new ListBoxItem()
                    {
                        Content = $"{employee.Id} {employee.Info}",
                    });
                }
            }

            return list;
        }
        private List<ListBoxItem> GetEmployeeList(int taskId)
        {
            List<ListBoxItem> list = new List<ListBoxItem>();

            using (var db = new ApplicationDbContext())
            {
                var task = db.Tasks
                    .Where(t => t.Id == taskId)
                    .SingleOrDefault();
                var employees = db.Employees
                    .Include(s => s.Tasks)
                    .ToList();

                if (task == null || employees == null)
                    return list;

                foreach (var employee in employees)
                {
                    list.Add(new ListBoxItem()
                    {
                        Content = $"{employee.Id} {employee.Info}",
                        IsSelected = (employee.Tasks.Contains(task)) ? true : false
                    });
                }
            }

            return list;
        }

        private List<int> GetSelectedTagIndexes()
        {
            List<int> list = new List<int>();

            foreach (var tag in TagList)
            {
                if (tag.IsSelected == true)
                {
                    int TagIndex = Convert.ToInt32(tag.Content.ToString().Split(' ').First());
                    list.Add(TagIndex);
                }
            }

            return list;
        }
        private List<int> GetSelectedEmployeeIndexes()
        {
            List<int> list = new List<int>();

            foreach (var employee in EmployeeList)
            {
                if (employee.IsSelected == true)
                {
                    int EmployeeIndex = Convert.ToInt32(employee.Content.ToString().Split(' ').First());
                    list.Add(EmployeeIndex);
                }
            }

            return list;
        }

        private void SaveTask(List<int> selectedTagIndexes, List<int> selectedEmployeeIndexes)
        {
            using (var db = new ApplicationDbContext())
            {
                var task = db.Tasks
                    .Include(t => t.Tags)
                    .Include(t => t.Employees)
                    .Where(s => s.Id == _taskId)
                    .SingleOrDefault();

                if (task == null)
                    return;

                // tags
                var SelectedTags = db.Tags
                    .Include(s => s.Tasks)
                    .Where(t => selectedTagIndexes.Contains(t.Id))
                    .ToList();
                var UnSelectedTags = db.Tags
                    .Include(s => s.Tasks)
                    .Where(t => !selectedTagIndexes.Contains(t.Id) && t.Tasks.Contains(task))
                    .ToList();

                SelectedTags.ForEach(tag =>
                {
                    if (!tag.Tasks.Contains(task) && !task.Tags.Contains(tag))
                    {
                        tag.Tasks.Add(task);
                        task.Tags.Add(tag);

                        db.Tags.Update(tag);
                        db.Tasks.Update(task);
                    }
                });

                UnSelectedTags.ForEach(tag =>
                {
                    tag.Tasks.Remove(task);
                    task.Tags.Remove(tag);

                    db.Tags.Update(tag);
                    db.Tasks.Update(task);
                });

                // employees
                var SelectedEmployees = db.Employees
                    .Include(e => e.Tasks)
                    .Where(e => selectedEmployeeIndexes.Contains(e.Id))
                    .ToList();
                var UnSelectedEmployees = db.Employees
                    .Include(e => e.Tasks)
                    .Where(e => !selectedEmployeeIndexes.Contains(e.Id) && e.Tasks.Contains(task))
                    .ToList();

                SelectedEmployees.ForEach(employee =>
                {
                    if (!employee.Tasks.Contains(task) && !task.Employees.Contains(employee))
                    {
                        employee.Tasks.Add(task);
                        task.Employees.Add(employee);

                        db.Employees.Update(employee);
                        db.Tasks.Update(task);
                    }
                });

                UnSelectedEmployees.ForEach(employee =>
                {
                    employee.Tasks.Remove(task);
                    task.Employees.Remove(employee);

                    db.Employees.Update(employee);
                    db.Tasks.Update(task);
                });

                // fields
                task.Title = Title;
                task.Description = Description;
                task.StartWork = Convert.ToDateTime(StartWork);
                task.EndWork = Convert.ToDateTime(EndWork);
                task.TaskCategoryId = GetTaskCategoryId(TaskCategorySelectedItem);
                task.DepartmentId = GetDepartmentId(DepartmentSelectedItem);

                db.Tasks.Update(task);
                db.SaveChanges();
            }
        }
        private void AddTask(List<int> selectedTagIndexes, List<int> selectedEmployeeIndexes)
        {
            Task task = new Task();

            // fields
            task.Title = Title;
            task.Description = Description;
            task.StartWork = Convert.ToDateTime(StartWork);
            task.EndWork = Convert.ToDateTime(EndWork);
            task.TaskCategoryId = GetTaskCategoryId(TaskCategorySelectedItem);
            task.DepartmentId = GetDepartmentId(DepartmentSelectedItem);

            using(var db = new ApplicationDbContext())
            {
                db.Tasks.Add(task);
                db.SaveChanges();

                // tags
                var SelectedTags = db.Tags
                    .Include(s => s.Tasks)
                    .Where(t => selectedTagIndexes.Contains(t.Id))
                    .ToList();

                SelectedTags.ForEach(tag =>
                {
                    tag.Tasks.Add(task);
                    task.Tags.Add(tag);

                    db.Tags.Update(tag);
                    db.Tasks.Update(task);
                });

                // employees
                var SelectedEmployees = db.Employees
                    .Include(e => e.Tasks)
                    .Where(e => selectedEmployeeIndexes.Contains(e.Id))
                    .ToList();

                SelectedEmployees.ForEach(employee =>
                {
                    employee.Tasks.Add(task);
                    task.Employees.Add(employee);

                    db.Employees.Update(employee);
                    db.Tasks.Update(task);
                });

                db.Tasks.Update(task);
                db.SaveChanges();
            }
        }

        private int GetTaskCategoryId(string title)
        {
            int id = -1;

            using(var db = new ApplicationDbContext())
            {
                var tc = db.TaskCategories.AsNoTracking().Where(t => t.Title == title).SingleOrDefault();

                if (tc == null)
                    return id;

                id = tc.Id;
            }

            return id;
        }
        private int GetDepartmentId(string title)
        {
            int id = -1;

            using (var db = new ApplicationDbContext())
            {
                var dp = db.Departments.AsNoTracking().Where(t => t.Title == title).SingleOrDefault();

                if (dp == null)
                    return id;

                id = dp.Id;
            }

            return id;
        }

        //private void ChangeTaskCategory(int taskId, string categoryTitle)
        //{
        //    using (var db = new ApplicationDbContext())
        //    {
        //        var task = db.Tasks.Include(c => c.TaskCategory).Where(t => t.Id == taskId).SingleOrDefault();
        //        var category = db.TaskCategories.Include(c => c.Tasks).Where(c => c.Title == categoryTitle).SingleOrDefault();

        //        if (task == null || category == null)
        //            return;

        //        var oldCategory = db.TaskCategories.Include(c => c.Tasks).Where(c => c.Title == task.TaskCategory.Title).SingleOrDefault();

        //        if (oldCategory == null)
        //            return;

        //        if (category.Tasks.Contains(task))
        //            return;

        //        task.TaskCategory = category;
        //        category.Tasks.Add(task);

        //        if (oldCategory.Tasks.Contains(task))
        //            oldCategory.Tasks.Remove(task);

        //        db.Tasks.Update(task);
        //        db.TaskCategories.Update(category);
        //        db.TaskCategories.Update(oldCategory);
        //        db.SaveChanges();
        //    }
        //}
    }
}
