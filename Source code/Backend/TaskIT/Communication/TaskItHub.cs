using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using TaskIT.Communication.GroupManager;
using TaskIT.Constants;

namespace TaskIT.Hubs
{
    [Authorize]
    public class TaskItHub:Hub
    {
        private readonly SignalRGroupManager groupManager;

        public TaskItHub(SignalRGroupManager groupManager)
        {
            this.groupManager = groupManager;
        }
       public override async Task OnConnectedAsync()
        {
            var userId =  Context.UserIdentifier;
            if (!string.IsNullOrEmpty(userId))
            {
                await groupManager.AddUserToGroupsAsync(userId, Context.ConnectionId);
                await base.OnConnectedAsync();
                Console.WriteLine($"User {userId} connected to SignalR, from OnConnectedAsync");
            }
            else
            {
                Console.WriteLine("User connected but userIdentiier is null");
            } 
        }
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.UserIdentifier;
            await base.OnDisconnectedAsync(exception);
            Console.WriteLine($"User {userId} disconnected from SignalR.Exception: {exception?.Message}");         
        } 

        public async Task FollowJobType(string jobType)
        {
            try
            {
                var userId = Context.UserIdentifier;
                if (string.IsNullOrEmpty(userId))
                {
                    throw new Exception("UserIdentifier je null ili prazan.");
                }
                Console.WriteLine($"{jobType}, linija 47");
                await Groups.AddToGroupAsync(Context.ConnectionId, $"jobType_{jobType}");
                Console.WriteLine($"Korisnik {Context.ConnectionId} dodat u grupu jobType_{jobType}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Greška u FollowJobType: {ex}");
                throw; 
            }

        }

        public async Task UnfollowJobType(string jobType)
        {
            try
            {
                var userId = Context.UserIdentifier;
                Console.WriteLine($"{jobType}, linija 64");
                if (string.IsNullOrEmpty(userId))
                {
                    throw new Exception("UserIdentifier je null ili prazan.");
                }
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"jobType_{jobType}");
                Console.WriteLine($"Korisnik {Context.ConnectionId} izbacen iz grupe jobType_{jobType}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Greška u FollowJobType: {ex.Message}");
                throw;
            }

        }

        public async Task FollowEmployer(string employerId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"employer_{employerId}");
        }

        public async Task UnfollowEmployer(string employerId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"employer_{employerId}");
        }
    }
}
