namespace YASDM.Model
{
    public class UserPair
    {
        public int Id { get; set; }

        public int RoomId { get; set; }

        public Room Room { get; set; }

        public int User1Id { get; set; }

        public int User2Id { get; set; }

        public string User1Alias { get; set; }

        public string User2Alias { get; set; }
    }
}