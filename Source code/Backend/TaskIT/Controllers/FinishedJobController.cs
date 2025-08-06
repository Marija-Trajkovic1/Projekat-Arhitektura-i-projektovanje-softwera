using Microsoft.AspNetCore.Mvc;
using TaskIT.Repository.UnityOfWork;

namespace TaskIT.Controllers
{
    public class FinishedJobController: Controller
    {
        private readonly TaskITContext context;
        public UnitOfWorkImpl unitOfWork { get; set; }
        public FinishedJobController(TaskITContext context)
        {
            this.context = context;
            unitOfWork = new UnitOfWorkImpl(context);
        }
        // Add methods to handle requests related to OdradjeniPosao here
    }
}
