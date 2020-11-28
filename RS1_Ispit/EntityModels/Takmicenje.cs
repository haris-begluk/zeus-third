using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace RS1_Ispit_asp.net_core.EntityModels
{
    public class Takmicenje
    {
        public int Id { get; set; }

        [ForeignKey(nameof(SkolaId))]
        public int SkolaId { get; set; }

        public Skola Skola { get; set; }

        [ForeignKey(nameof(PredmetId))]
        public int PredmetId { get; set; }

        public Predmet Predmet { get; set; }
        public DateTime Datum { get; set; }
        public bool Zakljucaj { get; set; }
    }
}