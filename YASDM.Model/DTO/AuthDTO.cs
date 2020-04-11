namespace YASDM.Model.DTO
{
    public class AuthDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }

        public string Email { get; set; }

        public string Token { get; set; }

        public string RefreshToken { get; set; }
    }
}