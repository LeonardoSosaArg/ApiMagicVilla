using MagicVilla.Data;
using MagicVilla.Models;
using MagicVilla.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaController : ControllerBase
    {   
        //inyeccion de dependencia del logger;
        private readonly ILogger<VillaController> _logger;
        public VillaController(ILogger<VillaController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<VillaDto>> GetVillas()
        {
            _logger.LogInformation("Se obtuvieron todas las villas");
            return Ok(VillaStore.villaList);
        }

        [HttpGet("id:int", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<VillaDto> GetVilla(int id)
        {
            var result = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
            if (id == 0)
            {
                _logger.LogError($"Error al intentar villa con el id: {id}");
                return BadRequest();
            }
            if (result == null)
            {
                _logger.LogError($"Error al intentar villa con el id: {id}");
                return StatusCode(404);
            }

            return Ok(result);

        }

        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<VillaDto> CreateVilla([FromBody] VillaDto villa)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (VillaStore.villaList.FirstOrDefault(v => v.Name.ToLower() == villa.Name.ToLower()) != null)
            {
                ModelState.AddModelError("NameExist", "Ya existe una villa con ese nombre.");
                return BadRequest(ModelState);
            }
            if (villa == null || villa.Id > 0 || String.IsNullOrEmpty(villa.Name))
            {
                return BadRequest();
            }

            villa.Id = VillaStore.villaList.OrderByDescending(v => v.Id).First().Id + 1;
            VillaStore.villaList.Add(villa);
            return CreatedAtRoute("GetVilla", new { id = villa.Id }, villa);

        }

        [HttpDelete("id:int")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult DeleteVilla(int id)
        {
            var result = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
            if (id == 0)
            {
                return BadRequest();
            }
            if (result == null)
            {
                return StatusCode(404);
            }

            VillaStore.villaList.Remove(result);

            return NoContent();

        }

        [HttpPut("id:int")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult UpdateVilla(int id, [FromBody] VillaDto villaDto)
        {
            if (id == 0 || villaDto == null)
            {
                return BadRequest();
            }
            var result = VillaStore.villaList.FirstOrDefault(v => v.Id == id);

            if (result == null)
            {
                return StatusCode(404);
            }

            result.Name = villaDto.Name;
            result.Capacity = villaDto.Capacity;
            result.Province = villaDto.Province;

            return NoContent();

        }

        [HttpPatch("id:int")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult UpdatePartialVilla(int id, JsonPatchDocument<VillaDto> villaDto)
        {
            if (id == 0 || villaDto == null)
            {
                return BadRequest();
            }
            var result = VillaStore.villaList.FirstOrDefault(v => v.Id == id);

            if (result == null)
            {
                return StatusCode(404);
            }

            //body example
            //[
            //  {
            //  "path": "/name",
            //   "op": "replace",
            //   "value": "nuevo nombre"
            //  }
            //]

            villaDto.ApplyTo(result, ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return NoContent();

        }
    }
}
