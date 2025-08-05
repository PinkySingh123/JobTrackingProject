using JobTrackingProject.Application.Interfaces;
using JobTrackingProject.Domain.Entities;
using JobTrackingProject.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace JobTrackingProject.Application.Services
{
    public class UserService : IUserService
    {
        private readonly JobDbContext _context;
        public UserService(JobDbContext context)
        {
            _context = context;
        }
        public IEnumerable<User> GetUsers() => _context.Users.ToList();
        public User GetUserById(string id) => _context.Users.FirstOrDefault(u => u.Id == id);
        public void AddUsers(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }
        public void UpdateUser(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }
        public void DeleteUser(string id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task SavePasswordResetToken(string email, string token)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
            if (user == null)
                throw new Exception("User not found");
            user.SecurityStamp = token;
            user.ConcurrencyStamp = token;
            user.LockoutEnd = DateTime.UtcNow.AddHours(1); // token valid for 1 hour
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}