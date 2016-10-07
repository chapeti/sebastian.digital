using Microsoft.AspNetCore.Mvc;
using Personal.Site.Models.SharedViewModels;
using Personal.Site.Services;

namespace Personal.Site.Controllers
{
    public abstract class BaseController : Controller
    {

        private readonly IBaseControllerService _baseControllerService;

        protected BaseController(IBaseControllerService baseControllerService)
        {
            _baseControllerService = baseControllerService;
        }

        protected TViewModel FromBaseViewModel<TViewModel>() where TViewModel : LayoutViewModel, new()
        {
            return _baseControllerService.AddBaseViewModel(new TViewModel());
        }


        protected TViewModel WithBaseViewModel<TViewModel>(TViewModel instance) where TViewModel : LayoutViewModel
        {
            return _baseControllerService.AddBaseViewModel(instance);
        }



    }
}
