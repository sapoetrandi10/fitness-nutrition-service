using fitness_db.Models;

namespace fitness_db.Interfaces
{
    public interface IUserRepository 
    {
        public bool CreateUser(User User);
        public User UpdateUser(User User);
        public bool DeleteUser(User User);
        public ICollection<User> GetUsers();
        public User GetUser(int userId);
    }
}
