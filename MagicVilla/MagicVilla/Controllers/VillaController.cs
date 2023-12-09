using MagicVilla.Data;
using MagicVilla.Models;
using MagicVilla.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace MagicVilla.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaController : ControllerBase
    {
        //inyeccion de dependencia del logger;
        private readonly ILogger<VillaController> _logger;
        private readonly ApplicationDbContext _db;
        public VillaController(ILogger<VillaController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<VillaDto>> GetVillas()
        {
            _logger.LogInformation("Se obtuvieron todas las villas");
            return Ok(_db.Villas.ToList());
        }

        [HttpGet("id:int", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<VillaDto> GetVilla(int id)
        {
            var result = _db.Villas.FirstOrDefault(v => v.Id == id);
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
        public ActionResult<VillaDto> CreateVilla([FromBody] VillaCreateDto villa)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (_db.Villas.FirstOrDefault(v => v.Name.ToLower() == villa.Name.ToLower()) != null)
            {
                ModelState.AddModelError("NameExist", "Ya existe una villa con ese nombre.");
                return BadRequest(ModelState);
            }
            if (villa == null || String.IsNullOrEmpty(villa.Name))
            {
                return BadRequest();
            }

            Villa model = new()
            {
                Name = villa.Name,
                Detail = villa.Detail,
                ImageUrl = villa.ImageUrl,
                Capacity = villa.Capacity,
                Price = villa.Price,
                Province = villa.Province
            };
            _db.Villas.Add(model);
            _db.SaveChanges();
            return CreatedAtRoute("GetVilla", villa);

        }

        [HttpDelete("id:int")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult DeleteVilla(int id)
        {
            var result = _db.Villas.FirstOrDefault(v => v.Id == id);
            if (id == 0)
            {
                return BadRequest();
            }
            if (result == null)
            {
                return StatusCode(404);
            }

            _db.Villas.Remove(result);
            _db.SaveChanges();
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
            var result = _db.Villas.FirstOrDefault(v => v.Id == id);

            if (result == null)
            {
                return StatusCode(404);
            }

                result.Name = villaDto.Name;
                result.Detail = villaDto.Detail;
                result.Capacity = villaDto.Capacity;
                result.ImageUrl = villaDto.ImageUrl;
                result.Price = villaDto.Price;
                result.Province = villaDto.Province;
                result.DateUpdated = DateTime.Now;

            _db.Villas.Update(result);
            _db.SaveChanges();
           

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

            var result = _db.Villas.FirstOrDefault(v => v.Id == id);


            if (result == null)
            {
                return StatusCode(404);
            }

            VillaDto villa = new()
            {
                Id = id,
                Name = result.Name,
                Detail = result.Detail,
                Capacity = result.Capacity,
                ImageUrl = result.ImageUrl,
                Price = result.Price,
                Province = result.Province
            };

            villaDto.ApplyTo(villa, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Villa model = new()
            {
                Id = villa.Id,
                Name = villa.Name,
                Detail = villa.Detail,
                Capacity = villa.Capacity,
                Province = villa.Province,
                Price = villa.Price,
                ImageUrl = villa.ImageUrl,
                DateUpdated = DateTime.Now
            };
            _db.Villas.Update(model);
            _db.SaveChanges();
            return NoContent();

        }
    }
}
