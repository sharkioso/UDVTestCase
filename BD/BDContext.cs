using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

public class BDContext : DbContext
{
    public DbSet<PostAnalyze> Analysis { get; set; }
    private readonly ILogger<BDContext> logger;

    public BDContext(DbContextOptions<BDContext> options, ILogger<BDContext> logger) : base(options)
    {
        this.logger =logger;
        logger.LogInformation("BDContext intialized");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PostAnalyze>()
            .Property(a => a.LetterCount)
            .HasConversion(
                j => JsonConvert.SerializeObject(j),
                j => JsonConvert.DeserializeObject<Dictionary<char, int>>(j)
            );
        logger.LogDebug("BD model configurated");
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            logger.LogDebug($"Saving {ChangeTracker.Entries().Count()} changes");
            return await base.SaveChangesAsync(cancellationToken);
        } 
}