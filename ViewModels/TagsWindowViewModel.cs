using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TaskTracking3.Commands;
using TaskTracking3.Entities;
using TaskTracking3.Views;

namespace TaskTracking3.ViewModels
{
    public class TagsWindowViewModel : ViewModelBase
    {
        List<Tag>? _tags = null;
        private int _selectedIndex = -1;
        private int _tagId;
        private string _title;
        private string _color;

        public List<Tag> Tags
        {
            get => _tags;
            set => Set(ref _tags, value, nameof(Tags));
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
        public string Color
        {
            get => _color;
            set => Set(ref _color, value, nameof(Color));
        }

        public ICommand AddTagCommand => new Command(a =>
        {
            AddTag();
            LoadTags();
        });
        public ICommand SaveTagCommand => new Command(a =>
        {
            SaveTag();
            LoadTags();
        });
        public ICommand DeleteTagCommand => new Command(a =>
        {
            DeleteTag();
            LoadTags();
        });
        public ICommand EditTagCommand => new Command(e =>
        {
            SetTag();
        });
        public ICommand BackCommand => new Command(a =>
        {
            OpenWindow(new MainWindow());
        });

        public TagsWindowViewModel()
        {
            LoadTags();
        }

        private void LoadTags()
        {
            using (var db = new ApplicationDbContext())
            {
                Tags = db.Tags.ToList();
            }
        }

        private void SetTag()
        {
            if (_selectedIndex < 0)
                return;

            _tagId = _tags[SelectedIndex].Id;

            using (var db = new ApplicationDbContext())
            {
                var tag = db.Tags.Where(t => t.Id == _tagId).SingleOrDefault();

                if (tag == null)
                    return;

                Title = tag.Title;
                Color = tag.BacklightColor;

                db.SaveChanges();
            }
        }

        private void AddTag()
        {
            if (Title == "" || Color == "")
            {
                MessageBox.Show("Please type info.");
                return;
            }

            Tag tag = new Tag();

            tag.Title = Title;
            tag.BacklightColor = Color;

            using(var db = new ApplicationDbContext())
            {
                db.Tags.Add(tag);
                db.SaveChanges();
            }
        }

        private void SaveTag()
        {
            if (_tagId < 0)
                return;

            using (var db = new ApplicationDbContext())
            {
                var tag = db.Tags.Where(t => t.Id == _tagId).SingleOrDefault();

                if (tag == null)
                    return;

                tag.Title = Title;
                tag.BacklightColor = Color;

                db.SaveChanges();
            }

            Title = "";
            Color = "";
            _tagId = -1;
        }

        private void DeleteTag()
        {
            if (_selectedIndex < 0)
                return;

            int tagId = _tags[SelectedIndex].Id;

            using (var db = new ApplicationDbContext())
            {
                var tag = db.Tags.Include(t => t.Tasks).Where(t => t.Id == tagId).SingleOrDefault();

                if (tag == null || tag.Tasks.Count > 0)
                    return;

                db.Tags.Remove(tag);
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
