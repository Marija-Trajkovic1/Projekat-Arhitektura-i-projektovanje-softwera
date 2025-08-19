using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq;
using TaskIT.Model;

namespace TaskIT.Repository.UserRepositoryF
{
    public class UserRepositoryImpl : RepositoryImpl<User>, UserRepository
    {
       
       public UserRepositoryImpl(TaskITContext context):base(context)
       {
       }
     
    }
}
