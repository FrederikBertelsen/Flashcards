using Microsoft.EntityFrameworkCore;

namespace Backend.Models.DbContext;

public class AppDbContext(DbContextOptions<AppDbContext> options) : Microsoft.EntityFrameworkCore.DbContext(options)
{
    public DbSet<Flashcard> Flashcards { get; init; }
    public DbSet<Deck> Decks { get; init; }
    public DbSet<User> Users { get; init; }
    public DbSet<DeckCollaborator> DeckCollaborators { get; init; }
    public DbSet<Session> Sessions { get; init; }
    
    public DbSet<LearningSession> LearningSessions { get; init; }
    public DbSet<Review> Reviews { get; init; }
    public DbSet<ReviewLog> ReviewLogs { get; init; }

protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    // user - deck
    modelBuilder.Entity<User>()
        .HasMany(u => u.Decks)
        .WithOne(d => d.User)
        .HasForeignKey(d => d.UserId);
    modelBuilder.Entity<Deck>()
        .HasOne(d => d.User)
        .WithMany(u => u.Decks);
    modelBuilder.Entity<Deck>()
        .HasMany(d => d.Flashcards)
        .WithOne(f => f.Deck)
        .HasForeignKey(f => f.DeckId);
    
    // deck - flashcard
    modelBuilder.Entity<Flashcard>()
        .HasOne(f => f.Deck)
        .WithMany(d => d.Flashcards)
        .HasForeignKey(f => f.DeckId);
    // deck - collaborator
    modelBuilder.Entity<DeckCollaborator>()
        .HasKey(dc => new { dc.DeckId, dc.UserId });
    modelBuilder.Entity<DeckCollaborator>()
        .HasOne(dc => dc.Deck)
        .WithMany()
        .HasForeignKey(dc => dc.DeckId);
    modelBuilder.Entity<DeckCollaborator>()
        .HasOne(dc => dc.User)
        .WithMany()
        .HasForeignKey(dc => dc.UserId);
    
    // session - user
    modelBuilder.Entity<Session>()
        .HasOne<User>()
        .WithMany()
        .HasForeignKey(s => s.UserId);
    
    // learning session - user
    modelBuilder.Entity<LearningSession>()
        .HasOne(s => s.User)
        .WithMany(u => u.LearningSessions)
        .HasForeignKey(s => s.UserId);
    // learning session - deck
    modelBuilder.Entity<LearningSession>()
        .HasOne(s => s.Deck)
        .WithMany()
        .HasForeignKey(s => s.DeckId);
    // learning session - review
    modelBuilder.Entity<LearningSession>()
        .HasMany(s => s.Reviews)
        .WithOne(r => r.LearningSession)
        .HasForeignKey(r => r.LearningSessionId);
    // learning session - review log
    modelBuilder.Entity<LearningSession>()
        .HasMany(s => s.ReviewLogs)
        .WithOne()
        .HasForeignKey(r => r.ReviewId);
    
    // review - flashcard
    modelBuilder.Entity<Review>()
        .HasOne(r => r.Flashcard)
        .WithMany()
        .HasForeignKey(r => r.FlashcardId);
    // add Card as owned entity
    modelBuilder.Entity<Review>()
        .OwnsOne(r => r.Card);
    
    // review log - review
    modelBuilder.Entity<ReviewLog>()
        .HasOne(rl => rl.Review)
        .WithMany()
        .HasForeignKey(rl => rl.ReviewId);
    // add Log as owned entity
    modelBuilder.Entity<ReviewLog>()
        .OwnsOne(r => r.Log);
}
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured) 
            optionsBuilder.UseSqlite("Data Source=Flashcards.db");  // Fallback if not configured
    }
}