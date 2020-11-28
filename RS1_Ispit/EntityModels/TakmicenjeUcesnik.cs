using System.ComponentModel.DataAnnotations.Schema;

namespace RS1_Ispit_asp.net_core.EntityModels
{
    public class TakmicenjeUcesnik
    {
        public int Id { get; set; }

        [ForeignKey(nameof(TakmicenjeId))]
        public int TakmicenjeId { get; set; }

        public Takmicenje Takmicenje { get; set; }

        [ForeignKey(nameof(OdjeljenjeStavkaId))]
        public int OdjeljenjeStavkaId { get; set; }

        public OdjeljenjeStavka OdjeljenjeStavka { get; set; }
        public int? Bodovi { get; set; }
        public bool Pristupio { get; set; }
    }
}