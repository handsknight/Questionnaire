using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Questionnaire.Core;

namespace Questionnaire.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProgamController : ControllerBase
    {
        private readonly IProgramInfoRepository _programInfoRepository;
        private readonly ILogger<ProgamController> _logger;

        public ProgamController(IProgramInfoRepository programInfoRepository, ILogger<ProgamController> logger)
        {
            _programInfoRepository = programInfoRepository;
            _logger = logger;

        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> CreateAsync(ProgramInfoRequestDTO programInfoRequestDTO)
        {
            try
            {
                var result = await _programInfoRepository.CreateAsync(programInfoRequestDTO);

                if (result == null)
                {
                    return NotFound("Something went wrong!");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest("Someting went wrong");
            }

        }

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> GetAsync(string id)
        {
            try
            {
                var result = await _programInfoRepository.GetAsync(id);
                if (result == null)
                {
                    return NotFound("Resource Not Found!");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest("Someting went wrong");
            }

        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAsync()
        {
            try
            {
                var result = await _programInfoRepository.GetAsync();
                if (result == null)
                {
                    return NotFound("Resource Not Found!");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest("Someting went wrong");
            }
        }

        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> UpdateAsync(string id, ProgramInfoRequestDTO programInfoRequestDTO)
        {
            try
            {
                var result = await _programInfoRepository.UpdateAsync(id, programInfoRequestDTO);
                if (result == null)
                {
                    return NotFound("Resource Not Found");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.Message);
                return BadRequest("Someting went wrong");
            }

        }

        
    }
}
