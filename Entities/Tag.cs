using System;
using System.Collections.Generic;

namespace TaskTracking3.Entities
{
    public partial class Tag
    {
        public Tag()
        {
            Tasks = new HashSet<Task>();
        }

        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string BacklightColor { get; set; } = null!;

        public virtual ICollection<Task> Tasks { get; set; }
    }
}
