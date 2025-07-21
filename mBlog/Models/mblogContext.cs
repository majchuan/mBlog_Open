using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace mBlog.Models
{
    public partial class mblogContext : DbContext
    {
        public mblogContext()
        {
        }

        public mblogContext(DbContextOptions<mblogContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Blog> Blog { get; set; }
        public virtual DbSet<BlogUser> BlogUser { get; set; }
        public virtual DbSet<Comment> Comment { get; set; }
        public virtual DbSet<Contact> Contact { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<SocialMedia> SocialMedia { get; set; }
        public virtual DbSet<UserInformation> UserInformation { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseNpgsql("host=localhost;database=mblog;username=postgres;password=2016June01");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Blog>(entity =>
            {
                entity.ToTable("blog");

                entity.Property(e => e.Id).HasColumnName("id").UseIdentityAlwaysColumn();

                entity.Property(e => e.BlogSubTitle)
                    .IsRequired()
                    .HasColumnName("blog_sub_title")
                    .HasMaxLength(512);

                entity.Property(e => e.BlogText)
                    .IsRequired()
                    .HasColumnName("blog_text");

                entity.Property(e => e.BlogTitle)
                    .IsRequired()
                    .HasColumnName("blog_title")
                    .HasMaxLength(255);

                entity.Property(e => e.CreateTime).HasColumnName("create_time");

                entity.Property(e => e.ModifyTime).HasColumnName("modify_time");
            });

            modelBuilder.Entity<BlogUser>(entity =>
            {
                entity.ToTable("blog_user");

                entity.HasIndex(e => e.Email)
                    .HasDatabaseName("blog_user_email_key")
                    .IsUnique();

                entity.HasIndex(e => e.Username)
                    .HasDatabaseName("blog_user_username_key")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id").UseIdentityAlwaysColumn();

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasMaxLength(50);

                entity.Property(e => e.LastLogin).HasColumnName("last_login");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasMaxLength(255);

                entity.Property(e => e.PasswordSalt)
                    .IsRequired()
                    .HasColumnName("password_salt")
                    .HasMaxLength(255);

                entity.Property(e => e.RoleId).HasColumnName("role_id");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasColumnName("username")
                    .HasMaxLength(50);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.BlogUser)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("blog_user_role_id_fkey");
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.ToTable("comment");

                entity.HasIndex(e => e.BlogId);

                entity.Property(e => e.Id).HasColumnName("id").UseIdentityAlwaysColumn();

                entity.Property(e => e.BlogId).HasColumnName("blog_id");

                entity.Property(e => e.CommentText)
                    .IsRequired()
                    .HasColumnName("comment_text");

                entity.Property(e => e.CommentUser)
                    .HasColumnName("comment_user")
                    .HasMaxLength(50);

                entity.Property(e => e.CreateTime).HasColumnName("create_time");

                entity.HasOne(d => d.Blog)
                    .WithMany(p => p.Comment)
                    .HasForeignKey(d => d.BlogId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("comment_blog_id_fkey");
            });

            modelBuilder.Entity<Contact>(entity =>
            {
                entity.ToTable("contact");

                entity.Property(e => e.Id).HasColumnName("id").UseIdentityAlwaysColumn();

                entity.Property(e => e.ContactMessage)
                    .IsRequired()
                    .HasColumnName("contact_message");

                entity.Property(e => e.ContactName)
                    .IsRequired()
                    .HasColumnName("contact_name")
                    .HasMaxLength(255);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("role");

                entity.Property(e => e.Id).HasColumnName("id").UseIdentityAlwaysColumn();

                entity.Property(e => e.RoleName)
                    .IsRequired()
                    .HasColumnName("role_name")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<SocialMedia>(entity =>
            {
                entity.ToTable("social_media");

                entity.Property(e => e.Id).HasColumnName("id").UseIdentityAlwaysColumn();

                entity.Property(e => e.SocialMediaAddress)
                    .HasColumnName("social_media_address")
                    .HasMaxLength(255);

                entity.Property(e => e.SocialMediaName)
                    .HasColumnName("social_media_name")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<UserInformation>(entity =>
            {
                entity.ToTable("user_information");

                entity.Property(e => e.Id).HasColumnName("id").UseIdentityAlwaysColumn();

                entity.Property(e => e.UserIntro).HasColumnName("user_intro");

                entity.Property(e => e.UserTitle).HasColumnName("user_title");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
