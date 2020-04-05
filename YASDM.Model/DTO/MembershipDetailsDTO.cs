using System;

namespace YASDM.Model.DTO
{
    public class MembershipDetailDTO : MembershipDTO
    {
        public UserDTO User { get; set; }
        public RoomDTO Room { get; set; }
    }
}