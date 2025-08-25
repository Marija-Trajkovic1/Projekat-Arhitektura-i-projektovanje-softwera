namespace TaskIT.Communication.WorkerJobTypeFollowingServices
{
    public interface WorkerJobTypeFollowingService
    {
        Task NotifyInterested(List<string> jobFollowersIds, string jobTypeName, string jobAdvertisementId);
    }
}
