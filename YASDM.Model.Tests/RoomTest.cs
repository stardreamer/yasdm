using System;
using Xunit;

namespace YASDM.Model.Tests
{
    public class RoomTest
    {
        [Fact]
        public void ShouldRepresentRoomAsDTO()
        {
            var room = new Room()
            {
                Id = 187,
                Name = "test",
                CreationDate = new DateTime(1920,4,12),
                ScheduledDate = new DateTime(1920,5,12),
            };

            var dto = room.AsDTO();

            Assert.Equal(room.Id, dto.Id);
            Assert.Equal(room.Name, dto.Name);
            Assert.Equal(room.CreationDate, dto.CreationDate);
            Assert.Equal(room.ScheduledDate, dto.ScheduledDate);
        }
    }
}
