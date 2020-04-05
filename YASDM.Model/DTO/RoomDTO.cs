using System;

namespace YASDM.Model.DTO
{
    public class RoomDTO
    {
        public string Name { get; set; }
        public int Id { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime ScheduledDate { get; set; }

    }
}