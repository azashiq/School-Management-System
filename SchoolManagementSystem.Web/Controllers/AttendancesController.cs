using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Domain.Entities;
using SchoolManagementSystem.Domain.Interfaces;

namespace SchoolManagementSystem.Web.Controllers
{
    public class AttendancesController : Controller
    {
        private readonly IRepository<Attendance> _attendanceRepository;
        private readonly IRepository<Student> _studentRepository;

        public AttendancesController(IRepository<Attendance> attendanceRepository, IRepository<Student> studentRepository)
        {
            _attendanceRepository = attendanceRepository;
            _studentRepository = studentRepository;
        }

        public async Task<IActionResult> Index()
        {
            var attendances = await _attendanceRepository.Query()
                .Include(a => a.Student)
                .OrderByDescending(a => a.Date)
                .ToListAsync();

            return View(attendances);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var attendance = await _attendanceRepository.Query()
                .Include(a => a.Student)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (attendance is null)
            {
                return NotFound();
            }

            return View(attendance);
        }

        public async Task<IActionResult> Create()
        {
            await PopulateStudentsAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StudentId,Date,IsPresent,Remarks")] Attendance attendance)
        {
            if (ModelState.IsValid)
            {
                await _attendanceRepository.AddAsync(attendance);
                await _attendanceRepository.SaveChangesAsync();
                TempData["SuccessMessage"] = "Attendance recorded successfully.";
                return RedirectToAction(nameof(Index));
            }

            await PopulateStudentsAsync(attendance.StudentId);
            return View(attendance);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var attendance = await _attendanceRepository.GetByIdAsync(id.Value);
            if (attendance is null)
            {
                return NotFound();
            }

            await PopulateStudentsAsync(attendance.StudentId);
            return View(attendance);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StudentId,Date,IsPresent,Remarks,CreatedAt,IsDeleted")] Attendance attendance)
        {
            if (id != attendance.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _attendanceRepository.Update(attendance);
                    await _attendanceRepository.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Attendance updated successfully.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _attendanceRepository.ExistsAsync(a => a.Id == attendance.Id))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            await PopulateStudentsAsync(attendance.StudentId);
            return View(attendance);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var attendance = await _attendanceRepository.Query()
                .Include(a => a.Student)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (attendance is null)
            {
                return NotFound();
            }

            return View(attendance);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var attendance = await _attendanceRepository.GetByIdAsync(id);
            if (attendance is not null)
            {
                _attendanceRepository.Remove(attendance);
                await _attendanceRepository.SaveChangesAsync();
                TempData["SuccessMessage"] = "Attendance record deleted successfully.";
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task PopulateStudentsAsync(int? selectedStudentId = null)
        {
            var students = await _studentRepository.GetAllAsync();
            ViewData["StudentId"] = new SelectList(
                students.Select(s => new { s.Id, s.FullName }).OrderBy(s => s.FullName),
                "Id",
                "FullName",
                selectedStudentId);
        }
    }
}
