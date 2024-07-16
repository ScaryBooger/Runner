using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.Interfaces;
using WebApplication1.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDashboardRepsitory dashboardRepsitory;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IPhotoService photoService;
        public DashboardController(IDashboardRepsitory dashboardRepsitory,IHttpContextAccessor httpContextAccessor, IPhotoService photoService)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.dashboardRepsitory = dashboardRepsitory;
            this.photoService = photoService;
        }

        public async Task<IActionResult> Index()
        {
            var userRaces=await dashboardRepsitory.GetAllUuserRaces();
            var userClubs=await dashboardRepsitory.GetAllUserClubs();
            var userViewModel = new DashboardViewModel()
            {
                Races = userRaces,
                Clubs = userClubs
            };                        
            return View(userViewModel);
        }
        public async Task<IActionResult> EditUserProfile() { 

            var curUserId=httpContextAccessor.HttpContext.User.GetUserId();
            var user=await dashboardRepsitory.GetUserById(curUserId);
            if (user == null) return View("Error");
            var editUserViewModel = new EditUserDashboardViewModel
            {
                Id=curUserId,
                Pace=user.Pace,
                Mileage=user.Mileage,
                ProfileImageUrl=user.ProfileImageUrl,
                City=user.City,
                State=user.State,


            };
            return View(editUserViewModel);
        }
        private void MapUserEdit(AppUser user, EditUserDashboardViewModel edit, ImageUploadResult photoResult) { 
        
            user.Id = edit.Id;
            user.Pace = edit.Pace;
            user.Mileage = edit.Mileage;
            user.ProfileImageUrl=photoResult.Url.ToString();
            user.City= edit.City;
            user.State=edit.State;

        }
        [HttpPost]
        public async Task<IActionResult> EditUserProfile(EditUserDashboardViewModel viewModel) {

            if (!ModelState.IsValid) {

                ModelState.AddModelError("", "Failed to edit profile");
                return View("EditUserProfile",viewModel);
            
            }
            AppUser user = await dashboardRepsitory.GetUserByIdNoTracking(viewModel.Id);

            if (user.ProfileImageUrl == "" || user.ProfileImageUrl == null)
            {

                var photoResult = await photoService.AddPhotoAsync(viewModel.Image);

                MapUserEdit(user, viewModel, photoResult);
               
                dashboardRepsitory.Update(user);
                return RedirectToAction("Index");
            }
            else
            {
                try
                {
                    await photoService.DeletePhotoAsync(user.ProfileImageUrl);
                }
                catch (Exception ex) {
                    ModelState.AddModelError("", "could not delete photo");
                    return View(viewModel);

                }
                var photoResult = await photoService.AddPhotoAsync(viewModel.Image);
                MapUserEdit(user, viewModel, photoResult);
                dashboardRepsitory.Update(user);
                return RedirectToAction("Index");

            }
        }

    }
}
