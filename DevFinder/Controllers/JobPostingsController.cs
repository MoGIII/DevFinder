using DevFinder.Constants;
using DevFinder.Models;
using DevFinder.Repositories;
using DevFinder.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DevFinder.Controllers
{
    [Authorize]
    public class JobPostingsController : Controller
    {
        private readonly IRepository<JobPosting> _repository;
        private readonly UserManager<IdentityUser> _userManager;

        public JobPostingsController(IRepository<JobPosting> repository, UserManager<IdentityUser> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }


        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var AllJobPostings = await _repository.GetAllAsync();
            if (User.IsInRole(Roles.Employer))
            {
                var userId = _userManager.GetUserId(User);
                var filteredJobPostings = AllJobPostings.Where(jobPost => jobPost.UserId == userId);
                return View(filteredJobPostings);
            }
            return View(AllJobPostings);
        }

        [Authorize(Roles = $"{Roles.Admin},{Roles.Employer}")]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Roles = $"{Roles.Admin},{Roles.Employer}")]
        public async Task<IActionResult> Create(JobPostingViewModel jobPostingVm)
        {


            if (ModelState.IsValid)
            {
                var jobPosting = new JobPosting
                {
                    Title = jobPostingVm.Title,
                    Description = jobPostingVm.Description,
                    Location = jobPostingVm.Location,
                    Company = jobPostingVm.Company,
                    UserId = _userManager.GetUserId(User)
                };
                await _repository.AddAsync(jobPosting);
                return RedirectToAction(nameof(Index));
            }
            return View(jobPostingVm);
        }

        [HttpDelete]
        [Authorize(Roles = $"{Roles.Admin},{Roles.Employer}")]
        public async Task<IActionResult> Delete(int id)
        {
            var jobPosting = await _repository.GetByIdAsync(id);
            if (jobPosting == null)
            {
                return NotFound();
            }
            var userId = _userManager.GetUserId(User);
            if (User.IsInRole(Roles.Admin) == false && jobPosting.UserId != userId)
            {
                return Forbid();
            }
            await _repository.DeleteAsync(id);

            return Ok();
        }

        //[Authorize(Roles = $"{Roles.Admin},{Roles.Employer}")]
        //public async Task<IActionResult> DeleteEasy(int id)
        //{
        //    var jobPosting = await _repository.GetByIdAsync(id);
        //    if (jobPosting == null)
        //    {
        //        return NotFound();
        //    }
        //    var userId = _userManager.GetUserId(User);
        //    if (User.IsInRole(Roles.Admin) == false && jobPosting.UserId != userId)
        //    {
        //        return Forbid();
        //    }
        //    await _repository.DeleteAsync(id);

        //    return RedirectToAction(nameof(Index));
        //}
    }
}
