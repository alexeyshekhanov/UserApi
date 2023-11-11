using System.Collections.Concurrent;
using UserApi.Exceptions;
using UserApi.Models;

namespace UserApi.Services
{
    public class UserService
    {
        private ConcurrentDictionary<int, User> users = new ConcurrentDictionary<int, User>();

        private int GetNewId()
        {
            if (users.IsEmpty)
                return 1;
            return users.Select(x => x.Key).Max() + 1;
        }

        public User AddUser(User user)
        {
            ValidationService.ValidateUser(user);
            user.Id = GetNewId();
            users.GetOrAdd(user.Id, user);
            return user;
        }

        public void DeleteUser(int id)
        {
            var userToDelete = users.GetValueOrDefault(id);
            if (userToDelete == null)
                throw new UserValidationException($"Не существует пользователя с id = {id}");
            users.Remove(id, out _);
        }

        public void UpdateUser(int id, User user)
        {
            var userToUpdate = users.GetValueOrDefault(id);
            if (userToUpdate == null)
                throw new UserValidationException($"Не существует пользователя с id = {id}");

            ValidationService.ValidateUser(user);
            user.Id = id;

            users.AddOrUpdate(id, user, (s, source) => user);
        }

        public User GetUserById(int id)
        {
            var user = users.Select(x => x.Value).Where(x => x.Id == id).FirstOrDefault();
            if (user == null)
                throw new UserValidationException($"Не существует пользователя с id = {id}");
            return user;
        }

        public List<User> GetAllUsers()
        {
            return users.Select(x => x.Value).ToList();
        }
    }
}
