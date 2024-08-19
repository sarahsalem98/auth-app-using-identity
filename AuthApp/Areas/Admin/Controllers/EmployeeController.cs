using AuthApp.Models;
using AuthAppBusiness.Constants;
using AuthAppBusiness.Interfaces;
using AuthAppBusiness.Models;
using AuthAppDataAccess.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AuthApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EmployeeController : Controller
    {
        private readonly IUser _iUser;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly RoleManager<Role> _roleManager;
        public EmployeeController(IMapper mapper, UserManager<User> userManager, IUser user, RoleManager<Role> roleManager)
        {
            _mapper = mapper;
            _iUser = user;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public IActionResult Index()
        {
            var Roles = _mapper.Map<List<RolesMV>>(_roleManager.Roles.Where(r => r.Type == (int)AuthAppBusiness.Constants.RoleType.Employee).ToList());
            ViewBag.Roles = Roles;
            return View();
        }
        [HttpPost]
        public IActionResult List(UserSearchMV SearchMV)
        {
            var data = _iUser.GetUsers(SearchMV);
            var Count = _iUser.UsersCount();
            var usersList = new ListUsersMV
            {
                UsersList = _mapper.Map<List<UserMV>>(data.Item2),
                ListCount = data.Item1,
            };

            return Json(new
            {
                draw = SearchMV.Draw,
                recordsTotal = Count,
                recordsFiltered = usersList.ListCount,
                data = usersList.UsersList
            });
        }
        [HttpGet]
        public IActionResult AddEdit( int Id)
        {

            var Model = Id > 0 ? _mapper.Map<UserMV>(_iUser.GetUserById(Id)) : new UserMV();
            var Roles = _mapper.Map<List<RolesMV>>(_roleManager.Roles.ToList());
            ViewBag.Roles = Roles.Where(r => r.Type == (int)AuthAppBusiness.Constants.RoleType.Employee).Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Name.ToString()
            }).ToList();
            return PartialView("_AddEdit", Model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddEdit(UserMV model)
        
        {

            var result = new ResultMV<string>();
            int res = 0;
            int userId = model.Id ;

            var user = new UserMV()
            {
                Email = model.Email,
                Gender = model.Gender,
                phoneNumber = model.phoneNumber,
                LastName = model.LastName,
                FirstName = model.FirstName
            };
            var role = new RolesMV {Name = model.SelectedRole, Type = (int)RoleType.Employee };
            var roles = new List<RolesMV>() {role };

            #region editUser
            if (model.Id > 0)
            {
                if (_iUser.GetUserById(model.Id) != null)
                {
                    user.Id = model.Id;
                    res = _iUser.UpdateUser(_mapper.Map<User>(user), _mapper.Map<List<Role>>(roles));


                }

            }
            #endregion
            #region addUser
            else
            {
                user.Type = (int)RoleType.Employee; 
                userId = _iUser.AddUser(_mapper.Map<User>(user), "Employee123!",_mapper.Map<List<Role>>(roles));
              
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
