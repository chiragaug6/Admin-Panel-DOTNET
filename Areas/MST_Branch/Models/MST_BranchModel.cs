using Microsoft.AspNetCore.Mvc;

namespace StartAdminPanel.Areas.MST_Branch.Models
{
    public class MST_BranchModel : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
