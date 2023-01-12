using Microsoft.AspNetCore.Mvc;
using TaskIT.Model;
using TaskIT.Repository.UnityOfWork;

namespace TaskIT.Controllers
{
    [Route("[controller]")]
    public class KorisnikController : Controller
    {
        private readonly TaskITContext context;
        public UnitOfWorkImpl unitOfWork { get; set; }

        public UnitOfWork _unitOfWork { get; set; }
        
        public KorisnikController(TaskITContext context) 
        {
            this.context = context;
            _unitOfWork = new UnitOfWorkImpl(this.context);
        }

        
    }
}
