using AutoMapper;
using HotelListing.iRepository;
using HotelListing.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelListing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriyController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public CountriyController(IUnitOfWork unitOfWork, ILogger<CountriyController> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetCountries()
        {
            try
            {
                var countries = await _unitOfWork.Countries.GetAll();
                var results = _mapper.Map<IList<CountryDTO>>(countries);
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(GetCountries)}", ex);
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetCountry(int id)
        {
            try
            {
                var country = await _unitOfWork.Countries.Get(q => q.Id == id, new List<string> { "Hotels" });
                var result = _mapper.Map<CountryDTO>(country);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(GetCountry)}", ex);
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }
    }
}
