using Microsoft.AspNetCore.Mvc;
using TaskIT.Repository.UnityOfWork;

namespace TaskIT.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RadnikRadiPosaoController
    {
        private readonly TaskITContext context;

        public UnitOfWorkImpl _unitOfWork { get; set; }

        public RadnikRadiPosaoController(TaskITContext context)
        {
            this.context = context;
            _unitOfWork = new UnitOfWorkImpl(this.context);
        }

    }
}
