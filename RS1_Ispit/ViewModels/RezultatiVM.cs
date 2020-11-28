using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ViewModels
{
    public class RezultatiVM
    {
        public int TakmicenjeId { get; set; }

        public string Skola { get; set; }
        public string Predmet { get; set; }
        public int Razred { get; set; }
        public DateTime Datum { get; set; }
        public bool Zakljucaj { get; set; }

        public List<ResRow> Rezultati { get; set; }
    }

    public class ResRow
    {
        public int UcesnikId { get; set; }
        public string Odjeljenje { get; set; }
        public int BrojUDnevniku { get; set; }
        public bool Pristupio { get; set; }
        public int? Bodovi { get; set; }
    }
}