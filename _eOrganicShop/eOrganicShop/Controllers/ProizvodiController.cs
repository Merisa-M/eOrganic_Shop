using eOrganicShop.Model.Request;
using eOrganicShop.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eOrganicShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProizvodiController : Controller
    {

        private readonly IProizvodiService _service;
        public ProizvodiController(IProizvodiService service)
        {
            _service = service;
        }
        [HttpGet]
        [Authorize]
        public async Task<List<Model.Proizvodi>> Get([FromQuery] ProizvodiSearchRequest search)
        {
            return await _service.Get(search);
        }
        [HttpGet("{ID}")]
        [Authorize]
        public async Task<Model.Proizvodi> GetById(int ID)
        {
            return await _service.GetById(ID);
        }
        [HttpPost]
        [Authorize(Roles = "Admin, Kupac")]
        public async Task<Model.Proizvodi> Insert(ProizvodiUpsertRequest request)
        {
            return await _service.Insert(request);
        }
        [HttpPut("{ID}")]
        [Authorize]
        public async Task<Model.Proizvodi> Update(int ID, ProizvodiUpsertRequest request)
        {
            return await _service.Update(ID, request);
        }
        [HttpDelete("{ID}")]
        [Authorize(Roles = "Admin")]
        public async Task<bool> Delete(int ID)
        {
            return await _service.Delete(ID);
        }
        [HttpGet("{ID}/GetProsjek")]
        public async Task<float> GetAvarageRating(int ID)
        {
            return await _service.GetProsjek(ID);
        }
    }
}
