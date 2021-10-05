using AutoMapper;
using HotelListing.Data;
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

        [HttpGet("{id:int}", Name = "GetCountry")]
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

        [HttpPost]
        public async Task<IActionResult> CreateCountry([FromBody] CreateCountryDTO countryDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid POST attempt in {nameof(CreateCountry)}");
                return BadRequest(ModelState);
            }

            try
            {
                var country = _mapper.Map<Country>(countryDTO);
                await _unitOfWork.Countries.Insert(country);
                await _unitOfWork.Save();

                return CreatedAtRoute("GetCountry", new { id = country.Id }, country);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, $"Something went wrong in the {nameof(CreateCountry)}");
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateCountry(int id, [FromBody] UpdateCountryDTO countryDTO)
        {
            if (!ModelState.IsValid || id < 1)
            {
                _logger.LogError($"Invalid POST attempt in {nameof(UpdateCountry)}");
                return BadRequest(ModelState);
            }

            try
            {
                var country = await _unitOfWork.Countries.Get(q => q.Id == id);
                if (country == null)
                {
                    _logger.LogError($"Invalid POST attempt in {nameof(UpdateCountry)}");
                    return BadRequest("Submitted data is invalid");
                }

                _mapper.Map(countryDTO, country);
                _unitOfWork.Countries.Update(country);
                await _unitOfWork.Save();

                return NoContent();
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, $"Something went wrong in the {nameof(UpdateCountry)}");
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            if (id < 1)
            {
                _logger.LogError($"Invalid POST attempt in {nameof(DeleteCountry)}");
                return BadRequest(ModelState);
            }

            try
            {
                var country = await _unitOfWork.Countries.Get(q => q.Id == id);
                if (country == null)
                {
                    _logger.LogError($"Invalid POST attempt in {nameof(DeleteCountry)}");
                    return BadRequest("Submitted data is invalid");
                }

                await _unitOfWork.Countries.Delete(id);
                await _unitOfWork.Save();

                return NoContent();
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, $"Something went wrong in the {nameof(DeleteCountry)}");
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }
    }
}
