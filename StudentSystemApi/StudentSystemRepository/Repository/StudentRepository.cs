using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StudentSystemRepository.Interfaces;
using StudentSytemData.Data;
using StudentSytemData.Dto;
using StudentSytemData.Models;


namespace StudentSystemRepository.Repository
{
    public class StudentRepository : IStudentRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly DataContext _context;


        public StudentRepository(UserManager<ApplicationUser> userManager,DataContext dataContext)
        {
            _userManager = userManager;
            _context = dataContext;
        }

        public async Task<List<ApplicationUser>> GetUsersInRoleAsync(string roleName)
        {
            return (List<ApplicationUser>)await _userManager.GetUsersInRoleAsync(roleName);
        }
        public async Task<ApplicationUser> GetByIdAsync(string userId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<bool> UpdateAsync(ApplicationUser user)
        {
            _context.Users.Update(user);
            return await _context.SaveChangesAsync() > 0;
        }

    }
}
