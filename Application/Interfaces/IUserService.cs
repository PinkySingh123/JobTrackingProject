using JobTrackingProject.Domain.Entities;

namespace JobTrackingProject.Application.Interfaces
{
    public interface IUserService
    {
        IEnumerable<User> GetUsers();
        User GetUserById(int id);
        void AddUsers(User job);
        void UpdateUser(User job);
        void DeleteUser(int id);
    }
}
