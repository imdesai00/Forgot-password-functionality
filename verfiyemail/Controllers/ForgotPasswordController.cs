//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Npgsql;
//using System.Net;
//using verfiyemail.DataAccess;
//using verfiyemail.Models;
//using verfiyemail.Services;

//namespace verfiyemail.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class ForgotPasswordController : ControllerBase
//    {
//        private readonly IEmailService _emailService;
//        private readonly IForgotPassword _forgotPassword;

//        public ForgotPasswordController(IEmailService emailService, IForgotPassword forgotPassword)
//        {
//            _emailService = emailService;
//            _forgotPassword = forgotPassword;
//        }

//        [HttpPost]
//        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPassword model)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest("Invalid request");
//            }

//            if(_forgotPassword.ValidateEmail(model.Email))
//            {
//                // Check if the email exists in your database or any other user store
//                // If exists, generate a reset token and send the email
//                var resetToken = Guid.NewGuid().ToString(); // Generate reset token
//                var resetLink = $"{Request.Scheme}://{Request.Host}/resetpassword?token={resetToken}";
//                var emailBody = $"Click the link below to reset your password: {resetLink}";
//                await _emailService.SendEmailAsync(model.Email, "Password Reset", emailBody);

//                return Ok("Password reset link sent successfully");
//            }
//            return BadRequest();   
//        }
//    }
//}
