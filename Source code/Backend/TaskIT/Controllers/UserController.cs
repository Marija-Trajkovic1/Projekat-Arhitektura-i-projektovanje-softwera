using Microsoft.AspNetCore.Mvc;
using TaskIT.Model;
using TaskIT.Repository.UnityOfWork;

namespace TaskIT.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly TaskITContext context;
        public UnitOfWorkImpl unitOfWork { get; set; }

        public UserController(TaskITContext context)
        {
            this.context = context;
            unitOfWork = new UnitOfWorkImpl(context);

        }

    }
}
