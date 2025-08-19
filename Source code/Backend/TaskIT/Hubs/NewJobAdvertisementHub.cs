using Microsoft.AspNetCore.SignalR;

namespace TaskIT.Hubs
{
    public class NewJobAdvertisementHub:Hub
    {
        public async Task NotifyAboutNewJobAdvertisement(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);

        }
    }
}
