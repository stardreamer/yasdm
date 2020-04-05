namespace YASDM.Model
{
    public class UserRoom
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
        public int RoomId { get; set; }
        public Room Room { get; set; }
    }
}