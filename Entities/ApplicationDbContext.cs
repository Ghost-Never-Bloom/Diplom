using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TaskTracking3.Entities
{
    public partial class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Department> Departments { get; set; } = null!;
        public virtual DbSet<Employee> Employees { get; set; } = null!;
        public virtual DbSet<Tag> Tags { get; set; } = null!;
        public virtual DbSet<Task> Tasks { get; set; } = null!;
        public virtual DbSet<TaskCategory> TaskCategories { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=DESKTOP-BMT8L5J; Database=Diplom; TrustServerCertificate=Yes; Integrated Security=SSPI; Trusted_Connection=True;");
                optionsBuilder.EnableSensitiveDataLogging();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Department>(entity =>
            {
                entity.ToTable("Department");
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.ToTable("Employee");

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.DepartmentId)
                    .HasConstraintName("FK__Employee__Depart__44FF419A");
            });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.ToTable("Tag");
            });

            modelBuilder.Entity<Task>(entity =>
            {
                entity.ToTable("Task");

                entity.Property(e => e.EndWork).HasColumnType("date");

                entity.Property(e => e.StartWork).HasColumnType("date");

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.Tasks)
                    .HasForeignKey(d => d.DepartmentId)
                    .HasConstraintName("FK__Task__Department__3C69FB99");

                entity.HasOne(d => d.TaskCategory)
                    .WithMany(p => p.Tasks)
                    .HasForeignKey(d => d.TaskCategoryId)
                    .HasConstraintName("FK__Task__TaskCatego__3B75D760");

                entity.HasMany(d => d.Employees)
                    .WithMany(p => p.Tasks)
                    .UsingEntity<Dictionary<string, object>>(
                        "EmployeeTask",
                        l => l.HasOne<Employee>().WithMany().HasForeignKey("EmployeeId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__EmployeeT__Emplo__48CFD27E"),
                        r => r.HasOne<Task>().WithMany().HasForeignKey("TaskId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__EmployeeT__TaskI__47DBAE45"),
                        j =>
                        {
                            j.HasKey("TaskId", "EmployeeId").HasName("PK__Employee__7BC44D408B1ABF44");

                            j.ToTable("EmployeeTask");
                        });

                entity.HasMany(d => d.Tags)
                    .WithMany(p => p.Tasks)
                    .UsingEntity<Dictionary<string, object>>(
                        "TaskTag",
                        l => l.HasOne<Tag>().WithMany().HasForeignKey("TagId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__TaskTag__TagId__4222D4EF"),
                        r => r.HasOne<Task>().WithMany().HasForeignKey("TaskId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__TaskTag__TaskId__412EB0B6"),
                        j =>
                        {
                            j.HasKey("TaskId", "TagId").HasName("PK__TaskTag__AA3E862BF65074DA");

                            j.ToTable("TaskTag");
                        });
            });

            modelBuilder.Entity<TaskCategory>(entity =>
            {
                entity.ToTable("TaskCategory");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
