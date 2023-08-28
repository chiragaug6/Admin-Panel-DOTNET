using Microsoft.AspNetCore.Mvc;

namespace StartAdminPanel.Areas.MST_Branch.Controllers
{
    public class MST_BranchController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
