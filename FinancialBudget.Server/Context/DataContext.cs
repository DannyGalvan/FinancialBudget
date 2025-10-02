using FinancialBudget.Server.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace FinancialBudget.Server.Context
{
    public class DataContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataContext"/> class.
        /// </summary>
        public DataContext()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataContext"/> class.
        /// </summary>
        /// <param name="options">The options<see cref="DbContextOptions{DataContext}"/></param>
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        /// <summary>
        /// The OnConfiguring
        /// </summary>
        /// <param name="optionsBuilder">The optionsBuilder<see cref="DbContextOptionsBuilder"/></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ConfigureWarnings(warn => { warn.Default(WarningBehavior.Ignore); });

            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseNpgsql("Name=ConnectionStrings:Default");
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<Operation> Operations { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<RolOperation> RolOperations { get; set; }
        public DbSet<Budget> Budgets { get; set; }
        public DbSet<BudgetItem> BudgetItems { get; set; }
        public DbSet<Origin> Origins { get; set; }
        public DbSet<Priority> Priorities { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<SplitType> SplitTypes { get; set; }
        public DbSet<RequestStatus> RequestStatuses { get; set; }
        public DbSet<Traceability> Traceabilities { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);
        }
    }
}