using Microsoft.EntityFrameworkCore;

namespace TaskItBackend.Models
{
    public class TaskItContext : DbContext
    {
        public TaskItContext(DbContextOptions op):base(op)
        {
            //dodaj DB
        }
    }
}