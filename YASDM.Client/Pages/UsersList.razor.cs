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

        private PaginationDTO CurrentPage { get; set; } = new PaginationDTO();

        private async Task Update()
        {
            try
            {
                ShowErrors = false;
                Users = (await UserService.GetPaginated(CurrentPage)).ToList();
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

        protected void ShowDetailedInfo(User user)
        {
            State.SelectedUser = user;
            NavigationManager.NavigateTo("/UserInfo");
        }

        protected override async void OnAfterRender(bool firstRender)
        {
            await Update();
        }
    }
}