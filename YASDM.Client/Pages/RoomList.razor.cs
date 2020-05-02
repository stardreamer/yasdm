using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using YASDM.Model;
using YASDM.Model.DTO;
using YASDM.Model.Services;

namespace YASDM.Client.Pages
{
    public class RoomListViewModel : ComponentBase
    {
        [Inject]
        protected State State { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [Parameter]
        public string Title { get; set; }

        protected bool ShowErrors = false;

        protected string Error;

        [Inject]
        private IRoomService RoomService { get; set; }

        [Inject]
        private IMembershipService MembershipService { get; set; }

        protected List<Room> Rooms { get; set; }

        private int TotalPageNumber { get; set; }

        private PaginationDTO CurrentPage { get; set; } = new PaginationDTO { PageSize = 4 };

        private async Task Update()
        {
            try
            {
                ShowErrors = false;
                var paginated = (await RoomService.GetPaginated(CurrentPage));
                TotalPageNumber = paginated.TotalPages;
                Rooms = paginated.ToList();
                StateHasChanged();
            }
            catch (ClientException e)
            {
                ShowErrors = true;
                Error = e.Message;
            }

        }

        protected async Task NextPage()
        {
            if(CurrentPage.PageNumber == TotalPageNumber)
                return;
            CurrentPage.PageNumber += 1;
            await Update();
        }

        protected async Task PrevPage()
        {
            if (CurrentPage.PageNumber == 1)
                return;
            CurrentPage.PageNumber -= 1;
            await Update();
        }

        protected void ShowDetailedInfo(Room room)
        {
            State.SelectedRoom = room;
            NavigationManager.NavigateTo("/RoomInfo");
        }

        protected void Selected(Room room)
        {
            MembershipService.Create(new MembershipDTO {RoomId = room.Id, UserId = State.User.Id});
        }

        protected override async void OnAfterRender(bool firstRender)
        {
            if (firstRender)
                await Update();
        }
    }
}