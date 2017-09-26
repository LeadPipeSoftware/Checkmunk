using System;
using Checkmunk.Domain.Checklists.AggregateRoots;
using Checkmunk.Domain.Checklists.ValueObjects;
using Checkmunk.Domain.Users.AggregateRoots;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Checkmunk.Data.Contexts
{
    public class CheckmunkContext : DbContext, IUserRepository, IChecklistRepository
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

        public async Task<User[]> GetAllUsers()
        {
            return await this.Users.ToArrayAsync();
        }

        public async Task<User> GetUserByEmailAddress(string emailAddress)
        {
            return await this.Users.FirstOrDefaultAsync(u => u.EmailAddress.Equals(emailAddress));
        }

        public async Task<Checklist[]> GetAllChecklists()
        {
            return await this.Checklists.Include(c => c.CreatedBy).Include(c => c.Items).ToArrayAsync();
        }

        public async Task<Checklist> GetChecklistById(Guid id)
        {
            return await this.Checklists.Include(c => c.CreatedBy).Include(c => c.Items).FirstOrDefaultAsync(c => c.Id.Equals(id));
        }
    }
}