using JobTrackingProject.Domain.Entities;

namespace JobTrackingProject.Application.Interfaces
{
    public interface IUserService
    {
        IEnumerable<User> GetUsers();
        User GetUserById(string id);
        void AddUsers(User job);
        void UpdateUser(User job);
        void DeleteUser(string id);

        public Task<User> GetUserByEmailAsync(string email);
        public Task SavePasswordResetToken(string email, string token);
    }
}
