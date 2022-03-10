using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace eOrganicShop.Database
{
    public partial class eOrganicShopContext
    {
        public static string GenerateSalt()
        {
            var buf = new byte[16];
            (new RNGCryptoServiceProvider()).GetBytes(buf);
            return Convert.ToBase64String(buf);
        }
        public static string GenerateHash(string salt, string password)
        {
            byte[] src = Convert.FromBase64String(salt);
            byte[] bytes = Encoding.Unicode.GetBytes(password);
            byte[] dst = new byte[src.Length + bytes.Length];

            Buffer.BlockCopy(src, 0, dst, 0, src.Length);
            Buffer.BlockCopy(bytes, 0, dst, src.Length, bytes.Length);

            HashAlgorithm algorithm = HashAlgorithm.Create("SHA1");
            byte[] inArray = algorithm.ComputeHash(dst);
            return Convert.ToBase64String(inArray);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {

            List<string> Salt = new List<string>();
            for (int i = 0; i < 3; i++)
            {
                Salt.Add(GenerateSalt());
            }
            modelBuilder.Entity<Korisnici>().HasData
            (
                new Korisnici
                {
                    KorisnikID = 1,
                    Ime = "Admin",
                    Prezime = "Admin",
                    KorisnickoIme = "admin",
                    Email = "admin.admin@gmail.com",
                    Telefon = "061111111",
                    LozinkaSalt = Salt[0],
                    LozinkaHash = GenerateHash(Salt[0], "Admin123"),
                    Image = File.ReadAllBytes("Files/korisnik.jpg")

                },
                new Korisnici
                {
                    KorisnikID = 2,
                    Ime = "Kupac",
                    Prezime = "Kupac",
                    KorisnickoIme = "kupac",
                    Email = "kupac.kupac@gmail.com",
                    Telefon = "061111000",
                    LozinkaSalt = Salt[0],
                    LozinkaHash = GenerateHash(Salt[0], "Kupac123"),
                    Image = File.ReadAllBytes("Files/korisnik.jpg")

                },
                new Korisnici
                {
                    KorisnikID = 3,
                    Ime = "KupacIme",
                    Prezime = "KupacPrezime",
                    KorisnickoIme = "kupac3",
                    Email = "kupacIme.kupacPrezime@gmail.com",
                    Telefon = "061111222",
                    LozinkaSalt = Salt[0],
                    LozinkaHash = GenerateHash(Salt[0], "Kupac111"),
                    Image = File.ReadAllBytes("Files/korisnik.jpg")

                },
                  new Korisnici
                  {
                      KorisnikID = 4,
                      Ime = "KupacIme2",
                      Prezime = "KupacPrezime2",
                      KorisnickoIme = "kupac4",
                      Email = "kupac.kupacPrezime@gmail.com",
                      Telefon = "061111333",
                      LozinkaSalt = Salt[0],
                      LozinkaHash = GenerateHash(Salt[0], "Kupac222"),
                      Image = File.ReadAllBytes("Files/korisnik.jpg")

                  },
                   new Korisnici
                   {
                       KorisnikID = 5,
                       Ime = "KupacIme3",
                       Prezime = "KupacPrezime3",
                       KorisnickoIme = "kupac5",
                       Email = "kupacIme3.kupacPrezime3@gmail.com",
                       Telefon = "061222222",
                       LozinkaSalt = Salt[0],
                       LozinkaHash = GenerateHash(Salt[0], "Kupac333"),
                       Image = File.ReadAllBytes("Files/korisnik.jpg")

                   }
                );

            modelBuilder.Entity<Uloge>().HasData
          (
              new Uloge { UlogaID = 1, Naziv = "Admin" },
              new Uloge { UlogaID = 2, Naziv = "Kupac" }
          );
            modelBuilder.Entity<KorisnikUloge>().HasData(
               new KorisnikUloge { KorisnikUlogeID = 1, KorisnikID = 1, UlogeID = 1 },
               new KorisnikUloge { KorisnikUlogeID = 2, KorisnikID = 2, UlogeID = 2 },
               new KorisnikUloge { KorisnikUlogeID = 3, KorisnikID = 3, UlogeID = 2 },
               new KorisnikUloge { KorisnikUlogeID = 4, KorisnikID = 4, UlogeID = 2 },
               new KorisnikUloge { KorisnikUlogeID = 5, KorisnikID = 5, UlogeID = 2 }

          );
            modelBuilder.Entity<VrsteProizvoda>().HasData(
              new VrsteProizvoda { VrsteProizvodaID = 1, Naziv = "Lice" },
              new VrsteProizvoda { VrsteProizvodaID = 2, Naziv = "Tijelo" },
              new VrsteProizvoda { VrsteProizvodaID = 3, Naziv = "Kosa" },
              new VrsteProizvoda { VrsteProizvodaID = 4, Naziv = "Usne" },
              new VrsteProizvoda { VrsteProizvodaID = 5, Naziv = "Ruke" }
         );
            modelBuilder.Entity<Proizvodi>().HasData(
             new Proizvodi
             {
                 ProizvodID = 1,
                 NazivProizvoda = "Pročišćavajuća maska za lice",
                 Cijena = 15,
                 Opis = "Pročišćavajuća maska za lice od Avrila sadrži crvenu i bijelu glinu i stoga je namijenjena za nježno, ali dubinsko čišćenje normalne do masne kože.",
                 Sifra = "3455",
                 VrstaProizvodaID = 1,
                 Slika = File.ReadAllBytes("Files/lice.jpg"),
                 KorisnikID = 1

             },
             new Proizvodi
             {
                 ProizvodID = 2,
                 NazivProizvoda = "Maska za lice Energizing & Instant Radiance",
                 Cijena = 10,
                 Opis = " Njezina formula sadrži tonizirajući, adstringentni i posvjetljujući ekstrakt lista jagode kao i regenerirajuću i zaštitnu organsku aloe veru. Chlorella dodatno pruža blistavi sjaj i glatki ten.",
                 Sifra = "3456",
                 VrstaProizvodaID = 1,
                 Slika = File.ReadAllBytes("Files/lice.jpg"),
                 KorisnikID = 1

             },
              new Proizvodi
              {
                  ProizvodID = 3,
                  NazivProizvoda = "Aloe vera gel",
                  Cijena = 15,
                  Opis = "Podupire proces regeneracije kože i intenzivno njeguje.",
                  Sifra = "3457",
                  VrstaProizvodaID = 2,
                  Slika = File.ReadAllBytes("Files/tijelo.jpg"),
                  KorisnikID = 1

              },
              new Proizvodi
              {
                  ProizvodID = 4,
                  NazivProizvoda = "Golden Nectar piling za ",
                  Cijena = 12,
                  Opis = "Jedinstvena tekstura uljnog gela u dodiru s vodom poprima teksturu mlijeka.",
                  Sifra = "3459",
                  VrstaProizvodaID = 2,
                  Slika = File.ReadAllBytes("Files/tijelo1.jpg"),
                  KorisnikID = 1

              },
               new Proizvodi
               {
                   ProizvodID = 5,
                   NazivProizvoda = "Argan šampon za kosu",
                   Cijena = 12,
                   Opis = "Čisti i izglađuje kosu te obnavlja i hidratizira korijen kose.",
                   Sifra = "H004-200",
                   VrstaProizvodaID = 3,
                   Slika = File.ReadAllBytes("Files/Hair_Mask.jpg"),
                   KorisnikID = 1

               },
                new Proizvodi
                {
                    ProizvodID = 6,
                    NazivProizvoda = "Krema za ruke",
                    Cijena = 10,
                    Opis = "Ono nešto što ovu kremu čini posebnom je i nježan voćni miris s notama kokosa i vanilije.",
                    Sifra = "3461",
                    VrstaProizvodaID = 5,
                    Slika = File.ReadAllBytes("Files/ruke.jpg"),
                    KorisnikID = 1

                },
                  new Proizvodi
                  {
                      ProizvodID = 7,
                      NazivProizvoda = "Krema za ruke Everyday Hand Cream",
                      Cijena = 8,
                      Opis = "Krema za ruke Everyday Hand Cream.",
                      Sifra = "R106-300",
                      VrstaProizvodaID = 5,
                      Slika = File.ReadAllBytes("Files/ruke1.jpg"),
                      KorisnikID = 1

                  },
                   new Proizvodi
                   {
                       ProizvodID = 8,
                       NazivProizvoda = "Aloe Vera balzam za usne",
                       Cijena = 8,
                       Opis = "Sjedinjuje umirujuća svojstva aloe vere i hranjiva svojstva karite maslaca, pčelinjeg voska i vitamina E.",
                       Sifra = "670303",
                       VrstaProizvodaID = 4,
                       Slika = File.ReadAllBytes("Files/usne.jpg"),
                       KorisnikID = 1

                   },
                   new Proizvodi
                   {
                       ProizvodID = 9,
                       NazivProizvoda = "Balzam za usne Vanilla Lips",
                       Cijena = 8,
                       Opis = "Balzam za usne slatkog mirisa vanilije, hrani i njeguje usne uljima kokosa, argana, jojobe, nevena te kakao i shea maslacem.",
                       Sifra = "5727-5",
                       VrstaProizvodaID = 4,
                       Slika = File.ReadAllBytes("Files/usne1.jpg"),
                       KorisnikID = 1
                   },
                   new Proizvodi
                   {
                       ProizvodID = 10,
                       NazivProizvoda = "Itch Away šampon",
                       Cijena = 10,
                       Opis = "Ultra-nježni, prirodni šampon bez dodanih mirisa za osjetljivo vlasište sklono ekcemima, dermatitisu i psorijazi.",
                       Sifra = "M110-150",
                       VrstaProizvodaID = 3,
                       Slika = File.ReadAllBytes("Files/sampon.jpg"),
                       KorisnikID = 1
                   }
        );



        }

    }
}
