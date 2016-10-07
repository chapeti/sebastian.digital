using AutoMapper;
using Microsoft.Extensions.Options;
using Personal.Site.Models.SharedViewModels;
using Personal.Site.Settings;

namespace Personal.Site.Services
{
    public class BaseControllerService : IBaseControllerService
    {
        #region Dependencies
        private readonly IOptions<LayoutSettings> _optionsAccessor;
        private readonly IMapper _mapper;
        #endregion

        #region Constructor
        public BaseControllerService(IOptions<LayoutSettings> optionsAccessor, IMapper mapper)
        {
            _optionsAccessor = optionsAccessor;
            _mapper = mapper;
        }
        #endregion

        #region IBaseControllerService
        public TViewModel AddBaseViewModel<TViewModel>(TViewModel instance) where TViewModel : LayoutViewModel
        {
            return _mapper.Map(_optionsAccessor.Value, instance);
        }
        #endregion
    }
}
