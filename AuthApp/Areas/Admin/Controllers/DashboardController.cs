
using AuthAppBusiness.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthApp.Areas.Admin.Controllers
{
    [Area("Admin")]

    [Authorize(Policy =nameof(UserType.SuperAdmin))]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        
    }
}
