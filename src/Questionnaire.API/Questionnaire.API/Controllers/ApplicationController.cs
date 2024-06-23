using Microsoft.AspNetCore.Mvc;
using Questionnaire.Core;
using System.Text.RegularExpressions;

namespace Questionnaire.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationController : ControllerBase
    {
        private readonly IPersonalInfoRepository _personalInfoRepository;
        private readonly ILogger<ApplicationController> _logger;

        public ApplicationController(IPersonalInfoRepository personalInfoRepository, ILogger<ApplicationController> logger)
        {
            _personalInfoRepository = personalInfoRepository;
            _logger = logger;

        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> CreateAsync(PersonalInfoRequestDTO personalInfoRequestDTO)
        {
            try
            {
                #region validate phone number
                {
                    // Regex pattern for validating phone number without dial code
                    string pattern = @"^\d{7,}$";
                    // Check if the phone number matches the pattern
                    if (!Regex.IsMatch(personalInfoRequestDTO.Phone!, pattern))
                    {
                        return BadRequest("Invalid phone number. Don't include Dial Code. Phone number not less than Seven digits!");
                    }
                }
                #endregion
                
                var result = await _personalInfoRepository.CreateAsync(personalInfoRequestDTO);

                if (result == null)
                {
                    return NotFound("Something went wrong!");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, nameof(CreateAsync));

                if (ex.Message.Contains("CustomeError"))
                {
                    return BadRequest(ex.Message.Split(':')[1]);
                }

                return BadRequest("Someting went wrong");
            }
        }

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> GetAsync(string id)
        {
            try
            {
                var result = await _personalInfoRepository.GetAsync(id);
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
                var result = await _personalInfoRepository.GetAsync();
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
        public async Task<IActionResult> UpdateAsync(string id, PersonalInfoRequestDTO personalInfoRequestDTO)
        {
            try
            {
                var result = await _personalInfoRepository.UpdateAsync(id, personalInfoRequestDTO);
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
                var result = await _personalInfoRepository.DeleteAsync(id);
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
