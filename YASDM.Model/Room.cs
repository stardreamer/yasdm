using System;
using System.Collections.Generic;

namespace YASDM.Model
{
    public class Room
    {
        public string Name { get; set; }
        public int Id { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime ScheduledDate { get; set; }

        public List<UserRoom> UserRooms { get; set; }

        public ICollection<UserPair> UserPairs { get; set; }
    }
}