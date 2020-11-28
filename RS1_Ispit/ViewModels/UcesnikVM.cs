using System.Collections.Generic;

namespace RS1_Ispit_asp.net_core.ViewModels
{
    public class UcesnikVM
    {
        public int? TakmicenjeId { get; set; }
        public int UcesnikId { get; set; }
        public List<UcesnikSelect> Ucesnici { get; set; }
        public int? Bodovi { get; set; }
    }

    public class UcesnikSelect
    {
        public string Id { get; set; }
        public string Naziv { get; set; }
    }
}