using System.Collections.Generic;

namespace YASDM.Model.DTO
{
    public class RoomDetailsDTO : RoomDTO
    {
        public List<UserDTO> Users { get; set; }
    }
}