using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using TaskIT.Communication.GroupManager;
using TaskIT.Constants;
using TaskIT.Repository.NotificationRepositoryF;

namespace TaskIT.Hubs
{
    [Authorize]
    public class TaskItHub:Hub
    {
        private readonly SignalRGroupManager groupManager;
        private readonly NotificationRepository notificationRepository;

        public TaskItHub(SignalRGroupManager groupManager, NotificationRepository notificationRepository)
        {
            this.groupManager = groupManager;
            this.notificationRepository = notificationRepository;
        }
       public override async Task OnConnectedAsync()
        {
            var userId =  Context.UserIdentifier;
            await groupManager.AddUserToGroupsAsync(userId, Context.ConnectionId);
            var userGroups = await groupManager.GetGroupsForUser(userId);
            var notifications = await notificationRepository.GetUnreadMesages(userId);
            foreach(var group in userGroups)
            {
                var groupNotifications = await notificationRepository.GetUnreadMesages(group);
                notifications.AddRange(groupNotifications);
            }
            
            await Clients.Caller.SendAsync(NotificationEvents.SavedNotifications, notifications);
            
            await base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.UserIdentifier;
            await base.OnDisconnectedAsync(exception); 
        } 

        public async Task FollowJobType(string jobType)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"jobType_{jobType}");
        }

        public async Task UnfollowJobType(string jobType)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"jobType_{jobType}");
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
