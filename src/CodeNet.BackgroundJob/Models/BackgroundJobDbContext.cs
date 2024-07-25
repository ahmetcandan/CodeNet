using Microsoft.EntityFrameworkCore;

namespace CodeNet.BackgroundJob.Models;

public class BackgroundJobDbContext(DbContextOptions options) : DbContext(options)
{
    public virtual DbSet<Job> Jobs { get; set; }
    public virtual DbSet<JobWorkingDetail> JobWorkingDetails { get; set; }
}
