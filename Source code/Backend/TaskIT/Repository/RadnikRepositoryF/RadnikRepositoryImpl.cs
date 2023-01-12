using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using System;
using TaskIT.Model;
using TaskIT.Repository;

namespace TaskIT.Repository.RadnikRepositoryF
{
    public class RadnikRepositoryImpl : RepositoryImpl<Radnik>, RadnikRepository
    {
    
        public RadnikRepositoryImpl(TaskITContext context) : base(context)
        {
         
        }

        public IEnumerable<Radnik> OceniRadnika(int idRadnika, int ocena)
        {
            //dodaj kod
            return null;
        }

        public TaskITContext TaskITContext
        {
            get { return Context as TaskITContext; }
        }
    }
}
