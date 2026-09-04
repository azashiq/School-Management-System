using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Domain.Entities;
using SchoolManagementSystem.Domain.Interfaces;

namespace SchoolManagementSystem.Web.Controllers
{
    public class SubjectsController : Controller
    {
        private readonly IRepository<Subject> _subjectRepository;
        private readonly IRepository<Teacher> _teacherRepository;
        private readonly IRepository<ClassRoom> _classRoomRepository;

        public SubjectsController(
            IRepository<Subject> subjectRepository,
            IRepository<Teacher> teacherRepository,
            IRepository<ClassRoom> classRoomRepository)
        {
            _subjectRepository = subjectRepository;
            _teacherRepository = teacherRepository;
            _classRoomRepository = classRoomRepository;
        }

        public async Task<IActionResult> Index()
        {
            var subjects = await _subjectRepository.Query()
                .Include(s => s.Teacher)
                .Include(s => s.ClassRoom)
                .OrderBy(s => s.Name)
                .ToListAsync();

            return View(subjects);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var subject = await _subjectRepository.Query()
                .Include(s => s.Teacher)
                .Include(s => s.ClassRoom)
                .Include(s => s.Grades)
                .ThenInclude(g => g.Student)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (subject is null)
            {
                return NotFound();
            }

            return View(subject);
        }

        public async Task<IActionResult> Create()
        {
            await PopulateDropdownsAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SubjectCode,Name,TeacherId,ClassRoomId")] Subject subject)
        {
            if (ModelState.IsValid)
            {
                await _subjectRepository.AddAsync(subject);
                await _subjectRepository.SaveChangesAsync();
                TempData["SuccessMessage"] = "Subject created successfully.";
                return RedirectToAction(nameof(Index));
            }

            await PopulateDropdownsAsync(subject.TeacherId, subject.ClassRoomId);
            return View(subject);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var subject = await _subjectRepository.GetByIdAsync(id.Value);
            if (subject is null)
            {
                return NotFound();
            }

            await PopulateDropdownsAsync(subject.TeacherId, subject.ClassRoomId);
            return View(subject);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SubjectCode,Name,TeacherId,ClassRoomId,CreatedAt,IsDeleted")] Subject subject)
        {
            if (id != subject.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _subjectRepository.Update(subject);
                    await _subjectRepository.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Subject updated successfully.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _subjectRepository.ExistsAsync(s => s.Id == subject.Id))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            await PopulateDropdownsAsync(subject.TeacherId, subject.ClassRoomId);
            return View(subject);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var subject = await _subjectRepository.Query()
                .Include(s => s.Teacher)
                .Include(s => s.ClassRoom)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (subject is null)
            {
                return NotFound();
            }

            return View(subject);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var subject = await _subjectRepository.GetByIdAsync(id);
            if (subject is not null)
            {
                _subjectRepository.Remove(subject);
                await _subjectRepository.SaveChangesAsync();
                TempData["SuccessMessage"] = "Subject deleted successfully.";
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task PopulateDropdownsAsync(int? selectedTeacherId = null, int? selectedClassRoomId = null)
        {
            var teachers = await _teacherRepository.GetAllAsync();
            ViewData["TeacherId"] = new SelectList(
                teachers.Select(t => new { t.Id, t.FullName }).OrderBy(t => t.FullName),
                "Id",
                "FullName",
                selectedTeacherId);

            var classRooms = await _classRoomRepository.GetAllAsync();
            ViewData["ClassRoomId"] = new SelectList(
                classRooms.Select(c => new { c.Id, c.DisplayName }).OrderBy(c => c.DisplayName),
                "Id",
                "DisplayName",
                selectedClassRoomId);
        }
    }
}
