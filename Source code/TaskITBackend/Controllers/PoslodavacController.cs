using Microsoft.AspNetCore.Mvc;
using TaskItBackend.Models;

namespace TaskITBackend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PoslodavacController:ControllerBase
    {
        public TaskItContext Context { get; set; }

        public PoslodavacController(TaskItContext context)
        {
            Context=context;
        }

    }
}