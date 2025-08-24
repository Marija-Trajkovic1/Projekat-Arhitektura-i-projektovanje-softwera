namespace TaskIT.Communication.FinishedJobNotificationServices
{
    public interface FinishedJobNotificationService
    {
        public Task NotifyEmployerEvaluated(string employerId, string finishedJobTitle, int employerEvaluation);
        public Task NotifyWorkerEvaluated(string workerId, string finishedJobTitle, int workerEvaluation);

    }
}
