using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using TaskIT.DTOs.JobAdvertisementDTOs;

namespace TaskIT.Mapping
{
    public static class JobAdvertisementMapper
    {
        public static JobAdvertisementResponse ToJobAdvertisementDTO(this JobAdvertisement jobAdvertisementModel)
        {
            if(jobAdvertisementModel == null) return null;
            return new JobAdvertisementResponse()
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
                JobType = jobAdvertisementModel.JobType
            };
        }

        public static JobAdvertisement ToJobAdvertisementFromCreateJobAdvertisementRequest(this CreateJobAdvertisementRequest jobAdvertisementDTO, string employerId)
        {
            if (jobAdvertisementDTO == null) return null;
            return new JobAdvertisement()
            {
                Title = jobAdvertisementDTO.Title,
                ShortDescription = jobAdvertisementDTO.ShortDescription,
                City = jobAdvertisementDTO.City,
                Street = jobAdvertisementDTO.Street,
                HomeNumber = jobAdvertisementDTO.HomeNumber,
                DateOfExecution = jobAdvertisementDTO.DateOfExecution,
                WorkDuration = jobAdvertisementDTO.WorkDuration, 
                JobSalary = jobAdvertisementDTO.JobSalary,
                JobType = jobAdvertisementDTO.JobType,
                MyEmployerId = employerId
            };
        }

        public static JobAdvertisement ToJobAdvertisementFromUpdateJobAdvertisementRequest(this UpdateJobAdvertisementRequest jobAdvertisementDTO, string id)
        {
            if (jobAdvertisementDTO == null) return null;
            return new JobAdvertisement
            {
                Id = id,
                Title = jobAdvertisementDTO.Title,
                ShortDescription = jobAdvertisementDTO.ShortDescription,
                City = jobAdvertisementDTO.City,
                Street = jobAdvertisementDTO.Street,
                HomeNumber = jobAdvertisementDTO.HomeNumber,
                DateOfExecution = jobAdvertisementDTO.DateOfExecution,
                WorkDuration = jobAdvertisementDTO.WorkDuration,
                IsAvailable = jobAdvertisementDTO.IsAvailable,
                JobSalary = jobAdvertisementDTO.JobSalary,
                JobType = jobAdvertisementDTO.JobType
            };
        }
    }
}
