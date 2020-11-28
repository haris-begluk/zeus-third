using RS1_Ispit_asp.net_core.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ViewModels
{
    public class DodajVM
    {
        public int PredmetId { get; set; }
        public List<Predmet> Predmeti { get; set; }
        public int SkolaId { get; set; }
        public List<Skola> Skole { get; set; }
        public DateTime Datum { get; set; }

    }
}
