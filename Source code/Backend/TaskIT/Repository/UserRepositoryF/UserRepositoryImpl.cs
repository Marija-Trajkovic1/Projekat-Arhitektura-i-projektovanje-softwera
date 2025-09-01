using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using TaskIT.Model;

namespace TaskIT.Repository.UserRepositoryF
{
    public class UserRepositoryImpl : RepositoryImpl<User>, UserRepository
    {
       
       public UserRepositoryImpl(TaskITContext context):base(context)
       {
       }

        public async Task<User> UpdateUserAsync(string userId, User userForUpdate)
        {
            var user = await context.Users.FindAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException($"Korisnik sa ID {userId} nije pronađen.");
            }

            user.PhoneNumber = userForUpdate.PhoneNumber;
            user.City = userForUpdate.City;
            user.Street = userForUpdate.Street;
            user.HomeNumber = userForUpdate.HomeNumber;

            await context.SaveChangesAsync();
            return user;
        }

    }
}
