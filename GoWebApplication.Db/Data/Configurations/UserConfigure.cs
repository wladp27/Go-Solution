using GoWebApplication.Db.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoWebApplication.Db.Data.Configurations
{
    class UserConfigure : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users");

            builder.Property(u=>u.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            builder.Property(u => u.DisplayName)
                    .HasColumnName("display_name")
                    .HasMaxLength(20)
                    .IsRequired();

            builder.Property(u=>u.BirthDate)
                   .HasColumnName("birth_date")
                   .IsRequired(false);

            builder.Property(u => u.ReliabilityVisit)
                   .HasColumnName("reliability_visit")
                   .HasDefaultValue(100)
                   .IsRequired();

            builder.Property(u => u.RegistrationDate)
                   .HasColumnName("registration_date")
                   .IsRequired();

            builder.Property(u => u.IsDeleted)
                   .HasColumnName("is_deleted")
                   .HasDefaultValue(false)
                   .IsRequired();

            builder.Property(u => u.TimeDeleted)
                   .HasColumnName("time_deleted")
                   .IsRequired(false);

            builder.Property(u => u.idCity)
                    .HasColumnName("id_city")
                    .IsRequired();

            builder.HasMany(u => u.Events)
                   .WithOne(e => e.Organizer)
                   .HasForeignKey(e => e.OrganizerId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(u => u.UserEvents)
                   .WithOne(ue => ue.User)
                   .HasForeignKey(ue=>ue.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(u => u.Following)
                   .WithOne(ue => ue.FollowingUser)
                   .HasForeignKey(u=>u.FollowingUserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(u => u.Followers)
                   .WithOne(ue => ue.FollowedUser)
                   .HasForeignKey(u => u.FollowedUserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(u => u.City)
                   .WithMany(c => c.Users)
                   .HasForeignKey(u=>u.idCity)
                   .OnDelete(DeleteBehavior.Restrict); 

            builder.HasMany(u => u.Ratings)
                    .WithOne(r => r.User)
                    .HasForeignKey(r => r.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasData(
                new User
                {
                    Id = "a1b2c3d4-e5f6-7890-abcd-ef0123456789",
                    UserName = "admin",
                    NormalizedUserName = "ADMIN",
                    Email = "admin@goweb.com",
                    NormalizedEmail = "ADMIN@GOWEB.COM",
                    EmailConfirmed = true,
                    PasswordHash = new PasswordHasher<User>().HashPassword(null, "Password123!"),
                    SecurityStamp = "11111111-2222-3333-4444-555555555555",
                    ConcurrencyStamp = "22222222-3333-4444-5555-666666666666",
                    DisplayName = "Админчик",
                    ReliabilityVisit = 100,
                    RegistrationDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    IsDeleted = false,
                    idCity = 1
                }
            );


        }
    }
}
