using Microsoft.AspNetCore.Mvc;

namespace SchoolManagementSystem.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Students");
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
