using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using WebApi.DataAccessLayer.Context;
using WebApi.DataAccessLayer.Entities;
using WebApi.Helpers;
using WebApi.Models.UIModels;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace WebApi.Services.User
{
    public class UserService : IUserService
    {
        private DataContext _context;
        private IMapper _mapper;
        public UserService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public UserModel Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;

            var user = _context.Users.SingleOrDefault(x => x.Username == username);

            // check if username exists
            if (user == null)
                return null;

            // check if password is correct
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            // authentication successful
            return user;
        }

        public IEnumerable<AspnetUser> GetAll()
        {
            return _context.AspnetUsers.Include(p => p.UserProfiles);
                //Where(p=>p.LockoutEnabled && p.UserProfiles.Where(q=>q.IsActive).Count()>0);
        }

        public UserDto GetById(string  id)
        {
            var data= _context.UserProfiles.Include(p=>p.AspNetUsers).Where(p=>p.UserId==id);
            if (data != null && data.Count() > 0)
            {
                var model = data.FirstOrDefault();
                return new UserDto
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.AspNetUsers.Email,
                    Phone = model.AspNetUsers.PhoneNumber,
          
                };
            }
            else
            {
                return new UserDto();
            }
        }

        public UserModel Create(UserModel user, string password)
        {
            // validation
            if (string.IsNullOrWhiteSpace(password))
                throw new AppException("Password is required");

            if (_context.Users.Any(x => x.Username == user.Username))
                throw new AppException("Username \"" + user.Username + "\" is already taken");

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            _context.Users.Add(user);
            _context.SaveChanges();

            return user;
        }

        public int CreateUserProfile(UserDto profile,string loggeduser)
        {
            var model = new UserProfile
            {
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                UserId = _context.AspnetUsers.Where(p => p.UserName == profile.Username).FirstOrDefault().Id,
                IsActive = true,
                InsertedDate = DateTime.Now,
                InsertedBy = loggeduser
            };
            _context.UserProfiles.Add(model);
            return  _context.SaveChanges();
        }

        public int Update(UserDto userParam, string loggeduser)
        {
            var user = _context.UserProfiles.Include(p=>p.AspNetUsers).Where(p=>p.Id==userParam.Id).FirstOrDefault();

            if (user == null)
                throw new AppException("User not found");

        
            // update user properties
            user.FirstName = userParam.FirstName;
            user.LastName = userParam.LastName;
            user.UpdatedDate = DateTime.Now;
            user.AspNetUsers.Email = userParam.Email;
            user.AspNetUsers.PhoneNumber = userParam.Phone;
            user.UpdatedBy = loggeduser;
         return _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var user = _context.UserProfiles.Include(p => p.AspNetUsers).Where(p => p.Id == id).FirstOrDefault();
            if (user != null)
            {
                user.IsActive = false;
                user.AspNetUsers.LockoutEnabled = false;
                _context.SaveChanges();
            }
        }

        public List<RoleUIModel> GetRoles()
        {
           return _mapper.Map<List<RoleUIModel>>(_context.Roles.ToList());
       
        }

        // private helper methods

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }


    }
}
