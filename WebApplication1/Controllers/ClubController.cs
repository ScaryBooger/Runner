using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Contracts;
using WebApplication1.Interfaces;
using WebApplication1.Models;
using WebApplication1.ViewModels;


namespace WebApplication1.Controllers
{
    public class ClubController : Controller
    {
        private readonly IClublRepository clubRepository;
        private readonly IPhotoService photoService;
        private readonly IHttpContextAccessor httpContextAccessor;
        public ClubController(IClublRepository clubRepository ,IPhotoService photoService,IHttpContextAccessor httpContextAccessor) {
            this.httpContextAccessor = httpContextAccessor;
            this.clubRepository = clubRepository;
            this.photoService = photoService;
        }
        public async Task<IActionResult> Index()
        {
           IEnumerable<Club> clubs = await clubRepository.GetAll();

            return View(clubs);
        }
        public async Task<IActionResult> Detail(int id) {

            Club club = await clubRepository.GetByIdAsync(id);
            return View(club);
        }
        public IActionResult Create()
        {
            var curUserId = httpContextAccessor.HttpContext?.User.GetUserId();
            var createClubViewModel = new CreateClubViewModel() { 
                AppUserId=curUserId,
                        
            };
            return View(createClubViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateClubViewModel clubVM) {

            if (ModelState.IsValid)
            {
                var result = await photoService.AddPhotoAsync(clubVM.Image);
                var club = new Club
                {   
                    AppUserId=clubVM.AppUserId,
                    Title = clubVM.Title,
                    Description = clubVM.Description,
                    Image =result.Uri.ToString(),
                    ClubCategory=clubVM.ClubCategory,
                    Address = new Address
                    {
                        City = clubVM.Address.City,
                        State = clubVM.Address.State,
                        Street = clubVM.Address.Street,
                    }
                };
                clubRepository.Add(club);
                return Redirect("index");
            }
            else {
                ModelState.AddModelError("", "Photo upload failed");
                return View();            
            }        
        }
        public async Task<IActionResult> Edit(int id) {
            var club = await clubRepository.GetByIdAsync(id);
            if (club==null) return View("Error");
            var clVM = new EditClubViewModel
            {
                Title = club.Title,
                Description=club.Description,
                AddressId=club.AddressId,
                Address=club.Address,
                URL=club.Image,
                ClubCategory=club.ClubCategory,
            };
            return View(clVM);
        }
      

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditClubViewModel clubVM) {

            if (ModelState.IsValid)
            {
                var userClub = await clubRepository.GetByIdAsyncNotracking(id);
                if (userClub != null) {
                    try
                    {
                        await photoService.DeletePhotoAsync(userClub.Image);
                    }
                    catch (Exception ex) {
                        ModelState.AddModelError(",", "Cloud not delete photo");
                        return View(clubVM);
                    }                                
                }
                var PhotoResult = await photoService.AddPhotoAsync(clubVM.Image);
                var club = new Club
                {
                    Id = id,
                    Title = clubVM.Title,
                    Description = clubVM.Description,
                    Image = PhotoResult.Url.ToString(),
                    AddressId = clubVM.AddressId,
                    Address = clubVM.Address,

                };
                clubRepository.Update(club);
                return RedirectToAction("Index");
            }
            else {
                ModelState.AddModelError(",", "Failed to edit club");
                return View("edit",clubVM);
            }      
        }


        public async Task<IActionResult> Delete(int id) { 
        
            var cludbDetails=await clubRepository.GetByIdAsync(id);
            if (cludbDetails == null) return View("Error");
            return View(cludbDetails);           
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteClub(int id) { 
            var clubDetails=await clubRepository.GetByIdAsync(id);
            if (clubDetails == null) return View("Error");
            clubRepository.Delete(clubDetails);
            return RedirectToAction("Index");

        
        
        }
        


           
        




    }
}
