using System;
using System.Collections.Generic;

namespace TaskTracking3.Entities
{
    public partial class TaskCategory
    {
        public TaskCategory()
        {
            Tasks = new HashSet<Task>();
        }

        public int Id { get; set; }
        public string Title { get; set; } = null!;

        public virtual ICollection<Task> Tasks { get; set; }
    }
}
