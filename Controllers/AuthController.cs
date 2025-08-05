//using Google.Apis.Gmail.v1.Data;
using JobTrackingProject.Application.DTO;
using JobTrackingProject.Application.Interfaces;
using JobTrackingProject.Application.Services;
using JobTrackingProject.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace JobTrackingProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly UserManager<User> _userManager;
        private readonly IUserService _userService;
        private readonly ILogger _logger;

        public AuthController(
            ILogger<AuthController> logger,
            IConfiguration configuration,
            UserManager<User> userManager,
            IEmailService emailService,
            IUserService userService)
        {
            _logger = logger;
            _configuration = configuration;
            _userManager = userManager;
            _emailService = emailService;
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginUser)
        {
            var user = await _userManager.FindByNameAsync(loginUser.Username);
            if (user != null && await _userManager.CheckPasswordAsync(user, loginUser.Password))
            {
                var token = GenerateJwtToken(user.UserName);
                return Ok(new { Token = token });
            }
            return Unauthorized("Invalid username or password");
        }

        private string GenerateJwtToken(string username)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"];
            var issuer = jwtSettings["Issuer"];
            var audience = jwtSettings["Audience"];

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(JwtRegisteredClaimNames.Sub, username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer,
                audience,
                claims,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        //[HttpPost("forgot-password")]
        //public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto model)
        //{
        //    var user = await _userManager.FindByEmailAsync(model.Email);
        //    if (user == null)
        //        return BadRequest("User not found");

        //    // Generate Identity password reset token
        //    var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
        //    var encodedToken = System.Web.HttpUtility.UrlEncode(resetToken);

        //    var resetLink = $"https://localhost:5173/reset-password?email={model.Email}&token={encodedToken}";

        //    // Send Email
        //    await _emailService.SendEmailAsync(
        //        model.Email,
        //        "Password Reset",
        //        $"Click <a href='{resetLink}'>here</a> to reset your password. This link is valid for a limited time.");

        //    return Ok(new { message = "Reset link sent to your email" });
        //}

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto model)
        {
            // Generate token
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return BadRequest("User not found");
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = WebUtility.UrlEncode(token);

            // Store token in DB with expiry
            //await _userService.SavePasswordResetToken(model.Email, token);

            // Create reset URL
            var frontendUrl = "http://localhost:5173"; //your frontend base URL
            var resetLink = $"{frontendUrl}/reset-password?email={model.Email}&token={encodedToken}";

            // Send email
            var emailBody = $"<h3>Reset Your Password</h3><p>Click <a href='{resetLink}'>here</a> to reset your password.</p>";
            await _emailService.SendEmailAsync(model.Email, "Password Reset", emailBody);

            return Ok("Reset link sent to email.");
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return BadRequest("User not found");
            _logger.LogInformation("Token received from client: {Token}", model.Token);
            _logger.LogInformation("Purpose passed to ValidateAsync: {Purpose}", "ResetPassword");
            _logger.LogInformation("User validating: {UserId}", user.Id);
            var decodedToken = WebUtility.UrlDecode(model.Token);
            var fixtocken = decodedToken.Replace(" ", "+");
            var result = await _userManager.ResetPasswordAsync(user, fixtocken, model.NewPassword);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                _logger.LogError("Password reset failed: {Errors}", errors);
                return BadRequest($"Password reset failed: {errors}");
            }

            return Ok(new { message = "Password reset successful" });
        }
    }
}