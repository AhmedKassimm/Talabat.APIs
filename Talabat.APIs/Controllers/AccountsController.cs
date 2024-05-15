using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.APIs.Extentions;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services;

namespace Talabat.APIs.Controllers
{
    public class AccountsController : ApiBaseController
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly ITokenService tokenService;
        private readonly IMapper mapper;

        public AccountsController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager,ITokenService tokenService,IMapper mapper) 
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.tokenService = tokenService;
            this.mapper = mapper;
        }

        [HttpPost("login")]//post: api/accounts/login
        public async Task<ActionResult<UserDto>> Login(LoginDto model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            if(user is null) return Unauthorized(new ApiErrorResponse(401));
            var result = await signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if(!result.Succeeded) return Unauthorized(new ApiErrorResponse(401));

            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await tokenService.CreateTokenAsync(user,userManager)
            });
        }
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto model)
        {
            if (CheckEmailExist(model.Email).Result.Value)
                return BadRequest(new ApiValidationErrorResponse() { Errors = new string[] { "This Email is Already Exist" } });
            var user = new AppUser()
            {
                DisplayName = model.DisplayName,
                Email = model.Email,//Ahmed.nasser@gmail.com
                UserName = model.Email.Split('@')[0],//Ahmed.nasser
                PhoneNumber = model.PhoneNumber
            };
            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded) return BadRequest(new ApiErrorResponse(400));

            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = "This will be token"
            });

        
        }

        [Authorize]
        [HttpGet("currentuser")]// Get: api/accounts/currentuser
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var email=User.FindFirstValue(ClaimTypes.Email);
            var user= await userManager.FindByIdAsync(email);
            return Ok(new UserDto() 
            {
            DisplayName=user.DisplayName,
            Email = user.Email,
            Token= await tokenService.CreateTokenAsync(user, userManager)
            });
        }

        [Authorize]
        [HttpGet("Address")]
        public async Task<ActionResult<AddressDto>> GetUserAddress()
        {
            
            var user= await userManager.FindUserWithAddressByEmailAsync(User);
            var address= mapper.Map<Address,AddressDto>(user.Address);
            return Ok(address);
        }
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto Updatedaddress)
        {
            var address= mapper.Map<AddressDto,Address>(Updatedaddress);
            var user = await userManager.FindUserWithAddressByEmailAsync(User);
            address.Id = user.Address.Id;
            user.Address = address;
            var Result= await userManager.UpdateAsync(user);
            if(!Result.Succeeded) return BadRequest(new ApiErrorResponse(400));

            return Ok(Updatedaddress);
        }

        [HttpGet("CheckEmail")]
        public async Task<ActionResult<bool>> CheckEmailExist(string email)
        {
            return await userManager.FindByEmailAsync(email) is not null;// True False
        }
    }
}
