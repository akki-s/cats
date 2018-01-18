using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AGL.Pets.Core.Domain.ViewModels;
using AGL.Pets.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AGL.Pets.Controllers
{
    /// <summary>
    /// Prodcues list of cats grouped by owner gender
    /// </summary>
    [Produces("application/json")]
    [Route("api/pets")]
    public class PetsController : Controller
    {
        private readonly IPetsService _petsService;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="petsService"></param>
        public PetsController(IPetsService petsService)
        {
            _petsService = petsService;
        }

        // GET: pets/
        /// <summary>
        /// Gets list of cats
        /// </summary>
        /// <returns></returns>
        [HttpGet("")]
        [ProducesResponseType(typeof(IEnumerable<GroupedByOwnerGenderViewModel>), 200)]
        public async Task<IActionResult> Get()
        {
            try
            {
                var result = await _petsService.GetCatsGroupedByOwnerGender().ConfigureAwait(false);                
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured while getting list of cats");
            }
        }
    }
}