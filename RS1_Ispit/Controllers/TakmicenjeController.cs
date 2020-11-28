using Microsoft.AspNetCore.Mvc;
using RS1_Ispit_asp.net_core.EF;
using RS1_Ispit_asp.net_core.EntityModels;
using RS1_Ispit_asp.net_core.ViewModels;
using System;
using System.Linq;

namespace RS1_Ispit_asp.net_core.Controllers
{
    public class TakmicenjeController : Controller
    {
        private readonly MojContext _context;

        public TakmicenjeController(MojContext mojContext)
        {
            _context = mojContext;
        }

        // GET: TakmicenjeController
        public ActionResult Index(int? skolaId, int? razredId)
        {
            var skole = _context.Skola.ToList();
            var model = new IndexVM
            {
                RazredId = razredId ?? 1,
                SkolaId = skolaId ?? skole.FirstOrDefault().Id,
                Skole = skole
            };

            return View(model);
        }

        public ActionResult Takmicenja(int SkolaId, int RazredId)
        {
            var model = new IndexVM
            {
                RazredId = RazredId,
                SkolaId = SkolaId,
                Skole = _context.Skola.ToList(),
                Takmicenja = _context.Takmicenje.Where(t => t.SkolaId.Equals(SkolaId) && t.Predmet.Razred.Equals(RazredId)).Select(s => new TakmicenjeRow
                {
                    TakmicenjeId = s.Id,
                    Skola = s.Skola.Naziv,
                    Predmet = s.Predmet.Naziv,
                    Datum = s.Datum,
                    NajboljiUcenik = _context.TakmicenjeUcesnik
                .OrderByDescending(o => o.Bodovi)
                .Where(w => w.TakmicenjeId.Equals(s.Id) && w.Pristupio.Equals(true))
                .Select(r => r.Takmicenje.Skola.Naziv + " | " + r.OdjeljenjeStavka.Odjeljenje.Oznaka + " | " + r.OdjeljenjeStavka.Ucenik.ImePrezime)
                .FirstOrDefault()
                }).ToList()
            };

            return PartialView("TakmicenjaPartial", model);
        }

        public IActionResult Dodaj(int skolaId)
        {
            var predmeti = _context.Predmet.ToList();
            var model = new DodajVM
            {
                Predmeti = predmeti,
                PredmetId = predmeti.FirstOrDefault().Id,
                SkolaId = skolaId,
                Skole = _context.Skola.ToList(),
                Datum = DateTime.Now
            };

            return PartialView("DodajPartial", model);
        }

        [HttpPost]
        public IActionResult Dodaj(DodajVM obj)
        {
            var model = _context.Takmicenje.Add(new Takmicenje
            {
                PredmetId = obj.PredmetId,
                SkolaId = obj.SkolaId,
                Datum = DateTime.Now,
                Zakljucaj = false
            });

            var relacije = _context.DodjeljenPredmet
                .Where(dp => dp.ZakljucnoKrajGodine.Equals(5) && dp.PredmetId.Equals(model.Entity.PredmetId) && dp.OdjeljenjeStavka.Odjeljenje.SkolaID.Equals(model.Entity.SkolaId))
                .Select(s => new { s.OdjeljenjeStavkaId, s.OdjeljenjeStavka.UcenikId });

            foreach (var item in relacije)
            {
                if (UcenikProsjek(item.UcenikId))
                    _context.TakmicenjeUcesnik.Add(new TakmicenjeUcesnik
                    {
                        TakmicenjeId = model.Entity.Id,
                        OdjeljenjeStavkaId = item.OdjeljenjeStavkaId,
                        Pristupio = true,
                        Bodovi = 0
                    }); ;
            }

            _context.SaveChanges();

            return RedirectToAction(nameof(Index), "Takmicenje", new { SkolaId = obj.SkolaId, RazredId = _context.Predmet.FirstOrDefault(p => p.Id.Equals(obj.PredmetId)).Razred });
        }

        public IActionResult Rezultati(int id)
        {
            var model = _context.Takmicenje.Where(f => f.Id.Equals(id)).Select(s => new RezultatiVM
            {
                TakmicenjeId = s.Id,
                Datum = s.Datum,
                Predmet = s.Predmet.Naziv,
                Skola = s.Skola.Naziv,
                Razred = s.Predmet.Razred,
                Zakljucaj = s.Zakljucaj,
            }).FirstOrDefault();
            if (!model.Zakljucaj)
                model.Rezultati = _context.TakmicenjeUcesnik.Where(t => t.TakmicenjeId.Equals(id))
                    .Select(sr => new ResRow
                    {
                        UcesnikId = sr.Id,
                        Bodovi = sr.Bodovi,
                        Pristupio = sr.Pristupio,
                        BrojUDnevniku = sr.OdjeljenjeStavka.BrojUDnevniku,
                        Odjeljenje = sr.OdjeljenjeStavka.Odjeljenje.Oznaka
                    }).ToList();
            return View(model);
        }

        public void Pristupio(int id)
        {
            var ucesnik = _context.TakmicenjeUcesnik.FirstOrDefault(t => t.Id.Equals(id));
            ucesnik.Pristupio = !ucesnik.Pristupio;
            _context.SaveChanges();
        }

        public void Zakljucaj(int id)
        {
            var ucesnik = _context.Takmicenje.FirstOrDefault(t => t.Id.Equals(id));
            ucesnik.Zakljucaj = !ucesnik.Zakljucaj;
            _context.SaveChanges();
        }

        public void EditBodovi(int id, int bodovi)
        {
            var ucesnik = _context.TakmicenjeUcesnik.FirstOrDefault(t => t.Id.Equals(id));
            ucesnik.Bodovi = bodovi;
            _context.SaveChanges();
        }
        public IActionResult DodajUcesnik(int id)
        {
            var ucesnici = _context.TakmicenjeUcesnik;
            var model = new UcesnikVM
            {
                TakmicenjeId = id,
                UcesnikId = ucesnici.FirstOrDefault().Id,
                Ucesnici = ucesnici.Select(s => new UcesnikSelect
                {
                    Id = s.Id.ToString(),
                    Naziv = s.OdjeljenjeStavka.Odjeljenje.Oznaka + " | " + s.OdjeljenjeStavka.Ucenik.ImePrezime + " | " + s.OdjeljenjeStavka.BrojUDnevniku
                }).ToList()
            };

            return PartialView("UcesnikPartial", model);
        }
        public IActionResult EditUcesnik(int id,int? bodovi)
        {
            var ucesnik = _context.TakmicenjeUcesnik.FirstOrDefault(t => t.Id.Equals(id));
            var model = new UcesnikVM
            {
                Bodovi = bodovi ?? ucesnik.Bodovi,
                UcesnikId = ucesnik.Id,
                Ucesnici = _context.TakmicenjeUcesnik.Select(s => new UcesnikSelect
                {
                    Id = s.Id.ToString(),
                    Naziv = s.OdjeljenjeStavka.Odjeljenje.Oznaka + " | " + s.OdjeljenjeStavka.Ucenik.ImePrezime + " | " + s.OdjeljenjeStavka.BrojUDnevniku
                }).ToList()
            };

            return PartialView("UcesnikPartial", model);
        }

        [HttpPost]
        public IActionResult EditUcesnik(UcesnikVM obj)
        {
            var ucesnik = _context.TakmicenjeUcesnik.FirstOrDefault(t => t.Id.Equals(obj.UcesnikId));
            if (obj.TakmicenjeId == null)
            {
                ucesnik.Bodovi = obj.Bodovi;
            }
            else
            {
                _context.TakmicenjeUcesnik.Add(new TakmicenjeUcesnik
                {
                    TakmicenjeId = (int)obj.TakmicenjeId,
                    OdjeljenjeStavkaId = ucesnik.OdjeljenjeStavkaId,
                    Pristupio = true,
                    Bodovi = obj.Bodovi
                });
            }

            _context.SaveChanges();
            return RedirectToAction(nameof(Rezultati), "Takmicenje", new { id = obj.TakmicenjeId ??ucesnik.TakmicenjeId });
        }

        private bool UcenikProsjek(int id)
        {
            if (_context.DodjeljenPredmet.Where(dp => dp.OdjeljenjeStavka.UcenikId.Equals(id)).Average(a => a.ZakljucnoKrajGodine) >= 4)
                return true;
            return false;
        }
    }
}