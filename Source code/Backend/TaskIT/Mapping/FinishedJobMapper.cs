using TaskIT.DTOs.FinishedJobDTOs;

namespace TaskIT.Mapping
{
    public static class FinishedJobMapper
    {
        public static FinishedJobResponse ToFinishedJobDTO(this FinishedJob finishedJobModel)
        {
            if (finishedJobModel == null) return null;
            return new FinishedJobResponse()
            {
                Id = finishedJobModel.Id,
                JobAdvertisementId = finishedJobModel.JobAdvertisementId,
                WorkerId = finishedJobModel.WorkerId,
                EmployerId = finishedJobModel.EmployerId
            };
        }
        public static FinishedJob ToFinishedJobFromCreateFinishedJobRequest(this CreateFinishedJobRequest finishedJobDTO, string workerId, string employerId)
        {
            if (finishedJobDTO == null) return null;
            return new FinishedJob()
            {
                JobAdvertisementId = finishedJobDTO.JobAdvertisementId,
                WorkerId = workerId,
                EmployerId = employerId
            };
        }

        //public static FinishedJob ToFinishedJobFromUpdateFinishedJobRequest(this UpdateFinishedJobRequestDTO finishedJobDTO, string workerId, string employerId)
        //{
        //    if (finishedJobDTO == null) return null;
        //    return new FinishedJob()
        //    {
        //        Id = finishedJobDTO.Id,
        //        JobAdvertisementId = finishedJobDTO.JobAdvertisementId,
        //        WorkerEvaluation = finishedJobDTO.WorkerEvaluation,
        //        WorkerId = workerId,
        //        EmployerEvaluation = finishedJobDTO.EmployerEvaluation,
        //        EmployerId = employerId
        //    };
        //}
    }
}
