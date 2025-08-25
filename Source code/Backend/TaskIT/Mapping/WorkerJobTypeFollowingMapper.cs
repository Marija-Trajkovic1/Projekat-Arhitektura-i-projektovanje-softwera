using TaskIT.DTOs.WorkerJobTypeDTOs;

namespace TaskIT.Mapping
{
    public static class WorkerJobTypeFollowingMapper
    {
        public static WorkerJobTypeFollowing ToWorkerJobTypeFollowingFromCreateWorkerJobTypeFollowingRequest(this CreateWorkerJobTypeFollowingRequest createWorkerJobTypeFollowingDTO)
        {
            if (createWorkerJobTypeFollowingDTO == null) { return null; }
            return new WorkerJobTypeFollowing
            {
                WorkerId = createWorkerJobTypeFollowingDTO.WorkerId,
                JobType = createWorkerJobTypeFollowingDTO.JobType
            };
        }
    }
}
