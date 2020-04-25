using System;
using Microsoft.AspNetCore.Components;
using YASDM.Model;

namespace YASDM.Client.Pages
{

    public class UserEntryViewModel : ComponentBase
    {
        [Parameter]
        public User User { get; set; }

        [Parameter]
        public string ButtonTitle { get; set; }

        [Parameter]
        public string ButtonClass { get; set; }

        [Parameter]
        public Action<User> Selected { get; set; }

        [Parameter]
        public Action<User> ShowDetailedInfo { get; set; }


    }
}