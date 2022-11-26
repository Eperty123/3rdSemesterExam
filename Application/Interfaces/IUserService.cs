using Application.DTOs;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUserService
    {
        List<User> GetAllUsers();
        User CreateUser(RegisterUserDTO dto);
        User GetUser(int id);
        User GetUserByUsername(LoginUserDTO dto);
        User DeleteUser(int id);
        User UpdateUser(int id, User user);
        void RebuildDB();
    }
}
