using TaskMIS.Data;
using TaskMIS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace TaskMIS.Controllers
{
    public class DailyTaskController : Controller
    {
        private readonly ApplicationDbContext _db;
        public DailyTaskController(ApplicationDbContext db)
        {
            _db = db;
            
        }
      
        public IActionResult Index()
        {
            // Get user ID
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); 
            var tasks = _db.Tasks.Where(t => t.UserId == int.Parse(userId)).ToList();
            return View(tasks);
        }

    [Authorize]
    public IActionResult Create()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            
            ModelState.AddModelError("Error", "User session is not valid. Please log in again.");
            return View("Error"); 
        }

        if (!int.TryParse(userId, out int parsedUserId))
        {
            ModelState.AddModelError("Error", "Failed to parse user ID. Please log in again.");
            return View("Error"); 
        }

        var tasks = _db.Tasks.Where(t => t.UserId == parsedUserId).ToList();
        if (tasks == null)
        {
            ModelState.AddModelError("Error", "Unable to load tasks.");
            return View("Error");
        }

        var viewModel = new TaskViewModel
        {
            Tasks = tasks
        };

        return View(viewModel);
    }

     // Save the tasks to database with userId

    [HttpPost]
        public async Task<IActionResult> Create(TaskViewModel model)
        {
            if (model.Task != null)
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (int.TryParse(userId, out int userIntId))
                {
                    model.Task.UserId = userIntId; 

                    _db.Tasks.Add(model.Task);
                    await _db.SaveChangesAsync();

                    return RedirectToAction("Create");
                }
                else
                {
                    ModelState.AddModelError("", "Error: User ID is invalid.");
                }
            }
            // Refresh the list displayed
            model.Tasks = _db.Tasks.ToList();  
            return View(model);
        }

     //Update the status from Active ot Expired

     [HttpPost]
        public IActionResult TaskCompleted(int id)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdString, out int userId))
            {
                return Unauthorized("Error: Unable to verify user identity.");
            }

            var task = _db.Tasks.FirstOrDefault(t => t.Id == id && t.UserId == userId);
            if (task == null)
            {
                return Unauthorized("Error: You are not authorized to modify this task.");
            }

            task.Status = !task.Status;
            _db.SaveChanges(); 

            return RedirectToAction("Create"); 
        }


    }
}
