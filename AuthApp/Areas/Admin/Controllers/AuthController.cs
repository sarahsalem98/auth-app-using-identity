using AuthApp.Areas.Admin.Models;
using AuthAppDataAccess.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AuthApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AuthController : Controller
    {
        private readonly SignInManager<User> _signInManger;
        public AuthController(SignInManager<User> signInManager)
        {
            _signInManger = signInManager;  
            
        }
        [HttpGet]
        public IActionResult Login()
        
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginMV loginMV)
        {
            if(ModelState.IsValid) {
                var result = await _signInManger.PasswordSignInAsync(loginMV.Email, loginMV.Password, false, false);
                if (result.Succeeded)
                {
                    return  RedirectToAction("Index", "Dashboard");  
                }
                ModelState.AddModelError("", "Invalid login attempt");
            
            
            }
            return View( loginMV);  


        }

        public IActionResult AccessDenied () { 
        
        return View();  
        }


        public async Task<IActionResult> Logout()
        {
             await _signInManger.SignOutAsync();
            return RedirectToAction("Login", "Auth");
        }
       
    }
}
