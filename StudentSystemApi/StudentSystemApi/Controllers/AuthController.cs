using Microsoft.AspNetCore.Mvc;
using StudentSystemService.Interfaces;
using StudentSytemData.Models;

namespace StudentSystemApi.Controllers
{
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] Registeration model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.RegistrationAsync(model);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            return Ok(result);
            //return Ok(new
            //{
            //    tohen = result.Token,
            //});
        }

        [HttpPost("token")]
        public async Task<IActionResult> GetTokenAsync([FromBody] login model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.LoginAsync(model);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpPost("addrole")]
        public async Task<IActionResult> AddRoleAsync([FromBody] RoleModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.AddRoleAsync(model);

            if (!string.IsNullOrEmpty(result))
                return BadRequest(result);

            return Ok(new
            {
                Result = model,
                Message = $"{model.Role} Role added successfully"
            });
        }

        [HttpPost("deleterole")]
        public async Task<IActionResult> DeleteRoleAsync([FromBody] RoleModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.DeleteRoleAsync(model);

            if (!string.IsNullOrEmpty(result))
                return BadRequest(result);

            return Ok(new
            {
                Result = model,
                Message = $"{model.Role} Role deleted successfully"
            });
        }
    }
}


