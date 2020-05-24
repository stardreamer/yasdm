namespace YASDM.Model.DTO
{
    public class MembershipSearchDTO
    {
        public int? UserId { get; set; }
        public int? RoomId { get; set; }

        public string AsQueryString()
        {
            var req = "";
            if(UserId.HasValue)
            {
                req += $"userid={UserId.Value}";
            }

            if(RoomId.HasValue)
            {
                req += $"&roomid={RoomId.Value}";
            }

            return req;
        }

    }
}