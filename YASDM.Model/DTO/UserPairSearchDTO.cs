namespace YASDM.Model.DTO
{
    public class UserPairSearchDTO
    {
        public int? User1Id { get; set; }

        public int? User2Id { get; set; }

        public int? RoomId { get; set; }

        public string User1Alias { get; set; }

        public string User2Alias { get; set; }

        public string AsQueryString()
        {
            var req = "";
            if(User1Id.HasValue)
            {
                req += $"User1Id={User1Id.Value}";
            }

            if(User2Id.HasValue)
            {
                req += $"User2Id={User2Id.Value}";
            }

            if(!string.IsNullOrWhiteSpace(User1Alias))
            {
                req += $"User1Alias={User1Alias}";
            }

            if(!string.IsNullOrWhiteSpace(User2Alias))
            {
                req += $"User1Alias={User2Alias}";
            }

            if(RoomId.HasValue)
            {
                if(!string.IsNullOrWhiteSpace(req))
                {
                    req += "&";
                }
                req += $"roomid={RoomId.Value}";
            }

            return req;
        }

    }
}