using Microsoft.AspNetCore.Identity;
using StudentSystemRepository.Interfaces;
using StudentSytemData.Dto;
using StudentSytemData.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace StudentSystemRepository.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthRepository(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        //public async Task<AuthResult> RegisterAsync(Registeration model)
        //{
        //    var user = new ApplicationUser
        //    {
        //        UserName = model.Username,
        //        Email = model.Email,
        //    };

        //    return new AuthResult
        //    {
        //        Username = user.UserName,
        //        Roles = new List<string> { "User" },
        //        Email = user.Email,

        //    };
        //}


        //public Task<AuthResult> loginAsync(login model)
        //{
        //    throw new NotImplementedException();
        //}
        public async Task<ApplicationUser> FindByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<ApplicationUser> FindByNameAsync(string username)
        {
            return await _userManager.FindByNameAsync(username);
        }





        public async Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }
        public async Task<List<Claim>> GetUserClaimsAsync(ApplicationUser user)
        {
            return (List<Claim>)await _userManager.GetClaimsAsync(user);
        }

        public async Task<List<string>> GetUserRolesAsync(ApplicationUser user)
        {
            return new List<string>(await _userManager.GetRolesAsync(user));
        }

        public async Task<string> AddToRoleAsync(ApplicationUser user, string role)
        {
            await _userManager.AddToRoleAsync(user, role);
            return string.Empty;
        }


        public async Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task<IdentityResult> RemoveFromRoleAsync(ApplicationUser user, string role)
        {
            return await _userManager.RemoveFromRoleAsync(user, role);
        }

        public async Task<ApplicationUser> FindByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }
        public async Task<bool> IsUserInRoleAsync(ApplicationUser user, string role)
        {
            return await _userManager.IsInRoleAsync(user, role);
        }

        public async Task<bool> RoleExistsAsync(string role)
        {
            return await _roleManager.RoleExistsAsync(role);
        }


    }
}

