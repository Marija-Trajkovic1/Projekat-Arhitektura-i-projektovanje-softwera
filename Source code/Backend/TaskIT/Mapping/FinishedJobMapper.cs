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
        public static FinishedJob ToFinishedJobFromCreateFinishedJobRequest(this CreateFinishedJobRequest finishedJobDTO)
        {
            if (finishedJobDTO == null) return null;
            return new FinishedJob()
            {
                JobAdvertisementId = finishedJobDTO.JobAdvertisementId,
                WorkerId = finishedJobDTO.WorkerId,
                WorkerEvaluation = finishedJobDTO.WorkerEvaluation,
                EmployerId = finishedJobDTO.EmployerId,
                EmployerEvaluation = finishedJobDTO.EmployerEvaluation
            };
        }
    }
}
