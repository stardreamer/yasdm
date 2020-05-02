using System.Collections.Generic;
using YASDM.Model;

namespace YASDM.Client
{
    public class State
    {
        public User User { get; set; }

        public User SelectedUser { get; set; }

        public Room SelectedRoom { get; set; }

        public List<Room> AvailableRooms { get; set; }
    }
}