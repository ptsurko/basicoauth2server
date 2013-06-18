using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OAuth2Server.Infrastructure
{
    public interface IUserRepository
    {
        void Save(User user);
        User Get(string name, string password);
    }

    public class User
    {
        public string Name { get; set; }
        public string Password { get; set; }
    }

    public class InMemoryUserRepository : IUserRepository
    {
        private readonly IList<User> _users = new List<User>();

        public void Save(User user)
        {
            if (Get(user.Name, user.Password) == null)
            {
                _users.Add(user);
            }
        }

        public User Get(string name, string password)
        {
            return _users.FirstOrDefault(user => user.Name == name && user.Password == password);
        }
    }
}