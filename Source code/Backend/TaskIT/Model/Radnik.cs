using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TaskIT.Model
{
    [Table("Radnik")]
    public class Radnik : Korisnik
    {
        [DefaultValue(0)]
        public int Ocena { get; set; }

        [DefaultValue(0)]
        public int BrojOdradjenihPoslova { get; set; }

        [DefaultValue(0)]
        public int UkupanZbirOcena { get; set; }

        [JsonIgnore]
        public List<Poslodavac>? ListaPoslodavaca { get; set; }

        [JsonIgnore]
        public List<TipPosla>? ListaTipova { get; set; }

        [JsonIgnore]
        public List<RadnikRadiPosao>? ListaPoslovaKojeRadi { get; set; }
    }
}
