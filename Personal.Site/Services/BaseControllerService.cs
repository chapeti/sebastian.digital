using AutoMapper;
using Microsoft.Extensions.Options;
using Personal.Site.Models.SharedViewModels;
using Personal.Site.Settings;

namespace Personal.Site.Services
{
    public class BaseControllerService : IBaseControllerService
    {
        #region Dependencies
        private readonly IOptions<LayoutSettings> _layoutSettings;
        private readonly IMapper _mapper;
        #endregion

        #region Constructor
        public BaseControllerService(IOptions<LayoutSettings> layoutSettings, IMapper mapper)
        {
            _layoutSettings = layoutSettings;
            _mapper = mapper;
        }
        #endregion

        #region IBaseControllerService
        public TViewModel AddBaseViewModel<TViewModel>(TViewModel instance) where TViewModel : LayoutViewModel
        {
            return _mapper.Map(_layoutSettings.Value, instance);
        }
        #endregion
    }
}
