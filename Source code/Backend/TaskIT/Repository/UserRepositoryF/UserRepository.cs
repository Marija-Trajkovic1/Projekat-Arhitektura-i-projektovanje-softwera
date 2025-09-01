using Microsoft.AspNetCore.Mvc;


namespace TaskIT.Repository.UserRepositoryF
{
    public interface UserRepository:Repository<User>
    {
        Task<User> UpdateUserAsync(string id, User entity);
    }
}