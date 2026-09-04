using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Domain.Entities;
using SchoolManagementSystem.Domain.Interfaces;

namespace SchoolManagementSystem.Web.Controllers
{
    public class TeachersController : Controller
    {
        private readonly IRepository<Teacher> _teacherRepository;
        private readonly IWebHostEnvironment _environment;

        public TeachersController(IRepository<Teacher> teacherRepository, IWebHostEnvironment environment)
        {
            _teacherRepository = teacherRepository;
            _environment = environment;
        }

        public async Task<IActionResult> Index()
        {
            var teachers = await _teacherRepository.Query().OrderBy(t => t.FirstName).ToListAsync();
            return View(teachers);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var teacher = await _teacherRepository.Query()
                .Include(t => t.Subjects)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (teacher is null)
            {
                return NotFound();
            }

            return View(teacher);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("EmployeeId,FirstName,LastName,Qualification,Email,Phone")] Teacher teacher,
            IFormFile? ImageFile)
        {
            if (ModelState.IsValid)
            {
                if (ImageFile is not null && ImageFile.Length > 0)
                {
                    teacher.ImagePath = await SaveImageAsync(ImageFile);
                }

                await _teacherRepository.AddAsync(teacher);
                await _teacherRepository.SaveChangesAsync();
                TempData["SuccessMessage"] = "Teacher created successfully.";
                return RedirectToAction(nameof(Index));
            }

            return View(teacher);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var teacher = await _teacherRepository.GetByIdAsync(id.Value);
            if (teacher is null)
            {
                return NotFound();
            }

            return View(teacher);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
    int id,
    [Bind("Id,EmployeeId,FirstName,LastName,Qualification,Email,Phone,CreatedAt,IsDeleted,ImagePath")] Teacher teacher,
    IFormFile? ImageFile)
        {
            if (id != teacher.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (ImageFile is not null && ImageFile.Length > 0)
                    {
                        teacher.ImagePath = await SaveImageAsync(ImageFile);
                    }
                    _teacherRepository.Update(teacher);
                    await _teacherRepository.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Teacher updated successfully.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _teacherRepository.ExistsAsync(t => t.Id == teacher.Id))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            return View(teacher);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var teacher = await _teacherRepository.GetByIdAsync(id.Value);
            if (teacher is null)
            {
                return NotFound();
            }

            return View(teacher);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var teacher = await _teacherRepository.GetByIdAsync(id);
            if (teacher is not null)
            {
                _teacherRepository.Remove(teacher);
                await _teacherRepository.SaveChangesAsync();
                TempData["SuccessMessage"] = "Teacher deleted successfully.";
            }

            return RedirectToAction(nameof(Index));
        }


        private async Task<string> SaveImageAsync(IFormFile imageFile)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            var extension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(extension))
            {
                throw new InvalidOperationException("Only JPG, PNG, or WEBP images are allowed.");
            }

            var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "teachers");
            Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            return $"/uploads/teachers/{uniqueFileName}";
        }
    }

}
