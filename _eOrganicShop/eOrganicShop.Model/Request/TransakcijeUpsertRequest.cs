using System;
using System.Collections.Generic;
using System.Text;

namespace eOrganicShop.Model.Request
{
  
        public class TransakcijeUpsertRequest
        {
            public int KorisnikID { get; set; }
            public DateTime DatumTranskacije { get; set; }
            public float Cijena { get; set; }
            public string BrojNarudzbe { get; set; }
            public string KorisnickoIme { get; set; }
            public int NarudzbaID { get; set; }
        }
    
}
