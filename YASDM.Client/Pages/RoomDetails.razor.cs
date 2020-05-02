using Microsoft.AspNetCore.Components;

namespace YASDM.Client.Pages
{
    public class RoomDetailsViewModel: ComponentBase
    {
        [Inject]
        protected State State { get; set; }
    }
}