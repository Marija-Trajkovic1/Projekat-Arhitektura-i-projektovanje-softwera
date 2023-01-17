//using Microsoft.AspNetCore.Mvc;
//using TaskIT.Model;
//using TaskIT.Repository.UnityOfWork;

//namespace TaskIT.Controllers
//{
//    [ApiController]
//    [Route("[controller]")]
//    public class KorisnikController : Controller
//    {
//        private readonly TaskITContext context;
//        public UnitOfWorkImpl unitOfWork { get; set; }
        
//        public KorisnikController(TaskITContext context) 
//        {
//            this.context = context;
//            unitOfWork = new UnitOfWorkImpl(context);
           
//        }

//        public async Task<IActionResult> DodajKorisnika([FromBody] Korisnik korisnik)
//        {
//            this.unitOfWork.Korisnici.Add(korisnik);
//            this.unitOfWork.Complete();
//            return Ok();
//           // return new JsonResult(this.unitOfWork.Korisnici.Add(korisnik)); za get  metode 

//        }
           
       

        
//    }
//}
