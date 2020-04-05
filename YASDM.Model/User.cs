using System.Collections.Generic;

namespace YASDM.Model
{
    public class User
    {
        public string UserName { get; set; }

        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public List<UserRoom> UserRooms {get;set;}


    }
}