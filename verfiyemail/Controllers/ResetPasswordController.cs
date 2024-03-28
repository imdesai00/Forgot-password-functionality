//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using System.Net;
//using verfiyemail.Models;
//using verfiyemail.Services;

//namespace verfiyemail.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class ResetPasswordController : ControllerBase
//    {
//        private readonly IEmailService _emailService;

//        public ResetPasswordController(IEmailService emailService)
//        {
//            _emailService = emailService;
//        }

//        [HttpPost("resetpassword")]
//        public async Task<IActionResult> ResetPassword([FromBody] ResetPassword model)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest("Invalid request");
//            }

//            // Validate token and reset password logic
//            // Example: You might validate the token against a database or any other user store

//            // If token is valid, proceed to reset password
//            // Example: Reset password logic here

//            return Ok("Password reset successfully");
//        }
//    }
//}
