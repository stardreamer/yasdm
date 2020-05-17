using System;
using System.Collections.Generic;
using YASDM.Model.DTO;

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

        public RoomDTO AsDTO()
        {
            return new RoomDTO
            {
                Id = this.Id,
                Name = this.Name,
                CreationDate = this.CreationDate,
                ScheduledDate = this.ScheduledDate
            };
        }
        
    }
}