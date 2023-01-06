using Microsoft.AspNetCore.Mvc;
using TaskItBackend.Models;

namespace TaskITBackend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TipPoslaController:ControllerBase
    {
        public TaskItContext Context { get; set; }

        public TipPoslaController(TaskItContext context)
        {
            Context=context;
        }

    }
}