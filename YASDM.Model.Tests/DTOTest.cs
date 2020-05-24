using System;
using Xunit;
using YASDM.Model.DTO;

namespace YASDM.Model.Tests
{
    public class DTOTest
    {
        [Fact]
        public void ShouldConstructCorrectQueryStringForPagination()
        {
            var pag = new PaginationDTO() {PageNumber= 10, PageSize=12};

            Assert.Equal(pag.AsQueryString(), "pagenumber=10&pagesize=12");
        }

        [Fact]
        public void ShouldConstructCorrectQueryStringForMembershipSearch()
        {
            var s1 = new MembershipSearchDTO() {RoomId=1};
            var s2 = new MembershipSearchDTO() {UserId=2};
            var s3 = new MembershipSearchDTO() {RoomId=1, UserId=2};
            var s4 = new MembershipSearchDTO();

            Assert.Equal(s1.AsQueryString(), "roomid=1");
            Assert.Equal(s2.AsQueryString(), "userid=2");
            Assert.Equal(s3.AsQueryString(), "userid=2&roomid=1");
            Assert.Equal(s4.AsQueryString(), "");
        }
    }
}
