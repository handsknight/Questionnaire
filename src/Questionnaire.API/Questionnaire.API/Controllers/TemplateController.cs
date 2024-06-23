using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Questionnaire.Core;

namespace Questionnaire.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TemplateController : ControllerBase
    {
        private readonly ITemplateRepository _templateRepository;
        private readonly ILogger<TemplateController> _logger;

        public TemplateController(ITemplateRepository templateRepository, ILogger<TemplateController> logger)
        {
            _templateRepository = templateRepository;
            _logger = logger;

        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> CreateAsync(TemplateRequestDTO templateRequestDTO)
        {
            try
            {
                var result = await _templateRepository.CreateAsync(templateRequestDTO);

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
                var result = await _templateRepository.GetAsync(id);
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
                var result = await _templateRepository.GetAsync();
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
        public async Task<IActionResult> UpdateAsync(string id, TemplateRequestDTO templateRequestDTO)
        {
            try
            {
                var result = await _templateRepository.UpdateAsync(id, templateRequestDTO);
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

        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            try
            {
                var result = await _templateRepository.DeleteAsync(id);
                if (result == -1)
                {
                    return NotFound("Resource Not Found");
                }
                return Ok("Resource Deleted!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }

        }
    }
}
