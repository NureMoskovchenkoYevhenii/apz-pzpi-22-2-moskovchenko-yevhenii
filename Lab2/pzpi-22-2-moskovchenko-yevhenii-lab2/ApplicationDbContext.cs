using Microsoft.EntityFrameworkCore;


public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<DayType> DayTypes { get; set; }
    public DbSet<WorkingDay> WorkingDays { get; set; }
    public DbSet<UserWorkingDay> UserWorkingDays { get; set; }
    public DbSet<ChangeRequest> ChangeRequests { get; set; }
    public DbSet<UserChangeRequest> UserChangeRequests { get; set; }
    public DbSet<SensorData> SensorData { get; set; }

}
