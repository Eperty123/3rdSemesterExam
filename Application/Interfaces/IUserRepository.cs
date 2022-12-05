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
        User ReadUserById(int id);
        User ReadUserByEmail(string email);
        User ReadUserByUsername(string username);
        User DeleteUser(int id);
        User UpdateUser(int id, User user, string oldPassword);

        Coach ReadCoachById(int id);
    }
}
