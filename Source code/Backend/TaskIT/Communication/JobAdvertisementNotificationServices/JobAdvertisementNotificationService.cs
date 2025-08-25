using TaskIT.DTOs.JobAdvertisementDTOs;
using TaskIT.Model;

namespace TaskIT.Communication.JobAdvertisementNotificationServices
{
    public interface JobAdvertisementNotificationService
    {

        //da li da saljem onom ko je prijavljen za taj posao? ima smisla ako ne prati mozda poslodavca... puzzled a little bit

        public Task NotifyNewJobAdvertisement(List<string> employerFollowersIds,string employerId, string jobAdvertisementId);
        public Task NotifyJobAdvertisementUpdate(List<string> jobAdvertisementFollowersIds, string jobAdvertisementTitle);
        public Task NotifyJobAdvertisementDeletion(List<string> jobAdvertisementFollowersIds, string jobAdvertisementTitle);
        public Task NotifyApplied(string employerId, string jobAdvertisementId, string jobAdvertisementTitle, string workerUserName);
        public Task NotifyDeclinedByWorker(string employerId, string jobAdvertisementId, string jobAdvertisementTitle,string workerUserName);

        public Task NotifyAvailableAgain(List<string> followersIds, string jobAdvertisementId, string jobAdvertisementTitle);
        public Task NotifyDeclined(string workerId, string jobAdvertisementId, string jobAdvertisementTitle, string employerUserName);

        public Task NotifyNewJobAdvertisementByType(List<string>jobFollowersIds,string jobAdvertisementId,string jobType);

    }
}
