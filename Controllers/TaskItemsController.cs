using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagerApi.Data;
using TaskManagerApi.Models;

namespace TaskManagerApi.Controllers
{
    [Authorize]
    public class TaskItemsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public TaskItemsController(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: TaskItems
        public async Task<IActionResult> Index(string? filter, string? sort)
        {
            var user = await _userManager.GetUserAsync(User);
            var tasks = _context.Tasks.Where(t => t.UserId == user.Id);

            if (!string.IsNullOrEmpty(filter))
            {
                if (filter == "completed") tasks = tasks.Where(t => t.IsCompleted);
                else if (filter == "incomplete") tasks = tasks.Where(t => !t.IsCompleted);
                else if (filter == "overdue") tasks = tasks.Where(t => t.Deadline < DateTime.Today && !t.IsCompleted);
                else if (filter == "upcoming") tasks = tasks.Where(t => t.Deadline >= DateTime.Today);
            }

            if (!string.IsNullOrEmpty(sort))
            {
                if (sort == "priority") tasks = tasks.OrderByDescending(t => t.Priority);
                else if (sort == "deadline") tasks = tasks.OrderBy(t => t.Deadline);
                else if (sort == "created") tasks = tasks.OrderByDescending(t => t.CreatedAt);
            }

            return View(await tasks.ToListAsync());
        }

        // POST: TaskItems/ToggleComplete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleComplete(int id, [FromBody] bool isCompleted)
        {
            var user = await _userManager.GetUserAsync(User);
            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id && t.UserId == user.Id);

            if (task == null) return NotFound();

            task.IsCompleted = isCompleted;
            task.CompletedAt = isCompleted ? DateTime.UtcNow : null;

            await _context.SaveChangesAsync();
            return Ok();
        }

        // GET: TaskItems/Create
        public IActionResult Create() => View();

        // POST: TaskItems/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TaskItem taskItem)
        {
            var user = await _userManager.GetUserAsync(User);
            if (ModelState.IsValid)
            {
                taskItem.UserId = user.Id;
                taskItem.CreatedAt = DateTime.UtcNow;
                if (taskItem.IsCompleted) taskItem.CompletedAt = DateTime.UtcNow;

                _context.Add(taskItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(taskItem);
        }

        // GET: TaskItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var user = await _userManager.GetUserAsync(User);
            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id && t.UserId == user.Id);
            if (task == null) return NotFound();

            return View(task);
        }

        // POST: TaskItems/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TaskItem taskItem)
        {
            if (id != taskItem.Id) return NotFound();

            var user = await _userManager.GetUserAsync(User);
            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id && t.UserId == user.Id);
            if (task == null) return NotFound();

            if (ModelState.IsValid)
            {
                task.Title = taskItem.Title;
                task.Priority = taskItem.Priority;
                task.Deadline = taskItem.Deadline;
                task.IsCompleted = taskItem.IsCompleted;
                task.CompletedAt = taskItem.IsCompleted ? DateTime.UtcNow : null;

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(taskItem);
        }

        // GET: TaskItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var user = await _userManager.GetUserAsync(User);
            var task = await _context.Tasks.FirstOrDefaultAsync(m => m.Id == id && m.UserId == user.Id);
            if (task == null) return NotFound();

            return View(task);
        }

        // GET: TaskItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var user = await _userManager.GetUserAsync(User);
            var task = await _context.Tasks.FirstOrDefaultAsync(m => m.Id == id && m.UserId == user.Id);
            if (task == null) return NotFound();

            return View(task);
        }

        // POST: TaskItems/DeleteConfirmed
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id && t.UserId == user.Id);
            if (task != null)
            {
                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
