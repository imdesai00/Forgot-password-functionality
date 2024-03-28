using verfiyemail.Data;
using verfiyemail.Models;
using verfiyemail.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace verfiyemail.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthDataAccess _authDataAccess;
        private readonly EmailService _emailService;
        private readonly TokenService _tokenService;

        public AuthController(EmailService emailService, TokenService tokenService, AuthDataAccess authDataAccess)
        {
            _authDataAccess = authDataAccess;
            _emailService = emailService;
            _tokenService = tokenService;
        }
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPassword model)
        {
            var user = await _authDataAccess.FindEmailAsync(model.Email);
            if (user == null)
            {
                return BadRequest("Email Not Found");
            }

            var token = await _tokenService.GenerateTokenAsync(user);
            var result = await _tokenService.StoreTokenAsync(user, token);

            if (!result)
            {
                return BadRequest("Error storing token.");
            }
            //string userEmail = user.Email;
            //string resetToken = token;
            //string angularAppBaseUrl = "http://localhost:4200";
            //string passwordResetRoute = "/resetpassword";
            //string callbackUrl = $"{angularAppBaseUrl}{passwordResetRoute}?email={Uri.EscapeDataString(userEmail)}&token={Uri.EscapeDataString(resetToken)}";
            var callbackUrl = Url.Action(nameof(ResetPassword), "Auth", new { email = user.Email, tokens = token }, protocol: HttpContext.Request.Scheme);

            await _emailService.SendEmailAsync(model.Email,"Reset Password",$"Please reset your password by clicking here: <a href='{callbackUrl}'>link</a>");

            return Ok("Password reset link has been sent to your email.");
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPassword resetPassword)
        {
            // Validate the token and userId...
            // Typically, you'd also accept a new password here and reset it for the user.
            var user = await _authDataAccess.FindEmailAsync(resetPassword.Email);
            if (user == null)
            {
                return BadRequest("Invalid request.");
            }

            // Verify the token is valid and not expired based on your stored data
            var isValid = await _tokenService.ValidateTokenAsync(user, resetPassword.Token);
            if (!isValid)
            {
                return BadRequest("Invalid or expired token.");
            }
            var result = await _authDataAccess.updatePassword(resetPassword.Email, resetPassword.Password);
            if (result)
            {
                return Ok("Password Reset");
            }
            return BadRequest("Password did not update");
           
        }
    }
}
