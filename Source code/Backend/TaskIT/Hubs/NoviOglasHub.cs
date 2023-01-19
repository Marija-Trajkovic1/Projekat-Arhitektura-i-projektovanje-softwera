using Microsoft.AspNetCore.SignalR;

namespace TaskIT.Hubs
{
    public class NoviOglasHub:Hub
    {
        public async Task ObavestiONovomPoslu(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);

        }
    }
}
