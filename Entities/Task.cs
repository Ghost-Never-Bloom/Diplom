using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TaskTracking3.Entities
{
    public partial class Task
    {
        public Task()
        {
            Employees = new HashSet<Employee>();
            Tags = new HashSet<Tag>();
        }

        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime? StartWork { get; set; }
        public DateTime? EndWork { get; set; }
        public int? TaskCategoryId { get; set; }
        public int? DepartmentId { get; set; }

        [NotMapped]
        public string StartDate
        {
            get => (StartWork != null) ? StartWork.Value.ToShortDateString() : string.Empty;
            //get => (StartWork == null) ? string.Empty : StartWork.GetValueOrDefault().Date.ToString("dd/MM/yyyy");
        }

        [NotMapped]
        public string EndDate
        {
            get => (EndWork != null) ? EndWork.Value.ToShortDateString() : string.Empty;
            //get => (EndWork == null) ? string.Empty : EndWork.GetValueOrDefault().Date.ToString("dd/MM/yyyy");
        }

        [NotMapped]
        public string EmployeesInTask
        {
            get
            {
                if (Employees.Count == 0)
                    return "None";

                StringBuilder fmt = new StringBuilder();

                foreach (var employee in Employees)
                    fmt.Append($"{employee.Id}, ");

                fmt.Remove(fmt.Length - 2, 2);

                return fmt.ToString();
            }
        }

        [NotMapped]
        public string TaskTags
        {
            get
            {
                if (Tags.Count == 0)
                    return "None";

                StringBuilder fmt = new StringBuilder();

                foreach (var tag in Tags)
                    fmt.Append($"{tag.Title}, ");

                fmt.Remove(fmt.Length - 2, 2);

                return fmt.ToString();
            }
        }

        public virtual Department? Department { get; set; }
        public virtual TaskCategory? TaskCategory { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
    }
}
