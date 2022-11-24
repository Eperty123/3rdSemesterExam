using Application.DTOs;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUserRepository
    {
        List<User> ReadAllUsers();
        User CreateUser(User user);
        User GetUserById(int id);
        User GetUserByEmail(string email);
        User GetUserByUsername(string username);
        User DeleteUser(int id);
        User UpdateUser(int id, User user);
        void RebuildDB();
    }
}
