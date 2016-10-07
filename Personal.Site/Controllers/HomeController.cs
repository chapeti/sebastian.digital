using Microsoft.AspNetCore.Mvc;
using Personal.Site.Models.HomeViewModels;
using Personal.Site.Services;

namespace Personal.Site.Controllers
{
    public class HomeController : BaseController
    {

        #region Constructor
        public HomeController(IBaseControllerService baseControllerService) : base(baseControllerService)
        {
        }
        #endregion


        public IActionResult Index()
        {
            var model = FromBaseViewModel<HomeViewModel>();

            return View(model);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

    }
}
