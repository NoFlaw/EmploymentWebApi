using System;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace EmploymentWebApi.Data.Entities
{
    public partial class AppDbContext : DbContext
    {
        public string UserProvider
        {
            get
            {
                if (!string.IsNullOrEmpty(WindowsIdentity.GetCurrent().Name))
                    return WindowsIdentity.GetCurrent().Name.Split('\\')[1];

                return string.Empty;
            }
        }

        public Func<DateTime> TimestampProvider { get; set; } = () => DateTime.UtcNow;

        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Department> Departments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Department>(entity =>
            {
                entity.ToTable("Department");

                entity.Property(e => e.DepartmentName).IsUnicode(false);

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.HasMany(e => e.Employees).WithOne(d => d.Department).OnDelete(DeleteBehavior.Cascade);


            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.ToTable("Employee");

                entity.Property(e => e.EmployeeName).IsUnicode(false);

                entity.Property(e => e.PhotoFileName).IsUnicode(false);

                entity.Property(e => e.StartDate).HasColumnType("date");

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.DepartmentId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Employee_Department");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        private void TrackChanges()
        {
            foreach (var entry in this.ChangeTracker.Entries().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified))
            {
                if (entry.Entity is IAuditable)
                {
                    var auditable = entry.Entity as IAuditable;

                    if (entry.State == EntityState.Added)
                    {
                        auditable.CreatedBy = UserProvider;
                        auditable.CreatedOn = TimestampProvider();
                        auditable.UpdatedOn = TimestampProvider();
                    }
                    else
                    {
                        auditable.UpdatedBy = UserProvider;
                        auditable.UpdatedOn = TimestampProvider();
                    }
                }
            }
        }

        public virtual void Save()
        {
            base.SaveChanges();
        }

        public override int SaveChanges()
        {
            TrackChanges();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            TrackChanges();
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
