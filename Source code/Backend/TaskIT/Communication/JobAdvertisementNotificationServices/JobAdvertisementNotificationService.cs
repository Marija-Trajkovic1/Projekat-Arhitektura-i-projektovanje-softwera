using TaskIT.DTOs.JobAdvertisementDTOs;

namespace TaskIT.Communication.JobAdvertisementNotificationServices
{
    public interface JobAdvertisementNotificationService
    {

        //da li da saljem onom ko je prijavljen za taj posao? ima smisla ako ne prati mozda poslodavca... puzzled a little bit

        public Task NotifyNewJobAdvertisement(List<string> employerFollowersIds,string employerId, string jobAdvertisementId);
        public Task NotifyJobAdvertisementUpdate(List<string> jobAdvertisementFollowersIds, JobAdvertisementResponse jobAdvertisementDTO);
        public Task NotifyJobAdvertisementDeletion(List<string> jobAdvertisementFollowersIds, string jobAdvertisementTitle);

        public Task NotifyApplied(string employerId, string jobAdvertisementId, string jobAdvertisementTitle, string workerUserName);
        public Task NotifyUnapplied(string employerId, string jobAdvertisementId, string jobAdvertisementTitle, string workerUserName);

    }
}
