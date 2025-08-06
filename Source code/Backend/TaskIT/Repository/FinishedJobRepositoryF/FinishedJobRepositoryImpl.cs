namespace TaskIT.Repository.FinishedJobRepositoryF
{
    public class FinishedJobRepositoryImpl:RepositoryImpl<FinishedJob>, FinishedJobRepository
    {
        public FinishedJobRepositoryImpl(TaskITContext context) : base(context)
        {
        }
        public TaskITContext TaskITContext
        {
            get { return TaskITContext as TaskITContext; }
        }
        // Implement any specific methods for OdradjenPosaoRepository here
    }
   
}
