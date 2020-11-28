using RS1_Ispit_asp.net_core.EntityModels;
using System;
using System.Collections.Generic;

namespace RS1_Ispit_asp.net_core.ViewModels
{
    public class IndexVM
    {
        public int SkolaId { get; set; }
        public List<Skola> Skole { get; set; }
        public int RazredId { get; set; }

        public List<int> Razredi { get; set; } = new List<int>() { 1, 2, 3, 4 };
        public List<TakmicenjeRow> Takmicenja { get; set; }
    }

    public class TakmicenjeRow
    {
        public int TakmicenjeId { get; set; }
        public string Skola { get; set; }
        public string Predmet { get; set; }
        public DateTime Datum { get; set; }
        public string NajboljiUcenik{ get; set; }
    }
}