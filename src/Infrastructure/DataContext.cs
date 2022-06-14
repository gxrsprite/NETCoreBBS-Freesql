using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FreeSql;
using IGeekFan.AspNetCore.Identity.FreeSql;
using Microsoft.AspNetCore.Identity;
using NetCoreBBS.Entities;

namespace NetCoreBBS.Infrastructure
{
    public class DataContext : IdentityDbContext<User, Role, string>
    {
        public DataContext()
            : base()
        {
        }
        public DataContext(IdentityOptions options, IFreeSql freeSql, DbContextOptions option) : base(options, freeSql, option)
        {
        }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<TopicReply> TopicReplys { get; set; }
        public DbSet<TopicNode> TopicNodes { get; set; }
        public DbSet<UserMessage> UserMessages { get; set; }
        public DbSet<UserCollection> UserTopics { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseFreeSql(FreeSqlFactory.Fsql);
        }
        protected override void OnModelCreating(ICodeFirst modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Topic>(e => e.ToTable("Topic"));
            modelBuilder.Entity<TopicReply>(e => e.ToTable("TopicReply"));
            modelBuilder.Entity<TopicNode>(e => e.ToTable("TopicNode"));
            modelBuilder.Entity<User>(e =>
                   {
                       e.ToTable("User");
                       e.Property(r => r.ConcurrencyStamp).Help().IsIgnore(true);
                   }
            );
            modelBuilder.Entity<UserMessage>(e => e.ToTable("UserMessage"));
            modelBuilder.Entity<UserCollection>(e => e.ToTable("UserCollection"));
            modelBuilder.Entity<IdentityUserClaim<string>>(e => e.ToTable("UserClaim"));

            modelBuilder.Entity<IdentityUser>(b =>
                        {
                            b.ToTable("User");
                            b.Property(r => r.ConcurrencyStamp).Help().IsIgnore(true);
                        });
            modelBuilder.Entity<Role>(b =>
            {
                b.ToTable("Role");
                b.Property(r => r.ConcurrencyStamp).Help().IsIgnore(true);
            });
            modelBuilder.Entity<IdentityRole>(b =>
                    {
                        b.ToTable("Role");
                        b.Property(r => r.ConcurrencyStamp).Help().IsIgnore(true);
                    });
            modelBuilder.Entity<UserRole>(b =>
                    {
                        b.ToTable("UserRole");
                    });
            modelBuilder.Entity<IdentityUserRole<string>>(b =>
            {
                b.ToTable("UserRole");
            });
        }
    }
}
