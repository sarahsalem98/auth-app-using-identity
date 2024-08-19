using AuthApp.Areas.Admin.Models;
using AuthApp.Areas.Doctor.Models;
using AuthApp.Models;
using AuthAppBusiness.Constants;
using AuthAppBusiness.Interfaces;
using AuthAppBusiness.Models;
using AuthAppDataAccess.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using LoginMV = AuthApp.Areas.Doctor.Models.LoginMV;

namespace AuthApp.Areas.Doctor.Controllers
{
    [Area("Doctor")]
    public class AuthController : Controller
    {
        private readonly SignInManager<User> _signInManger;
        private readonly IDoctor doctor;
        private readonly IUser user;
        private readonly IMapper mapper;
        public AuthController(SignInManager<User> signInManager, IUser user, IDoctor doctor, IMapper mapper)
        {
            _signInManger = signInManager;
            this.user = user;   
            this.doctor = doctor;
            this.mapper = mapper;   
        }
        [HttpGet]
        public IActionResult Login()

        {
            return View();
        }
        [HttpGet]
        public IActionResult Register()

        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginMV loginMV)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManger.PasswordSignInAsync(loginMV.Email, loginMV.Password, false, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Dashboard");
                }
                ModelState.AddModelError("", "Invalid login attempt");


            }
            return View(loginMV);


        }


        [HttpPost]
        [ValidateAntiForgeryToken]  
        public IActionResult Register (RegisterMV registerMV)
        {
            var result = new ResultMV<string>();
            int res = 0;
           if (ModelState.IsValid)
           {
                var newUser = new UserMV
                {
                    Age = registerMV.Age,
                    Email = registerMV.Email,
                    Address = registerMV.Address,
                    Gender = registerMV.Gender,
                    phoneNumber = registerMV.phoneNumber,
                    LastName = registerMV.LastName,
                    FirstName = registerMV.FirstName,
                    Type=(int)UserType.Doctor
                };


                int userId = user.AddUser(mapper.Map<User>(newUser), registerMV.Password);
                if (userId > 0)
                {
                    var newDoctor = new DoctorMV
                    {
                        UserId = userId,
                        UniversityDegree = registerMV.UniversityDegree
                    };
                    res = doctor.AddDoctor(mapper.Map<AuthAppDataAccess.Models.Doctor>(newDoctor));

                }
                if (res > 0)
                {
                     if(_signInManger.PasswordSignInAsync(registerMV.Email, registerMV.Password, false, false).Result.Succeeded)
                    {
                        result = new ResultMV<string>()
                        {
                            Data = res.ToString(),
                            Status = Status.Success,
                            ServerInfo = new ServerInfo
                            {
                                CustomeStatusCode = CustomeStatusCodes.Success,
                                Message = "success"
                            }
                        };
                    }
                    else
                    {
                        result = new ResultMV<string>()
                        {
                            Data ="0",
                            Status = Status.notValid,
                            ServerInfo = new ServerInfo
                            {
                                CustomeStatusCode = CustomeStatusCodes.notValid,
                                Message = "can not sign in"
                            }
                        };
                    }
                }
           


            }
            else
            {
                var errors = ModelState.Values.SelectMany(x => x.Errors)
                                             .Select(e => e.ErrorMessage)
                                             .ToList();
                result = new ResultMV<string>()
                {
                    Data = res.ToString(),
                    Status = Status.notValid,
                    ServerInfo = new ServerInfo
                    {
                        CustomeStatusCode = CustomeStatusCodes.notValid,
                        Message = string.Join(", ", errors) 
                    }
                };
            }


            return Ok(result);

        }
        public async Task<IActionResult> Logout()
        {
            await _signInManger.SignOutAsync();
            return RedirectToAction("Login", "Auth");
        }
        public IActionResult AccessDenied()
        {

            return View();
        }

    }
}
