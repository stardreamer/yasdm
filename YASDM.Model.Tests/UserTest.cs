using System;
using Xunit;

namespace YASDM.Model.Tests
{
    public class UserTest
    {
        [Fact]
        public void ShouldRepresentUserAsDTO()
        {
            var user = new User()
            {
                Email = "1@test.com",
                FirstName = "Alex",
                LastName = "Brim",
                Id = 187,
                UserName = "alexbrim"
            };

            var dto = user.AsDTO();

            Assert.Equal(user.Email, dto.Email);
            Assert.Equal(user.FirstName, dto.FirstName);
            Assert.Equal(user.LastName, dto.LastName);
            Assert.Equal(user.Id, dto.Id);
            Assert.Equal(user.UserName, dto.Username);
        }
    }
}
