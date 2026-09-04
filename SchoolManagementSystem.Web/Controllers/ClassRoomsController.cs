using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Domain.Entities;
using SchoolManagementSystem.Domain.Interfaces;

namespace SchoolManagementSystem.Web.Controllers
{
    public class ClassRoomsController : Controller
    {
        private readonly IRepository<ClassRoom> _classRoomRepository;

        public ClassRoomsController(IRepository<ClassRoom> classRoomRepository)
        {
            _classRoomRepository = classRoomRepository;
        }

        public async Task<IActionResult> Index()
        {
            var classRooms = await _classRoomRepository.Query().OrderBy(c => c.Name).ToListAsync();
            return View(classRooms);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var classRoom = await _classRoomRepository.Query()
                .Include(c => c.Students)
                .Include(c => c.Subjects)
                .ThenInclude(s => s.Teacher)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (classRoom is null)
            {
                return NotFound();
            }

            return View(classRoom);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Section,Capacity")] ClassRoom classRoom)
        {
            if (ModelState.IsValid)
            {
                await _classRoomRepository.AddAsync(classRoom);
                await _classRoomRepository.SaveChangesAsync();
                TempData["SuccessMessage"] = "Classroom created successfully.";
                return RedirectToAction(nameof(Index));
            }

            return View(classRoom);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var classRoom = await _classRoomRepository.GetByIdAsync(id.Value);
            if (classRoom is null)
            {
                return NotFound();
            }

            return View(classRoom);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Section,Capacity,CreatedAt,IsDeleted")] ClassRoom classRoom)
        {
            if (id != classRoom.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _classRoomRepository.Update(classRoom);
                    await _classRoomRepository.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Classroom updated successfully.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _classRoomRepository.ExistsAsync(c => c.Id == classRoom.Id))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            return View(classRoom);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var classRoom = await _classRoomRepository.GetByIdAsync(id.Value);
            if (classRoom is null)
            {
                return NotFound();
            }

            return View(classRoom);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var classRoom = await _classRoomRepository.GetByIdAsync(id);
            if (classRoom is not null)
            {
                _classRoomRepository.Remove(classRoom);
                await _classRoomRepository.SaveChangesAsync();
                TempData["SuccessMessage"] = "Classroom deleted successfully.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
