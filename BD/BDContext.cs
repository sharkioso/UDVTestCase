using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Newtonsoft.Json;

public class BDContext : DbContext
{
    public DbSet<PostAnalyze> Analysis { get; set; }

    public BDContext(DbContextOptions<BDContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PostAnalyze>()
            .Property(a => a.LetterCount)
            .HasConversion(
                j => JsonConvert.SerializeObject(j),
                j => JsonConvert.DeserializeObject<Dictionary<char, int>>(j)
            );
    }
}