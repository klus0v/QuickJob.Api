using Microsoft.EntityFrameworkCore;
using QuickJob.DataModel.Postgres.Entities;

namespace QuickJob.DataModel.Postgres;

public class QuickJobContext : DbContext
{
    public QuickJobContext(DbContextOptions<QuickJobContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>();
    }
}