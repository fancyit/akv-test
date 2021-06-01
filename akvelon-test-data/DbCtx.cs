using akvelon_test_data.DataModels;
using Microsoft.EntityFrameworkCore;
using System;
using akvelon_test_data.DataModels.Enums;

namespace akvelon_test_data
{
    public class DbCtx : DbContext
    {
        public DbCtx(DbContextOptions<DbCtx> options) : base(options) { }

        public DbSet<Project> Projects { get; set; }
        public DbSet<TaskItem> TaskItems { get; set; }

                
        /// <summary>
        /// 
        /// </summary>
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Entites PK definition  //
            builder.Entity<Project>()
                .HasKey(p => p.Name);
            
            builder.Entity<TaskItem>()
                .HasKey(t => t.Id);

            // Enums conversion to string //
            builder.Entity<TaskItem>()
                .Property(t => t.CurrentStatus)
                .HasConversion(
                    v => v.ToString(),
                    v => (TaskItemStatus)Enum.Parse(typeof(TaskItemStatus), v));
            
            builder.Entity<Project>()
                .Property(p => p.CurrentStatus)
                .HasConversion(
                    v => v.ToString(),
                    v => (ProjectStatus)Enum.Parse(typeof(ProjectStatus), v));

            // Entities relations definition //         

            builder.Entity<TaskItem>()
                .HasOne(p => p.Project)
                .WithMany(t => t.TaskItems)
                .OnDelete(DeleteBehavior.NoAction);
            
            builder.Entity<Project>()
                .HasMany(t => t.TaskItems)
                .WithOne(p => p.Project)
                .HasForeignKey( p => p.ProjectName)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
