using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Domain.Entities;
using SchoolManagementSystem.Domain.Interfaces;

namespace SchoolManagementSystem.Web.Controllers
{
    public class GradesController : Controller
    {
        private readonly IRepository<Grade> _gradeRepository;
        private readonly IRepository<Student> _studentRepository;
        private readonly IRepository<Subject> _subjectRepository;

        public GradesController(
            IRepository<Grade> gradeRepository,
            IRepository<Student> studentRepository,
            IRepository<Subject> subjectRepository)
        {
            _gradeRepository = gradeRepository;
            _studentRepository = studentRepository;
            _subjectRepository = subjectRepository;
        }

        public async Task<IActionResult> Index()
        {
            var grades = await _gradeRepository.Query()
                .Include(g => g.Student)
                .Include(g => g.Subject)
                .OrderByDescending(g => g.ExamDate)
                .ToListAsync();

            return View(grades);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var grade = await _gradeRepository.Query()
                .Include(g => g.Student)
                .Include(g => g.Subject)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (grade is null)
            {
                return NotFound();
            }

            return View(grade);
        }

        public async Task<IActionResult> Create()
        {
            await PopulateDropdownsAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StudentId,SubjectId,Score,ExamType,ExamDate")] Grade grade)
        {
            if (ModelState.IsValid)
            {
                await _gradeRepository.AddAsync(grade);
                await _gradeRepository.SaveChangesAsync();
                TempData["SuccessMessage"] = "Grade recorded successfully.";
                return RedirectToAction(nameof(Index));
            }

            await PopulateDropdownsAsync(grade.StudentId, grade.SubjectId);
            return View(grade);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var grade = await _gradeRepository.GetByIdAsync(id.Value);
            if (grade is null)
            {
                return NotFound();
            }

            await PopulateDropdownsAsync(grade.StudentId, grade.SubjectId);
            return View(grade);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StudentId,SubjectId,Score,ExamType,ExamDate,CreatedAt,IsDeleted")] Grade grade)
        {
            if (id != grade.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _gradeRepository.Update(grade);
                    await _gradeRepository.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Grade updated successfully.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _gradeRepository.ExistsAsync(g => g.Id == grade.Id))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            await PopulateDropdownsAsync(grade.StudentId, grade.SubjectId);
            return View(grade);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var grade = await _gradeRepository.Query()
                .Include(g => g.Student)
                .Include(g => g.Subject)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (grade is null)
            {
                return NotFound();
            }

            return View(grade);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var grade = await _gradeRepository.GetByIdAsync(id);
            if (grade is not null)
            {
                _gradeRepository.Remove(grade);
                await _gradeRepository.SaveChangesAsync();
                TempData["SuccessMessage"] = "Grade deleted successfully.";
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task PopulateDropdownsAsync(int? selectedStudentId = null, int? selectedSubjectId = null)
        {
            var students = await _studentRepository.GetAllAsync();
            ViewData["StudentId"] = new SelectList(
                students.Select(s => new { s.Id, s.FullName }).OrderBy(s => s.FullName),
                "Id",
                "FullName",
                selectedStudentId);

            var subjects = await _subjectRepository.GetAllAsync();
            ViewData["SubjectId"] = new SelectList(
                subjects.OrderBy(s => s.Name),
                "Id",
                "Name",
                selectedSubjectId);
        }
    }
}
