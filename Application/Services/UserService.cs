using JobTrackingProject.Application.Interfaces;
using JobTrackingProject.Domain.Entities;
using JobTrackingProject.Infrastructure.Data;

namespace JobTrackingProject.Application.Services
{
    public class UserService :IUserService
    {
        private readonly JobDbContext _context;
        public UserService(JobDbContext context)
        {
            _context = context;
        }
        public IEnumerable<User> GetUsers() => _context.Users.ToList();
        public User GetUserById(int id) => _context.Users.FirstOrDefault(u => u.Id == id);
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
        public void DeleteUser(int id)
        {
            var user = _context.Users.Find(id);
            if(user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
        }
    }
}
