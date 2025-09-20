using Microsoft.AspNetCore.SignalR;
using TaskIT.Constants;

namespace TaskIT.Hubs
{
    public class TaskItHub:Hub
    {
       public override async Task OnConnectedAsync()
        {
            var userId =  Context.User.GetUserId();
            if(!string.IsNullOrEmpty(userId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, userId);
            }

            await base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.User.GetUserId();
            if(!string.IsNullOrEmpty(userId))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, userId);
            }
            Console.WriteLine($"Client disconnected: {exception?.Message}");
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SubscribeToEmployer(string employerId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"employer_{employerId}_followers");
        }

        public async Task UnsubscribeFromEmployer(string employerId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"employer_{employerId}_followers");
        }

        public async Task SubscribeToJobType(string jobType)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"jobtype_{jobType}_followers");
        }

        public async Task UnsubscribeFromJobType(string jobType)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"jobtype_{jobType}_followers");
        }

        public async Task SubscribeToJob(string jobId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"job_{jobId}_followers");
        }

        public async Task UnsubscribeFromJob(string jobId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"job_{jobId}_followers");
        }

    }
}
