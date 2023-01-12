namespace TaskIT.Repository.RadnikRepositoryF
{
    public interface RadnikRepository:Repository<Radnik>
    {
        IEnumerable<Radnik> OceniRadnika(int idRadnika, int ocena);

    }
}
