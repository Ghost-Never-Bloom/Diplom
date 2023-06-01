using System;
using System.Collections.Generic;

namespace TaskTracking3.Entities
{
    public partial class Department
    {
        public Department()
        {
            Employees = new HashSet<Employee>();
            Tasks = new HashSet<Task>();
        }

        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public int PersonalNumber { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }
        public virtual ICollection<Task> Tasks { get; set; }
    }
}
