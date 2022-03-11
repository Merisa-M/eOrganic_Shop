using System;
using System.Collections.Generic;
using System.Text;

namespace eOrganicShop.Model.ViewModel
{
    public class ProizvodiListVM
    {
        public int ProizvodID { get; set; }
        public string NazivProizvoda { get; set; }
        public string Sifra { get; set; }
        public float Cijena { get; set; }
        public string Opis { get; set; }
        public byte[] Slika { get; set; }
        public int VrstaProizvodaID { get; set; }
        public Model.VrsteProizvoda VrsteProizvoda { get; set; }
    }
}
