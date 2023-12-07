using InstapotAPI.Entity;
using InstapotAPI.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace InstapotAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {

        private readonly IProfileReposetory _profileReposetory;

        private readonly ILogger<LoginController> _logger;
        
        public LoginController(IProfileReposetory profileReposetory, ILogger<LoginController> logger) 
        {
            _profileReposetory = profileReposetory; 
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<Profile>> Get(int id)
        {
            var createdProfile = await _profileReposetory.Profile(id);

            if (createdProfile == null)
            {
                return NotFound();
            }

            return Ok(createdProfile);

        }

        // POST api/<ValuesController>
        [HttpPost]
        public async Task<ActionResult<Profile>> Create(Profile profile)
        {
            var createdProfile = await _profileReposetory.Create(profile);

            return CreatedAtAction("Get", new { id = createdProfile.Id }, createdProfile);
        }

        [HttpPost("Logout")]
        public async Task<ActionResult> Logout()
        {
            return new RedirectResult("/");
        }

        [HttpPost("Login")]
        public async Task<ActionResult> Login()
        {
            return new RedirectResult("/");
        }


    }
}
