using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskTracking3.Entities
{
    public partial class Employee
    {
        public Employee()
        {
            Tasks = new HashSet<Task>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public int Age { get; set; }
        public string? PhotoPath { get; set; }
        public int? DepartmentId { get; set; }

        [NotMapped]
        public string Info
        {
            get => $"{FirstName} {LastName} {Age}";
        }

        public virtual Department? Department { get; set; }

        public virtual ICollection<Task> Tasks { get; set; }
    }
}
