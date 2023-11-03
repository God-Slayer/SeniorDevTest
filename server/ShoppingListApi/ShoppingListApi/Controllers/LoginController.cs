using Microsoft.AspNetCore.Mvc;
using ShoppingListApi.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ShoppingListApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginProvider _loginProvider;

        public LoginController(ILoginProvider loginProvider)
        {
            _loginProvider = loginProvider;
        }

        // POST api/<LoginController>
        [HttpPost]
        public async Task<IActionResult> LoginWithFacebook([FromBody] string credentials)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(credentials))
                {
                    throw new ArgumentException($"{nameof(credentials)} not found.");
                }

                var response = await _loginProvider.LoginWithFacebook(credentials);

                HttpContext.Response.Cookies.Append("token", response.Message, new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(7),
                    HttpOnly = true,
                    Secure = true,
                    IsEssential = true,
                    SameSite = SameSiteMode.None
                });

                response.Message = "Authorized";

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}