using AutoMapper;
using HotelListing.Data;
using HotelListing.iRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelListing.Controllers
{
    [ApiVersion("2.0")]
    [Route("api/countriy")]
    [ApiController]
    public class CountryV2Controller : ControllerBase
    {
        private DataBaseContext _context;

        public CountryV2Controller(DataBaseContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetCountries()
        {
            return Ok(_context.Countries);
        }
    }
}
