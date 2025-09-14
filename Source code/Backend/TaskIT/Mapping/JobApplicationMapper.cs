using TaskIT.DTOs.JobApplicationDTOs;

namespace TaskIT.Mapping
{
    public static class JobApplicationMapper
    {
        public static JobApplicationResponse ToJobApplicationDTO(this JobApplication jobApplicationModel)
        {
            if (jobApplicationModel == null) return null;
            return new JobApplicationResponse()
            { 
                IsAccepted=jobApplicationModel.IsAccepted,
                Id= jobApplicationModel.Id,
                JobId= jobApplicationModel.JobId,
                WorkerId= jobApplicationModel.WorkerId,

            };

        }

        public static JobApplicationWithUsernameResponse ToJobApplicationWithUsernameResponse(this JobApplication jobApplication)
        {
            if (jobApplication == null) return null;
            return new JobApplicationWithUsernameResponse()
            {
                Id = jobApplication.Id,
                JobId = jobApplication.JobId,
                WorkerId = jobApplication.WorkerId,
                WorkerUserName = jobApplication.Worker.UserName,
                IsAccepted = jobApplication.IsAccepted
            };
        }

        public static JobApplication ToJobApplicationFromCreateJobApplicationRequest(this CreateJobApplicationRequest jobApplicationDTO) 
        {
            if (jobApplicationDTO == null) return null;
            return new JobApplication()
            {
                JobId = jobApplicationDTO.JobId,
                WorkerId = jobApplicationDTO.WorkerId
            };
        }
    }
}
