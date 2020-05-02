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
    public class UsersListViewModel : ComponentBase
    {
        [Inject]
        protected State State { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [Parameter]
        public string Title { get; set; }

        [Parameter]
        public Action<User> Selected { get; set; }

        protected bool ShowErrors = false;

        protected string Error;

        [Inject]
        private IUserService UserService { get; set; }

        protected List<User> Users { get; set; }

        private int TotalPageNumber { get; set; }

        private PaginationDTO CurrentPage { get; set; } = new PaginationDTO { PageSize = 4 };

        private async Task Update()
        {
            try
            {
                ShowErrors = false;
                var paginated = (await UserService.GetPaginated(CurrentPage));
                TotalPageNumber = paginated.TotalPages;
                Users = paginated.ToList();
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

        protected async void ShowDetailedInfo(User user)
        {
            State.SelectedUser = await UserService.GetEagerById(user.Id);
            NavigationManager.NavigateTo("/UserInfo");
        }

        protected override async void OnAfterRender(bool firstRender)
        {
            if (firstRender)
                await Update();
        }
    }
}