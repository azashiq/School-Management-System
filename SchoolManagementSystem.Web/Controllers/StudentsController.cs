using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Domain.Entities;
using SchoolManagementSystem.Domain.Interfaces;

namespace SchoolManagementSystem.Web.Controllers
{
    public class StudentsController : Controller
    {
        private readonly IRepository<Student> _studentRepository;
        private readonly IRepository<ClassRoom> _classRoomRepository;
        private readonly IWebHostEnvironment _environment;

        public StudentsController(
            IRepository<Student> studentRepository,
            IRepository<ClassRoom> classRoomRepository,
            IWebHostEnvironment environment)
        {
            _studentRepository = studentRepository;
            _classRoomRepository = classRoomRepository;
            _environment = environment;
        }

        public async Task<IActionResult> Index(string? searchTerm)
        {
            var query = _studentRepository.Query().Include(s => s.ClassRoom).AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(s =>
                    s.FirstName.Contains(searchTerm) ||
                    s.LastName.Contains(searchTerm) ||
                    s.RegistrationNumber.Contains(searchTerm));
            }

            ViewData["SearchTerm"] = searchTerm;

            var students = await query.OrderBy(s => s.FirstName).ToListAsync();
            return View(students);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var student = await _studentRepository.Query()
                .Include(s => s.ClassRoom)
                .Include(s => s.Attendances)
                .Include(s => s.Grades)
                .ThenInclude(g => g.Subject)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (student is null)
            {
                return NotFound();
            }

            return View(student);
        }

        public async Task<IActionResult> Create()
        {
            await PopulateClassRoomsAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
         [Bind("RegistrationNumber,FirstName,LastName,DateOfBirth,Gender,Email,PhoneNumber,Address,ClassRoomId")] Student student,
         IFormFile? ImageFile)
        {
            if (ModelState.IsValid)
            {
                if (ImageFile is not null && ImageFile.Length > 0)
                {
                    student.ImagePath = await SaveImageAsync(ImageFile);
                }


                await _studentRepository.AddAsync(student);
                await _studentRepository.SaveChangesAsync();
                TempData["SuccessMessage"] = "Student created successfully.";
                return RedirectToAction(nameof(Index));
            }

            await PopulateClassRoomsAsync(student.ClassRoomId);
            return View(student);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var student = await _studentRepository.GetByIdAsync(id.Value);
            if (student is null)
            {
                return NotFound();
            }

            await PopulateClassRoomsAsync(student.ClassRoomId);
            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
     int id,
     [Bind("Id,RegistrationNumber,FirstName,LastName,DateOfBirth,Gender,Email,PhoneNumber,Address,ClassRoomId,CreatedAt,IsDeleted,ImagePath")] Student student,
     IFormFile? ImageFile)
        {
            if (id != student.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (ImageFile is not null && ImageFile.Length > 0)
                    {
                        student.ImagePath = await SaveImageAsync(ImageFile);
                    }

                    _studentRepository.Update(student);
                    await _studentRepository.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Student updated successfully.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _studentRepository.ExistsAsync(s => s.Id == student.Id))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            await PopulateClassRoomsAsync(student.ClassRoomId);
            return View(student);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var student = await _studentRepository.Query()
                .Include(s => s.ClassRoom)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (student is null)
            {
                return NotFound();
            }

            return View(student);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _studentRepository.GetByIdAsync(id);
            if (student is not null)
            {
                _studentRepository.Remove(student);
                await _studentRepository.SaveChangesAsync();
                TempData["SuccessMessage"] = "Student deleted successfully.";
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task PopulateClassRoomsAsync(int? selectedClassRoomId = null)



        {
            var classRooms = await _classRoomRepository.GetAllAsync();
            ViewData["ClassRoomId"] = new SelectList(
                classRooms.OrderBy(c => c.Name).ThenBy(c => c.Section),
                "Id",
                "DisplayName",
                selectedClassRoomId);
        }

        private async Task<string> SaveImageAsync(IFormFile imageFile)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            var extension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(extension))
            {
                throw new InvalidOperationException("Only JPG, PNG, or WEBP images are allowed.");
            }

            var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "students");
            Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            return $"/uploads/students/{uniqueFileName}";
        }
    }
}
