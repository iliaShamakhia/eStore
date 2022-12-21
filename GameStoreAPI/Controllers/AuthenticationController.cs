using GameStoreBLL.Models;
using GameStoreDAL.Data;
using GameStoreDAL.Data.Helpers;
using GameStoreDAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace GameStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly GameStoreDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly TokenValidationParameters _tokenValidationParameters;

        public AuthenticationController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, GameStoreDbContext context, IConfiguration configuration, TokenValidationParameters tokenValidationParameters)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _configuration = configuration;
            _tokenValidationParameters = tokenValidationParameters;
        }

        [Authorize]
        [HttpPost("change-avatar")]
        public async Task<IActionResult> ChangeAvatar([FromBody] UserAvatarChangeModel model)
        {
            if(string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.ImageUrl))
            {
                return BadRequest("username or imageUrl can not be empty");
            }
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null)
            {
                return BadRequest("user does not exist");
            }

            user.ImageUrl = model.ImageUrl;

            IdentityResult result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return Ok();
            }
            
            return BadRequest("could not update");
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Please provide all the required fields");
            }

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user != null)
            {
                return BadRequest($"User {user.Email} already exists");
            }

            User newUser = new User()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                UserName = model.UserName,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await _userManager.CreateAsync(newUser, model.Password);

            if (result.Succeeded) 
            {
                switch (model.Role)
                {
                    case UserRoles.User:
                        await _userManager.AddToRoleAsync(newUser, UserRoles.User);
                        break;
                    case UserRoles.Manager:
                        await _userManager.AddToRoleAsync(newUser, UserRoles.Manager);
                        break;
                    case UserRoles.Admin:
                        await _userManager.AddToRoleAsync(newUser, UserRoles.Admin);
                        break;
                    default:
                        break;
                }

                return Ok(); 
            }
            return BadRequest(result.Errors);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Please provide all required fields");
            }

            var user = await _userManager.FindByNameAsync(model.Username);//await _userManager.FindByEmailAsync(model.Email);
            if(user == null){
                return BadRequest("user does not exist");
            }
            var passwordMatches = await _userManager.CheckPasswordAsync(user, model.Password);

            if(!passwordMatches){
                return BadRequest("wrong password");
            }
            if(user != null && passwordMatches)
            {
                var tokenValue = await GenerateJWTTokenAsync(user, null);
                return Ok(new { tokenValue, user });
            }
            return Unauthorized();
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Please provide all required fields");
            }

            var result = await VerifyAndGenerateTokenAsync(model);
            return Ok(result);
        }

        private async Task<AuthResultModel> VerifyAndGenerateTokenAsync(TokenRequestModel model)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var storedToken = await _context.RefreshTokens.FirstOrDefaultAsync(x => x.Token == model.RefreshToken);
            var dbUser = await _userManager.FindByIdAsync(storedToken.UserId);

            try
            {
                var tokenCheckResult = jwtTokenHandler.ValidateToken(model.Token, _tokenValidationParameters, out var validatedToken);
                return await GenerateJWTTokenAsync(dbUser, storedToken);
            }
            catch (SecurityTokenExpiredException)
            {
                if(storedToken.DateExpire >= DateTime.UtcNow)
                {
                   return await GenerateJWTTokenAsync(dbUser, storedToken);
                }
                else
                {
                   return await GenerateJWTTokenAsync(dbUser, null);
                }
            }
        }

        private async Task<AuthResultModel> GenerateJWTTokenAsync(User user, RefreshToken rToken)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var userRoles = await _userManager.GetRolesAsync(user);

            foreach(var userRole in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var authSignInKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]));
            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                expires: DateTime.UtcNow.AddMinutes(1),
                claims: claims,
                signingCredentials : new SigningCredentials(authSignInKey, SecurityAlgorithms.HmacSha256)
            );

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            if(rToken != null)
            {
                var rTokenResponse = new AuthResultModel()
                {
                    Token = jwtToken,
                    RefreshToken = rToken.Token,
                    ExpiresAt = token.ValidTo
                };
                return rTokenResponse;
            }

            var refreshToken = new RefreshToken()
            {
                JwtId = token.Id,
                IsRevoked = false,
                UserId = user.Id,
                DateAdded = DateTime.UtcNow,
                DateExpire = DateTime.UtcNow.AddMonths(6),
                Token = Guid.NewGuid().ToString() + "-" + Guid.NewGuid().ToString()
            };

            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();

            var response = new AuthResultModel()
            {
                Token = jwtToken,
                RefreshToken = refreshToken.Token,
                ExpiresAt = token.ValidTo
            };

            return response;
        }
    }
}
