using AuthApp.Areas.Doctor.Models;
using AuthApp.Models;
using AuthAppBusiness.Constants;
using AuthAppBusiness.Interfaces;
using AuthAppBusiness.Models;
using AuthAppDataAccess.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.Protocol.Core.Types;

namespace AuthApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = RolesConstants.SuperAdmin)]
    public class DoctorController : Controller
    {
        private readonly IDoctor _iDoctor;
        private readonly IUser _iUser;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly RoleManager<Role> _roleManager;
        public DoctorController(IMapper mapper, UserManager<User> userManager, IUser user, IDoctor doctor, RoleManager<Role> roleManager)
        {
            _iDoctor = doctor;
            _mapper = mapper;
            _iUser = user;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public IActionResult Index()
        {
            var Roles = _mapper.Map<List<RolesMV>>(_roleManager.Roles.Where(r => r.Type == (int)AuthAppBusiness.Constants.RoleType.Doctor).ToList());
            ViewBag.Roles = Roles;
            return View();
        }
        [HttpPost]
        public IActionResult List(DoctorSearchMV doctorSearchMV)
        {
            var data = _iDoctor.GetDoctors(doctorSearchMV);
            var allDoctorsCount = _iDoctor.DoctorsCount();
            var doctorList = new ListDoctorsMV
            {
                DoctorsList = _mapper.Map<List<DoctorMV>>(data.Item2),
                ListCount = data.Item1,
            };

            return Json(new
            {
                draw = doctorSearchMV.Draw,
                recordsTotal = allDoctorsCount,
                recordsFiltered = doctorList.ListCount,
                data = doctorList.DoctorsList
            });
        }

        [HttpGet]
        public IActionResult AddEdit( int doctorId)
        {

            var doctorModel = doctorId > 0 ? _mapper.Map<DoctorMV>(_iDoctor.GetDoctorById(doctorId)) : new DoctorMV();
            var Roles = _mapper.Map<List<RolesMV>>(_roleManager.Roles.ToList());
            ViewBag.Roles = Roles.Where(r => r.Type == (int)AuthAppBusiness.Constants.RoleType.Doctor).Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Name.ToString()
            }).ToList();
            return PartialView("_AddEdit", doctorModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddEdit(DoctorMV doctor)
        {

            var result = new ResultMV<string>();
            int res = 0;
            int userId = doctor.UserId ?? 0;

            var user = new UserMV()
            {
                Email = doctor.User.Email,
                Gender = doctor.User.Gender,
                phoneNumber = doctor.User.phoneNumber,
                LastName = doctor.User.LastName,
                FirstName = doctor.User.FirstName
            };
            var role = new RolesMV { Name = doctor.User.SelectedRole, Type = (int)RoleType.Doctor };
            var roles = new List<RolesMV>() {role };
            #region edit Doctor
            if (doctor.Id > 0)
            {
                if (_iUser.GetUserById(doctor.UserId ?? 0) != null)
                {
                    user.Id = doctor.UserId??0;
                    res = _iUser.UpdateUser(_mapper.Map<User>(user), _mapper.Map<List<Role>>(roles) );
                  

                }

            }
            #endregion
            #region addDoctor
            else
            {
                user.Type = (int)UserType.Doctor;
                userId = _iUser.AddUser(_mapper.Map<User>(user), "Doctor123!",_mapper.Map<List<Role>>(roles));
                if (userId > 0)
                {
                    var newDoctor = new DoctorMV
                    {
                        UserId = userId,
                        UniversityDegree = doctor.UniversityDegree
                    };
                    res = _iDoctor.AddDoctor(_mapper.Map<AuthAppDataAccess.Models.Doctor>(newDoctor));

                }
            }
            #endregion
            if (res > 0)
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
                    Data = res.ToString(),
                    Status = Status.Error,
                    ServerInfo = new ServerInfo
                    {
                        CustomeStatusCode = CustomeStatusCodes.InternalServerError,
                        Message = "something went wrong"
                    }
                };
            }

            return Ok(result);

        }
    }
}
