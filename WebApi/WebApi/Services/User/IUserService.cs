using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.DataAccessLayer.Entities;
using WebApi.Models.UIModels;

namespace WebApi.Services.User
{
   public interface IUserService
    {
         UserModel Authenticate(string username, string password);
        IEnumerable<AspnetUser> GetAll();
        UserDto GetById(string id);
        UserModel Create(UserModel user, string password);

        int CreateUserProfile(UserDto profile, string loggeduser);
        int Update(UserDto user, string loggeduser);
        void Delete(int id);
        List<RoleUIModel> GetRoles();
    }
}

