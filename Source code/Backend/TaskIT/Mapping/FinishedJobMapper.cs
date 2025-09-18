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

        public static FinishedJobAdvertisementResponse ToFinishedJobAdvertisementResponseEvaluation(this JobAdvertisement jobAdvertisementModel, string finishedJobId, int employerEvaluation)
        {
            if (jobAdvertisementModel == null) return null;
            return new FinishedJobAdvertisementResponse()
            {
                Id = jobAdvertisementModel.Id,
                Title = jobAdvertisementModel.Title,
                ShortDescription = jobAdvertisementModel.ShortDescription,
                City = jobAdvertisementModel.City,
                Street = jobAdvertisementModel.Street,
                HomeNumber = jobAdvertisementModel.HomeNumber,
                DateOfExecution = jobAdvertisementModel.DateOfExecution,
                WorkDuration = jobAdvertisementModel.WorkDuration,
                IsAvailable = jobAdvertisementModel.IsAvailable,
                JobSalary = jobAdvertisementModel.JobSalary,
                JobType = jobAdvertisementModel.JobType,
                FinishedJobId= finishedJobId,
                EmployerEvaluation=employerEvaluation

            };

        }
        public static FinishedJobWithAdvertisementResponse ToFinishedJobForWorkerResponse(this FinishedJob finishedJobModel)
        {
            if (finishedJobModel == null) return null;
            return new FinishedJobWithAdvertisementResponse
            {
                FinishedJobId = finishedJobModel.Id,
                EmployerEvaluation = finishedJobModel.EmployerEvaluation.GetValueOrDefault(),
                JobAdvertisementId = finishedJobModel.JobAdvertisementId,
                Title = finishedJobModel.JobAdvertisement.Title,
                ShortDescription = finishedJobModel.JobAdvertisement.ShortDescription,
                City = finishedJobModel.JobAdvertisement.City,
                Street = finishedJobModel.JobAdvertisement.Street,
                HomeNumber = finishedJobModel.JobAdvertisement.HomeNumber,
                DateOfExecution = finishedJobModel.JobAdvertisement.DateOfExecution,
                WorkDuration = finishedJobModel.JobAdvertisement.WorkDuration,
                IsAvailable = finishedJobModel.JobAdvertisement.IsAvailable,
                JobSalary = finishedJobModel.JobAdvertisement.JobSalary,
                JobType = finishedJobModel.JobAdvertisement.MyEmployerId

            };
        }
    }
}
