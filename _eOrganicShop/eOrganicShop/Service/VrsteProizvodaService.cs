using AutoMapper;
using eOrganicShop.Database;
using eOrganicShop.Exceptions;
using eOrganicShop.Model.Request;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eOrganicShop.Service
{
    public class VrsteProizvodaService : CRUDService<Model.VrsteProizvoda, VProizvodaSearchRequest, Database.VrsteProizvoda, VProizvodaUpsertRequest, VProizvodaUpsertRequest>
    {
        private readonly eOrganicShopContext _context;
        private readonly IMapper _mapper;
        public VrsteProizvodaService(eOrganicShopContext context, IMapper mapper) : base(context, mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public override async Task<List<Model.VrsteProizvoda>> Get(VProizvodaSearchRequest request)
        {
            var query = _context.VrsteProizvoda.AsQueryable().OrderBy(c => c.Naziv);

            if (!string.IsNullOrWhiteSpace(request?.Naziv))
            {
                query = query.Where(x => x.Naziv.StartsWith(request.Naziv)).OrderBy(c => c.Naziv);
            }
            var list = await query.ToListAsync();

            return _mapper.Map<List<Model.VrsteProizvoda>>(list);
        }
        public override async Task<Model.VrsteProizvoda> GetById(int ID)
        {
            var entity = await _context.VrsteProizvoda
              .Where(i => i.VrsteProizvodaID == ID)
              .SingleOrDefaultAsync();

            return _mapper.Map<Model.VrsteProizvoda>(entity);

        }
        public override async Task<Model.VrsteProizvoda> Insert(VProizvodaUpsertRequest request)
        {
            if (await _context.VrsteProizvoda.AnyAsync(i => i.Naziv == request.Naziv))
            {
                throw new UserException("Vrsta postoji!");
            }
            var entity = _mapper.Map<VrsteProizvoda>(request);

            _context.Set<VrsteProizvoda>().Add(entity);
            await _context.SaveChangesAsync();

            return _mapper.Map<Model.VrsteProizvoda>(entity);
        }
        public override async Task<Model.VrsteProizvoda> Update(int ID, VProizvodaUpsertRequest request)
        {
            var vrsta = await _context.VrsteProizvoda.FindAsync(ID);
            if (await _context.VrsteProizvoda.AnyAsync(i => i.Naziv == request.Naziv) && request.Naziv != vrsta.Naziv)
            {
                throw new UserException("Vrsta proizvoda vec postoji");
            }

            var entity = _context.Set<VrsteProizvoda>().Find(ID);
            _context.Set<VrsteProizvoda>().Attach(entity);
            _context.Set<VrsteProizvoda>().Update(entity);

            _mapper.Map(request, entity);

            await _context.SaveChangesAsync();

            return _mapper.Map<Model.VrsteProizvoda>(entity);
        }
        public override async Task<bool> Delete(int ID)
        {
            var vrsta = await _context.VrsteProizvoda.Where(i => i.VrsteProizvodaID == ID).FirstOrDefaultAsync();
            var proizvod = await _context.Proizvodi.Where(i => i.VrstaProizvodaID == vrsta.VrsteProizvodaID).ToListAsync();
            var NarudzbeStavke = await _context.NarudzbaStavke.Where(i => i.Proizvod.VrstaProizvodaID == vrsta.VrsteProizvodaID).ToListAsync();
            var review = await _context.Review.Where(i => i.Proizvod.VrstaProizvodaID == vrsta.VrsteProizvodaID).ToListAsync();
            var rate = await _context.Rate.Where(i => i.Proizvod.VrstaProizvodaID == vrsta.VrsteProizvodaID).ToListAsync();
            var favoriti = await _context.Favoriti.Where(i => i.Proizvod.VrstaProizvodaID == vrsta.VrsteProizvodaID).ToListAsync();

            if (vrsta != null)
            {
                if (proizvod.Count > 0)
                {
                    _context.Proizvodi.RemoveRange(proizvod);
                    if (NarudzbeStavke.Count > 0)
                      _context.NarudzbaStavke.RemoveRange(NarudzbeStavke);
                    if (rate.Count > 0)
                        _context.Rate.RemoveRange(rate);

                    if (review.Count > 0)
                        _context.Review.RemoveRange(review);
                    if (favoriti.Count > 0)
                        _context.Favoriti.RemoveRange(favoriti);
                }
                _context.VrsteProizvoda.Remove(vrsta);
                await _context.SaveChangesAsync();

                return true;
            }
            return false;
        }
    }
}
