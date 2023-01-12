namespace TaskIT.Repository.RadnikRadiPosaoRepositoryF
{
    public interface RadnikRadiPosaoRepository:Repository<RadnikRadiPosao>
    {
        IEnumerable<Radnik> OceniRadnika(int idRadnika, int ocena);
    }
}
