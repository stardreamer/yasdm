using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using YASDM.Client.Services;
using YASDM.Model.DTO;

namespace YASDM.Client.Pages
{
    public class LoginViewModel : ComponentBase
    {
        [Inject]
        private IAuthService AuthService { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        protected AuthRegisterDTO LoginDto = new AuthRegisterDTO();


        protected bool ShowErrors = false;

        protected string Error;

        protected async Task HandleLogin()
        {
            try
            {
                ShowErrors = false;

                var result = await AuthService.Login(LoginDto);
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