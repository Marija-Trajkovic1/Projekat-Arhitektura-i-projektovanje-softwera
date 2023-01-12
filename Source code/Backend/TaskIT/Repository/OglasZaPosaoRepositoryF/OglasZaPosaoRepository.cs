namespace TaskIT.Repository.OglasZaPosaoRepositoryF
{
    public interface OglasZaPosaoRepository:Repository<OglasZaPosao>
    {
        IEnumerable<OglasZaPosao> KreirajNoviPosao(OglasZaPosao oglas, int idPoslodavca);
    }
}
