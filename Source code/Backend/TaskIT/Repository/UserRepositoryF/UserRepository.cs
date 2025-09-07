using Microsoft.AspNetCore.Mvc;
using TaskIT.DTOs.UserDTOs;


namespace TaskIT.Repository.UserRepositoryF
{
    public interface UserRepository:Repository<User>
    {
        Task<User> UpdateUserAsync(string id, UpdateUserRequest entity);
    }
}