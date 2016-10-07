using Personal.Site.Models.SharedViewModels;

namespace Personal.Site.Services
{
    public interface IBaseControllerService
    {
        TViewModel AddBaseViewModel<TViewModel>(TViewModel instance) where TViewModel : LayoutViewModel;
    }
}
