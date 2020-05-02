using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using YASDM.Client.Services;
using YASDM.Model;
using YASDM.Model.DTO;
using YASDM.Model.Services;

namespace YASDM.Client.Pages
{
    public class LoginViewModel : ComponentBase
    {
        [Inject]
        private IAuthService AuthService { get; set; }

        [Inject]
        private IUserService UserService { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        protected AuthRegisterDTO LoginDto = new AuthRegisterDTO();

        [Inject]
        protected State State { get; set; }

        protected bool ShowErrors = false;

        protected string Error;

        protected async Task HandleLogin()
        {
            try
            {
                ShowErrors = false;

                var result = await AuthService.Login(LoginDto);
                State.User = await UserService.GetById(result.Id);
                NavigationManager.NavigateTo("/");

            }
            catch (ClientException e)
            {
                ShowErrors = true;
                Error = e.Message;
            }

        }

    }
}