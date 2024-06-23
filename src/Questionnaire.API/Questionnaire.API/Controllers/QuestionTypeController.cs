using Microsoft.AspNetCore.Mvc;
using Questionnaire.Core;

namespace Questionnaire.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionTypeController : ControllerBase
    {
        private readonly IQuestionTypeRepository _questionTypeRepository;
        private readonly ILogger<QuestionTypeController> _logger;

        public QuestionTypeController(IQuestionTypeRepository questionTypeRepository, ILogger<QuestionTypeController> logger)
        {
            _questionTypeRepository = questionTypeRepository;
            _logger = logger;

        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> CreateAsync(QuestionTypeRequestDTO questionTypeRequestDTO)
        {
            try
            {
                var result = await _questionTypeRepository.CreateAsync(questionTypeRequestDTO);

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
                var result = await _questionTypeRepository.GetAsync(id);
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
                var result = await _questionTypeRepository.GetAsync();
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
        public async Task<IActionResult> UpdateAsync(string id, QuestionTypeRequestDTO questionTypeRequestDTO)
        {
            try
            {
                var result = await _questionTypeRepository.UpdateAsync(id,questionTypeRequestDTO);
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
                var result = await _questionTypeRepository.DeleteAsync(id);
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
