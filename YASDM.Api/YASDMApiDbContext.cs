using Microsoft.EntityFrameworkCore;
using YASDM.Model;


namespace YASDM.Api
{
    public class YASDMApiDbContext : DbContext
    {
        public YASDMApiDbContext(DbContextOptions<YASDMApiDbContext> options) : base(options)
        {
        }

        public DbSet<Room> Rooms { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<UserRoom> UserRooms { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var userEntity = modelBuilder.Entity<User>();
            userEntity.HasIndex(u => u.UserName).IsUnique();

            userEntity.HasKey(user => user.Id);

            var roomEntity = modelBuilder.Entity<Room>();

            roomEntity.HasKey(room => room.Id);


            var userRoomEntity = modelBuilder.Entity<UserRoom>();

            userRoomEntity.HasOne(ur => ur.User).WithMany(u => u.UserRooms).HasForeignKey(ur => ur.UserId).OnDelete(DeleteBehavior.Cascade);
            userRoomEntity.HasOne(ur => ur.Room).WithMany(r => r.UserRooms).HasForeignKey(ur => ur.RoomId).OnDelete(DeleteBehavior.Cascade);

            userRoomEntity.HasIndex(c => new { c.UserId, c.RoomId }).IsUnique();
        }
    }
}