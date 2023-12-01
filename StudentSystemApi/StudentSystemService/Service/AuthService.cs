using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using StudentSystemRepository.Interfaces;
using StudentSystemService.Interfaces;
using StudentSytemData.Dto;
using StudentSytemData.Models;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;



namespace StudentSystemService.Service
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly JWT _jwt;


        public AuthService(IAuthRepository authRepository, IOptions<JWT> jwt)
        {
            _authRepository = authRepository;
            _jwt = jwt.Value;

        }

        public async Task<AuthResult> RegistrationAsync(Registeration model)
        {
            var existingUserByEmail = await _authRepository.FindByEmailAsync(model.Email);
            var existingUserByUsername = await _authRepository.FindByNameAsync(model.Username);

            if (existingUserByEmail != null)
            {
                return new AuthResult { Message = "Email is already registered!" };
            }

            if (existingUserByUsername != null)
            {
                return new AuthResult { Message = "Username is already registered!" };
            }

            var user = new ApplicationUser
            {
                UserName = model.Username,
                Email = model.Email,
            };

            var result = await _authRepository.CreateUserAsync(user,model.Password);

            if (!result.Succeeded)
            {

                var errors = string.Empty;

                foreach (var error in result.Errors)
                    errors += $"{error.Description},";

                return new AuthResult { Message = errors };
            }

            await _authRepository.AddToRoleAsync(user, "User");

            var jwtSecurityToken = await CreateJwtToken(user);

            return new AuthResult
            {
                ExpiresOn = jwtSecurityToken.ValidTo,
                IsAuthenticated = true,
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                Username = user.UserName,
                Roles = new List<string> { "User" },
                Email = user.Email,
            };
        }
        public async Task<AuthResult> LoginAsync(login model)
        {
            var authModel = new AuthResult();
            var user = await _authRepository.FindByEmailAsync(model.Email);

            if (user is null || !await _authRepository.CheckPasswordAsync(user, model.Password))
            {
                authModel.Message = "Email or Password is incorrect!";
                return authModel;
            }
            if (!user.IsActive)
            {
                authModel.Message = "Your account is disabled :(";
                authModel.IsAuthenticated = false;
                return authModel;
            }

            var jwtSecurityToken = await CreateJwtToken(user);

            var rolesList = await _authRepository.GetUserRolesAsync(user);

            authModel.IsAuthenticated = true;
            authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            authModel.Email = user.Email;
            authModel.Username = user.UserName;
            authModel.ExpiresOn = jwtSecurityToken.ValidTo;
            authModel.Roles = rolesList.ToList();

            return authModel;
        }

        public async Task<string> AddRoleAsync(RoleModel model)
        {
            var user = await _authRepository.FindByIdAsync(model.UserId);

            if (user is null || !await _authRepository.RoleExistsAsync(model.Role))
                return "Invalid user ID or Role";

            if (await _authRepository.IsUserInRoleAsync(user, model.Role))
                return "User already assigned to this role";

            var result = await _authRepository.AddToRoleAsync(user, model.Role);

            return string.IsNullOrEmpty(result) ? string.Empty : "Something went wrong";
        }

        public async Task<string> DeleteRoleAsync(RoleModel model)
        {
            var user = await _authRepository.FindByIdAsync(model.UserId);

            if (user is null || !await _authRepository.RoleExistsAsync(model.Role))
                return "Invalid user ID or Role";

            if (!await _authRepository.IsUserInRoleAsync(user, model.Role))
                return "User is not assigned to this role";

            var result = await _authRepository.RemoveFromRoleAsync(user, model.Role);

            return result.Succeeded ? string.Empty : "Something went wrong";
        }
        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            var userClaims = await _authRepository.GetUserClaimsAsync(user);
            var roles = await _authRepository.GetUserRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
            issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(_jwt.DurationInDays),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }

    }
}
