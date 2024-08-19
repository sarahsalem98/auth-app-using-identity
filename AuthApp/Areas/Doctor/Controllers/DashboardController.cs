using AuthAppBusiness.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthApp.Areas.Doctor.Controllers
{
    [Area("Doctor")]

    public class DashboardController : Controller
    {
        [Authorize(Policy=nameof(UserType.Doctor)+":"+ nameof(Doctor_Pages.Dashboard)+":"+nameof(Operations.Index))]
        public IActionResult Index()
        {
            return View();
        }
    }
}
