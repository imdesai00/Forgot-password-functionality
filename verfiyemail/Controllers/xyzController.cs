//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.IdentityModel.Tokens;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;

//namespace corewebapiandangular.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class xyzController : ControllerBase
//    {
//        private readonly string _tokenSecret;

//        public xyzController(IConfiguration configuration)
//        {
//            _tokenSecret = configuration["Jwt:SecretKey"];
//        }

//        [HttpPost]
//        public IActionResult ForgotPassword(string email)
//        {
//            //var user = _userService.GetUserByEmail(email);
//            //if (user == null)
//            //    return NotFound(); // Handle non-existing email address

//            // Generate token
//            var token = GenerateToken(email);

//            // Send token to the user (e.g., via email)

//            return Ok(new { Token = token });
//        }

//        [HttpPost("reset")]
//        public IActionResult ResetPassword(string token, string newPassword)
//        {
//            // Validate token
//            var email = ValidateToken(token);
//            if (email == null)
//                return BadRequest("Invalid or expired token");

//            // Reset password for the user with userId using newPassword

//            return Ok("Password reset successful");
//        }

//        private string GenerateToken(string email)
//        {
//            var tokenHandler = new JwtSecurityTokenHandler();
//            var key = Encoding.ASCII.GetBytes(_tokenSecret);

//            var tokenDescriptor = new SecurityTokenDescriptor
//            {
//                Subject = new ClaimsIdentity(new[] { new Claim("email", email.ToString()) }),
//                Expires = DateTime.UtcNow.AddMinutes(30),
//                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
//            };

//            var token = tokenHandler.CreateToken(tokenDescriptor);
//            return tokenHandler.WriteToken(token);
//        }

//        private string ValidateToken(string token)
//        {
//            var tokenHandler = new JwtSecurityTokenHandler();
//            var key = Encoding.ASCII.GetBytes(_tokenSecret);

//            try
//            {
//                tokenHandler.ValidateToken(token, new TokenValidationParameters
//                {
//                    ValidateIssuerSigningKey = true,
//                    IssuerSigningKey = new SymmetricSecurityKey(key),
//                    ValidateIssuer = false,
//                    ValidateAudience = false,
//                    ClockSkew = TimeSpan.Zero // No delay for expiry validation
//                }, out SecurityToken validatedToken);

//                var jwtToken = (JwtSecurityToken)validatedToken;
//                var email = (jwtToken.Claims.First(x => x.Type == "userId").Value);
//                return email;
//            }
//            catch
//            {
//                // Token validation failed
//                return null;
//            }
//        }
//    }
//}
