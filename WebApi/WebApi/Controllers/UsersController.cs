using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApi.DataAccessLayer.Entities;
using WebApi.Helpers;
using WebApi.Models;
using WebApi.Models.UIModels;
using WebApi.Services;
using WebApi.Services.User;

namespace WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        private readonly UserManager<ApplicationUser> _userManager;
        private IPasswordHasher<ApplicationUser> _passwordHasher;

        public UsersController(UserManager<ApplicationUser> userManager,
            IUserService userService,
            IMapper mapper,
            IOptions<AppSettings> appSettings,
            IPasswordHasher<ApplicationUser> passwordHasher)
        {
            _userService = userService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
            _userManager = userManager;
            _passwordHasher = passwordHasher;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> AuthenticateAsync([FromBody]UserDto userDto)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(userDto.Username);




                if (user == null)
                    return Ok(new { status=1, message = "Username or password is incorrect" });

                if (_passwordHasher.VerifyHashedPassword(user, user.PasswordHash, userDto.Password) == PasswordVerificationResult.Success)
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[]
                        {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                        }),
                        Expires = DateTime.UtcNow.AddDays(7),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                    };
                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    var tokenString = tokenHandler.WriteToken(token);

                    var model=_userService.GetById(user.Id);

                    // return basic user info (without password) and token to store client side
                    return Ok(new
                    {
                        Id = user.Id,
                        Username = user.UserName,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Phone=model.Phone,
                        Email=model.Email,
                        Token = tokenString
                    });

                }

                return Ok(new { status = user.LockoutEnabled?2:3, message = "Invalid credentials" });//here 2 -invaid password, 3- locked
            }catch(Exception ex)
            {
                return BadRequest(new { message =ex.ToString() });
            }
              
       
        }

        //[AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody]UserDto userDto)
        {
            // map dto to entity
            // var user = _mapper.Map<User>(userDto);

            try
            {
                // save 
                // _userService.Create(user, userDto.Password);
                if (userDto.Id > 0)
                {
                   _userService.Update(userDto, HttpContext.User.Identity.Name);
                   return Ok(); 
                }
                else
                {

                    var user = new ApplicationUser()
                    {
                        UserName = userDto.Username,
                        Email = userDto.Email,
                        PhoneNumber = userDto.Phone
                    };

                    var result = await _userManager.CreateAsync(user, userDto.Password);

                    ApplicationUser usr = await _userManager.FindByNameAsync(userDto.Username);
                    await _userManager.AddToRoleAsync(usr, userDto.Role);

                    _userService.CreateUserProfile(userDto, HttpContext.User.Identity.Name);
                    if (!result.Succeeded)
                    {
                        return BadRequest(new { message = "Unable to register, try again later" });
                    }
                    else
                    {

                        return Ok();
                    }
                }
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult GetAll()
        {
    
            var userDtos = _userService.GetAll().Select(p=>new UserDto {
                Id= p.UserProfiles.Count() > 0 ? p.UserProfiles.First().Id : 0,
                Username =p.UserName,
                FirstName= p.UserProfiles.Count()>0?p.UserProfiles.First().FirstName:string.Empty,
                LastName = p.UserProfiles.Count() > 0 ? p.UserProfiles.First().LastName : string.Empty,
                Email=p.Email,
                Phone=p.PhoneNumber,
                IsActive=p.LockoutEnabled,
                RegistrationDate = p.UserProfiles.Count() > 0 ? p.UserProfiles.First().InsertedDate :DateTime.Now.AddYears(100),
             //   Role =  : string.Empty,

            }).OrderByDescending(p=>p.RegistrationDate).ToList();
           //var userDtos = _mapper.Map<IList<UserDto>>(users);
            return Ok(userDtos);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(string id)
        {
            var user = _userService.GetById(id);
          //  var userDto = _mapper.Map<UserDto>(user);
            return Ok(user);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody]UserDto userDto)
        {
            // map dto to entity and set id
            var user = _mapper.Map<UserModel>(userDto);
            user.Id = id;

            try
            {
                // save 
                //_userService.Update(user, userDto.Password);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _userService.Delete(id);
            return Ok();
        }
        [HttpGet]
        [Route("~/Roles")]
        public IActionResult GetRoles()
        {
           var model= _userService.GetRoles();
            return Ok(model);
        }
    }
}
