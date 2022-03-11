using System;
using System.Collections.Generic;
using System.Text;

namespace eOrganicShop.Model.ViewModel
{
   public class TransakcijeListVM
    {
        public int KupovinaID { get; set; }
        public int KorisnikID { get; set; }
        public int NarudzbaID { get; set; }
        public DateTime DatumKupovine { get; set; }
        public float Cijena { get; set; }
        public Model.Narudzba Narudzba { get; set; }
        public string BrojNarudzbe { get; set; }

        public DateTime DatumTranskacije { get; set; }
        public string DatumTransakcijeString { get; set; }

        public string KorisnickoIme { get; set; }
        public string NazivProizovda { get; set; }
        public virtual Korisnici Korisnik { get; set; }
  
    }
}
