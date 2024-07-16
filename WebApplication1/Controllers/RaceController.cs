using Microsoft.AspNetCore.Mvc;
using WebApplication1.Interfaces;
using WebApplication1.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    public class RaceController : Controller
    {
        private readonly IPhotoService photoService;
        private readonly IRaceRepository raceRepository;
        private readonly IHttpContextAccessor httpContextAccessor;
        public RaceController(IPhotoService photoService, IRaceRepository raceRepository, IHttpContextAccessor httpContextAccessor)
        {
            this.photoService = photoService;
            this.raceRepository = raceRepository;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Race> races= await raceRepository.GetAll();
            return View(races);
        }
        public async Task<IActionResult> Detail(int id) {

            Race races = await raceRepository.GetByIdAsync(id);

            return View(races);
        
        
        }
        public IActionResult Create() {
            var userId=httpContextAccessor.HttpContext.User.GetUserId();
            var createRaceViewModel = new CreateRaceViewModel
            {
                AppUserId = userId,

            };
            return View(createRaceViewModel);
         
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateRaceViewModel RaceVM) {
            if (ModelState.IsValid)
            {
                var result = await photoService.AddPhotoAsync(RaceVM.Image);
                var race = new Race
                {
                    Title = RaceVM.Title,
                    AppUserId=RaceVM.AppUserId,
                    Description = RaceVM.Description,
                    Image = result.Uri.ToString(),
                    RaceCategory=RaceVM.RaceCategory,
                    Address = new Address
                    {
                        City = RaceVM.Address.City,
                        State = RaceVM.Address.State,
                        Street = RaceVM.Address.Street,
                    }
                };
                raceRepository.Add(race);
                return Redirect("index");
            }
            else {
                ModelState.AddModelError("", "Photo upload failed");
                return View(RaceVM);
            }             
        }
        public async Task<IActionResult> Edit(int id) { 
            var race= await raceRepository.GetByIdAsync((int)id);
            if (race != null)
            {

                var raceVM = new EditRaceViewModel
                {
                    Id = id,
                    Title = race.Title,
                    Description = race.Description,
                    Url = race.Image,
                    RaceCategory = race.RaceCategory,
                    Address = race.Address,
                    AddressId = race.AddressId,
                };
                return View(raceVM);
            }
            else {
                return View("Error");
            }     
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditRaceViewModel raceVM) {

            if (ModelState.IsValid)
            {

                var raceUser = await raceRepository.GetByIdAsyncNotracking(id);
                if (raceUser != null)
                {
                    try
                    {
                        photoService.DeletePhotoAsync(raceUser.Image);
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "Cloud not delete Photo");
                        return View(raceVM);
                    }
                }
                var PhotoResult = await photoService.AddPhotoAsync(raceVM.Image);
                var race = new Race
                {
                    Id = id,
                    Title = raceVM.Title,
                    Description = raceVM.Description,
                    Image = PhotoResult.Url.ToString(),
                    RaceCategory = raceVM.RaceCategory,
                    Address = raceVM.Address,
                    AddressId = raceVM.AddressId,
                };
                raceRepository.Update(race);
                return RedirectToAction("Index");
            }
            else {
                ModelState.AddModelError("", "Failed to edit Race");
                return View("Edit",raceVM);
            
            }
            
          
        }
        public async Task<IActionResult> Delete(int id) { 
        
            var RaceDetail=await raceRepository.GetByIdAsync(id);
            if (RaceDetail == null) return View("Error");

            return View(RaceDetail);


        
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteRace(int id) { 
            
            var RaceDetail= await raceRepository.GetByIdAsync(id);
            if (RaceDetail == null) return View("Error");
            raceRepository.Delete(RaceDetail);
            return RedirectToAction("Index");

        
        }






    }
}
