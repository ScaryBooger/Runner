using Microsoft.AspNetCore.Mvc;
using WebApplication1.Interfaces;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet("User")]
        public async Task<IActionResult> Index()
        {
            var users = await _userRepository.GetAppUsers();
            List<UserViewModel> result= new List<UserViewModel>();
            foreach (var item in users)
            {
                var userViewModel = new UserViewModel
                {
                    Id=item.Id,
                    UserName = item.UserName,
                    Pace = item.Pace,
                    Mileage = item.Mileage,
                };
                result.Add(userViewModel);              
            }
          
            return View(result);
        }
        public async Task<IActionResult> Detail(string id) { 
        
            var user=await _userRepository.GetUserById(id);
            var userDetailViewModel = new UserDetailViewModel
            {
                Id = id,
                UserName=user.UserName,
                Pace=user.Pace,
                Mileage=user.Mileage

            };
            return View(userDetailViewModel);
        
        
        }
    }
}
