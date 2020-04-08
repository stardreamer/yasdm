using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using YASDM.Client.Services;
using YASDM.Model.DTO;

namespace YASDM.Client.Pages
{
    public class RegisterViewModel : ComponentBase
    {
        [Inject]
        private IAuthService AuthService { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        protected AuthRegisterDTO RegisterDto = new AuthRegisterDTO();

        protected string ConfirmPassword {get;set;}

        protected bool ShowErrors = false;

        protected IEnumerable<string> Errors;

        protected async Task HandleRegistration()
        {
            ShowErrors = false;
            var result = await AuthService.Register(RegisterDto);
            NavigationManager.NavigateTo("/login");
        }

    }
}