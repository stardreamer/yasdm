using System.Collections.Generic;

namespace YASDM.Model.DTO
{
    public class UserDetailsDTO : UserDTO
    {
        public List<RoomDTO> Rooms { get; set; }
    }
}