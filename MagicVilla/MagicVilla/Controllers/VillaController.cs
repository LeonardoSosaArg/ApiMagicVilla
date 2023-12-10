using AutoMapper;
using MagicVilla.Data;
using MagicVilla.Models;
using MagicVilla.Models.Dto;
using MagicVilla.Repository.IRepository;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaController : ControllerBase
    {
        //inyeccion de dependencia del logger, bd y mapper;
        private readonly ILogger<VillaController> _logger;
        private readonly IVillaRepository _villaRepo;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _db;
        protected APIResponse _response;
        public VillaController(ApplicationDbContext db,ILogger<VillaController> logger, IVillaRepository villaRepo, IMapper mapper)
        {
            _logger = logger;
            _villaRepo = villaRepo;
            _mapper = mapper;
            _db = db;
            _response = new();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetVillas()
        {
            try
            {
                _logger.LogInformation("Se obtuvieron todas las villas");

                IEnumerable<Villa> villaList = await _villaRepo.GetAll();

                _response.Result = _mapper.Map<IEnumerable<Villa>>(villaList);
                _response.statusCode = System.Net.HttpStatusCode.OK;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.Message };
                //_response.ErrorMessages = new List<string> { ex.ToString() };
            }

            return _response;
           
        }

        [HttpGet("id:int", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetVilla(int id)
        {
            try
            {
                var result = await _villaRepo.Get(v => v.Id == id);
                if (id == 0)
                {
                    _logger.LogError($"Error al intentar villa con el id: {id}");
                    _response.IsSuccess = false;
                    _response.statusCode = System.Net.HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                if (result == null)
                {
                    _logger.LogError($"Error al intentar villa con el id: {id}");
                    _response.IsSuccess = false;
                    _response.statusCode = System.Net.HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                _response.Result = _mapper.Map<VillaDto>(result);
                _response.statusCode = System.Net.HttpStatusCode.OK;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.Message };
            }

            return _response;
        }

        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateVilla([FromBody] VillaCreateDto villaCreate)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (await _villaRepo.Get(v => v.Name.ToLower() == villaCreate.Name.ToLower()) != null)
                {
                    ModelState.AddModelError("NameExist", "Ya existe una villa con ese nombre.");
                    return BadRequest(ModelState);
                }
                if (villaCreate == null || String.IsNullOrEmpty(villaCreate.Name))
                {
                    return BadRequest(villaCreate);
                }

                await _villaRepo.Create(_mapper.Map<Villa>(villaCreate));
                _response.Result = _mapper.Map<Villa>(villaCreate);
                _response.statusCode = System.Net.HttpStatusCode.Created;
                return CreatedAtRoute("GetVilla", _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }

            return _response;

        }

        [HttpDelete("id:int")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteVilla(int id)
        {
            try
            {
                var result = await _villaRepo.Get(v => v.Id == id);
                if (id == 0)
                {
                    _response.statusCode = System.Net.HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }
                if (result == null)
                {
                    _response.statusCode = System.Net.HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    return NotFound(_response);
                }

                await _villaRepo.Remove(result);
                _response.statusCode = System.Net.HttpStatusCode.NoContent;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }

            return BadRequest(_response);

        }

        [HttpPut("id:int")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateVilla(int id, [FromBody] VillaUpdateDto updateDto)
        {
            try
            {
                if (id == 0 || updateDto == null)
                {
                    _response.statusCode = System.Net.HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }
                var result = await _db.Villas.FindAsync(id);

                if (result == null)
                {
                    _response.statusCode = System.Net.HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    return NotFound(_response);
                }
                result.DateUpdated = DateTime.Now;
                Villa model = _mapper.Map<VillaUpdateDto, Villa>(updateDto, result);

                await _villaRepo.Update(model);
                _response.statusCode = System.Net.HttpStatusCode.NoContent;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }

            return BadRequest(_response);
        }

        [HttpPatch("id:int")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDto> villaDto)
        {
            try
            {
                if (id == 0 || villaDto == null)
                {
                    return BadRequest();
                }

                var result = await _villaRepo.Get(v => v.Id == id);

                VillaUpdateDto villa = _mapper.Map<VillaUpdateDto>(villaDto);


                if (result == null)
                {
                    return StatusCode(404);
                }



                villaDto.ApplyTo(villa, ModelState);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _villaRepo.Update(_mapper.Map<Villa>(villa));
                return NoContent();
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }

            return BadRequest(_response);

        }
    }
}
