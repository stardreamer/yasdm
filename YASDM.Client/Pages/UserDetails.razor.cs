using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace YASDM.Client.Pages
{
    public class UserDetailsViewModel: ComponentBase
    {
        [Inject]
        protected State State { get; set; }
    }
}