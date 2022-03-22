using AutoMapper;
using eOrganicShop.Database;
using eOrganicShop.Model.Request;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace eOrganicShop.Service
{
    public class KorisniciService : CRUDService<Model.Korisnici, KorisnikSearchRequest, Korisnici, KorisnikUpsertRequest, KorisnikUpsertRequest>, IKorisniciService
    {
        private readonly eOrganicShopContext _context;
        private readonly IMapper _mapper;
        public KorisniciService(eOrganicShopContext context, IMapper mapper) : base(context, mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async override Task<List<Model.Korisnici>> Get(KorisnikSearchRequest search)
        {
            var query = _context.Korisnici.Include(x => x.KorisnikUloge).AsQueryable().OrderBy(c => c.Ime);

            if (!string.IsNullOrWhiteSpace(search?.KorisnickoIme))
            {
                query = query.Where(x => x.KorisnickoIme.ToLower().StartsWith(search.KorisnickoIme.ToLower())).OrderBy(c => c.Ime);
            }
            var list = await query.ToListAsync();
            return _mapper.Map<List<Model.Korisnici>>(list);
        }

        public override async Task<Model.Korisnici> GetById(int ID)
        {
            var entity = await _context.Set<Korisnici>()
                .Where(i => i.KorisnikID == ID)
                .Include(i => i.KorisnikUloge)
                .SingleOrDefaultAsync();

            return _mapper.Map<Model.Korisnici>(entity);
        }
        public override async Task<Model.Korisnici> Insert(KorisnikUpsertRequest request)
        {
            if (request.Password != request.PasswordConfirmation)
            {
                throw new Exception("Passwords do not match!");
            }

            var entity = _mapper.Map<Korisnici>(request);
            entity.LozinkaSalt = GenerateSalt();
            entity.LozinkaHash = GenerateHash(entity.LozinkaSalt, request.Password);

            await _context.Korisnici.AddAsync(entity);
            await _context.SaveChangesAsync();

            foreach (var ulogaID in request.Uloge)
            {
                var uloge = new KorisnikUloge()
                {
                    KorisnikID = entity.KorisnikID,
                    UlogeID = ulogaID
                };

                await _context.KorisnikUloge.AddAsync(uloge);
            }

            await _context.SaveChangesAsync();

            return _mapper.Map<Model.Korisnici>(entity);
        }
        public override async Task<Model.Korisnici> Update(int ID, KorisnikUpsertRequest request)
        {
            var entity = _context.Korisnici.Find(ID);

            _context.Korisnici.Attach(entity);
            _context.Korisnici.Update(entity);

            if (!string.IsNullOrWhiteSpace(request.Password))
            {
                if (request.Password != request.PasswordConfirmation)
                {
                    throw new Exception("Passwords do not match!");
                }

                entity.LozinkaSalt = GenerateSalt();
                entity.LozinkaHash = GenerateHash(entity.LozinkaSalt, request.Password);
            }

            foreach (var UlogeID in request.Uloge)
            {
                var korisnikUloge = await _context.KorisnikUloge
                    .Where(i => i.UlogeID == UlogeID && i.KorisnikID == ID)
                    .SingleOrDefaultAsync();

                if (korisnikUloge == null)
                {
                    var novaU = new KorisnikUloge()
                    {
                        KorisnikID = ID,
                        UlogeID = UlogeID
                    };
                    await _context.Set<KorisnikUloge>().AddAsync(novaU);
                }
            }
            foreach (var UlogeID in request.UlogeBrisanje)
            {
                var korisnikUloge = await _context.KorisnikUloge
                    .Where(i => i.UlogeID == UlogeID && i.KorisnikID == ID)
                    .SingleOrDefaultAsync();

                if (korisnikUloge != null)
                {
                    _context.Set<KorisnikUloge>().Remove(korisnikUloge);
                }
            }


            _mapper.Map(request, entity);
            await _context.SaveChangesAsync();

            return _mapper.Map<Model.Korisnici>(entity);
        }
        public override async Task<bool> Delete(int ID)
        {
            var entity = await _context.Korisnici.
                Include(i => i.KorisnikUloge).Include(i=>i.Review).Include(i=>i.Rate).
                FirstOrDefaultAsync(i => i.KorisnikID == ID);


            if (entity.KorisnikUloge.Count != 0)
                _context.KorisnikUloge.RemoveRange(entity.KorisnikUloge);

            if (entity.Review.Count != 0)
                _context.Review.RemoveRange(entity.Review);
            if (entity.Rate.Count != 0)
                _context.Rate.RemoveRange(entity.Rate);


            var rate = await _context.Rate.Where(i => i.KorisnikID == ID).ToListAsync();
            if (rate.Count > 0)
            {
                _context.Rate.RemoveRange(rate);
            }
            var review = await _context.Review.Where(i => i.KorisnikID == ID).ToListAsync();
            if (review.Count > 0)
            {
                _context.Review.RemoveRange(review);
            }

            var favoriti = await _context.Favoriti.Where(i => i.KorisnikID == ID).ToListAsync();
            if (favoriti.Count > 0)
            {
                _context.Favoriti.RemoveRange(favoriti);
            }
            var transakcije = await _context.Transakcije.Where(i => i.KorisnikID == ID).ToListAsync();
            if (transakcije.Count > 0)
            {
                _context.Transakcije.RemoveRange(transakcije);
            }
            var narudzbe = await _context.Narudzba.Where(i => i.KorisnikID == ID).ToListAsync();
            if (narudzbe.Count > 0)
            {
                _context.Narudzba.RemoveRange(narudzbe);
            }
            var kupovine = await _context.Kupovine.Where(i => i.KorisnikID == ID).ToListAsync();
            if (kupovine.Count > 0)
            {
                _context.Kupovine.RemoveRange(kupovine);
            }
            var narudzbes = await _context.NarudzbaStavke.Where(i => i.Narudzba.KorisnikID == ID).ToListAsync();
            if (narudzbes.Count > 0)
            {
                _context.NarudzbaStavke.RemoveRange(narudzbes);
            }
            await _context.SaveChangesAsync();


            _context.Korisnici.Remove(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<Model.Proizvodi>> GetLikedProizvodi(int ID, ProizvodiSearchRequest request)
        {
            var query = _context.Favoriti
                 .Include(i => i.Proizvod)
                 .ThenInclude(i => i.Korisnik)
                 .Where(i => i.KorisnikID == ID)
                 .AsQueryable();

            if (!string.IsNullOrWhiteSpace(request?.NazivProizvoda))
            {
                query = query.Where(x => x.Proizvod.NazivProizvoda.StartsWith(request.NazivProizvoda));
            }

            var list = await query.ToListAsync();

            return _mapper.Map<List<Model.Proizvodi>>(list.Select(i => i.Proizvod).ToList());
        }
        public async Task<Model.Proizvodi> InsertLikedProizvodi(int ID, int ProizvodID)
        {
            var entity = new Favoriti()
            {
                KorisnikID = ID,
                ProizvodID = ProizvodID
            };

            await _context.Favoriti.AddAsync(entity);
            await _context.SaveChangesAsync();

            return _mapper.Map<Model.Proizvodi>(entity.Proizvod);
        }
        public async Task<Model.Proizvodi> DeleteLikedProizvodi(int ID, int ProizvodID)
        {
            var entity = await _context.Favoriti
               .Where(i => i.KorisnikID == ID && i.ProizvodID == ProizvodID).Include(i=>i.Proizvod)
               .SingleOrDefaultAsync();
            if (entity != null)
            {
                _context.Favoriti.Remove(entity);
                await _context.SaveChangesAsync();
            }
            return _mapper.Map<Model.Proizvodi>(entity.Proizvod);
        }


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

            System.Buffer.BlockCopy(src, 0, dst, 0, src.Length);
            System.Buffer.BlockCopy(bytes, 0, dst, src.Length, bytes.Length);

            HashAlgorithm algorithm = HashAlgorithm.Create("SHA1");
            byte[] inArray = algorithm.ComputeHash(dst);
            return Convert.ToBase64String(inArray);
        }
        public async Task<Model.Korisnici> Authenticate(UserAuthenticationRequest request)
        {
            var korisnik = await _context.Korisnici
                .Include(i => i.KorisnikUloge)
                .ThenInclude(j => j.Uloge)
                .FirstOrDefaultAsync(i => i.KorisnickoIme == request.Username);

            if (korisnik != null)
            {
                var newHash = GenerateHash(korisnik.LozinkaSalt, request.Password);

                if (newHash == korisnik.LozinkaHash)
                {
                    return _mapper.Map<Model.Korisnici>(korisnik);
                }
            }
            return null;
        }
        public async Task<Model.Korisnici> Login(KorisnikUpsertRequest request)
        {
            if (request.Password != request.PasswordConfirmation)
            {
                throw new Exception("Passwords do not match!");
            }
            request.Uloge = new List<int> { 1, 2 };
            var entity = _mapper.Map<Korisnici>(request);
            entity.LozinkaSalt = GenerateSalt();
            entity.LozinkaHash = GenerateHash(entity.LozinkaSalt, request.Password);

            await _context.Korisnici.AddAsync(entity);
            await _context.SaveChangesAsync();

            return _mapper.Map<Model.Korisnici>(entity);
        }


    }
}
