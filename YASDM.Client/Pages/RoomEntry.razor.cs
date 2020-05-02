using System;
using Microsoft.AspNetCore.Components;
using YASDM.Model;

namespace YASDM.Client.Pages
{
    public class RoomEntryViewModel : ComponentBase
    {
        [Parameter]
        public Room Room { get; set; }

        [Parameter]
        public string ButtonTitle { get; set; }

        [Parameter]
        public string ButtonClass { get; set; }

        [Parameter]
        public Action<Room> Selected { get; set; }

        [Parameter]
        public Action<Room> ShowDetailedInfo { get; set; }


    }
}