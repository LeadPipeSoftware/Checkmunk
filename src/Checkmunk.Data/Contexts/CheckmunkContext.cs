using Checkmunk.Domain.Checklists.AggregateRoots;
using Checkmunk.Domain.Checklists.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Checkmunk.Data.Contexts
{
    public class CheckmunkContext : DbContext
    {
        public CheckmunkContext(DbContextOptions<CheckmunkContext> options)
            : base(options)
        {
        }

        public DbSet<Checklist> Checklists { get; set; }

        public DbSet<Domain.Checklists.Entities.User> ChecklistUsers { get; set; }

        public DbSet<Domain.Users.AggregateRoots.User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // CHECKLISTS

            modelBuilder.Entity<Domain.Checklists.Entities.User>(u =>
            {
                u.ToTable("ChecklistUser");

                u.HasKey(x => x.PersistenceId);

                u.Property(x => x.EmailAddress).IsRequired();

                u.HasMany(x => x.Checklists)
                    .WithOne(x => x.CreatedBy)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ChecklistItem>(c =>
            {
                c.ToTable("ChecklistItem");

                c.HasKey(x => x.PersistenceId);

                c.Property(x => x.Id).IsRequired();
            });

            modelBuilder.Entity<Checklist>(c =>
            {
                c.ToTable("Checklist");

                c.HasKey(x => x.PersistenceId);

                c.Property(x => x.Id).IsRequired();

                c.HasMany(x => x.Items)
                    .WithOne(x => x.Checklist)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // USERS

            modelBuilder.Entity<Domain.Users.AggregateRoots.User>(u =>
            {
                u.ToTable("User");

                u.HasKey(x => x.PersistenceId);

                u.Property(x => x.EmailAddress).IsRequired();

                u.Ignore(x => x.PhoneNumber);
                u.Ignore(x => x.BillingAddress);
                u.Ignore(x => x.MailingAddress);

                //u.OwnsOne(x => x.BillingAddress);
                //u.OwnsOne(x => x.MailingAddress);
            });
        }
    }
}
